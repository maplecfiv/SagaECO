using SagaDB.Actor;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Machinery
{
    /// <summary>
    ///     提升機器人的HP（ロボットHP上昇）
    /// </summary>
    public class RobotHpUp : ISkill
    {
        #region ISkill Members

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
                var skill = new DefaultPassiveSkill(args.skill, sActor, "RobotHpUp", active);
                skill.OnAdditionStart += StartEventHandler;
                skill.OnAdditionEnd += EndEventHandler;
                SkillHandler.ApplyAddition(sActor, skill);
            }
        }

        private void StartEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            actor.MaxHP += (uint)(0.07f + 0.03f * skill.skill.Level);
        }

        private void EndEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            actor.MaxHP -= (uint)(1.07f + 0.03f * skill.skill.Level);
        }

        #endregion
    }
}