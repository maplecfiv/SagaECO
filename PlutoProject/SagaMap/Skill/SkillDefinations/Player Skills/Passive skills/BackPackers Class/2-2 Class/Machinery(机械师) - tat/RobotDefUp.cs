using SagaDB.Actor;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.BackPackers_Class._2_2_Class.Machinery_机械师____tat
{
    /// <summary>
    ///     提升機器人的防禦力（ロボット防御力上昇）
    /// </summary>
    public class RobotDefUp : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var active = false;
            var pet = SkillHandler.Instance.GetPet(sActor);
            if (pet != null)
            {
                if (SkillHandler.Instance.CheckMobType(pet, "MACHINE_RIDE_ROBOT")) active = true;
                var skill = new DefaultPassiveSkill(args.skill, sActor, "RobotDefUp", active);
                skill.OnAdditionStart += StartEventHandler;
                skill.OnAdditionEnd += EndEventHandler;
                SkillHandler.ApplyAddition(sActor, skill);
            }
        }

        private void StartEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            int level = skill.skill.Level;
            //右防禦
            var def_add_add = (int)(actor.Status.def_add * (0.08f + 0.02f * level));
            if (skill.Variable.ContainsKey("RobotDefUp_def_add"))
                skill.Variable.Remove("RobotDefUp_def_add");
            skill.Variable.Add("RobotDefUp_def_add", def_add_add);
            actor.Status.def_add_skill += (short)def_add_add;

            //右魔防
            var mdef_add_add = (int)(actor.Status.mdef_add * (0.08f + 0.02f * level));
            if (skill.Variable.ContainsKey("RobotDefUp_mdef_add"))
                skill.Variable.Remove("RobotDefUp_mdef_add");
            skill.Variable.Add("RobotDefUp_mdef_add", mdef_add_add);
            actor.Status.mdef_add_skill += (short)mdef_add_add;
        }

        private void EndEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            //右防禦
            actor.Status.def_add_skill -= (short)skill.Variable["RobotDefUp_def_add"];

            //右魔防
            actor.Status.mdef_add_skill -= (short)skill.Variable["RobotDefUp_mdef_add"];
        }

        //#endregion
    }
}