using SagaDB.Actor;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._2_2_Class.Command_特工____sco
{
    /// <summary>
    ///     初級舞蹈（デッドリーアタック）
    /// </summary>
    public class WildDance : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 0.5f + 1.0f * level;
            var rate = 20 * level;
            var lifetime = 7000;
            args.argType = SkillArg.ArgType.Attack;
            args.type = ATTACK_TYPE.STAB;
            SkillHandler.Instance.PhysicalAttack(sActor, dActor, args, sActor.WeaponElement, factor);
            if (SagaLib.Global.Random.Next(0, 99) < rate)
            {
                var skill1 = new Stiff(args.skill, dActor, lifetime);
                SkillHandler.ApplyAddition(dActor, skill1);
            }
        }

        //#endregion
    }
}