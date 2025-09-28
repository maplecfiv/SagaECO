using System.Collections.Generic;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._2_1_Class.Explorer_探险家____rag
{
    /// <summary>
    ///     鳥類分析
    /// </summary>
    public class BirdAnalysis : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            if (dActor.type == ActorType.MOB)
            {
                var types = new List<MobType>();
                types.Add(MobType.BIRD);
                types.Add(MobType.BIRD_BOSS);
                types.Add(MobType.BIRD_BOSS_SKILL);
                types.Add(MobType.BIRD_NOTOUCH);
                types.Add(MobType.BIRD_SPBOSS_SKILL);
                types.Add(MobType.BIRD_UNITE);
                types.Add(MobType.BIRD);
                types.Add(MobType.BIRD_BOSS);
                types.Add(MobType.BIRD_BOSS_SKILL);
                types.Add(MobType.BIRD_NOTOUCH);
                types.Add(MobType.BIRD_SPBOSS_SKILL);
                types.Add(MobType.BIRD_UNITE);

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