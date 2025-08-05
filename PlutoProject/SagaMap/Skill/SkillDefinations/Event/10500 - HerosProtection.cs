using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Event
{
    /// <summary>
    ///     英雄の加護
    /// </summary>
    public class HerosProtection : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            if (sActor.Status.Additions.ContainsKey("HerosProtection"))
                return -30;
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 180000;
            var map = MapManager.Instance.GetMap(sActor.MapID);
            if (map == null)
                return;
            var targets = map.GetActorsArea(sActor, 250, true);
            foreach (var i in targets)
            {
                if (i.type != ActorType.PC)
                    continue;
                if (!i.Status.Additions.ContainsKey("HerosProtection"))
                {
                    var skill = new DefaultBuff(args.skill, i, "HerosProtection", lifetime);
                    skill.OnAdditionStart += StartEventHandler;
                    skill.OnAdditionEnd += EndEventHandler;
                    SkillHandler.ApplyAddition(i, skill);
                }
            }
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            var map = MapManager.Instance.GetMap(actor.MapID);

            actor.Buff.StateOfMonsterKillerChamp = true;
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            var map = MapManager.Instance.GetMap(actor.MapID);

            actor.Buff.StateOfMonsterKillerChamp = false;
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        #endregion
    }
}