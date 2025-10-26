using System;
using SagaLib;
using SagaLib.Tasks;
using SagaMap.Network.Client;

namespace SagaMap.Tasks.PC {
    public class SkillCast : MultiRunTask {
        private readonly MapClient client;
        private readonly SkillArg skill;

        public SkillCast(MapClient client, SkillArg skill) {
            if (skill.argType == SkillArg.ArgType.Cast) {
                DueTime = (int)skill.delay;
                Period = (int)skill.delay;
            }
            else if (skill.argType == SkillArg.ArgType.Item_Cast) {
                DueTime = (int)skill.item.BaseData.cast;
                Period = (int)skill.item.BaseData.cast;
            }

            this.client = client;
            this.skill = skill;
        }

        public override void CallBack() {
            ClientManager.EnterCriticalArea();
            try {
                if (client.Character != null) {
                    client.Character.Tasks.Remove("SkillCast");
                    if (skill.argType == SkillArg.ArgType.Cast)
                        client.OnSkillCastComplete(skill);
                    if (skill.argType == SkillArg.ArgType.Item_Cast)
                        client.OnItemCastComplete(skill);
                }

                Deactivate();
            }
            catch (Exception ex) {
                Logger.ShowError(ex);
                Deactivate();
            }

            ClientManager.LeaveCriticalArea();
        }
    }
}