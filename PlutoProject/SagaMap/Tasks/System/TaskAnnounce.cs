using System;
using SagaLib.Tasks;
using SagaMap.Manager;

namespace SagaMap.Tasks.System {
    public class TaskAnnounce : MultiRunTask {
        private readonly string announce;
        private string aname;

        public TaskAnnounce(string taskname, string announce, int period) {
            aname = taskname;
            Period = period;
            DueTime = 0;
            this.announce = announce;
        }

        public TaskAnnounce(string taskname, string announce, int duetime, int period) {
            aname = taskname;
            Period = period;
            DueTime = duetime;
            this.announce = announce;
        }

        public override void CallBack() {
            try {
                foreach (var i in MapClientManager.Instance.OnlinePlayer)
                    /*if (i.Character.Account.GMLevel >= 100)
                        i.SendAnnounce(this.announce + " Task:" + this.aname + "period:"+period.ToString());
                    else*/
                    i.SendAnnounce(announce);
            }
            catch (Exception exception) {
                SagaLib.Logger.ShowError(exception);
            }
        }
    }
}