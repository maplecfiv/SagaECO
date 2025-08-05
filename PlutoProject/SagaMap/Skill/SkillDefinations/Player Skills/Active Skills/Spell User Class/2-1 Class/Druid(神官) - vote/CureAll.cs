using SagaDB.Actor;
using SagaLib;
using SagaMap.ActorEventHandlers;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._2_1_Class.Druid_神官____vote
{
    /// <summary>
    ///     女神的加護（アレス）
    /// </summary>
    public class CureAll : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
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
            float[] factor = { -0f, -1.0f, -1.0f, -1.0f, -1.7f, -1.7f, -2.3f, -2.3f, -2.7f, -2.7f, -3.0f };
            var factors = factor[level];
            factors += sActor.Status.Cardinal_Rank;
            int[] rate = { 92, 96, 96, 97, 98, 99, 99, 99, 99, 99 };
            SkillHandler.Instance.MagicAttack(sActor, dActor, args, SkillHandler.DefType.IgnoreAll, Elements.Holy,
                factors);
            var cure = false;
            if (SagaLib.Global.Random.Next(0, 99) < rate[level - 1]) cure = true;
            if (cure)
            {
                RemoveAddition(dActor, "Poison");
                RemoveAddition(dActor, "MoveSpeedDown");
                RemoveAddition(dActor, "MoveSpeedDown2");
                RemoveAddition(dActor, "Stone");
                RemoveAddition(dActor, "Silence");
                RemoveAddition(dActor, "Stun");
                RemoveAddition(dActor, "Sleep");
                RemoveAddition(dActor, "Frosen");
                RemoveAddition(dActor, "Confuse");
            }
        }

        public void RemoveAddition(Actor actor, string additionName)
        {
            if (actor.Status.Additions.ContainsKey(additionName))
            {
                var addition = actor.Status.Additions[additionName];
                actor.Status.Additions.Remove(additionName);
                if (addition.Activated) addition.AdditionEnd();
                addition.Activated = false;
            }
        }

        #endregion
    }
}