using System.Collections.Generic;
using SagaDB.Actor;
using SagaLib;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._2_2_Class.BountyHunter_赏金猎人____swm
{
    /// <summary>
    ///     刀劍亂舞（リミテイションエッジ）
    /// </summary>
    public class EdgedSlash : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            if (SkillHandler.Instance.CheckValidAttackTarget(sActor, dActor)) return 0;

            return -14;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 1.0f + 0.5f * level;
            var affected = new List<Actor>();
            affected.Add(dActor);
            SkillHandler.Instance.PhysicalAttack(sActor, affected, args, SkillHandler.DefType.IgnoreRight,
                Elements.Neutral, 0, factor, false);
        }

        //#endregion
    }
}