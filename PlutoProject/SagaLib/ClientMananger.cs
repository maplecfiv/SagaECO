//#define Debug

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading;
using ThreadState = System.Threading.ThreadState;
using Microsoft.Diagnostics.Runtime;
using System.Linq;

namespace SagaLib
{
    public class ClientManager
    {
        private static readonly Serilog.Core.Logger _logger = Logger.InitLogger<ClientManager>();

        //public byte CheckUnLockSecond = 1;
        public static bool NoCheckDeadLock = false;

        private static bool _enteredcriarea;
        public static readonly List<Thread> BlockedThread = new List<Thread>();
        private static readonly Dictionary<string, Thread> Threads = new Dictionary<string, Thread>();
        private static Thread _currentBlocker;
        private static DateTime _timestamp;
        private static string _blockdetail;

        //#if Debug
        private static StackTrace _stacktrace;
        //#endif

        /// <summary>
        ///     Command table contains the commands that need to be called when a
        ///     packet is received. Key will be the packet type
        /// </summary>
        public Dictionary<ushort, Packet> CommandTable;

        public TcpListener Listener;

        public Thread PacketCoordinator;

        public AutoResetEvent WaitressQueue;

        public static bool Blocked => BlockedThread.Contains(Thread.CurrentThread);

        public static void AddThread(Thread thread)
        {
            AddThread(thread.Name, thread);
        }

        public static void AddThread(string name, Thread thread)
        {
            if (Threads.ContainsKey(name))
            {
                return;
            }

            lock (Threads)
            {
                try
                {
                    Threads.Add(name, thread);
                }
                catch (Exception ex)
                {
                    Logger.getLogger().Error(ex, ex.Message);
                    Logger.ShowDebug("Threads count:" + Threads.Count, null);
                }
            }
        }

        public static void RemoveThread(string name)
        {
            if (!Threads.ContainsKey(name))
            {
                return;
            }

            lock (Threads)
            {
                Threads.Remove(name);
            }
        }

        public static Thread GetThread(string name)
        {
            if (Threads.ContainsKey(name))
            {
                lock (Threads)
                {
                    return Threads[name];
                }
            }

            return null;
        }

