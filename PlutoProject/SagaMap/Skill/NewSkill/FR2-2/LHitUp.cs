using SagaDB.Actor;
using SagaDB.Item;
using SagaMap.Manager;
using SagaMap.Network.Client;
using SagaMap.Skill.Additions;
using SagaMap.Skill.SkillDefinations;

namespace SagaMap.Skill.NewSkill.FR2_2
{
    /// <summary>
    ///     飄渺之境
    /// </summary>
    public class LHitUp : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            if (CheckPossible(pc))
                return 0;
            return -5;
        }

        private bool CheckPossible(Actor sActor)
        {
            if (sActor.type == ActorType.PC)
            {
                var pc = (ActorPC)sActor;
                pc = SkillHandler.Instance.GetPossesionedActor((ActorPC)sActor);
                if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND))
                {
                    if (pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.BOW ||
                        pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.GUN ||
                        pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.RIFLE ||
                        pc.Inventory.GetContainer(ContainerType.RIGHT_HAND2).Count > 0)
                        return true;
                    return false;
                }

                return false;
            }

            return true;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var map = MapManager.Instance.GetMap(sActor.MapID);
            Actor realdActor = SkillHandler.Instance.GetPossesionedActor((ActorPC)sActor);
            var life = 0;
            life = 180000;
            var skill = new DefaultBuff(args.skill, realdActor, "MarkMannAura", life);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(realdActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            var Lhit = (short)(6 * skill.skill.Level);
            if (skill.Variable.ContainsKey("MarkMannAura_LHit"))
                skill.Variable.Remove("MarkMannAura_LHit");
            skill.Variable.Add("MarkMannAura_LHit", Lhit);
            actor.Status.hit_ranged_skill += Lhit;
            if (actor.type == ActorType.PC) MapClient.FromActorPC((ActorPC)actor).SendSystemMessage("受到名射手的光环的效果");
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.Status.hit_ranged_skill -= (short)skill.Variable["MarkMannAura_LHit"];
            if (actor.type == ActorType.PC) MapClient.FromActorPC((ActorPC)actor).SendSystemMessage("名射手的光环的效果解除");
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        #endregion
    }
}