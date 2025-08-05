using System.Collections.Generic;
using SagaDB.Actor;
using SagaDB.Item;
using SagaMap.Manager;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Bard
{
    /// <summary>
    ///     演講（オラトリオ）
    /// </summary>
    public class ORaToRiO : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            if (SkillHandler.Instance.isEquipmentRight(sActor, ItemType.STRINGS) ||
                sActor.Inventory.GetContainer(ContainerType.RIGHT_HAND2).Count > 0) return 0;
            return -5;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 45000 + 15000 * level;
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var affected = map.GetActorsArea(sActor, 150, true);
            var realAffected = new List<Actor>();
            foreach (var act in affected)
                if (act.type == ActorType.PC || act.type == ActorType.PET || act.type == ActorType.PARTNER)
                {
                    var skill = new DefaultBuff(args.skill, act, "ORaToRiO", lifetime, 3000);
                    skill.OnAdditionStart += StartEventHandler;
                    skill.OnAdditionEnd += EndEventHandler;
                    skill.OnUpdate += TimerUpdate;
                    SkillHandler.ApplyAddition(act, skill);
                }

            SkillHandler.Instance.ShowEffectByActor(sActor, 5316);
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.Buff.Oritorio = true;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.Buff.Oritorio = false;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void TimerUpdate(Actor actor, DefaultBuff skill)
        {
            if (actor.Buff.NoRegen)
                return;
            var map = MapManager.Instance.GetMap(actor.MapID);
            var hpadd = (uint)(50 + 10 * skill.skill.Level);
            if (actor.HP + hpadd < actor.MaxHP)
                actor.HP = actor.HP + hpadd;
            else
                actor.HP = actor.MaxHP;
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.HPMPSP_UPDATE, null, actor, true);
        }

        #endregion
    }
}