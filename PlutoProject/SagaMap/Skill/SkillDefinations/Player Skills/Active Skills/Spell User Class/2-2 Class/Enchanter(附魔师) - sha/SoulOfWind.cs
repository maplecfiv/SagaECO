using SagaDB.Actor;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._2_2_Class.Enchanter_附魔师____sha
{
    /// <summary>
    ///     神風勢力（ウィンドオーラ）
    /// </summary>
    public class SoulOfWind : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            foreach (var e in dActor.Status.Additions)
            {
                var AdditionName = e.Key;
                if (AdditionName.IndexOf("WeaponDC") >= 0 || AdditionName.IndexOf("PoisonReate") >= 0)
                    ExtendCancelTypeAddition(dActor, AdditionName, level);
            }
        }

        public void ExtendCancelTypeAddition(Actor actor, string additionName, byte level)
        {
            if (actor.Status.Additions.ContainsKey(additionName))
            {
                var addition = actor.Status.Additions[additionName];
                addition.TotalLifeTime += 5000 * level;
            }
        }

        #endregion
    }
}