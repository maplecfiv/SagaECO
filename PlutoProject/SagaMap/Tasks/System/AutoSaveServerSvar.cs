using System;
using SagaLib;
using SagaLib.Tasks;
using SagaMap.Manager;

namespace SagaMap.Tasks.System {
    public class AutoSaveServerSvar : MultiRunTask {
        public AutoSaveServerSvar() {
            Period = 120000;
        }

        public override void CallBack() {
            //ClientManager.EnterCriticalArea();
            try {
                Logger.GetLogger().Information("Autosaving Server Svar data...");
                MapServer.charDB.SaveServerVar(ScriptManager.Instance.VariableHolder);
            }
            catch (Exception ex) {
                Logger.ShowError(ex);
            }
            //ClientManager.LeaveCriticalArea();
        }
    }
}