        //solve deadlock
        public void checkCriticalArea()
        {
            while (true)
            {
                if (_enteredcriarea)
                {
                    var span = DateTime.Now - _timestamp;
                    if (span.TotalSeconds > 10 && !NoCheckDeadLock && !Debugger.IsAttached)
                    {
                        Logger.getLogger().Error("Deadlock detected");
                        Logger.getLogger().Error("Automatically unlocking....");
                        Logger.ShowDebug(_blockdetail, Logger.defaultlogger);
                        //#if Debug
                        Logger.getLogger().Error("Call Stack Before Entered Critical Area:");
                        try
                        {
                            Logger.getLogger().Error("Thread name:" + getThreadName(_currentBlocker));
                            foreach (var i in _stacktrace.GetFrames())
                            {
                                Logger.getLogger().Error("at " + i.GetMethod().ReflectedType.FullName + "." +
                                                         i.GetMethod().Name + " " + i.GetFileName() + ":" +
                                                         i.GetFileLineNumber());
                            }
                        }
                        catch (Exception e)
                        {
                            _logger.Error(e, e.Message);
                        }

                        // _logger.Debug();
                        //#endif
                        StackTrace running;
                        try
                        {
                            if (_currentBlocker != null)
                            {
                                Logger.getLogger().Error("Call Stack of current blocking Thread:");
                                Logger.getLogger().Error("Thread name:" + getThreadName(_currentBlocker));
                                if (_currentBlocker.ThreadState != ThreadState.Running)
                                {
                                    Logger.getLogger()
                                        .Warning("Unexpected thread state:" + _currentBlocker.ThreadState);
                                }

                                foreach (var i in GetThreadFrames(_currentBlocker.ManagedThreadId))
                                {
                                    Logger.getLogger().Warning("at " + i.Method.Type + "." + i.Method.Name +
                                                               " " + i.ToString());
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.getLogger().Error(ex, ex.Message);
                        }

                        // _logger.Debug();
                        Logger.getLogger().Error("Call Stack of all blocking Threads:");
                        var list = BlockedThread.ToArray();
                        foreach (var j in list)
                        {
                            try
                            {
                                Logger.getLogger().Error("Thread name:" + getThreadName(j));
                                if (j.ThreadState != ThreadState.Running)
                                {
                                    Logger.getLogger().Warning("Unexpected thread state:" + j.ThreadState);
                                }

                                foreach (var i in GetThreadFrames(j.ManagedThreadId))
                                {
                                    Logger.getLogger().Warning("at " + i.Method.Type + "." + i.Method.Name +
                                                               " " + i.ToString());
                                }
                            }
                            catch (Exception ex)
                            {
                                Logger.getLogger().Error(ex, ex.Message);
                            }

                            // _logger.Debug();
                        }

                        // _logger.Debug();
                        Logger.getLogger().Error("Call Stack of all Threads:");
                        var keys = new string[Threads.Keys.Count];
                        Threads.Keys.CopyTo(keys, 0);
                        foreach (var k in keys)
                        {
                            try
                            {
                                var j = GetThread(k);
                                Logger.getLogger().Error("Thread name:" + k);
                                if (j.ThreadState != ThreadState.Running)
                                {
                                    Logger.getLogger().Warning("Unexpected thread state:" + j.ThreadState);
                                }

                                foreach (var i in GetThreadFrames(j.ManagedThreadId))
                                {
                                    Logger.getLogger().Warning("at " + i.Method.Type + "." + i.Method.Name +
                                                               " " + i.ToString());
                                }
                            }
                            catch (Exception e)
                            {
                                _logger.Error(e, e.Message);
                            }

                            // _logger.Debug();
                        }

                        LeaveCriticalArea(_currentBlocker);
                    }
                }

                Thread.Sleep(5000);
            }
        }

        private static string getThreadName(Thread thread)
        {
            foreach (var i in Threads.Keys)
            {
                if (thread == Threads[i])
                {
                    return i;
                }
            }

            return "";
        }

        public static void PrintAllThreads()
        {
            Logger.getLogger().Warning("Call Stack of all blocking Threads:");
            var list = BlockedThread.ToArray();
            foreach (var j in list)
            {
                try
                {
                    Logger.getLogger().Warning("Thread name:" + getThreadName(j));
                    foreach (var i in GetThreadFrames(j.ManagedThreadId))
                    {
                        Logger.getLogger().Warning("at " + i.Method.Type + "." + i.Method.Name +
                                                   " " + i.ToString());
                    }
                }
                catch (Exception e)
                {
                    _logger.Error(e, e.Message);
                }
                // _logger.Debug();
            }

            Logger.getLogger().Warning("Call Stack of all Threads:");
            var keys = new string[Threads.Keys.Count];
            Threads.Keys.CopyTo(keys, 0);
            foreach (var k in keys)
            {
                try
                {
                    var j = GetThread(k);
                    // j.Suspend();
                    // j.Resume();
                    Logger.getLogger().Warning("Thread name:" + k);
                    foreach (var i in GetThreadFrames(j.ManagedThreadId))
                    {
                        Logger.getLogger().Warning("at " + i.Method.Type + "." + i.Method.Name +
                                                   " " + i.ToString());
                    }
                }
                catch (Exception e)
                {
                    _logger.Error(e, e.Message);
                }

                // _logger.Debug();
            }
        }

        public static void EnterCriticalArea()
        {
            if (BlockedThread.Contains(Thread.CurrentThread))
            {
                //Logger.ShowDebug("Current thread is already blocked, skip blocking to avoid deadlock!", Logger.defaultlogger);
            }
            else
            {
                //Global.clientMananger.AddWaitingWaitress();
                Global.clientMananger.WaitressQueue.WaitOne();
                _timestamp = DateTime.Now;
                _enteredcriarea = true;
                BlockedThread.Add(Thread.CurrentThread);
                _currentBlocker = Thread.CurrentThread;
                _blockdetail = "锁定线程名:" + Thread.CurrentThread.Name;
                //Global.clientMananger.waitressQueue.WaitOne();
                //#if Debug
                _stacktrace = new StackTrace(1, true);
                //#endif
            }
        }

        public static void LeaveCriticalArea()
        {
            LeaveCriticalArea(Thread.CurrentThread);
        }

        public static void LeaveCriticalArea(Thread blocker)
        {
            if (!BlockedThread.Contains(blocker) && BlockedThread.Count == 0)
            {
                //Logger.ShowDebug("Current thread isn't blocked while trying unblock, skiping", Logger.defaultlogger);
                Global.clientMananger.WaitressQueue.Set();
                return;
            }

            //Global.clientMananger.RemoveWaitingWaitress();
            // Global.clientMananger.waitressHasFinished.Set();
            var sec = (DateTime.Now - _timestamp).Seconds;
            if (sec >= 1)
            {
                Logger.ShowDebug(
                    string.Format("Thread({0}) used unnormal time till unlock({1} sec)", blocker.Name, sec),
                    Logger.defaultlogger);
            }

            _enteredcriarea = false;
            if (BlockedThread.Contains(blocker))
            {
                try
                {
                    BlockedThread.Remove(blocker);
                }
                catch (Exception ex)
                {
                    if (BlockedThread.Count > 0)
                    {
                        BlockedThread.RemoveAt(0);
                    }

                    Logger.getLogger().Error("a2333" + ex);
                }
            }
            else
            {
                if (BlockedThread.Count > 0)
                {
                    BlockedThread.RemoveAt(0);
                }
                else
                {
                    Logger.getLogger().Error("线程不存在！！");
                }
            }

            _currentBlocker = null;
            _timestamp = DateTime.Now;
            Global.clientMananger.WaitressQueue.Set();
        }


        public void Start()
        {
        }

        public void Stop()
        {
            Listener.Stop();
        }


        /// <summary>
        ///     Starts the network listener socket.
        /// </summary>
        public bool StartNetwork(int port)
        {
            /*IPAddress host = IPAddress.Parse(lcfg.Host);
            listener = new TcpListener(host, lcfg.Port);*/
            Listener = new TcpListener(port);
            try
            {
                Listener.Start();
            }
            catch (Exception ex)
            {
                Logger.getLogger().Error(ex, ex.Message);
                return false;
            }

            return true;
        }

        public virtual Client GetClient(uint SessionID)
        {
            return null;
        }

        public virtual Client GetClientForName(string SessionName)
        {
            return null;
        }

        public virtual void NetworkLoop(int maxNewConnections)
        {
        }

        public virtual void OnClientDisconnect(Client client)
        {
        }

        private static IEnumerable<ClrStackFrame> GetThreadFrames(int managedThreadId)
        {
            using (var target = DataTarget.CreateSnapshotAndAttach(Process.GetCurrentProcess().Id))
            {
                var runtime = target.ClrVersions.First().CreateRuntime();

                var threadNameLookup = new Dictionary<int, string>();
                foreach (var obj in runtime.Heap.EnumerateObjects())
                {
                    if (!(obj.Type is null) && obj.Type.Name == "System.Threading.Thread")
                    {
                        var threadId = obj.ReadField<int>("m_ManagedThreadId");
                        if (threadId == managedThreadId)
                        {
                            var threadName = obj.ReadStringField("_Name");
                            threadNameLookup[threadId] = threadName;
                            break;
                        }
                    }
                }

                foreach (var thread in runtime.Threads)
                {
                    if (thread.ManagedThreadId != managedThreadId)
                    {
                        continue;
                    }

                    threadNameLookup.TryGetValue(thread.ManagedThreadId, out _);
                    return thread.EnumerateStackTrace();
                }
            }

            return null;
        }
    }
}