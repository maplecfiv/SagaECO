using SagaDB.Actor;
using SagaLib;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Cabalist
{
    /// <summary>
    ///     吸收靈魂（ソウルスティール）
    /// </summary>
    public class SoulSteal : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            if (SkillHandler.Instance.CheckValidAttackTarget(sActor, dActor)) return 0;

            return -14;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 1.0f;
            SkillHandler.Instance.PhysicalAttack(sActor, dActor, args, Elements.Dark, factor);
            if (sActor.type == ActorType.PC)
            {
                var pc = (ActorPC)sActor;
                pc = SkillHandler.Instance.GetPossesionedActor((ActorPC)sActor);
                //吸收MP恢復
                var mp_recovery = 0;
                foreach (var hp in args.hp) mp_recovery += hp;
                var mp_add = (uint)(mp_recovery * (0.15f + 0.03f * level));
                pc.MP += mp_add;
                if (pc.MP > pc.MaxMP) pc.MP = pc.MaxMP;
                SkillHandler.Instance.ShowVessel(pc, 0, -(int)mp_add);
                var map = MapManager.Instance.GetMap(pc.MapID);
                map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.HPMPSP_UPDATE, null, pc, true);
            }
        }

        #endregion
    }
}