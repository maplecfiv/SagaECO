using SagaDB.Actor;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._2_1_Class.Trader_贸易商____mer
{
    /// <summary>
    ///     賞金（チップ）[接續技能]
    /// </summary>
    public class PetHitupSelf : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 10000 - 1000 * level;
            var skill = new DefaultBuff(args.skill, dActor, "PetHitupSelf", lifetime);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(dActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            int level = skill.skill.Level;
            //近命中
            var hit_melee_add = actor.Status.hit_melee * 8 * level;
            if (skill.Variable.ContainsKey("PetAtkupSelf_hit_melee"))
                skill.Variable.Remove("PetAtkupSelf_hit_melee");
            skill.Variable.Add("PetAtkupSelf_hit_melee", hit_melee_add);
            actor.Status.hit_melee_skill += (short)hit_melee_add;
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            //近命中
            actor.Status.hit_melee_skill -= (short)skill.Variable["PetAtkupSelf_hit_melee"];
        }

        #endregion
    }
}