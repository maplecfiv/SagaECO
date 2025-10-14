using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace SagaLib.Tasks
{
    public class TaskManager : Singleton<TaskManager>
    {
        private readonly ConcurrentQueue<MultiRunTask> fifo = new ConcurrentQueue<MultiRunTask>();
        private readonly HashSet<MultiRunTask> registered = new HashSet<MultiRunTask>();
        private readonly ConcurrentQueue<MultiRunTask> slowFifo = new ConcurrentQueue<MultiRunTask>();
        private readonly List<Thread> threadpool = new List<Thread>();
        private readonly AutoResetEvent waiter = new AutoResetEvent(false);
        private readonly AutoResetEvent waiterSlow = new AutoResetEvent(false);
        private readonly Stopwatch watch = new Stopwatch();
        private int exeCount;
        private DateTime exeStamp = DateTime.Now, schedulerStamp = DateTime.Now;
        private int exeTime;

        private Thread main;
        private int schedulerCount;
        private int schedulerTime;
        private MultiRunTask[] tasks = new MultiRunTask[0];

        public TaskManager()
        {
            //DefaultValue;
            SetWorkerCount(4, 8);
            Start();
        }

        /// <summary>
        ///     平均调度器调度时间
        /// </summary>
        public int AverageScheduleTime { get; set; }

        /// <summary>
        ///     Task的平均执行时间
        /// </summary>
        public int AverageExecutionTime { get; set; }

        /// <summary>
        ///     总Task数
        /// </summary>
        public int RegisteredCount => registered.Count;

        /// <summary>
        ///     每分钟的Task执行量
        /// </summary>
        public int ExecutionCountPerMinute { get; set; }

        /// <summary>
        ///     返回註冊中的任務名
        /// </summary>
        public List<string> RegisteredTasks
        {
            get
            {
                var list = new List<string>();
                lock (registered)
                {
                    foreach (var i in registered) list.Add(i.ToString());
                }

                return list;
            }
        }

        /// <summary>
        ///     设置Worker线程数量
        /// </summary>
        /// <param name="count">普通Task线程数</param>
        /// <param name="slowCount">执行时间较长的Task线程数</param>
        public void SetWorkerCount(int count, int slowCount)
        {
            foreach (var i in threadpool)
            {
                ClientManager.RemoveThread(i.Name);
                i.Abort();
            }

            threadpool.Clear();
            for (var i = 0; i < count; i++)
            {
                var thread = new Thread(Worker);
                thread.Priority = ThreadPriority.Highest;
                thread.Name = string.Format("Worker({0})", thread.ManagedThreadId);
                ClientManager.AddThread(thread);
                thread.Start();
                threadpool.Add(thread);
            }

            for (var i = 0; i < slowCount; i++)
            {
                var thread = new Thread(WorkerSlow);
                thread.Name = $"WorkerSlow({thread.ManagedThreadId})";
                ClientManager.AddThread(thread);
                thread.Start();
                threadpool.Add(thread);
            }
        }

        /// <summary>
        ///     启动任务管理器线程池
        /// </summary>
        public void Start()
        {
            if (main != null)
            {
                ClientManager.RemoveThread(main.Name);
                main.Abort();
            }

            main = new Thread(MainLoop);
            main.Name = string.Format("ThreadPoolMainLoop({0})", main.ManagedThreadId);
            Logger.ShowInfo("主线程启动！：" + main.Name);
            ClientManager.AddThread(main);
            main.Start();
        }

        /// <summary>
        ///     停止任务管理器线程池
        /// </summary>
        public void Stop()
        {
            foreach (var i in threadpool)
            {
                ClientManager.RemoveThread(i.Name);
                i.Abort();
            }

            threadpool.Clear();
            if (main == null)
            {
                return;
            }
            
            ClientManager.RemoveThread(main.Name);
            main.Abort();
        }

        /// <summary>
        ///     注册任务，通常不需要调用，直接调用Task.Activate()即可
        /// </summary>
        /// <param name="task">任务</param>
        public void RegisterTask(MultiRunTask task)
        {
            lock (registered)
            {
                registered.Add(task);
            }
        }

        /// <summary>
        ///     注销任务，通常不需要调用，直接调用Task.Deactivate()即可
        /// </summary>
        /// <param name="task"></param>
        public void RemoveTask(MultiRunTask task)
        {
            lock (registered)
            {
                registered.Remove(task);
            }
        }

        private void PushTaskes()
        {
            var now = DateTime.Now;
            if ((now - exeStamp).TotalMinutes > 1)
            {
                AverageExecutionTime = exeCount > 0 ? exeTime / exeCount : 0;
                ExecutionCountPerMinute = exeCount;
                Interlocked.Exchange(ref exeCount, 0);
                Interlocked.Exchange(ref exeTime, 0);
                exeStamp = now;
            }

            if ((now - schedulerStamp).TotalMinutes > 1)
            {
                AverageScheduleTime = schedulerCount > 0 ? schedulerTime / schedulerCount : 0;
                Interlocked.Exchange(ref schedulerCount, 0);
                Interlocked.Exchange(ref schedulerTime, 0);
                schedulerStamp = now;
            }

            Interlocked.Increment(ref schedulerCount);
            watch.Restart();
            int length;
            lock (registered)
            {
                var count = registered.Count;
                if (tasks.Length < count)
                    tasks = new MultiRunTask[count];
                length = count;
                registered.CopyTo(tasks);
            }

            for (var i = 0; i < length; i++)
            {
                var task = tasks[i];

                if (task.executing || now <= task.NextUpdateTime)
                {
                    continue;
                }

                try
                {
                    if (!task.executing && now > task.NextUpdateTime)
                    {
                            
                        task.executing = true;
                    }
                    task.NextUpdateTime = now.AddMilliseconds(task.Period);
                    task.TaskBeginTime = now;
                    if (task.IsSlowTask)
                    {
                        slowFifo.Enqueue(task);
                        waiterSlow.Set();
                    }
                    else
                    {
                        fifo.Enqueue(task);
                        waiter.Set();
                    }
                }
                catch (Exception ex)
                {
                    Logger.ShowError(ex);
                }
                
            }

            watch.Stop();
            Interlocked.Add(ref schedulerTime, (int)watch.ElapsedMilliseconds);
        }

        /// <summary>
        ///     主要線程
        /// </summary>
        private void MainLoop()
        {
            try
            {
                while (true)
                {
                    PushTaskes();
                    if (registered.Count <= 1000)
                    {
                        
                        Thread.Sleep(1);
                        continue;
                    }

                    var waitTime = 10000 / registered.Count;
                    if (waitTime > 10)
                    {
                        waitTime = 10;
                    }

                    if (waitTime == 0)
                    {
                            
                        waitTime = 1;
                    }

                    Thread.Sleep(waitTime);
                }
            }
            catch (ThreadAbortException)
            {
                ClientManager.RemoveThread(Thread.CurrentThread.Name);
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);
            }

            ClientManager.RemoveThread(Thread.CurrentThread.Name);
        }

        private void Worker()
        {
            WorkerIntern(fifo, waiter);
        }

        private void WorkerSlow()
        {
            WorkerIntern(slowFifo, waiterSlow);
        }

        private void WorkerIntern(ConcurrentQueue<MultiRunTask> fifo, AutoResetEvent waiter)
        {
            try
            {
                while (true)
                {
                    while (fifo.TryDequeue(out var task))
                        try
                        {
                            task.CallBack();
                            Interlocked.Add(ref exeTime, (int)(DateTime.Now - task.TaskBeginTime).TotalMilliseconds);
                            Interlocked.Increment(ref exeCount);
                            task.executing = false;
                        }
                        catch (Exception ex)
                        {
                            Logger.ShowError(ex);
                        }

                    waiter.WaitOne(5);
                }
            }
            catch (ThreadAbortException)
            {
                ClientManager.RemoveThread(Thread.CurrentThread.Name);
            }
            catch (Exception ex)
            {
                Logger.ShowError("Critical ERROR! Worker terminated unexpected!");
                Logger.ShowError(ex);
            }

            ClientManager.RemoveThread(Thread.CurrentThread.Name);
        }
    }
}