using System.Collections.Generic;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._3_0_Class.
    Royaldealer_皇家贸易商____mer
{
    /// <summary>
    ///     ストレートフラッシュ
    /// </summary>
    internal class StraightFlush : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 1.0f + 0.1f * level;
            var number = 0;
            if (level == 5)
                number = 3;
            else
                number = 1;
            args.argType = SkillArg.ArgType.Attack;
            args.type = ATTACK_TYPE.STAB;
            var dest = new List<Actor>();
            for (var i = 0; i < number; i++)
                dest.Add(dActor);
            args.delayRate = 4.5f;
            SkillHandler.Instance.PhysicalAttack(sActor, dest, args, sActor.WeaponElement, factor);
            if (dActor.HP > 0)
                args.autoCast.Add(SkillHandler.Instance.CreateAutoCastInfo(2518, level, 2000));
        }

        //#endregion
    }
}