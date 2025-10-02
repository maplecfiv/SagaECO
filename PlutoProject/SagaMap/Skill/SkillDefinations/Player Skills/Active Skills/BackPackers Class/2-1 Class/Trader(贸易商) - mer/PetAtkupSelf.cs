using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._2_1_Class.Trader_贸易商____mer
{
    /// <summary>
    ///     賞金（チップ）[接續技能]
    /// </summary>
    public class PetAtkupSelf : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 10000 - 1000 * level;
            var skill = new DefaultBuff(args.skill, dActor, "PetAtkupSelf", lifetime);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(dActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            int level = skill.skill.Level;
            //最大攻擊
            var max_atk1_add = 10 * level;
            if (skill.Variable.ContainsKey("PetAtkupSelf_max_atk1"))
                skill.Variable.Remove("PetAtkupSelf_max_atk1");
            skill.Variable.Add("PetAtkupSelf_max_atk1", max_atk1_add);
            actor.Status.max_atk1_skill += (short)max_atk1_add;

            //最大攻擊
            var max_atk2_add = 10 * level;
            if (skill.Variable.ContainsKey("PetAtkupSelf_max_atk2"))
                skill.Variable.Remove("PetAtkupSelf_max_atk2");
            skill.Variable.Add("PetAtkupSelf_max_atk2", max_atk2_add);
            actor.Status.max_atk2_skill += (short)max_atk2_add;

            //最大攻擊
            var max_atk3_add = 10 * level;
            if (skill.Variable.ContainsKey("PetAtkupSelf_max_atk3"))
                skill.Variable.Remove("PetAtkupSelf_max_atk3");
            skill.Variable.Add("PetAtkupSelf_max_atk3", max_atk3_add);
            actor.Status.max_atk3_skill += (short)max_atk3_add;
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            //最大攻擊
            actor.Status.max_atk1_skill -= (short)skill.Variable["PetAtkupSelf_max_atk1"];

            //最大攻擊
            actor.Status.max_atk2_skill -= (short)skill.Variable["PetAtkupSelf_max_atk2"];

            //最大攻擊
            actor.Status.max_atk3_skill -= (short)skill.Variable["PetAtkupSelf_max_atk3"];
        }

        //#endregion
    }
}