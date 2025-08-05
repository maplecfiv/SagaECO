using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Monster
{
    /// <summary>
    ///     科學祝福
    /// </summary>
    public class MobHolyfeather : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 12000;
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var affected = map.GetActorsArea(sActor, 200, true);
            foreach (var act in affected)
                if (act.type == ActorType.MOB)
                {
                    var skill = new DefaultBuff(args.skill, act, "MobHolyfeather", lifetime, 3000);
                    skill.OnAdditionStart += StartEvent;
                    skill.OnAdditionEnd += EndEvent;
                    skill.OnUpdate += TimerUpdate;
                    SkillHandler.ApplyAddition(act, skill);
                }
        }

        private void StartEvent(Actor actor, DefaultBuff skill)
        {
            var map = MapManager.Instance.GetMap(actor.MapID);
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEvent(Actor actor, DefaultBuff skill)
        {
            var map = MapManager.Instance.GetMap(actor.MapID);
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void TimerUpdate(Actor actor, DefaultBuff skill)
        {
            var map = MapManager.Instance.GetMap(actor.MapID);
            uint hpadd = 800;
            if (!actor.Buff.NoRegen)
                actor.HP += hpadd;
            if (actor.HP > actor.MaxHP) actor.HP = actor.MaxHP;
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.HPMPSP_UPDATE, null, actor, true);
        }

        #endregion
    }
}