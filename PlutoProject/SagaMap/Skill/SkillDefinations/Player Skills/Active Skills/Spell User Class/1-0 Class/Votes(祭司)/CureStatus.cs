using SagaDB.Actor;

namespace SagaMap.Skill.SkillDefinations.Vates
{
    /// <summary>
    ///     解除異常狀態
    /// </summary>
    public class CureStatus : ISkill
    {
        private readonly string AdditionName;

        public CureStatus(string AdditionName)
        {
            this.AdditionName = AdditionName;
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

        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            RemoveAddition(dActor, AdditionName);
        }

        #endregion
    }
}