using SagaMap.Skill.Additions;
using SagaMap.Skill.SkillDefinations.Global.Active;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._2_2_Class.Command_特工____sco
{
    /// <summary>
    ///     窒息毒氣（チョーキングガス）
    /// </summary>
    public class ChokingGas : Trap
    {
        public ChokingGas()
            : base(true, 200, PosType.sActor)
        {
        }

        public override void BeforeProc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            LifeTime = 30000;
        }

        public override void ProcSkill(Actor sActor, Actor mActor, ActorSkill actor, SkillArg args, Map map, int level,
            float factor)
        {
            var rate = 35 + 10 * level;
            if (!mActor.Status.Additions.ContainsKey("Silence"))
                if (SkillHandler.Instance.CanAdditionApply(sActor, mActor, SkillHandler.DefaultAdditions.Silence, rate))
                {
                    var sk = new Silence(args.skill, mActor, 10000);
                    SkillHandler.ApplyAddition(mActor, sk);
                }
        }
    }
}