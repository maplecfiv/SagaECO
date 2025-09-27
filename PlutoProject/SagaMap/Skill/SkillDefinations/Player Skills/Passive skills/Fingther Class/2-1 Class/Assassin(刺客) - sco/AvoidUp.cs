using SagaDB.Actor;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.Fingther_Class._2_1_Class.Assassin_刺客____sco
{
    /// <summary>
    ///     魅影步法（フットワーク）
    /// </summary>
    public class AvoidUp : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var skill = new DefaultPassiveSkill(args.skill, sActor, "AvoidUp", true);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(sActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            int level = skill.skill.Level;
            //近戰迴避
            var avoid_melee_adds = new[] { 0f, 0.05f, 0.07f, 0.11f }[level];
            var avoid_melee_add = (int)(actor.Status.avoid_melee * avoid_melee_adds);
            if (skill.Variable.ContainsKey("AvoidUp_avoid_melee"))
                skill.Variable.Remove("AvoidUp_avoid_melee");
            skill.Variable.Add("AvoidUp_avoid_melee", avoid_melee_add);
            actor.Status.avoid_melee_skill += (short)avoid_melee_add;

            //遠距迴避
            var avoid_ranged_adds = new[] { 0f, 0.06f, 0.08f, 0.10f }[level];
            var avoid_ranged_add = (int)(actor.Status.avoid_ranged * avoid_ranged_adds);
            if (skill.Variable.ContainsKey("AvoidUp_avoid_ranged"))
                skill.Variable.Remove("AvoidUp_avoid_ranged");
            skill.Variable.Add("AvoidUp_avoid_ranged", avoid_ranged_add);
            actor.Status.avoid_ranged_skill += (short)avoid_ranged_add;
        }

        private void EndEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            //近戰迴避
            actor.Status.avoid_melee_skill -= (short)skill.Variable["AvoidUp_avoid_melee"];
            //遠距迴避
            actor.Status.avoid_ranged_skill -= (short)skill.Variable["AvoidUp_avoid_ranged"];
        }

        #endregion
    }
}