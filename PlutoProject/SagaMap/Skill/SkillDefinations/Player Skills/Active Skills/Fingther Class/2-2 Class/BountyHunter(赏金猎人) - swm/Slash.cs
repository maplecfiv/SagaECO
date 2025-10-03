using SagaDB.Actor;
using SagaLib;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._2_2_Class.BountyHunter_赏金猎人____swm
{
    /// <summary>
    ///     砍擊裝備系列 (スラッシュ)
    /// </summary>
    public abstract class Slash
    {
        //#region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            if (SkillHandler.Instance.CheckValidAttackTarget(pc, dActor)) return 0;

            return -14;
        }

        public void SkillProc(Actor sActor, Actor dActor, SkillArg args, byte level, PossessionPosition Position)
        {
            if (dActor.type == ActorType.PC)
            {
                var dePossessionRate = 10 + 20 * level;
                if (SagaLib.Global.Random.Next(0, 99) < dePossessionRate)
                {
                    var actor = (ActorPC)dActor;
                    SkillHandler.Instance.PossessionCancel(actor, Position);
                }
            }

            var factor = 0.8f + 0.2f * level;
            SkillHandler.Instance.PhysicalAttack(sActor, dActor, args, sActor.WeaponElement, factor);
        }

        //#endregion
    }
}