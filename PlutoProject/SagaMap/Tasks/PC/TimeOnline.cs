using System;
using SagaLib;
using SagaLib.Tasks;

namespace SagaMap.Tasks.PC
{
    public class TimeOnline : MultiRunTask
    {
        private readonly ActorPC pc;

        public TimeOnline(ActorPC pc)
        {
            period = 1000;
            this.pc = pc;
        }

        public override void CallBack()
        {
            if (pc.Status == null)
            {
                Deactivate();
                return;
            }

            ClientManager.EnterCriticalArea();
            try
            {
                if (pc.TimeOnline[0].Day == DateTime.Now.Day)
                {
                    pc.TimeOnline[1].AddSeconds(1);
                }
                else
                {
                    pc.TimeOnline[0] = DateTime.Now;
                    pc.TimeOnline[1] = new DateTime();
                }
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);
            }

            ClientManager.LeaveCriticalArea();
        }
    }
}