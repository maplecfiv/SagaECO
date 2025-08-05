using SagaDB.Actor;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Cardinal
{
    /// <summary>
    ///     キュアアンデッド
    /// </summary>
    public class CureTheUndead : ISkill
    {
        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var reset = false;
            if (dActor.Buff.Zombie)
            {
                dActor.Buff.Zombie = false;
                dActor.Status.Additions["Zombie"].AdditionEnd();
                reset = true;
            }

            if (dActor.Buff.Reborn)
            {
                dActor.Buff.Reborn = false;
                dActor.Status.Additions["Reborn"].AdditionEnd();
                reset = true;
            }

            if (dActor.Buff.Undead)
            {
                dActor.Buff.Undead = false;
                dActor.Status.Additions["Undead"].AdditionEnd();
                reset = true;
            }

            if (reset)
                MapManager.Instance.GetMap(dActor.MapID)
                    .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, dActor, true);
        }
    }
}