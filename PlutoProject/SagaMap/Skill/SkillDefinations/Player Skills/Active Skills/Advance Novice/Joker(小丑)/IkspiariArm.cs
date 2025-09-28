namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Advance_Novice.Joker_小丑_
{
    /// <summary>
    ///     イクスパンジアーム
    /// </summary>
    public class IkspiariArmusing : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }


        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var cure = false;
            if (SagaLib.Global.Random.Next(0, 99) < 70) cure = true;
            if (cure)
            {
                RemoveAddition(sActor, "Poison");
                RemoveAddition(sActor, "MoveSpeedDown");
                RemoveAddition(sActor, "MoveSpeedDown2");
                RemoveAddition(sActor, "Stone");
                RemoveAddition(sActor, "Silence");
                RemoveAddition(sActor, "Stun");
                RemoveAddition(sActor, "Sleep");
                RemoveAddition(sActor, "Frosen");
                RemoveAddition(sActor, "Confuse");
            }

            var factor = 1.4f + 0.3f * level;
            SkillHandler.Instance.PhysicalAttack(sActor, dActor, args, sActor.WeaponElement, factor);
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