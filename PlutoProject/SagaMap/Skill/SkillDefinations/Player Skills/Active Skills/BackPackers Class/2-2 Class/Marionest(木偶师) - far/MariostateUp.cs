using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._2_2_Class.Marionest_木偶师____far
{
    /// <summary>
    ///     木偶應援（フォーティファイ）
    /// </summary>
    public class MariostateUp : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var active = true;
            var skill = new DefaultPassiveSkill(args.skill, sActor, "MariostateUp", active);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(sActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            int level = skill.skill.Level;

            //最大魔攻
            var max_matk_add = (int)(actor.Status.max_matk * (0.05f * level));
            if (skill.Variable.ContainsKey("MariostateUp_max_matk"))
                skill.Variable.Remove("MariostateUp_max_matk");
            skill.Variable.Add("MariostateUp_max_matk", max_matk_add);
            actor.Status.max_matk_skill += (short)max_matk_add;


            //最小魔攻
            var min_matk_add = (int)(actor.Status.min_matk * (0.05f * level));
            if (skill.Variable.ContainsKey("MariostateUp_min_matk"))
                skill.Variable.Remove("MariostateUp_min_matk");
            skill.Variable.Add("MariostateUp_min_matk", min_matk_add);
            actor.Status.min_matk_skill += (short)min_matk_add;
        }

        private void EndEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            //最大魔攻
            actor.Status.max_matk_skill -= (short)skill.Variable["MariostateUp_max_matk"];

            //最小魔攻
            actor.Status.min_matk_skill -= (short)skill.Variable["MariostateUp_min_matk"];
        }

        //#endregion
    }
}