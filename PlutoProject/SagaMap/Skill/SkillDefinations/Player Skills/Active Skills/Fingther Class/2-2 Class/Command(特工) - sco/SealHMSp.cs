using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._2_2_Class.Command_特工____sco
{
    /// <summary>
    ///     要害突刺（リカバリーブロック）
    /// </summary>
    public class SealHMSp : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            if (SkillHandler.Instance.CheckValidAttackTarget(sActor, dActor)) return 0;

            return -14;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 1000 * level;
            var rate = 25 * level;
            if (SagaLib.Global.Random.Next(0, 99) < rate)
            {
                var skill = new DefaultBuff(args.skill, dActor, "SealHMSp", lifetime);
                skill.OnAdditionStart += StartEventHandler;
                skill.OnAdditionEnd += EndEventHandler;
                SkillHandler.ApplyAddition(dActor, skill);
            }
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.Buff.NoRegen = true;
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.Buff.NoRegen = false;
        }

        //#endregion
    }
}