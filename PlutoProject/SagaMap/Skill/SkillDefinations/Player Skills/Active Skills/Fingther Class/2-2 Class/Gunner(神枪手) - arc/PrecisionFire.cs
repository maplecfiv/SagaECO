using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._2_2_Class.Gunner_神枪手____arc
{
    /// <summary>
    ///     精密射擊（精密射撃）
    /// </summary>
    public class PrecisionFire : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 5000 * level;
            var skill = new DefaultBuff(args.skill, dActor, "PrecisionFire", lifetime);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(dActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            //一定時間內讓自己可以100％命中目標
            //效果期間不會發生致命攻擊，也不會發生AVOID、GUARD狀態
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
        }

        #endregion
    }
}