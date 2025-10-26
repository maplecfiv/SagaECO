using System;
using System.Collections.Generic;
using System.Threading;
using SagaLib;

namespace SagaMap.Partner {
    public class AIThread : Singleton<AIThread> {
        private static readonly List<PartnerAI> ais = new List<PartnerAI>(); //线程中的AI
        private static readonly List<PartnerAI> deleting = new List<PartnerAI>(); //删除ai队列
        private static readonly List<PartnerAI> adding = new List<PartnerAI>(); //增加ai队列
        private readonly Thread mainThread; //主线程

        public AIThread() //构造函数
        {
            mainThread = new Thread(mainLoop);
            mainThread.Name = string.Format("PartnerAIThread({0})", mainThread.ManagedThreadId);
            Logger.GetLogger().Information("PartnerAI线程启动：" + mainThread.Name);
            ClientManager.AddThread(mainThread);
            mainThread.Start();
        }

        public int ActiveAI => ais.Count; //返回线程中ai的数量

        public void RegisterAI(PartnerAI ai) {
            lock (adding) {
                adding.Add(ai); //如果adding没有被其他线程访问中，则将ai添加入增加队列
            }
        }

        public void RemoveAI(PartnerAI ai) {
            lock (deleting) {
                deleting.Add(ai); //如果deleting没有被其他线程访问中，则将ai添加入删除队列
            }
        }

        private static void mainLoop() {
            while (true) {
                lock (deleting) //如果deleting没有被其他线程访问中，则遍历删除队列，并移除线程中ai中的要删除的线程，然后清空删除队列
                {
                    foreach (var i in deleting)
                        if (ais.Contains(i))
                            ais.Remove(i);
                    deleting.Clear();
                }

                lock (adding) {
                    foreach (var i in adding)
                        if (!ais.Contains(i))
                            ais.Add(i);
                    adding.Clear();
                }

                foreach (var i in ais) {
                    if (!i.Activated)
                        continue;
                    if (DateTime.Now > i.NextUpdateTime) {
                        //ClientManager.EnterCriticalArea();
                        try {
                            i.CallBack(null);
                        }
                        catch (Exception ex) {
                            Logger.ShowError(ex);
                        }

                        i.NextUpdateTime = DateTime.Now + new TimeSpan(0, 0, 0, 0, i.Period);
                        //ClientManager.LeaveCriticalArea();
                    }
                }

                if (ais.Count == 0)
                    Thread.Sleep(500);
                else
                    Thread.Sleep(10);
            }
        }
    }
}