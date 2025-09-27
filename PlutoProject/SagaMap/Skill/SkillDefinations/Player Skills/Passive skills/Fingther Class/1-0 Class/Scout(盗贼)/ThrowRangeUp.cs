using SagaDB.Actor;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.Fingther_Class._1_0_Class.Scout_盗贼_
{
    /// <summary>
    ///     投擲距離提升（投擲射程上昇）
    /// </summary>
    public class ThrowRangeUp : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var active = true;
            /*
             * Lv   1 2 3
             * 射程 1 2 3
             *
             * 投擲武器最高射程只能提升到 6
             * 有些投擲武器射程為 4 ，技能LV3時射程也不會超過 6
             *
             */
            var skill = new DefaultPassiveSkill(args.skill, sActor, "ThrowRangeUp", active);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(sActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
        }

        private void EndEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
        }

        #endregion
    }
}