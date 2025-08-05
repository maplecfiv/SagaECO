using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Hawkeye
{
    public class WithinWeeks : ISkill
    {
        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 120000;
            var skill = new DefaultBuff(args.skill, dActor, "WithinWeeks", lifetime);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(dActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            if (actor.type == ActorType.PC)
            {
                var pc = (ActorPC)actor;
                pc.CInt["WithinWeeksLevel"] = skill.skill.Level;
            }

            actor.Buff.三转ウィークネスショット = true;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            if (actor.type == ActorType.PC)
            {
                var pc = (ActorPC)actor;
                pc.CInt["WithinWeeksLevel"] = 0;
            }

            actor.Buff.三转ウィークネスショット = false;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }
    }
}