using SagaDB.Actor;
using SagaLib;

namespace SagaMap.Skill.SkillDefinations.Parnter
{
    /// <summary>
    ///     九尾の霊験☆復刻版
    /// </summary>
    public class TheRevelationOfKuo : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            var actor = (Actor)pc;
            var pet = (ActorPartner)actor;
            if (pet.HP / (float)pet.MaxHP < 0.75f || pet.Owner.HP / (float)pet.Owner.MaxHP < 0.75f) return 0;
            return -1;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factors = -2.8f;
            factors += sActor.Status.Cardinal_Rank;
            var rate = 30;
            var num = SagaLib.Global.Random.Next(1, 2);
            var pet = (ActorPartner)sActor;
            switch (num)
            {
                case 1:
                    dActor = pet;
                    break;
                case 2:
                    dActor = pet.Owner;
                    break;
            }

            SkillHandler.Instance.MagicAttack(sActor, dActor, args, SkillHandler.DefType.IgnoreAll, Elements.Holy,
                factors);
            var cure = false;
            if (SagaLib.Global.Random.Next(0, 99) < rate) cure = true;
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