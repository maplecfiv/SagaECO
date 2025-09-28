using System.Collections.Generic;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._2_1_Class.Alchemist_炼金术士____far
{
    /// <summary>
    ///     木材分析
    /// </summary>
    public class TreeAnalysis : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            if (dActor.type == ActorType.MOB)
            {
                var types = new List<MobType>();
                types.Add(MobType.TREE);
                types.Add(MobType.TREE_MATERIAL);

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