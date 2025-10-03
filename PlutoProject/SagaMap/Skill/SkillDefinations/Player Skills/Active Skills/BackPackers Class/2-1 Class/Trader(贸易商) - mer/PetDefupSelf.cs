using SagaDB.Actor;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._2_1_Class.Trader_贸易商____mer
{
    /// <summary>
    ///     賞金（チップ）[接續技能]
    /// </summary>
    public class PetDefupSelf : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 10000 - 1000 * level;
            var skill = new DefaultBuff(args.skill, dActor, "PetDefupSelf", lifetime);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(dActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            int level = skill.skill.Level;
            //左防禦
            var def_add = actor.Status.def * 5 * level;
            if (skill.Variable.ContainsKey("PetAtkupSelf_def"))
                skill.Variable.Remove("PetAtkupSelf_def");
            skill.Variable.Add("PetAtkupSelf_def", def_add);
            actor.Status.def_skill += (short)def_add;

            //右防禦
            var def_add_add = actor.Status.def_add * 8 + 2 * level;
            if (skill.Variable.ContainsKey("PetAtkupSelf_def_add"))
                skill.Variable.Remove("PetAtkupSelf_def_add");
            skill.Variable.Add("PetAtkupSelf_def_add", def_add_add);
            actor.Status.def_add_skill += (short)def_add_add;

            //左魔防
            var mdef_add = actor.Status.mdef * 4 + level;
            if (skill.Variable.ContainsKey("PetAtkupSelf_mdef"))
                skill.Variable.Remove("PetAtkupSelf_mdef");
            skill.Variable.Add("PetAtkupSelf_mdef", mdef_add);
            actor.Status.mdef_skill += (short)mdef_add;

            //右魔防
            var mdef_add_add = actor.Status.mdef_add * 5 + 2 * level;
            if (skill.Variable.ContainsKey("PetAtkupSelf_mdef_add"))
                skill.Variable.Remove("PetAtkupSelf_mdef_add");
            skill.Variable.Add("PetAtkupSelf_mdef_add", mdef_add_add);
            actor.Status.mdef_add_skill += (short)mdef_add_add;
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            //左防禦
            actor.Status.def_skill -= (short)skill.Variable["PetAtkupSelf_def"];

            //右防禦
            actor.Status.def_add_skill -= (short)skill.Variable["PetAtkupSelf_def_add"];

            //左魔防
            actor.Status.mdef_skill -= (short)skill.Variable["PetAtkupSelf_mdef"];

            //右魔防
            actor.Status.mdef_add_skill -= (short)skill.Variable["PetAtkupSelf_mdef_add"];
        }

        //#endregion
    }
}