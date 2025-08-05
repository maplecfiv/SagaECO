using SagaDB.Actor;
using SagaLib;

namespace SagaMap.Skill.SkillDefinations.Event
{
    /// <summary>
    ///     カチカチのパン
    /// </summary>
    public class PanTick : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 999.0f;
            SkillHandler.Instance.FixAttack(sActor, dActor, args, Elements.Neutral, factor);
            dActor.MP += 999;
            if (dActor.MP > dActor.MaxMP) dActor.MP = dActor.MaxMP;
            dActor.SP += 0;
            if (dActor.SP > dActor.MaxSP) dActor.SP = dActor.MaxSP;
            args.Init();
            args.flag[0] = AttackFlag.HP_HEAL | AttackFlag.MP_HEAL | AttackFlag.SP_HEAL;
        }

        #endregion
    }
}