using System.Linq;
using SagaDB.Actor;
using SagaLib;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Event
{
    /// <summary>
    ///     禮物 (來！這是我送的禮物！、啊啊…這個…、會接受吧？ )
    /// </summary>
    public class HpRecoveryMax : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = -9999.0f;
            SkillHandler.Instance.FixAttack(sActor, dActor, args, Elements.Neutral, factor);
            if (sActor.Slave.Count > 0)
            {
                var m = sActor.Slave.Last();
                var map = MapManager.Instance.GetMap(sActor.MapID);
                m.ClearTaskAddition();
                map.DeleteActor(m);
                sActor.Slave.Remove(m);
            }
        }

        #endregion
    }
}