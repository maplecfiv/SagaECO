using SagaDB.Actor;
using SagaLib;
using SagaMap.ActorEventHandlers;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._1_0_Class.Votes_祭司_
{
    /// <summary>
    ///     ヒーリング
    /// </summary>
    public class Healing : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            if (pc.Status.Additions.ContainsKey("Spell")) return -7;
            if (dActor.type == ActorType.MOB)
            {
                var eh = (MobEventHandler)dActor.e;
                if (eh.AI.Mode.Symbol)
                    return -14;
            }

            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factors = new[] { 0, -1.0f, -1.7f, -2.3f, -2.7f, -3.0f, -100f };
            var factor = factors[level];
            factor += sActor.Status.Cardinal_Rank;
            if (dActor.type == ActorType.MOB)
            {
                var m = (ActorMob)dActor;
                if (m.Status.undead)
                    factor = -factors[level];
            }

            SkillHandler.Instance.MagicAttack(sActor, dActor, args, SkillHandler.DefType.IgnoreAll, Elements.Holy,
                factor);
        }

        #endregion
    }
}