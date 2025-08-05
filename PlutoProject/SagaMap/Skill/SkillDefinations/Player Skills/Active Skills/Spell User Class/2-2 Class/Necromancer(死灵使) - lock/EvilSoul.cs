using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Necromancer
{
    /// <summary>
    ///     邪惡靈魂（イビルソウル）
    /// </summary>
    public class EvilSoul : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 10000 + 10000 * level;

            var realDActor = SkillHandler.Instance.GetPossesionedActor((ActorPC)sActor);
            var skill = new DefaultBuff(args.skill, realDActor, "EvilSoul", lifetime);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(realDActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            var factor = new[] { 0, 20, 40, 60, 80, 95 }[skill.skill.Level];
            if (skill.Variable.ContainsKey("EvilSoul"))
                skill.Variable.Remove("EvilSoul");
            skill.Variable.Add("EvilSoul", factor);
            actor.Buff.イビルソウル = true;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            if (skill.Variable.ContainsKey("EvilSoul"))
                skill.Variable.Remove("EvilSoul");
            actor.Buff.イビルソウル = false;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        #endregion
    }
}