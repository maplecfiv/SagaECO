using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._2_1_Class.Striker_猎人____arc
{
    /// <summary>
    ///     斷腕（アームブレイク）
    /// </summary>
    public class ArmBreak : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return SkillHandler.Instance.CheckPcBowAndArrow(sActor);
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            SkillHandler.Instance.PcArrowDown(sActor);
            var lifetime = 2000 + 2000 * level;
            var rate = 50 + 6 * level;
            var factor = 0.75f + 0.25f * level;
            //對首腦級魔物無效
            if (dActor.type == ActorType.MOB)
            {
                var dActorMob = (ActorMob)dActor;
                if (SkillHandler.Instance.isBossMob(dActorMob)) return;
            }

            SkillHandler.Instance.PhysicalAttack(sActor, dActor, args, sActor.WeaponElement, factor);
            if (SkillHandler.Instance.CanAdditionApply(sActor, dActor, "ArmBreak", rate))
            {
                var skill = new DefaultBuff(args.skill, dActor, "ArmBreak", lifetime);
                skill.OnAdditionStart += StartEventHandler;
                skill.OnAdditionEnd += EndEventHandler;
                SkillHandler.ApplyAddition(dActor, skill);
            }
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            //物理技能和攻擊無法使用
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
        }

        #endregion
    }
}