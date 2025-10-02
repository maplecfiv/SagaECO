using SagaDB.Actor;
using SagaDB.Item;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._3_0_Class.Guardian_守护者____fen
{
    /// <summary>
    ///     ブロッキング
    /// </summary>
    public class Blocking : ISkill
    {
        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            var realdActor = SkillHandler.Instance.GetPossesionedActor(sActor);
            if (CheckPossible(realdActor))
                return 0;
            return -5;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 30000 * level;
            var realdActor = SkillHandler.Instance.GetPossesionedActor((ActorPC)sActor);
            var skill = new DefaultBuff(args.skill, realdActor, "Blocking", lifetime);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            skill.OnCheckValid += ValidCheck;
            SkillHandler.ApplyAddition(realdActor, skill);
        }

        private bool CheckPossible(ActorPC pc)
        {
            if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.LEFT_HAND))
            {
                if (pc.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].BaseData.itemType == ItemType.SHIELD) return true;

                return false;
            }

            return false;
        }

        private void ValidCheck(ActorPC pc, Actor dActor, out int result)
        {
            result = TryCast(pc, dActor, null);
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.Status.Blocking_LV = skill.skill.Level;
            actor.Buff.Blocking = true;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.Status.Blocking_LV = 0;
            actor.Buff.Blocking = false;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }
    }
}