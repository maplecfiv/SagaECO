using System.Collections.Generic;
using SagaDB.Actor;
using SagaDB.Mob;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Explorer
{
    /// <summary>
    ///     昆蟲分析
    /// </summary>
    public class InsectAnalysis : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            if (dActor.type == ActorType.MOB)
            {
                var types = new List<MobType>();
                types.Add(MobType.INSECT);
                types.Add(MobType.INSECT_BOSS);
                types.Add(MobType.INSECT_BOSS_NOTPTDROPRANGE);
                types.Add(MobType.INSECT_BOSS_SKILL);
                types.Add(MobType.INSECT_NOTOUCH);
                types.Add(MobType.INSECT_NOTPTDROPRANGE);
                types.Add(MobType.INSECT_RIDE);
                types.Add(MobType.INSECT_SKILL);
                types.Add(MobType.INSECT_UNITE);

                var mob = (ActorMob)dActor;
                if (types.Contains(mob.BaseData.mobType)) return 0;
            }

            return -4;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var skill = new Analysis(args.skill, dActor);
            SkillHandler.ApplyAddition(dActor, skill);
        }

        #endregion
    }
}