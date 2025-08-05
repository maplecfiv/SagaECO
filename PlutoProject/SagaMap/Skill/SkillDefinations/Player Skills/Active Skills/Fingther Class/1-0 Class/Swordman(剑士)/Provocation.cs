using SagaDB.Actor;
using SagaMap.ActorEventHandlers;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._1_0_Class.Swordman_剑士_
{
    /// <summary>
    ///     挑釁
    /// </summary>
    public class Provocation : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            if (dActor.type == ActorType.MOB)
            {
                var eh = (MobEventHandler)dActor.e;
                if (eh.AI.Hate.ContainsKey(sActor.ActorID))
                    eh.AI.Hate[sActor.ActorID] += dActor.MaxHP * 5;
                else
                    eh.AI.Hate.Add(sActor.ActorID, dActor.MaxHP * 5);
            }
        }

        #endregion
    }
}