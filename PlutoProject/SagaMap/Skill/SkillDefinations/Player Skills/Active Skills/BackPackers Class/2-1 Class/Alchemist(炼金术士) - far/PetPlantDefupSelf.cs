using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._2_1_Class.Alchemist_炼金术士____far
{
    /// <summary>
    ///     皮膚強化（硬質化）
    /// </summary>
    public class PetPlantDefupSelf : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 90000;
            var skill = new DefaultBuff(args.skill, sActor, "PetPlantDefupSelf", lifetime);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(sActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            int level = skill.skill.Level;
            //左防禦
            var def_add = 2 + 3 * level;
            if (skill.Variable.ContainsKey("PetPlantDefupSelf_def"))
                skill.Variable.Remove("PetPlantDefupSelf_def");
            skill.Variable.Add("PetPlantDefupSelf_def", def_add);
            actor.Status.def_skill += (short)def_add;

            //右防禦
            var def_add_add = 2 + 3 * level;
            if (skill.Variable.ContainsKey("PetPlantDefupSelf_def_add"))
                skill.Variable.Remove("PetPlantDefupSelf_def_add");
            skill.Variable.Add("PetPlantDefupSelf_def_add", def_add_add);
            actor.Status.def_add_skill += (short)def_add_add;

            //左魔防
            var mdef_add = 2 + 3 * level;
            if (skill.Variable.ContainsKey("PetPlantDefupSelf_mdef"))
                skill.Variable.Remove("PetPlantDefupSelf_mdef");
            skill.Variable.Add("PetPlantDefupSelf_mdef", mdef_add);
            actor.Status.mdef_skill += (short)mdef_add;

            //右魔防
            var mdef_add_add = 2 + 3 * level;
            if (skill.Variable.ContainsKey("PetPlantDefupSelf_mdef_add"))
                skill.Variable.Remove("PetPlantDefupSelf_mdef_add");
            skill.Variable.Add("PetPlantDefupSelf_mdef_add", mdef_add_add);
            actor.Status.mdef_add_skill += (short)mdef_add_add;
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            //左防禦
            actor.Status.def_skill -= (short)skill.Variable["PetPlantDefupSelf_def"];

            //右防禦
            actor.Status.def_add_skill -= (short)skill.Variable["PetPlantDefupSelf_def_add"];

            //左魔防
            actor.Status.mdef_skill -= (short)skill.Variable["PetPlantDefupSelf_mdef"];

            //右魔防
            actor.Status.mdef_add_skill -= (short)skill.Variable["PetPlantDefupSelf_mdef_add"];
        }

        #endregion
    }
}