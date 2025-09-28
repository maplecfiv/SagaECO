using System.Collections.Generic;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._2_1_Class.Elementaler_元素使____sha
{
    /// <summary>
    ///     精靈分析（精霊分析）
    /// </summary>
    public class ElementAnalysis : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            if (dActor.type == ActorType.MOB)
            {
                var types = new List<MobType>();
                types.Add(MobType.ELEMENT);
                types.Add(MobType.ELEMENT_BOSS_SKILL);
                types.Add(MobType.ELEMENT_MATERIAL_NOTOUCH_SKILL);
                types.Add(MobType.ELEMENT_NOTOUCH);
                types.Add(MobType.ELEMENT_NOTOUCH_SKILL);
                types.Add(MobType.ELEMENT_NOTPTDROPRANGE);
                types.Add(MobType.ELEMENT_SKILL);
                types.Add(MobType.ELEMENT_SKILL_BOSS);

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