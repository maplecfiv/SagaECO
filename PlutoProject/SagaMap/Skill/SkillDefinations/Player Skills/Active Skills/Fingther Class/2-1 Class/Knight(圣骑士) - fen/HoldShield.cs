using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._2_1_Class.Knight_圣骑士____fen
{
    /// <summary>
    ///     盾屏障（ホールドシールド）
    /// </summary>
    public class HoldShield : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            //只能在對方背靠牆壁時才可使用
            //使用條件：盾
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 1000 * level;
            var skills = new Stiff(args.skill, sActor, lifetime);
            SkillHandler.ApplyAddition(sActor, skills);
            var skilld = new Stiff(args.skill, dActor, lifetime);
            SkillHandler.ApplyAddition(dActor, skills);
        }

        #endregion
    }
}