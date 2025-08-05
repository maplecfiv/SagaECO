using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.SunFlowerAdditions
{
    public class ModeChange : ISkill
    {
        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            SkillHandler.RemoveAddition(dActor, "Transition");
            var skill = new DefaultPassiveSkill(args.skill, dActor, "ModeChange", true);
            skill.OnAdditionStart += skill_OnAdditionStart;
            skill.OnAdditionEnd += skill_OnAdditionEnd;
            SkillHandler.ApplyAddition(dActor, skill);
        }

        private void skill_OnAdditionStart(Actor actor, DefaultPassiveSkill skill)
        {
            actor.Buff.三转ATK与MATK互换 = true;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void skill_OnAdditionEnd(Actor actor, DefaultPassiveSkill skill)
        {
            actor.Buff.三转ATK与MATK互换 = false;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }
    }
}