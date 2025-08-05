using SagaDB.Actor;
using SagaLib;
using SagaMap.Manager;
using SagaMap.Network.Client;

namespace SagaMap.Skill.SkillDefinations.Knight
{
    /// <summary>
    ///     犧牲（サクリファイス）
    /// </summary>
    public class VicariouslyResu : ISkill
    {
        private void ProcessRevive(Actor dActor, byte level)
        {
            if (dActor.type == ActorType.PC)
            {
                var client = MapClient.FromActorPC((ActorPC)dActor);
                if (client.Character.Buff.Dead)
                {
                    client.Character.BattleStatus = 0;
                    client.SendChangeStatus();
                    client.Character.TInt["Revive"] = 5;
                    client.EventActivate(0xF1000000);
                    client.Character.HP = (uint)(client.Character.MaxHP * 0.1f * level);
                    var map = MapManager.Instance.GetMap(dActor.MapID);
                    map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.HPMPSP_UPDATE, null, dActor, true);
                }
            }
        }

        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            ProcessRevive(dActor, level);
            sActor.HP = 0;
            sActor.e.OnDie();
            args.affectedActors.Add(sActor);
            args.Init();
            args.flag[0] = AttackFlag.DIE;
        }

        #endregion
    }
}