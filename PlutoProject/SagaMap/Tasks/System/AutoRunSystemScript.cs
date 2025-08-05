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
            period = 5000;
            dueTime = 10000;
            ID = EventID;
        }

        public override void CallBack()
        {
            if (ScriptManager.Instance.Events.ContainsKey(ID))
            {
                var evnt = ScriptManager.Instance.Events[ID];
                evnt.OnEvent(ScriptManager.Instance.VariableHolder);
                Logger.ShowInfo("已成功加載腳本：" + evnt);
            }

            Deactivate();
        }
    }
}