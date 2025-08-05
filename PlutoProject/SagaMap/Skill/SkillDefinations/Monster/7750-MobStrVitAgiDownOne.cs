using SagaDB.Actor;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Monster
{
    /// <summary>
    ///     你已經弱了
    /// </summary>
    public class MobStrVitAgiDownOne : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var rate = 30;
            if (SkillHandler.Instance.CanAdditionApply(sActor, dActor, "MobStrVitAgiDownOne", rate))
            {
                var lifetime = 24000;
                var skill = new DefaultBuff(args.skill, dActor, "MobStrVitAgiDownOne", lifetime);
                skill.OnAdditionStart += StartEventHandler;
                skill.OnAdditionEnd += EndEventHandler;
                SkillHandler.ApplyAddition(dActor, skill);
            }
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            //STR
            var str_add = -11;
            if (skill.Variable.ContainsKey("MobStrVitAgiDownOne_str"))
                skill.Variable.Remove("MobStrVitAgiDownOne_str");
            skill.Variable.Add("MobStrVitAgiDownOne_str", str_add);
            actor.Status.str_skill += (short)str_add;

            //AGI
            var agi_add = -18;
            if (skill.Variable.ContainsKey("MobStrVitAgiDownOne_agi"))
                skill.Variable.Remove("MobStrVitAgiDownOne_agi");
            skill.Variable.Add("MobStrVitAgiDownOne_agi", agi_add);
            actor.Status.agi_skill += (short)agi_add;

            //VIT
            var vit_add = -12;
            if (skill.Variable.ContainsKey("MobStrVitAgiDownOne_vit"))
                skill.Variable.Remove("MobStrVitAgiDownOne_vit");
            skill.Variable.Add("MobStrVitAgiDownOne_vit", vit_add);
            actor.Status.vit_skill += (short)vit_add;
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            //STR
            actor.Status.str_skill -= (short)skill.Variable["MobStrVitAgiDownOne_str"];

            //AGI
            actor.Status.agi_skill -= (short)skill.Variable["MobStrVitAgiDownOne_agi"];

            //VIT
            actor.Status.vit_skill -= (short)skill.Variable["MobStrVitAgiDownOne_vit"];
        }

        #endregion
    }
}