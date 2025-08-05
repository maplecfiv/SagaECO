using System;
using SagaDB.Actor;
using SagaLib;

namespace SagaMap.Tasks.PC
{
    public class AutoSave : MultiRunTask
    {
        private readonly ActorPC pc;

        public AutoSave(ActorPC pc)
        {
            period = 300000;
            this.pc = pc;
        }

        public override void CallBack()
        {
            if (pc == null)
            {
                Deactivate();
                return;
            }

            if (!pc.Online)
            {
                Deactivate();
                return;
            }

            ClientManager.EnterCriticalArea();
            try
            {
                var now = DateTime.Now;
                MapServer.charDB.SaveChar(pc, false);
                Logger.ShowInfo(
                    "Autosaving " + pc.Name + "'s data, 耗时:" + (DateTime.Now - now).TotalMilliseconds + "ms");
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);
            }

            ClientManager.LeaveCriticalArea();
        }
    }
}