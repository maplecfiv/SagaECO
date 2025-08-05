using System;
using System.Collections.Generic;
using System.Linq;
using SagaDB.Actor;
using SagaDB.Item;
using SagaLib;
using SagaMap.ActorEventHandlers;
using SagaMap.Manager;
using SagaMap.Packets.Client;
using SagaMap.Packets.Server;
using SagaMap.PC;
using SagaMap.Skill;
using SagaMap.Skill.Additions.Global;
using SagaMap.Tasks.PC;

namespace SagaMap.Network.Client
{
    public partial class MapClient
    {
        public void OnPossessionRequest(CSMG_POSSESSION_REQUEST p)
        {
            var target = (ActorPC)Map.GetActor(p.ActorID);
            var pos = p.PossessionPosition;
            var result = TestPossesionPosition(target, pos);
            if (result >= 0)
            {
                Character.Buff.GetReadyPossession = true;
                Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, Character, true);
                var reduce = 0;
                if (Character.Status.Additions.ContainsKey("TranceSpdUp"))
                {
                    var passive = (DefaultPassiveSkill)Character.Status.Additions["TranceSpdUp"];
                    reduce = passive["TranceSpdUp"];
                }

                var task = new Possession(this, target, pos, p.Comment, reduce);
                Character.Tasks.Add("Possession", task);
                task.Activate();
            }
            else
            {
                var p1 = new SSMG_POSSESSION_RESULT();
                p1.FromID = Character.ActorID;
                p1.ToID = 0xFFFFFFFF;
                p1.Result = result;
                netIO.SendPacket(p1);
            }
        }

        public void OnPossessionCancel(CSMG_POSSESSION_CANCEL p)
        {
            var pos = p.PossessionPosition;
            switch (pos)
            {
                case PossessionPosition.NONE:
                    var actor = Map.GetActor(Character.PossessionTarget);
                    if (actor == null)
                        return;
                    var arg = new PossessionArg();
                    arg.fromID = Character.ActorID;
                    arg.cancel = true;
                    arg.result = (int)Character.PossessionPosition;
                    arg.x = Global.PosX16to8(Character.X, Map.Width);
                    arg.y = Global.PosY16to8(Character.Y, Map.Height);
                    arg.dir = (byte)(Character.Dir / 45);
                    if (actor.type == ActorType.ITEM)
                    {
                        var item = GetPossessionItem(Character, Character.PossessionPosition);
                        item.PossessionedActor = null;
                        item.PossessionOwner = null;
                        Character.PossessionTarget = 0;
                        Character.PossessionPosition = PossessionPosition.NONE;
                        arg.toID = 0xFFFFFFFF;
                        Map.DeleteActor(actor);
                    }
                    else if (actor.type == ActorType.PC)
                    {
                        var pc = (ActorPC)actor;
                        arg.toID = pc.ActorID;
                        var item = GetPossessionItem(pc, Character.PossessionPosition);
                        if (item.PossessionOwner != Character)
                        {
                            item.PossessionedActor = null;
                            Character.PossessionTarget = 0;
                            Character.PossessionPosition = PossessionPosition.NONE;
                        }
                        else
                        {
                            var item2 = GetPossessionItem(Character, Character.PossessionPosition);
                            item2.PossessionedActor = null;
                            item2.PossessionOwner = null;
                            Character.PossessionTarget = 0;
                            Character.PossessionPosition = PossessionPosition.NONE;
                            var p3 = new CSMG_ITEM_MOVE();
                            p3.data = new byte[9];
                            p3.InventoryID = item.Slot;
                            p3.Target = ContainerType.BODY;
                            p3.Count = 1;
                            FromActorPC(pc).OnItemMove(p3, true);
                            pc.Inventory.DeleteItem(item.Slot, 1);

                            var p2 = new SSMG_ITEM_DELETE();
                            p2.InventorySlot = item.Slot;
                            ((PCEventHandler)pc.e).Client.netIO.SendPacket(p2);
                            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.CHANGE_EQUIP, null, pc, true);
                        }

                        item.PossessionedActor = null;
                        item.PossessionOwner = null;

                        StatusFactory.Instance.CalcStatus(Character);
                        SendPlayerInfo();
                        StatusFactory.Instance.CalcStatus(pc);
                        ((PCEventHandler)pc.e).Client.SendPlayerInfo();
                    }

                    Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.POSSESSION, arg, Character, true);
                    break;
                default:
                    var item3 = GetPossessionItem(Character, pos);
                    if (item3 == null)
                        return;
                    if (item3.PossessionedActor == null)
                        return;
                    var arg2 = new PossessionArg();
                    arg2.fromID = item3.PossessionedActor.ActorID;
                    arg2.toID = Character.ActorID;
                    arg2.cancel = true;
                    arg2.result = (int)item3.PossessionedActor.PossessionPosition;
                    arg2.x = Global.PosX16to8(Character.X, Map.Width);
                    arg2.y = Global.PosY16to8(Character.Y, Map.Height);
                    arg2.dir = (byte)(Character.Dir / 45);


