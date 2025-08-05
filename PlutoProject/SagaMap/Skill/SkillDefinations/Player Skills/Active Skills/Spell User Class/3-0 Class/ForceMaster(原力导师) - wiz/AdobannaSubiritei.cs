using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.ForceMaster
{
    /// <summary>
    ///     アドバンスアビリティー
    /// </summary>
    public class AdobannaSubiritei : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 100000 * level;
            var skill = new DefaultBuff(args.skill, dActor, "AdobannaSubiritei", lifetime);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(dActor, skill);
            if (dActor.ActorID == sActor.ActorID)
            {
                Map map = map = MapManager.Instance.GetMap(sActor.MapID);
                var arg2 = new EffectArg();
                arg2.effectID = 4354;
                arg2.actorID = sActor.ActorID;
                map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SHOW_EFFECT, arg2, sActor, true);
            }
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            var MinAttack = new[] { 0, 0.7f, 0.8f, 0.9f };

            //最小攻擊
            var min_matk_add = (int)((actor.Status.max_matk - actor.Status.min_matk) * MinAttack[skill.skill.Level]);
            if (min_matk_add < 0)
                min_matk_add = 0;
            if (skill.Variable.ContainsKey("AdobannaSubiritei"))
                skill.Variable.Remove("AdobannaSubiritei");
            skill.Variable.Add("AdobannaSubiritei", min_matk_add);
            actor.Status.min_matk_skill += (short)min_matk_add;

            actor.Buff.三转アドバンスアビリテイー = true;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            //最小攻擊
            actor.Status.min_matk_skill -= (short)skill.Variable["AdobannaSubiritei"];

            actor.Buff.三转アドバンスアビリテイー = false;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        #endregion
    }
}