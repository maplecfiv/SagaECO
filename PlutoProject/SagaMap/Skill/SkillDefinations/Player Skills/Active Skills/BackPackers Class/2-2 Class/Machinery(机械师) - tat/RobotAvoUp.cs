using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._2_2_Class.Machinery_机械师____tat
{
    /// <summary>
    ///     提升機器人的迴避率（ロボット回避力上昇）
    /// </summary>
    public class RobotAvoUp : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            var pet = SkillHandler.Instance.GetPet(sActor);
            if (pet == null) return -54; //需回傳"需裝備寵物"
            if (SkillHandler.Instance.CheckMobType(pet, "MACHINE_RIDE_ROBOT"))
                //return 0;
                return -54; //封印
            return -54; //需回傳"需裝備寵物"
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 1000 * level;
            var skill = new DefaultBuff(args.skill, dActor, "RobotAvoUp", lifetime);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(dActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            int level = skill.skill.Level;
            //近戰迴避
            var avoid_melee_add = 3 + 2 * level;
            if (skill.Variable.ContainsKey("RobotAvoUp_avoid_melee"))
                skill.Variable.Remove("RobotAvoUp_avoid_melee");
            skill.Variable.Add("RobotAvoUp_avoid_melee", avoid_melee_add);
            actor.Status.avoid_melee_skill += (short)avoid_melee_add;

            //遠距迴避
            var avoid_ranged_add = 3 + 2 * level;
            if (skill.Variable.ContainsKey("RobotAvoUp_avoid_ranged"))
                skill.Variable.Remove("RobotAvoUp_avoid_ranged");
            skill.Variable.Add("RobotAvoUp_avoid_ranged", avoid_ranged_add);
            actor.Status.avoid_ranged_skill += (short)avoid_ranged_add;
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            //近戰迴避
            actor.Status.avoid_melee_skill -= (short)skill.Variable["RobotAvoUp_avoid_melee"];

            //遠距迴避
            actor.Status.avoid_ranged_skill -= (short)skill.Variable["RobotAvoUp_avoid_ranged"];
        }

        #endregion
    }
}