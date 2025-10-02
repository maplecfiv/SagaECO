using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Monster
{
    public class DelayCancel : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            args.dActor = 0;
            var life = 20000;
            var skill = new DefaultBuff(args.skill, dActor, "DelayCancel", life);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(dActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            //actor.Status.aspd_skill_perc += (float)(0.2f + 0.3f * skill.skill.Level);应该是魔物专用迅捷?
            actor.Status.aspd_skill_perc += 1.7f;
            actor.Buff.DelayCancel = true;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            //actor.Status.aspd_skill_perc -= (float)(0.2f + 0.3f * skill.skill.Level);
            actor.Status.aspd_skill_perc -= 1.7f;
            actor.Buff.DelayCancel = false;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        //#endregion
    }
}