                    if (item3.PossessionOwner != Character && item3.PossessionOwner != null)
                    {
                        var item4 = GetPossessionItem(item3.PossessionedActor,
                            item3.PossessionedActor.PossessionPosition);
                        if (item4 != null)
                        {
                            item4.PossessionedActor = null;
                            item4.PossessionOwner = null;
                        }

                        var p3 = new CSMG_ITEM_MOVE();
                        p3.data = new byte[9];
                        p3.InventoryID = item3.Slot;
                        p3.Target = ContainerType.BODY;
                        p3.Count = 1;
                        OnItemMove(p3, true);
                        Character.Inventory.DeleteItem(item3.Slot, 1);

                        var p2 = new SSMG_ITEM_DELETE();
                        p2.InventorySlot = item3.Slot;
                        netIO.SendPacket(p2);
                        map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.CHANGE_EQUIP, null, Character, true);

                        Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.POSSESSION, arg2, Character, true);
                        if (((PCEventHandler)item3.PossessionedActor.e).Client.state == SESSION_STATE.DISCONNECTED)
                        {
                            var itemactor = PossessionItemAdd(item3.PossessionedActor,
                                item3.PossessionedActor.PossessionPosition, "");
                            item3.PossessionedActor.PossessionTarget = itemactor.ActorID;
                            MapServer.charDB.SaveChar(item3.PossessionedActor, false, false);
                            MapServer.accountDB.WriteUser(item3.PossessionedActor.Account);
                            return;
                        }
                    }
                    else
                    {
                        var actor2 = map.GetActor(Character.PossessionTarget);
                        if (actor2 != null)
                        {
                            if (actor2.type == ActorType.ITEM)
                                map.DeleteActor(actor2);
                            if (!item3.PossessionedActor.Online) arg2.fromID = 0xFFFFFFFF;
                        }

                        Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.POSSESSION, arg2, Character, true);
                    }

                    item3.PossessionedActor.PossessionTarget = 0;
                    item3.PossessionedActor.PossessionPosition = PossessionPosition.NONE;
                    item3.PossessionedActor = null;
                    item3.PossessionOwner = null;
                    StatusFactory.Instance.CalcStatus(Character);
                    SendPlayerInfo();
                    break;
            }
        }

        public void PossessionPerform(ActorPC target, PossessionPosition position, string comment)
        {
            var result = TestPossesionPosition(target, position);
            if (result >= 0)
            {
                var arg = new PossessionArg();
                arg.fromID = Character.ActorID;
                arg.toID = target.ActorID;
                arg.result = result;
                arg.comment = comment;
                Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.POSSESSION, arg, Character, true);

                var pos = "";
                switch (position)
                {
                    case PossessionPosition.RIGHT_HAND:
                        pos = LocalManager.Instance.Strings.POSSESSION_RIGHT;
                        break;
                    case PossessionPosition.LEFT_HAND:
                        pos = LocalManager.Instance.Strings.POSSESSION_LEFT;
                        break;
                    case PossessionPosition.NECK:
                        pos = LocalManager.Instance.Strings.POSSESSION_NECK;
                        break;
                    case PossessionPosition.CHEST:
                        pos = LocalManager.Instance.Strings.POSSESSION_ARMOR;
                        break;
                }

                SendSystemMessage(string.Format(LocalManager.Instance.Strings.POSSESSION_DONE, pos));
                if (target == Character)
                {
                    Character.PossessionTarget = PossessionItemAdd(Character, position, comment).ActorID;
                    Character.PossessionPosition = position;
                }
                else
                {
                    FromActorPC(target)
                        .SendSystemMessage(string.Format(LocalManager.Instance.Strings.POSSESSION_DONE, pos));
                    Character.PossessionTarget = target.ActorID;
                    Character.PossessionPosition = position;
                    var item = GetPossessionItem(target, position);
                    item.PossessionedActor = Character;
                }

                if (!Character.Tasks.ContainsKey("PossessionRecover"))
                {
                    var task = new PossessionRecover(this);
                    Character.Tasks.Add("PossessionRecover", task);
                    task.Activate();
                }

                SkillHandler.Instance.CastPassiveSkills(Character);
                StatusFactory.Instance.CalcStatus(Character);
                SendPlayerInfo();
                StatusFactory.Instance.CalcStatus(target);
                ((PCEventHandler)target.e).Client.SendPlayerInfo();
            }
            else
            {
                var p1 = new SSMG_POSSESSION_RESULT();
                p1.FromID = Character.ActorID;
                p1.ToID = 0xFFFFFFFF;
                p1.Result = result;
                netIO.SendPacket(p1);
            }
        }

        private int TestPossesionPosition(ActorPC target, PossessionPosition pos)
        {
            Item item = null;
            if (Character.PossessionTarget != 0)
                return -1; //憑依失敗 : 憑依中です
            if (Character.PossesionedActors.Count != 0)
                return -2; //憑依失敗 : 宿主です
            if (target.type != ActorType.PC)
                return -3; //憑依失敗 : プレイヤーのみ憑依可能です
            var targetPC = target;
            //if (Math.Abs(target.Level - this.Character.Level) > 30)
            //    return -4; //憑依失敗 : レベルが離れすぎです
            switch (pos)
            {
                case PossessionPosition.CHEST:
                    if (targetPC.Inventory.Equipments.ContainsKey(EnumEquipSlot.UPPER_BODY))
                        item = targetPC.Inventory.Equipments[EnumEquipSlot.UPPER_BODY];
                    else
                        return -5; //憑依失敗 : 装備がありません
                    break;
                case PossessionPosition.LEFT_HAND:
                    if (targetPC.Inventory.Equipments.ContainsKey(EnumEquipSlot.LEFT_HAND))
                        item = targetPC.Inventory.Equipments[EnumEquipSlot.LEFT_HAND];
                    else
                        return -5; //憑依失敗 : 装備がありません
                    break;
                case PossessionPosition.NECK:
                    if (targetPC.Inventory.Equipments.ContainsKey(EnumEquipSlot.CHEST_ACCE))
                        item = targetPC.Inventory.Equipments[EnumEquipSlot.CHEST_ACCE];
                    else
                        return -5; //憑依失敗 : 装備がありません
                    break;
                case PossessionPosition.RIGHT_HAND:
                    if (targetPC.Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND))
                    {
                        if (targetPC.Buff.FishingState) return -15;
                        item = targetPC.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND];
                    }
                    else
                    {
                        return -5; //憑依失敗 : 装備がありません
                    }

                    break;
            }

            if (item == null)
                return -5; //憑依失敗 : 装備がありません
            if (item.Stack == 0)
                return -5; ////憑依失敗 : 装備がありません
            if (item.PossessionedActor != null)
                return -6; //憑依失敗 : 誰かが憑依しています
            if (item.BaseData.itemType == ItemType.CARD || item.BaseData.itemType == ItemType.THROW ||
                item.BaseData.itemType == ItemType.ARROW || item.BaseData.itemType == ItemType.BULLET)
                return -7; //憑依失敗 : 憑依不可能なアイテムです
            if (targetPC.PossesionedActors.Count >= 3)
                return -8; //憑依失敗 : 満員宿主です
            if (Character.Marionette != null || targetPC.Marionette != null
                                             || Character.Buff.Confused || Character.Buff.Frosen ||
                                             Character.Buff.Paralysis
                                             || Character.Buff.Sleep || Character.Buff.Stone || Character.Buff.Stun)
                return -15; //憑依失敗 : 状態異常中です
            if (targetPC.PossessionTarget != 0)
                return -16; //憑依失敗 : 相手は憑依中です
            if (target.Buff.GetReadyPossession)
                return -17; //憑依失敗 : 相手はGetReadyPossession中です
            if (target.Buff.Dead)
                return -18; //憑依失敗 : 相手は行動不能状態です
            if (scriptThread != null || ((PCEventHandler)target.e).Client.scriptThread != null ||
                target.Buff.FishingState)
                return -19; //憑依失敗 : イベント中です
            if (Character.Tasks.ContainsKey("ItemCast"))
                return -19; //憑依失敗 : イベント中です
            if (Character.MapID != target.MapID)
                return -20; //憑依失敗 : 相手とマップが違います
            if (Math.Abs(target.X - Character.X) > 300 || Math.Abs(target.Y - Character.Y) > 300)
                return -21; //憑依失敗 : 相手と離れすぎています
            if (!target.canPossession)
                return -22; //憑依失敗 : 相手が憑依不許可設定中です
            if (Character.Pet != null)
                if (Character.Pet.Ride)
                    return -27; //憑依失敗 : 騎乗中です
            if (Character.Buff.Dead)
                return -29; //憑依失敗: 憑依できない状態です
            if (Character.Race == PC_RACE.DEM)
                return -29; //憑依失敗 : 憑依できない状態です
            if (targetPC.Race == PC_RACE.DEM && targetPC.Form == DEM_FORM.MACHINA_FORM)
                return -31; //憑依失敗 : マシナフォームのＤＥＭキャラクターに憑依することはできません
            /*
            if (this.Character.Buff.GetReadyPossession == true || this.Character.PossessionTarget != 0)
                return -100; //憑依失敗 : 何らかの原因で出来ませんでした
            */
            return (int)pos;
        }

        private Item GetPossessionItem(ActorPC target, PossessionPosition pos)
        {
            Item item = null;
            switch (pos)
            {
                case PossessionPosition.CHEST:
                    if (target.Inventory.Equipments.ContainsKey(EnumEquipSlot.UPPER_BODY))
                        item = target.Inventory.Equipments[EnumEquipSlot.UPPER_BODY];
                    break;
                case PossessionPosition.LEFT_HAND:
                    if (target.Inventory.Equipments.ContainsKey(EnumEquipSlot.LEFT_HAND))
                        item = target.Inventory.Equipments[EnumEquipSlot.LEFT_HAND];
                    break;
                case PossessionPosition.NECK:
                    if (target.Inventory.Equipments.ContainsKey(EnumEquipSlot.CHEST_ACCE))
                        item = target.Inventory.Equipments[EnumEquipSlot.CHEST_ACCE];
                    break;
                case PossessionPosition.RIGHT_HAND:
                    if (target.Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND))
                        item = target.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND];
                    break;
            }

            return item;
        }

        private ActorItem PossessionItemAdd(ActorPC target, PossessionPosition position, string comment)
        {
            var itemDroped = GetPossessionItem(target, position);
            if (itemDroped == null) return null;
            itemDroped.PossessionedActor = target;
            itemDroped.PossessionOwner = target;
            var actor = new ActorItem(itemDroped);
            actor.e = new ItemEventHandler(actor);
            actor.MapID = target.MapID;
            actor.X = target.X;
            actor.Y = target.Y;
            actor.Comment = comment;
            Map.RegisterActor(actor);
            actor.invisble = false;
            Map.OnActorVisibilityChange(actor);
            return actor;
        }

        private ActorPC GetPossessionTarget()
        {
            if (Character.PossessionTarget == 0)
                return null;
            var actor = Map.GetActor(Character.PossessionTarget);
            if (actor == null)
                return null;
            if (actor.type != ActorType.PC)
                return null;
            return (ActorPC)actor;
        }

        private void PossessionPrepareCancel()
        {
            if (Character.Buff.GetReadyPossession)
            {
                Character.Buff.GetReadyPossession = false;
                Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, Character, true);
                if (Character.Tasks.ContainsKey("Possession"))
                {
                    Character.Tasks["Possession"].Deactivate();
                    Character.Tasks.Remove("Possession");
                }
            }
        }

        public void OnPossessionCatalogRequest(CSMG_POSSESSION_CATALOG_REQUEST p)
        {
            var list = new List<ActorItem>();
            foreach (var actor in map.Actors.Values)
                if (actor is ActorItem)
                {
                    var item = (ActorItem)actor;
                    if (item.Item.PossessionedActor.PossessionPosition == p.Position)
                        list.Add(item);
                }

            var pageSize = 5;
            var skip = pageSize * (p.Page - 1);
            var items = list.Select(x => x)
                .Skip(skip)
                .Take(pageSize)
                .ToArray();
            for (var i = 0; i < items.Length; i++)
            {
                var p1 = new SSMG_POSSESSION_CATALOG();
                p1.ActorID = items[i].ActorID;
                p1.comment = items[i].Comment;
                p1.Index = (uint)i + 1;
                p1.Item = items[i].Item;
                netIO.SendPacket(p1);
            }

            var p2 = new SSMG_POSSESSION_CATALOG_END();
            p2.Page = p.Page;
            netIO.SendPacket(p2);
        }

        public void OnPossessionCatalogItemInfoRequest(CSMG_POSSESSION_CATALOG_ITEM_INFO_REQUEST p)
        {
            var target = map.GetActor(p.ActorID);
            if (target != null)
                if (target is ActorItem)
                {
                    var item = (ActorItem)target;
                    var p2 = new SSMG_POSSESSION_CATALOG_ITEM_INFO();
                    p2.ActorID = item.ActorID;
                    p2.ItemID = item.Item.ItemID;
                    p2.Level = item.Item.BaseData.possibleLv;
                    p2.X = Global.PosX16to8(item.X, map.Width);
                    p2.Y = Global.PosY16to8(item.Y, map.Height);
                    netIO.SendPacket(p2);
                }
        }

        public void OnPartnerPossessionRequest(CSMG_POSSESSION_PARTNER_REQUEST p)
        {
            var partneritem = Character.Inventory.GetItem(p.InventorySlot);
            if (Character.Inventory.Equipments.ContainsKey(EnumEquipSlot.PET))
                if (partneritem == Character.Inventory.Equipments[EnumEquipSlot.PET])
                    return;
            var partner = Character.Partner;
            if (partner == null) return;
            var Pict = partneritem.BaseData.petID;
            if (Pict == partner.BaseData.pictid) return;
            if (partneritem != null)
            {
                switch (p.PossessionPosition)
                {
                    case PossessionPosition.RIGHT_HAND:
                        if (Character.Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND))
                        {
                            Character.PossessionPartnerSlotIDinRightHand = Pict;
                            Character.Status.max_atk1_petpy = (short)(partner.Status.max_atk1 / 2);
                            Character.Status.min_atk1_petpy = (short)(partner.Status.min_atk1 / 2);
                            Character.Status.max_matk_petpy = (short)(partner.Status.max_matk / 2);
                            Character.Status.min_matk_petpy = (short)(partner.Status.min_matk / 2);
                        }
                        else
                        {
                            Character.PossessionPartnerSlotIDinRightHand = 0;
                            Character.Status.max_atk1_petpy = 0;
                            Character.Status.min_atk1_petpy = 0;
                            Character.Status.max_matk_petpy = 0;
                            Character.Status.min_matk_petpy = 0;
                        }

                        break;
                    case PossessionPosition.LEFT_HAND:
                        if (Character.Inventory.Equipments.ContainsKey(EnumEquipSlot.LEFT_HAND))
                        {
                            Character.PossessionPartnerSlotIDinLeftHand = Pict;
                            Character.Status.def_add_petpy = (short)(partner.Status.def_add / 2);
                            Character.Status.mdef_add_petpy = (short)(partner.Status.mdef_add / 2);
                        }
                        else
                        {
                            Character.PossessionPartnerSlotIDinLeftHand = 0;
                            Character.Status.def_add_petpy = 0;
                            Character.Status.mdef_add_petpy = 0;
                        }

                        break;
                    case PossessionPosition.NECK:
                        if (Character.Inventory.Equipments.ContainsKey(EnumEquipSlot.CHEST_ACCE))
                        {
                            Character.PossessionPartnerSlotIDinAccesory = Pict;
                            Character.Status.aspd_petpy = (short)(partner.Status.aspd / 2);
                            Character.Status.cspd_petpy = (short)(partner.Status.cspd / 2);
                        }
                        else
                        {
                            Character.PossessionPartnerSlotIDinAccesory = 0;
                            Character.Status.aspd_petpy = 0;
                            Character.Status.cspd_petpy = 0;
                        }

                        break;
                    case PossessionPosition.CHEST:
                        if (Character.Inventory.Equipments.ContainsKey(EnumEquipSlot.UPPER_BODY))
                        {
                            Character.PossessionPartnerSlotIDinClothes = Pict;
                            Character.Status.hp_petpy = (short)(partner.MaxHP / 20);
                        }
                        else
                        {
                            Character.PossessionPartnerSlotIDinClothes = 0;
                            Character.Status.hp_petpy = 0;
                        }

                        break;
                }

                StatusFactory.Instance.CalcStatus(Character);
                var p1 = new SSMG_POSSESSION_PARTNER_RESULT();
                p1.InventorySlot = p.InventorySlot;
                p1.Pos = p.PossessionPosition;
                netIO.SendPacket(p1);
                map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.CHAR_INFO_UPDATE, null, Character, true);
            }
        }

        public void OnPartnerPossessionCancel(CSMG_POSSESSION_PARTNER_CANCEL p)
        {
            var p1 = new SSMG_POSSESSION_PARTNER_CANCEL();
            netIO.SendPacket(p1);
            switch (p.PossessionPosition)
            {
                case PossessionPosition.RIGHT_HAND:
                    Character.PossessionPartnerSlotIDinRightHand = 0;
                    break;
                case PossessionPosition.LEFT_HAND:
                    Character.PossessionPartnerSlotIDinLeftHand = 0;
                    break;
                case PossessionPosition.NECK:
                    Character.PossessionPartnerSlotIDinAccesory = 0;
                    break;
                case PossessionPosition.CHEST:
                    Character.PossessionPartnerSlotIDinClothes = 0;
                    break;
            }

            p1.Pos = p.PossessionPosition;
            netIO.SendPacket(p1);
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.CHAR_INFO_UPDATE, null, Character, true);
        }
    }
}