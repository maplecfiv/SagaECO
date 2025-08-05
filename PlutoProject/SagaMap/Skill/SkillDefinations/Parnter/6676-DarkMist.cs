using SagaDB.Actor;
using SagaLib;
using SagaMap.Manager;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Global
{
    /// <summary>
    ///     ダークミスト
    /// </summary>
    public class DarkMist : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 1.2f;
            var skill = new DefaultBuff(args.skill, dActor, "DarkMist", 30000);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(dActor, skill);
            SkillHandler.Instance.MagicAttack(sActor, dActor, args, Elements.Dark, factor);
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.Status.hit_melee_skill -= 50;
            actor.Status.hit_ranged_skill -= 50;
            actor.Buff.ShortHitDown = true;
            actor.Buff.LongHitDown = true;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.Status.hit_melee_skill += 50;
            actor.Status.hit_ranged_skill += 50;
            actor.Buff.ShortHitDown = true;
            actor.Buff.LongHitDown = true;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        #endregion
    }
}