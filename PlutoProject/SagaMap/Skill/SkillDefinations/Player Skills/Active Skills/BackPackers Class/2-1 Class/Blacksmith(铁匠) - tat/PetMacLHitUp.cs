using SagaDB.Actor;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._2_1_Class.Blacksmith_铁匠____tat
{
    /// <summary>
    ///     平靜射擊（評定射撃）
    /// </summary>
    public class PetMacLHitUp : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 35000 - 5000 * level;
            var skill = new DefaultBuff(args.skill, dActor, "PetMacLHitUp", lifetime);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(dActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            int level = skill.skill.Level;
            //遠命中
            var hit_ranged_add = (int)(actor.Status.hit_ranged * (0.1f + 0.1f * level));
            if (skill.Variable.ContainsKey("PetMacLHitUp_hit_ranged"))
                skill.Variable.Remove("PetMacLHitUp_hit_ranged");
            skill.Variable.Add("PetMacLHitUp_hit_ranged", hit_ranged_add);
            actor.Status.hit_ranged_skill += (short)hit_ranged_add;
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            //遠命中
            actor.Status.hit_ranged_skill -= (short)skill.Variable["PetMacLHitUp_hit_ranged"];
        }

        //#endregion
    }
}