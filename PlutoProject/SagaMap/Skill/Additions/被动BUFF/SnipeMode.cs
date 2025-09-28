using SagaMap.Network.Client;
using SagaMap.PC;

namespace SagaMap.Skill.Additions.被动BUFF
{
    /// <summary>
    ///     居合姿态
    /// </summary>
    public class SnipeMode : DefaultBuff
    {
        public SnipeMode(SagaDB.Skill.Skill skill, Actor actor, int lifetime)
            : base(skill, actor, "狙击模式", lifetime, 5000)
        {
            OnAdditionStart += StartEvent;
            OnAdditionEnd += EndEvent;
            OnUpdate += TimerUpdate;
        }

        private void StartEvent(Actor actor, DefaultBuff skill)
        {
            if (actor.type == ActorType.PC)
            {
                var pc = (ActorPC)actor;
                StatusFactory.Instance.CalcStatus(pc);
                MapClient.FromActorPC(pc).SendRange();
            }
            /*Map map = Manager.MapManager.Instance.GetMap(actor.MapID);
            actor.Buff.狂戦士 = true;
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);*/
        }

        private void EndEvent(Actor actor, DefaultBuff skill)
        {
            if (actor.type == ActorType.PC)
            {
                if (actor.Status.Additions.ContainsKey("狙击模式射程"))
                {
                    actor.Status.Additions["狙击模式射程"].AdditionEnd();
                    actor.Status.Additions.Remove("狙击模式射程");
                }

                var pc = (ActorPC)actor;
                StatusFactory.Instance.CalcStatus(pc);
                MapClient.FromActorPC(pc).SendRange();
            }
        }

        private void TimerUpdate(Actor actor, DefaultBuff skill)
        {
            if (actor.EP >= 15)
            {
                actor.EP -= 15;
            }
            else
            {
                actor.EP = 0;
                AdditionEnd();
            }

            actor.e.OnHPMPSPUpdate(actor);
        }
    }
}