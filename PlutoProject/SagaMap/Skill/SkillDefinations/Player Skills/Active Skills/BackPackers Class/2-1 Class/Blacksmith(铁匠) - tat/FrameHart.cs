using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Blacksmith
{
    /// <summary>
    ///     火焰之心（フレイムハート）
    /// </summary>
    public class FrameHart : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 10000 + 10000 * level;
            var realDActor = SkillHandler.Instance.GetPossesionedActor((ActorPC)sActor);
            var skill = new DefaultBuff(args.skill, realDActor, "FrameHart", lifetime);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(realDActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            if (skill.Variable.ContainsKey("FrameHart"))
                skill.Variable.Remove("FrameHart");

            skill.Variable.Add("FrameHart", 105 + skill.skill.Level * 10);

            actor.Buff.FlameHart = true;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            if (skill.Variable.ContainsKey("FrameHart"))
                skill.Variable.Remove("FrameHart");

            actor.Buff.FlameHart = false;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        #endregion
    }
}