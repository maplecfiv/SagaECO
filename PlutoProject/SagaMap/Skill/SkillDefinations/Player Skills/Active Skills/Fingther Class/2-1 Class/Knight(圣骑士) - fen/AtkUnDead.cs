using SagaDB.Actor;
using SagaDB.Mob;

namespace SagaMap.Skill.SkillDefinations.Knight
{
    public class AtkUnDead : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            if (SkillHandler.Instance.CheckValidAttackTarget(sActor, dActor)) return 0;

            return -14;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 2.0f;
            if (dActor is ActorMob)
            {
                var dActorMob = (ActorMob)dActor;
                if (dActorMob.BaseData.mobType == MobType.UNDEAD)
                    //加成
                    factor = 4.0f;
            }

            factor = factor + 0.2f * level;
            SkillHandler.Instance.PhysicalAttack(sActor, dActor, args, sActor.WeaponElement, factor);
        }

        #endregion
    }
}