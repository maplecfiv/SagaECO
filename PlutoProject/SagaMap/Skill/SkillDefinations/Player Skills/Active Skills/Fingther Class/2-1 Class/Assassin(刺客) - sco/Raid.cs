using System.Collections.Generic;
using SagaLib;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._2_1_Class.Assassin_刺客____sco
{
    /// <summary>
    ///     奇襲（奇襲）
    /// </summary>
    public class Raid : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            if (sActor.Status.Additions.ContainsKey("Hiding") ||
                sActor.Status.Additions.ContainsKey("Cloaking")) return 0;

            return -12;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factors = new[] { 0, 6.5f, 7.0f, 7.25f, 7.5f, 8.0f };
            var factor = factors[level];
            var crirate = 30 + 5 * level;
            var affected = new List<Actor>();
            affected.Add(dActor);
            SkillHandler.Instance.PhysicalAttack(sActor, affected, args, SkillHandler.DefType.Def, Elements.Neutral, 0,
                factor, false, 0, false, 0, crirate);
            var rate = 25 + 5 * level;
            var lifetime = 5000 + 1000 * level;
            if (SkillHandler.Instance.CanAdditionApply(sActor, dActor, SkillHandler.DefaultAdditions.Stun, rate))
            {
                var skill = new Stun(args.skill, dActor, lifetime);
                SkillHandler.ApplyAddition(dActor, skill);
            }
        }

        //#endregion
    }
}