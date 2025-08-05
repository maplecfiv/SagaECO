using SagaDB.Actor;
using SagaDB.Item;
using SagaLib;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._2_1_Class.Striker_猎人____arc
{
    /// <summary>
    ///     各種屬性箭
    /// </summary>
    public class ElementArrow : ISkill
    {
        private readonly Elements element = Elements.Neutral;

        public ElementArrow(Elements e)
        {
            element = e;
        }

        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            if (CheckPossible(pc))
            {
                uint ItemID = 0;
                switch (element)
                {
                    case Elements.Earth:
                        ItemID = 10026505;
                        break;
                    case Elements.Water:
                        ItemID = 10026504;
                        break;
                    case Elements.Fire:
                        ItemID = 10026500;
                        break;
                    case Elements.Wind:
                        ItemID = 10026507;
                        break;
                }

                if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.LEFT_HAND))
                {
                    if (pc.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].ItemID == ItemID)
                    {
                        if (SkillHandler.Instance.CountItem(pc, ItemID) > 0)
                        {
                            SkillHandler.Instance.TakeItem(pc, ItemID, 1);
                            return 0;
                        }

                        return -55;
                    }

                    if (SkillHandler.Instance.CountItem(pc, ItemID) > 0)
                    {
                        SkillHandler.Instance.TakeItem(pc, ItemID, 1);
                        return 0;
                    }

                    return -2;
                }

                if (SkillHandler.Instance.CountItem(pc, ItemID) > 0)
                {
                    SkillHandler.Instance.TakeItem(pc, ItemID, 1);
                    return 0;
                }

                return -2;
            }

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
                    if (pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.BOW)
                        return true;
                    return false;
                }

                return false;
            }

            return true;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            if (sActor.type != ActorType.PC) level = 5;
            if (dActor.Status.Additions.ContainsKey("HolyShield"))
                SkillHandler.RemoveAddition(dActor, "HolyShield");
            if (dActor.Status.Additions.ContainsKey("DarkShield"))
                SkillHandler.RemoveAddition(dActor, "DarkShield");
            if (dActor.Status.Additions.ContainsKey("FireShield"))
                SkillHandler.RemoveAddition(dActor, "FireShield");
            if (dActor.Status.Additions.ContainsKey("WaterShield"))
                SkillHandler.RemoveAddition(dActor, "WaterShield");
            if (dActor.Status.Additions.ContainsKey("WindShield"))
                SkillHandler.RemoveAddition(dActor, "WindShield");
            if (dActor.Status.Additions.ContainsKey("EarthShield"))
                SkillHandler.RemoveAddition(dActor, "EarthShield");
            dActor.Buff.BodyDarkElementUp = false;
            dActor.Buff.BodyEarthElementUp = false;
            dActor.Buff.BodyFireElementUp = false;
            dActor.Buff.BodyWaterElementUp = false;
            dActor.Buff.BodyWindElementUp = false;
            dActor.Buff.BodyHolyElementUp = false;
            args.argType = SkillArg.ArgType.Active;
            args.type = ATTACK_TYPE.STAB;
            SkillHandler.Instance.PhysicalAttack(sActor, dActor, args, Elements.Neutral, 1.0f);
            var life = new[] { 0, 10000, 12000, 14000, 16000, 18000 }[level];
            var skill = new DefaultBuff(args.skill, dActor, element + "Shield", life);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(dActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            var atk1 = 60;
            if (skill.Variable.ContainsKey("ElementShield"))
                skill.Variable.Remove("ElementShield");
            skill.Variable.Add("ElementShield", atk1);
            actor.Status.elements_skill[element] += atk1;

            var type = actor.Buff.GetType();
            var propertyInfo = type.GetProperty("Body" + element + "ElementUp");
            propertyInfo.SetValue(actor.Buff, true, null);
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            var value = skill.Variable["ElementShield"];
            actor.Status.elements_skill[element] -= (short)value;

            var type = actor.Buff.GetType();
            var propertyInfo = type.GetProperty("Body" + element + "ElementUp");
            propertyInfo.SetValue(actor.Buff, false, null);
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        #endregion
    }
}