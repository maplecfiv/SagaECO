using System;
using SagaMap.Network.Client;

namespace SagaMap.Skill.Additions.被动BUFF
{
    /// <summary>
    ///     居合姿态
    /// </summary>
    public class AttackRhythm : DefaultBuff
    {
        public AttackRhythm(SagaDB.Skill.Skill skill, Actor actor, int lifetime)
            : base(skill, actor, "AttackRhythm", lifetime)
        {
            OnAdditionStart += StartEvent;
            OnAdditionEnd += EndEvent;
        }

        private void StartEvent(Actor actor, DefaultBuff skill)
        {
            if (actor.AttackRhythm < 10)
            {
                if (actor.AttackRhythm == 0)
                    MapClient.FromActorPC((ActorPC)actor).SendSystemMessage("进入进攻节奏！");
                actor.AttackRhythm++;
                if (actor.AttackRhythm == 10)
                    MapClient.FromActorPC((ActorPC)actor).SendSystemMessage("进攻节奏达到了最大层数！");
            }

            actor.Status.aspd_skill_perc += actor.AttackRhythm * 0.1f;
            MapClient.FromActorPC((ActorPC)actor).SendStatusExtend();
        }

        private void EndEvent(Actor actor, DefaultBuff skill)
        {
            actor.Status.aspd_skill_perc -= actor.AttackRhythm * 0.1f;

            if (skill.endTime < DateTime.Now)
            {
                actor.AttackRhythm = 0;
                MapClient.FromActorPC((ActorPC)actor).SendSystemMessage("效果消失了。");
                MapClient.FromActorPC((ActorPC)actor).SendStatusExtend();
            }
        }
    }
}