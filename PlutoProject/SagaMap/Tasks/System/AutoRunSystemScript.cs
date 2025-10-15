using SagaLib;
using SagaLib.Tasks;
using SagaMap.Manager;

namespace SagaMap.Tasks.System
{
    public class AutoRunSystemScript : MultiRunTask
    {
        private readonly uint ID;

        public AutoRunSystemScript(uint EventID)
        {
            Period = 5000;
            DueTime = 10000;
            ID = EventID;
        }

        public override void CallBack()
        {
            if (ScriptManager.Instance.Events.ContainsKey(ID))
            {
                var evnt = ScriptManager.Instance.Events[ID];
                evnt.OnEvent(ScriptManager.Instance.VariableHolder);
                Logger.getLogger().Information("已成功加載腳本：" + evnt);
            }

            Deactivate();
        }
    }
}