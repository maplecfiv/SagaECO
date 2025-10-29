using System.Collections.Generic;
using System.Threading;

namespace SagaLib {
    public class DatabaseWaitress {
        public static AutoResetEvent waitressQueue = new AutoResetEvent(true);
        public static Thread Coordinator;
        private static readonly List<Thread> blockedThread = new List<Thread>();
        private static Thread currentBlocker;
        public static bool Blocked => blockedThread.Contains(Thread.CurrentThread);

        public static void EnterCriticalArea() {
            if (blockedThread.Contains(Thread.CurrentThread)) {
                Logger.ShowDebug("Current thread is already blocked, skip blocking to avoid deadlock!");
            }
            else {
                waitressQueue.WaitOne();
                blockedThread.Add(Thread.CurrentThread);
                currentBlocker = Thread.CurrentThread;
            }
        }

        public static void LeaveCriticalArea() {
            LeaveCriticalArea(Thread.CurrentThread);
        }

        public static void LeaveCriticalArea(Thread blocker) {
            if (!blockedThread.Contains(blocker) && blockedThread.Count == 0) {
                Logger.ShowDebug("Current thread isn't blocked while trying unblock, skiping");
                return;
            }

            if (blockedThread.Contains(blocker)) {
                blockedThread.Remove(blocker);
            }
            else {
                if (blockedThread.Count > 0) {
                    blockedThread.RemoveAt(0);
                }
            }

            currentBlocker = null;
            waitressQueue.Set();
        }
    }
}