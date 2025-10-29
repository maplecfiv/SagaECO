using System;
using System.Collections.Generic;
using SagaDB.Actor;
using SagaDB.Item;
using SagaLib;
using SagaMap.Network.Client;
using Version = SagaLib.Version;

namespace SagaMap.Packets.Server.Actor {
    public class SSMG_ACTOR_PC_INFO : Packet {
        public SSMG_ACTOR_PC_INFO() {
            if (Configuration.Configuration.Instance.Version >= Version.Saga17)
                //this.data = new byte[166];
                data = new byte[239];
            else if (Configuration.Configuration.Instance.Version >= Version.Saga14_2)
                data = new byte[145];
            else if (Configuration.Configuration.Instance.Version >= Version.Saga14)
                data = new byte[150];
            else if (Configuration.Configuration.Instance.Version >= Version.Saga13)
                data = new byte[145];
            else if (Configuration.Configuration.Instance.Version >= Version.Saga11)
                data = new byte[140];
            else if (Configuration.Configuration.Instance.Version >= Version.Saga10)
                data = new byte[137];
            else
                data = new byte[144];
            offset = 2;
            if (Configuration.Configuration.Instance.Version >= Version.Saga17)
                ID = 0x020D;
            else
                ID = 0x020E;
        }

        private ActorShadow SetShadow {
            set {
                //#region Old

                if (Configuration.Configuration.Instance.Version < Version.Saga17) {
                    byte[] buf, buff;
                    byte size;
                    ushort offset;
                    PutUInt(value.ActorID, 2);
                    PutUInt(0xFFFFFFFF, 6);

                    buf = Global.Unicode.GetBytes(value.Name + "\0");
                    size = (byte)buf.Length;
                    buff = new byte[data.Length - 1 + size];
                    data.CopyTo(buff, 0);
                    data = buff;

                    PutByte(size, 10);
                    PutBytes(buf, 11);
                    offset = (ushort)(11 + size);

                    if (value.Owner.Marionette == null) {
                        PutByte((byte)value.Owner.Race, offset);
                        if (Configuration.Configuration.Instance.Version >= Version.Saga10) {
                            offset++;
                            PutByte((byte)value.Owner.Form, offset);
                        }

                        PutByte((byte)value.Owner.Gender, (ushort)(offset + 1));
                        if (Configuration.Configuration.Instance.Version >= Version.Saga11) {
                            PutUShort(value.Owner.HairStyle, (ushort)(offset + 2));
                            PutByte(value.Owner.HairColor, (ushort)(offset + 4));
                            PutUShort(value.Owner.Wig, (ushort)(offset + 5));
                            PutByte(0xff, (ushort)(offset + 7));
                            PutUShort(value.Owner.Face, (ushort)(offset + 8));
                            offset++;
                            PutByte(0, (ushort)(offset + 8)); //unknown
                            offset += 2;
                        }
                        else {
                            PutUShort(value.Owner.HairStyle, (ushort)(offset + 2));
                            PutByte(value.Owner.HairColor, (ushort)(offset + 3));
                            PutUShort(value.Owner.Wig, (ushort)(offset + 4));
                            PutByte(0xff, (ushort)(offset + 5));
                            PutUShort(value.Owner.Face, (ushort)(offset + 6));
                        }
                    }
                    else {
                        PutByte(0xff, offset);
                        if (Configuration.Configuration.Instance.Version >= Version.Saga10) {
                            offset++;
                            PutByte(0xff, offset);
                        }

                        PutByte(0xff, (ushort)(offset + 1));
                        PutByte(0xff, (ushort)(offset + 2));
                        PutByte(0xff, (ushort)(offset + 3));
                        PutByte(0xff, (ushort)(offset + 4));
                        PutByte(0xff, (ushort)(offset + 5));
                        PutByte(0xff, (ushort)(offset + 6));
                        PutByte(0xff, (ushort)(offset + 7));
                        PutByte(0xff, (ushort)(offset + 8));
                        PutByte(0xff, (ushort)(offset + 9));
                        if (Configuration.Configuration.Instance.Version >= Version.Saga11) {
                            PutByte(0xff, (ushort)(++offset + 9));
                            PutByte(0xff, (ushort)(++offset + 9));
                            PutByte(0xff, (ushort)(++offset + 9));
                        }
                    }

                    PutByte(0x0D, (ushort)(offset + 10));
                    if (value.Owner.Marionette == null) {
                        for (var j = 0; j < 14; j++)
                            if (value.Owner.Inventory.Equipments.ContainsKey((EnumEquipSlot)j)) {
                                var item = value.Owner.Inventory.Equipments[(EnumEquipSlot)j];
                                if (item.Stack == 0) continue;
                                if (item.PictID == 0)
                                    PutUInt(item.BaseData.imageID, (ushort)(offset + 11 + j * 4));
                                else
                                    PutUInt(item.PictID, (ushort)(offset + 11 + j * 4));
                            }
                    }
                    else {
                        PutUInt(value.Owner.Marionette.PictID, (ushort)(offset + 11));
                    }

                    //left hand weapon motion
                    PutByte(3, (ushort)(offset + 63));
                    if (value.Owner.Inventory.Equipments.ContainsKey(EnumEquipSlot.LEFT_HAND)) {
                        if (value.Owner.Marionette == null) {
                            var leftHand = value.Owner.Inventory.Equipments[EnumEquipSlot.LEFT_HAND];
                            PutUShort(leftHand.BaseData.handMotion, (ushort)(offset + 64));
                            PutByte(leftHand.BaseData.handMotion2, (ushort)(offset + 66));
                        }
                    }
                    else {
                        PutByte(0, (ushort)(offset + 64));
                    }

                    //right hand weapon motion
                    PutByte(3, (ushort)(offset + 67));
                    if (value.Owner.Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND)) {
                        if (value.Owner.Marionette == null) {
                            var rightHand = value.Owner.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND];
                            PutUShort(rightHand.BaseData.handMotion, (ushort)(offset + 68));
                            PutByte(rightHand.BaseData.handMotion2, (ushort)(offset + 80));
                        }
                    }
                    else {
                        PutByte(0, (ushort)(offset + 68));
                    }

                    PutByte(3, (ushort)(offset + 71));

                    PutByte(0, (ushort)(offset + 80));
                    PutByte(1, (ushort)(offset + 81)); //party name
                    PutByte(0, (ushort)(offset + 83)); //party name

                    PutByte(1, (ushort)(offset + 88)); //Ring name

                    PutByte(0, (ushort)(offset + 90)); //Ring master

                    PutByte(1, (ushort)(offset + 91)); //Sign name

                    PutByte(1, (ushort)(offset + 93)); //shop name

                    PutUInt(500, (ushort)(offset + 96)); //size
                    PutUInt(2, (ushort)(offset + 106)); //size

                    PutUShort(0, (ushort)(offset + 110));
                }

                //#endregion

                //#region Saga17

                if (Configuration.Configuration.Instance.Version >= Version.Saga17) {
                    byte[] buf, buff;
                    byte size;
                    ushort offset;

                    PutUInt(value.ActorID, 2);
                    PutUInt(0xFFFFFFFF, 6);

                    ///////////////玩家角色名///////////////
                    buf = Global.Unicode.GetBytes(value.Name + "\0");
                    size = (byte)buf.Length; //角色名长度
                    buff = new byte[data.Length - 1 + size];
                    data.CopyTo(buff, 0);
                    data = buff;
                    PutByte(size, 10);
                    PutBytes(buf, 11);
                    offset = (ushort)(11 + size);

                    ////////////////玩家外观////////////////
                    if (value.Owner.Marionette == null) {
                        PutByte((byte)value.Owner.Race, offset);
                        PutByte((byte)value.Owner.Form, offset + 1);
                        PutByte((byte)value.Owner.Gender, offset + 2);
                        PutUShort(value.Owner.HairStyle, offset + 3);
                        PutByte(value.Owner.HairColor, offset + 5);
                        PutUShort(value.Owner.Wig, offset + 6);
                        PutByte(0xff, offset + 8);
                        PutUShort(value.Owner.Face, offset + 9);
                        //3转外观
                        PutByte(value.Owner.TailStyle, offset + 11);
                        PutByte(value.Owner.WingStyle, offset + 13);
                        PutByte(value.Owner.WingColor, offset + 14);
                    }
                    else {
                        PutByte(0xff, offset);
                        PutByte(0xff, offset + 1);
                        PutByte(0xff, offset + 2);
                        PutByte(0xff, offset + 3);
                        PutByte(0xff, offset + 4);
                        PutByte(0xff, offset + 5);
                        PutByte(0xff, offset + 6);
                        PutByte(0xff, offset + 7);
                        PutByte(0xff, offset + 8);
                        PutByte(0xff, offset + 9);
                        PutByte(0xff, offset + 10);
                        PutByte(0xff, offset + 11);
                        PutByte(0xff, offset + 12);
                        PutByte(0xff, offset + 13);
                        PutByte(0xff, offset + 14);
                    }

                    PutByte(0x0E, offset + 15);

                    ////////////////玩家装备////////////////
                    Dictionary<EnumEquipSlot, SagaDB.Item.Item> equips;
                    if (value.Owner.Form != DEM_FORM.MACHINA_FORM)
                        equips = value.Owner.Inventory.Equipments;
                    else
                        equips = value.Owner.Inventory.Parts;
                    if (value.Owner.Marionette == null) {
                        if (value.Owner.TranceID == 0)
                            for (var j = 0; j < 14; j++) {
                                if (equips.ContainsKey((EnumEquipSlot)j)) {
                                    var item = equips[(EnumEquipSlot)j];
                                    if (item.Stack == 0) continue;
                                    if (item.PictID == 0)
                                        PutUInt(item.BaseData.imageID, offset + 16 + j * 4);
                                    else if (item.BaseData.itemType != ItemType.PET_NEKOMATA)
                                        PutUInt(item.PictID, offset + 16 + j * 4);
                                }
                            }
                        else
                            PutUInt(value.Owner.TranceID, offset + 16);
                    }
                    else {
                        PutUInt(value.Owner.Marionette.PictID, offset + 16);
                    }

                    ////////////////左手动作////////////////
                    PutByte(3, offset + 72);
                    if (equips.ContainsKey(EnumEquipSlot.LEFT_HAND))
                        if (value.Owner.Marionette == null && value.Owner.TranceID == 0) {
                            var leftHand = equips[EnumEquipSlot.LEFT_HAND];
                            PutUShort(leftHand.BaseData.handMotion, offset + 73);
                            PutByte(leftHand.BaseData.handMotion2, offset + 75);
                        }

                    ////////////////右手动作////////////////
                    PutByte(3, offset + 76);
                    if (equips.ContainsKey(EnumEquipSlot.RIGHT_HAND))
                        if (value.Owner.Marionette == null && value.Owner.TranceID == 0) {
                            var rightHand = equips[EnumEquipSlot.RIGHT_HAND];
                            PutUShort(rightHand.BaseData.handMotion, offset + 77);
                            PutByte(rightHand.BaseData.handMotion2, offset + 79);
                        }

                    //////////////////骑乘//////////////////
                    PutByte(3, offset + 80);
                    if (equips.ContainsKey(EnumEquipSlot.PET) && value.Owner.Pet != null)
                        if (value.Owner.Pet.Ride) {
                            var pet = equips[EnumEquipSlot.PET];
                            PutUShort(pet.BaseData.handMotion, offset + 81);
                            PutByte(pet.BaseData.handMotion2, offset + 83);
                        }

                    if (equips.ContainsKey(EnumEquipSlot.PET) && value.Owner.Pet != null)
                        if (value.Owner.Pet.Ride)
                            PutUInt(equips[EnumEquipSlot.PET].ItemID, offset + 84);

                    //BYTE ride_color; //乗り物の染色値
                    PutByte(0, offset + 89);

                    offset += 4; //2015年12月10日，对应449版本

                    ////////////////队伍信息////////////////
                    if (value.Owner.Party != null) {
                        buf = Global.Unicode.GetBytes(value.Owner.Party.Name + "\0");
                        buff = new byte[data.Length + buf.Length];
                        data.CopyTo(buff, 0);
                        data = buff;
                        PutByte((byte)buf.Length, offset + 90);
                        PutBytes(buf, offset + 91);
                        offset += (ushort)(buf.Length - 1);
                        PutByte(0, offset + 92);
                    }
                    else {
                        PutByte(1, offset + 90);
                        PutByte(1, offset + 92);
                    }

                    //UINT UNKNOMW
                    ////////////////军团信息////////////////
                    if (value.Owner.Ring != null) {
                        buf = Global.Unicode.GetBytes(value.Owner.Ring.Name + "\0");
                        buff = new byte[data.Length + buf.Length];
                        data.CopyTo(buff, 0);
                        data = buff;
                        PutByte((byte)buf.Length, offset + 97);
                        PutBytes(buf, offset + 98);
                        offset += (ushort)(buf.Length - 1);
                        PutByte(0, offset + 99);
                    }
                    else {
                        PutByte(1, offset + 97);
                        PutByte(1, offset + 99);
                    }

                    ///////////////聊天室信息///////////////
                    buf = Global.Unicode.GetBytes(value.Owner.Sign + "\0");
                    buff = new byte[data.Length + buf.Length];
                    data.CopyTo(buff, 0);
                    data = buff;
                    PutByte((byte)buf.Length, offset + 100);
                    PutBytes(buf, offset + 101);
                    offset += (ushort)(buf.Length - 1);

                    /////////////////露天商店////////////////
                    PutByte(1, offset + 102);

                    PutUInt(500, offset + 105);

                    PutUShort((ushort)value.Owner.Motion, offset + 109);

                    PutUInt(0, offset + 111); //unknown

                    /////////////////阵容信息////////////////
                    PutUInt(2, offset + 115);
                    PutUInt(0, offset + 119);

                    /////////////////等级信息////////////////
                    PutUInt(value.Level, offset + 128);
                    PutUInt(value.Owner.WRPRanking, offset + 132);
                    PutByte(0xFF, offset + 140); //师徒图标
                }

                //#endregion
            }
        }

        private ActorMob SetMob {
            set {
                byte[] buf, buff;
                byte size;
                ushort offset;

                PutUInt(value.ActorID, 2);
                PutUInt(0xFFFFFFFF, 6);

                buf = Global.Unicode.GetBytes(value.Name + "\0");
                size = (byte)buf.Length; //角色名长度
                buff = new byte[data.Length - 1 + size];
                data.CopyTo(buff, 0);
                data = buff;
                PutByte(size, 10);
                PutBytes(buf, 11);
                offset = (ushort)(11 + size);

                PutByte(0xff, offset);
                PutByte(0xff, offset + 1);
                PutByte(0xff, offset + 2);
                PutByte(0xff, offset + 3);
                PutByte(0xff, offset + 4);
                PutByte(0xff, offset + 5);
                PutByte(0xff, offset + 6);
                PutByte(0xff, offset + 7);
                PutByte(0xff, offset + 8);
                PutByte(0xff, offset + 9);
                PutByte(0xff, offset + 10);
                PutByte(0xff, offset + 11);
                PutByte(0xff, offset + 12);
                PutByte(0xff, offset + 13);
                PutByte(0xff, offset + 14);
                PutByte(14, offset + 15);
                if (value.IllusionPictID != 0) //变形状态
                    PutUInt(value.IllusionPictID, (ushort)(offset + 16));
                else if (value.PictID != 0)
                    PutUInt(value.PictID, (ushort)(offset + 16));
                else if (value.AnotherID != 0)
                    PutUInt(91000013, (ushort)(offset + 16));
                else if (value.BaseData.pictid != 0)
                    PutUInt(value.BaseData.pictid, (ushort)(offset + 16));
                PutByte(3, offset + 72);
                PutByte(3, offset + 79);
                PutByte(3, offset + 86);
                if (value.RideID != 0 && ItemFactory.Instance.Items.ContainsKey(value.RideID)) {
                    var pet = ItemFactory.Instance.GetItem(value.RideID);
                    PutByte((byte)pet.BaseData.handMotion, offset + 81);
                    PutByte(pet.BaseData.handMotion2, offset + 83);
                    PutUInt(pet.ItemID, offset + 84);
                }

                PutByte(0, offset + 89);

                //offset += 4;//2015年12月10日，对应449版本
                PutByte(1, offset + 103);
                //this.PutByte(0, offset + );
                ////////////////军团信息////////////////
                PutByte(1, offset + 110);
                //this.PutByte(0, offset + 99);
                PutByte(1, offset + 113);

                PutByte(1, offset + 115);

                if (value.TInt["playersize"] != 0)
                    PutUInt((uint)value.TInt["playersize"], offset + 118);
                else
                    PutUInt(900, offset + 118);

                //offset++;//2015年12月11日，对应449版本

                PutUInt(2, offset + 129);
                //this.PutUInt(0, offset + 119);

                PutByte(0x1, offset + 145);
                PutUInt(0xffffffff, offset + 146);
                PutUInt(0xffffffff, offset + 150);
                PutByte(0xff, offset + 154); //2015年12月11日，对应449版本
                PutUInt(0xffffffff, offset + 162);
                buff = new byte[data.Length + 1];
                data.CopyTo(buff, 0);
                data = buff;
            }
        }

        private ActorPC SetPC {
            set {
                //#region Saga14

                if (Configuration.Configuration.Instance.Version >= Version.Saga9 &&
                    Configuration.Configuration.Instance.Version < Version.Saga14_2) {
                    byte[] buf, buff;
                    byte size;
                    ushort offset;
                    PutUInt(value.ActorID, 2);
                    PutUInt(value.CharID, 6);

                    buf = Global.Unicode.GetBytes(value.Name + "\0");
                    size = (byte)buf.Length;
                    buff = new byte[data.Length - 1 + size];
                    data.CopyTo(buff, 0);
                    data = buff;

                    PutByte(size, 10);
                    PutBytes(buf, 11);
                    offset = (ushort)(11 + size);

                    if (value.Marionette == null && value.TranceID == 0) {
                        PutByte((byte)value.Race, offset);
                        if (Configuration.Configuration.Instance.Version >= Version.Saga10) {
                            offset++;
                            PutByte((byte)value.Form, offset);
                        }

                        PutByte((byte)value.Gender, (ushort)(offset + 1));
                        if (Configuration.Configuration.Instance.Version >= Version.Saga11) {
                            PutUShort(value.HairStyle, (ushort)(offset + 2));
                            PutByte(value.HairColor, (ushort)(offset + 4));
                            PutUShort(value.Wig, (ushort)(offset + 5));

                            PutByte(0xff, (ushort)(offset + 7));
                            PutUShort(value.Face, (ushort)(offset + 8));
                            offset++;
                            PutByte(value.TailStyle, (ushort)(offset + 9)); //3轉外觀
                            PutByte(value.WingStyle, (ushort)(offset + 10)); //3轉外觀
                            PutByte(value.WingColor, (ushort)(offset + 11)); //3轉外觀
                            offset += 2;
                        }
                        else {
                            PutUShort(value.HairStyle, (ushort)(offset + 2));
                            PutByte(value.HairColor, (ushort)(offset + 3));
                            PutUShort(value.Wig, (ushort)(offset + 4));
                            PutByte(0xff, (ushort)(offset + 5));
                            PutUShort(value.Face, (ushort)(offset + 6));
                        }
                    }
                    else {
                        PutByte(0xff, offset);
                        if (Configuration.Configuration.Instance.Version >= Version.Saga10) {
                            offset++;
                            PutByte(0xff, offset);
                        }

                        PutByte(0xff, (ushort)(offset + 1));
                        PutByte(0xff, (ushort)(offset + 2));
                        PutByte(0xff, (ushort)(offset + 3));
                        PutByte(0xff, (ushort)(offset + 4));
                        PutByte(0xff, (ushort)(offset + 5));
                        PutByte(0xff, (ushort)(offset + 6));
                        PutByte(0xff, (ushort)(offset + 7));
                        PutByte(0xff, (ushort)(offset + 8));
                        PutByte(0xff, (ushort)(offset + 9));
                        if (Configuration.Configuration.Instance.Version >= Version.Saga11) {
                            PutByte(0xff, (ushort)(++offset + 9));
                            PutByte(0xff, (ushort)(++offset + 9));
                            PutByte(0xff, (ushort)(++offset + 9));
                        }
                    }

                    Dictionary<EnumEquipSlot, SagaDB.Item.Item> equips;
                    if (value.Form != DEM_FORM.MACHINA_FORM)
                        equips = value.Inventory.Equipments;
                    else
                        equips = value.Inventory.Parts;
                    PutByte(0x0D, (ushort)(offset + 10));
                    if (value.Marionette == null) {
                        if (value.TranceID == 0) {
                            for (var j = 0; j < 14; j++)
                                if (equips.ContainsKey((EnumEquipSlot)j)) {
                                    var item = equips[(EnumEquipSlot)j];
                                    if (item.Stack == 0) continue;
                                    if (item.PictID == 0)
                                        PutUInt(item.BaseData.imageID, (ushort)(offset + 11 + j * 4));
                                    else if (item.BaseData.itemType != ItemType.PET_NEKOMATA)
                                        PutUInt(item.PictID, (ushort)(offset + 11 + j * 4));
                                }
                        }
                        else {
                            PutUInt(value.TranceID, (ushort)(offset + 11));
                        }
                    }
                    else {
                        PutUInt(value.Marionette.PictID, (ushort)(offset + 11));
                    }

                    //left hand weapon motion
                    PutByte(3, (ushort)(offset + 63));
                    if (equips.ContainsKey(EnumEquipSlot.LEFT_HAND)) {
                        if (value.Marionette == null) {
                            var leftHand = equips[EnumEquipSlot.LEFT_HAND];
                            PutUShort(leftHand.BaseData.handMotion, (ushort)(offset + 64));
                            PutByte(leftHand.BaseData.handMotion2, (ushort)(offset + 65));
                        }
                    }
                    else {
                        PutByte(0, (ushort)(offset + 64));
                    }

                    //right hand weapon motion
                    PutByte(3, (ushort)(offset + 67));
                    if (equips.ContainsKey(EnumEquipSlot.RIGHT_HAND)) {
                        if (value.Marionette == null) {
                            var rightHand = equips[EnumEquipSlot.RIGHT_HAND];
                            PutUShort(rightHand.BaseData.handMotion, (ushort)(offset + 68));
                            PutUShort(rightHand.BaseData.handMotion2, (ushort)(offset + 69));
                            if (rightHand.BaseData.itemType == ItemType.SHORT_SWORD ||
                                rightHand.BaseData.itemType == ItemType.SWORD)
                                PutByte(1, (ushort)(offset + 70)); //匕首
                            else if (rightHand.BaseData.itemType == ItemType.SWORD)
                                PutByte(2, (ushort)(offset + 70)); //剑
                            else if (rightHand.BaseData.itemType == ItemType.RAPIER)
                                PutByte(3, (ushort)(offset + 70)); //长剑
                            else if (rightHand.BaseData.itemType == ItemType.CLAW)
                                PutByte(4, (ushort)(offset + 70)); //爪
                            else if (rightHand.BaseData.itemType == ItemType.HAMMER)
                                PutByte(6, (ushort)(offset + 70)); //锤
                            else if (rightHand.BaseData.itemType == ItemType.AXE)
                                PutByte(7, (ushort)(offset + 70)); //斧
                            else if (rightHand.BaseData.itemType == ItemType.SPEAR)
                                PutByte(8, (ushort)(offset + 70)); //矛
                            else if (rightHand.BaseData.itemType == ItemType.STAFF)
                                PutByte(9, (ushort)(offset + 70)); //杖
                        }
                    }
                    else {
                        PutByte(0, (ushort)(offset + 68));
                    }

                    //riding motion
                    PutByte(3, (ushort)(offset + 71));
                    if (equips.ContainsKey(EnumEquipSlot.PET) && value.Pet != null)
                        if (value.Pet.Ride) {
                            var pet = equips[EnumEquipSlot.PET];
                            PutUShort(pet.BaseData.handMotion, (ushort)(offset + 72));
                            PutUShort(pet.BaseData.handMotion2, (ushort)(offset + 73));
                        }

                    if (equips.ContainsKey(EnumEquipSlot.PET) && value.Pet != null)
                        if (value.Pet.Ride)
                            PutUInt(equips[EnumEquipSlot.PET].ItemID, (ushort)(offset + 75));

                    //BYTE ride_color;  //乗り物の染色値
                    PutByte(value.BattleStatus, (ushort)(offset + 80));
                    if (value.Party == null) {
                        PutByte(1, (ushort)(offset + 81)); //party name
                        PutByte(1, (ushort)(offset + 83)); //party name
                    }
                    else {
                        buf = Global.Unicode.GetBytes(value.Party.Name + "\0");
                        buff = new byte[data.Length + buf.Length];
                        data.CopyTo(buff, 0);
                        data = buff;
                        PutByte((byte)buf.Length, (ushort)(offset + 81));
                        PutBytes(buf, (ushort)(offset + 82));
                        offset += (ushort)(buf.Length - 1);
                        if (value == value.Party.Leader)
                            PutByte(1, (ushort)(offset + 83)); //party name
                        else
                            PutByte(0, (ushort)(offset + 83)); //party name
                    }

                    if (value.Ring == null) {
                        PutByte(1, (ushort)(offset + 88)); //Ring name
                        PutByte(1, (ushort)(offset + 90)); //Ring master
                    }
                    else {
                        buf = Global.Unicode.GetBytes(value.Ring.Name + "\0");
                        buff = new byte[data.Length + buf.Length];
                        data.CopyTo(buff, 0);
                        data = buff;
                        PutByte((byte)buf.Length, (ushort)(offset + 88));
                        PutBytes(buf, (ushort)(offset + 89));
                        offset += (ushort)(buf.Length - 1);
                        if (value == value.Ring.Leader)
                            PutByte(1, (ushort)(offset + 90)); //party name
                        else
                            PutByte(0, (ushort)(offset + 90)); //party name
                    }

                    buf = Global.Unicode.GetBytes(value.Sign + "\0");
                    buff = new byte[data.Length + buf.Length];
                    data.CopyTo(buff, 0);
                    data = buff;
                    PutByte((byte)buf.Length, (ushort)(offset + 91)); //sign name
                    PutBytes(buf, (ushort)(offset + 92));
                    offset += (ushort)(buf.Length - 1);

                    /*if (value.Shoptitle != null)
                    {
                        buf = Global.Unicode.GetBytes(value.Shoptitle + "\0");
                        buff = new byte[this.data.Length + buf.Length];
                        this.data.CopyTo(buff, 0);
                        this.data = buff;
                        this.PutByte((byte)buf.Length, (ushort)(offset + 93));//shop name
                        this.PutBytes(buf, (ushort)(offset + 94));
                        offset += (ushort)(buf.Length - 1);
                        if (value.Shopswitch == 1)
                            this.PutByte(1, (ushort)(offset + 95));
                        else
                            this.PutByte(0, (ushort)(offset + 95));
                    }
                    else*/
                    PutByte(1, (ushort)(offset + 93)); //shop name

                    PutUInt(1000, (ushort)(offset + 96)); //size

                    PutUShort((ushort)value.Motion, (ushort)(offset + 100));

                    PutUInt(0, (ushort)(offset + 102)); //unknown
                    switch (value.Mode) {
                        case PlayerMode.NORMAL:
                            PutInt(2, (ushort)(offset + 106)); //mode1
                            PutInt(0, (ushort)(offset + 110)); //mode2
                            break;
                        case PlayerMode.COLISEUM_MODE:
                            PutInt(0x42, (ushort)(offset + 106)); //mode1
                            PutInt(1, (ushort)(offset + 110)); //mode2
                            break;
                        case PlayerMode.KNIGHT_WAR:
                            PutInt(0x22, (ushort)(offset + 106)); //mode1
                            PutInt(2, (ushort)(offset + 110)); //mode2
                            break;
                        case PlayerMode.WRP:
                            PutInt(0x102, (ushort)(offset + 106)); //mode1
                            PutInt(4, (ushort)(offset + 110)); //mode2
                            break;
                        case PlayerMode.KNIGHT_EAST:
                            PutInt(0x22, (ushort)(offset + 106)); //mode1
                            PutInt(2, (ushort)(offset + 110)); //mode2
                            PutByte(1, (ushort)(offset + 114)); //emblem; //演習時のエンブレムとか　1東2西4南8北Aヒーロー状態
                            PutByte(0, (ushort)(offset + 115)); //metamo; //メタモーバトルのチーム　1花2岩
                            break;
                        case PlayerMode.KNIGHT_WEST:
                            PutInt(0x22, (ushort)(offset + 106)); //mode1
                            PutInt(2, (ushort)(offset + 110)); //mode2
                            PutByte(2, (ushort)(offset + 114)); //emblem; //演習時のエンブレムとか　1東2西4南8北Aヒーロー状態
                            PutByte(0, (ushort)(offset + 115)); //metamo; //メタモーバトルのチーム　1花2岩
                            break;
                        case PlayerMode.KNIGHT_SOUTH:
                            PutInt(0x22, (ushort)(offset + 106)); //mode1
                            PutInt(2, (ushort)(offset + 110)); //mode2
                            PutByte(4, (ushort)(offset + 114)); //emblem; //演習時のエンブレムとか　1東2西4南8北Aヒーロー状態
                            PutByte(0, (ushort)(offset + 115)); //metamo; //メタモーバトルのチーム　1花2岩
                            break;
                        case PlayerMode.KNIGHT_NORTH:
                            PutInt(0x22, (ushort)(offset + 106)); //mode1
                            PutInt(2, (ushort)(offset + 110)); //mode2
                            PutByte(8, (ushort)(offset + 114)); //emblem; //演習時のエンブレムとか　1東2西4南8北Aヒーロー状態
                            PutByte(0, (ushort)(offset + 115)); //metamo; //メタモーバトルのチーム　1花2岩
                            break;
                        case PlayerMode.KNIGHT_FLOWER:
                            PutInt(0x22, (ushort)(offset + 106)); //mode1
                            PutInt(2, (ushort)(offset + 110)); //mode2
                            PutByte(0, (ushort)(offset + 114)); //emblem; //演習時のエンブレムとか　1東2西4南8北Aヒーロー状態
                            PutByte(1, (ushort)(offset + 115)); //metamo; //メタモーバトルのチーム　1花2岩
                            break;
                        case PlayerMode.KNIGHT_ROCK:
                            PutInt(0x22, (ushort)(offset + 106)); //mode1
                            PutInt(2, (ushort)(offset + 110)); //mode2
                            PutByte(0, (ushort)(offset + 114)); //emblem; //演習時のエンブレムとか　1東2西4南8北Aヒーロー状態
                            PutByte(2, (ushort)(offset + 115)); //metamo; //メタモーバトルのチーム　1花2岩
                            break;
                    }

                    //Logger.getLogger().Information("SSMG_PLAYER_PC_INFO");
                    PutByte(0, (ushort)(offset + 116));
                    PutByte(0, (ushort)(offset + 117));
                    PutByte(0, (ushort)(offset + 118));
                    PutByte(value.Level, (ushort)(offset + 119));
                    PutUInt(value.WRPRanking, (ushort)(offset + 120)); //// WRP順位（ペットは -1固定。別のパケで主人の値が送られてくる

                    /*
                    DWORD mode2   //0 r0fa7参照
                    BYTE emblem; //演習時のエンブレムとか　1東2西4南8北Aヒーロー状態
                    BYTE metamo; //メタモーバトルのチーム　1花2岩
                    BYTE unknown; //1にすると/joyのモーションを取る（マリオネット変身時。）2にすると～
                    BYTE unknown; // 0?
                    BYTE guest; // ゲストIDかどうか
                    BYTE level; // レベル（ペットは1固定
                    DWORD wrp_rank; // WRP順位（ペットは -1固定。別のパケで主人の値が送られてくる
                    */
                }

                //#endregion

                //#region Saga14_2

                if (Configuration.Configuration.Instance.Version >= Version.Saga14_2 &&
                    Configuration.Configuration.Instance.Version < Version.Saga17) {
                    byte[] buf, buff;
                    byte size;
                    ushort offset;
                    PutUInt(value.ActorID, 2);
                    PutUInt(value.CharID, 6);

                    buf = Global.Unicode.GetBytes(value.Name + "\0");
                    size = (byte)buf.Length;
                    buff = new byte[data.Length - 1 + size];
                    data.CopyTo(buff, 0);
                    data = buff;

                    PutByte(size, 10);
                    PutBytes(buf, 11);
                    offset = (ushort)(11 + size);
                    if (value.Marionette == null && value.TranceID == 0) {
                        PutByte((byte)value.Race, offset);
                        offset++;
                        PutByte((byte)value.Form, offset);
                        PutByte((byte)value.Gender, (ushort)(offset + 1));
                        PutUShort(value.HairStyle, (ushort)(offset + 2));
                        PutByte(value.HairColor, (ushort)(offset + 4));
                        PutUShort(value.Wig, (ushort)(offset + 5));

                        PutByte(0xff, (ushort)(offset + 7));
                        PutUShort(value.Face, (ushort)(offset + 8)); //unknown
                        offset += 2;
                        PutByte(value.TailStyle, (ushort)(offset + 9)); //3轉外觀
                        PutByte(value.WingStyle, (ushort)(offset + 10)); //3轉外觀
                        PutByte(value.WingColor, (ushort)(offset + 11)); //3轉外觀
                        offset += 2;
                    }
                    else {
                        PutByte(0xff, offset);

                        if (Configuration.Configuration.Instance.Version >= Version.Saga10) {
                            offset++;
                            PutByte(0xff, offset);
                        }

                        PutByte(0xff, (ushort)(offset + 1));
                        PutByte(0xff, (ushort)(offset + 2));
                        PutByte(0xff, (ushort)(offset + 3));
                        PutByte(0xff, (ushort)(offset + 4));
                        PutByte(0xff, (ushort)(offset + 5));
                        PutByte(0xff, (ushort)(offset + 6));
                        PutByte(0xff, (ushort)(offset + 7));
                        PutByte(0xff, (ushort)(offset + 8));
                        offset++;
                        PutByte(0, (ushort)(offset + 8)); //unknown
                        PutByte(0xff, (ushort)(offset + 9));
                        PutByte(0xff, (ushort)(++offset + 9));
                        PutByte(0xff, (ushort)(++offset + 9));
                        PutByte(0xff, (ushort)(++offset + 9));
                    }

                    Dictionary<EnumEquipSlot, SagaDB.Item.Item> equips;
                    if (value.Form != DEM_FORM.MACHINA_FORM)
                        equips = value.Inventory.Equipments;
                    else
                        equips = value.Inventory.Parts;
                    PutByte(0x0E, (ushort)(offset + 10));
                    if (value.Marionette == null) {
                        if (value.TranceID == 0) {
                            for (var j = 0; j < 14; j++)
                                if (equips.ContainsKey((EnumEquipSlot)j)) {
                                    var item = equips[(EnumEquipSlot)j];
                                    if (item.Stack == 0) continue;
                                    if (item.PictID == 0)
                                        PutUInt(item.BaseData.imageID, (ushort)(offset + 11 + j * 4));
                                    else if (item.BaseData.itemType != ItemType.PET_NEKOMATA)
                                        PutUInt(item.PictID, (ushort)(offset + 11 + j * 4));
                                }
                        }
                        else {
                            PutUInt(value.TranceID, (ushort)(offset + 11));
                        }
                    }
                    else {
                        PutUInt(value.Marionette.PictID, (ushort)(offset + 11));
                    }

                    offset += 4;
                    //left hand weapon motion
                    PutByte(3, (ushort)(offset + 63));
                    if (equips.ContainsKey(EnumEquipSlot.LEFT_HAND)) {
                        if (value.Marionette == null) {
                            var leftHand = equips[EnumEquipSlot.LEFT_HAND];
                            PutUShort(leftHand.BaseData.handMotion, (ushort)(offset + 64)); //
                            PutUShort(leftHand.BaseData.handMotion2, (ushort)(offset + 65)); //
                        }
                    }
                    else {
                        PutByte(0, (ushort)(offset + 64));
                    }

                    if (equips.ContainsKey(EnumEquipSlot.RIGHT_HAND)) {
                        if (value.Marionette == null) {
                            var rightHand = equips[EnumEquipSlot.RIGHT_HAND];
                            PutUShort(rightHand.BaseData.handMotion, (ushort)(offset + 66)); //
                        }
                    }
                    else {
                        PutByte(0, (ushort)(offset + 64));
                    }

                    //right hand weapon motion
                    PutByte(3, (ushort)(offset + 67));
                    if (equips.ContainsKey(EnumEquipSlot.RIGHT_HAND)) {
                        if (value.Marionette == null) {
                            var rightHand = equips[EnumEquipSlot.RIGHT_HAND];
                            PutUShort(rightHand.BaseData.handMotion, (ushort)(offset + 68));
                            PutUShort(rightHand.BaseData.handMotion2, (ushort)(offset + 69));
                            if (rightHand.BaseData.itemType == ItemType.SHORT_SWORD ||
                                rightHand.BaseData.itemType == ItemType.SWORD)
                                PutByte(1, (ushort)(offset + 70)); //匕首
                            else if (rightHand.BaseData.itemType == ItemType.SWORD)
                                PutByte(2, (ushort)(offset + 70)); //剑
                            else if (rightHand.BaseData.itemType == ItemType.RAPIER)
                                PutByte(3, (ushort)(offset + 70)); //长剑
                            else if (rightHand.BaseData.itemType == ItemType.CLAW)
                                PutByte(4, (ushort)(offset + 70)); //爪
                            else if (rightHand.BaseData.itemType == ItemType.HAMMER)
                                PutByte(6, (ushort)(offset + 70)); //锤
                            else if (rightHand.BaseData.itemType == ItemType.AXE)
                                PutByte(7, (ushort)(offset + 70)); //斧
                            else if (rightHand.BaseData.itemType == ItemType.SPEAR)
                                PutByte(8, (ushort)(offset + 70)); //矛
                            else if (rightHand.BaseData.itemType == ItemType.STAFF)
                                PutByte(9, (ushort)(offset + 70)); //杖
                        }
                    }
                    else {
                        PutByte(0, (ushort)(offset + 68));
                    }

                    //riding motion
                    PutByte(3, (ushort)(offset + 71));
                    if (equips.ContainsKey(EnumEquipSlot.PET) && value.Pet != null)
                        if (value.Pet.Ride) {
                            var pet = equips[EnumEquipSlot.PET];
                            PutUShort(pet.BaseData.handMotion, (ushort)(offset + 72));
                            PutUShort(pet.BaseData.handMotion2, (ushort)(offset + 73));
                        }

                    if (equips.ContainsKey(EnumEquipSlot.PET) && value.Pet != null)
                        if (value.Pet.Ride)
                            PutUInt(equips[EnumEquipSlot.PET].ItemID, (ushort)(offset + 75));

                    //BYTE ride_color;  //乗り物の染色値
                    PutByte(0, (ushort)(offset + 80));
                    if (value.Party == null) {
                        PutByte(1, (ushort)(offset + 81)); //party name
                        PutByte(1, (ushort)(offset + 83)); //party name
                    }
                    else {
                        buf = Global.Unicode.GetBytes(value.Party.Name + "\0");
                        buff = new byte[data.Length + buf.Length];
                        data.CopyTo(buff, 0);
                        data = buff;
                        PutByte((byte)buf.Length, (ushort)(offset + 81));
                        PutBytes(buf, (ushort)(offset + 82));
                        offset += (ushort)(buf.Length - 1);
                        if (value == value.Party.Leader)
                            PutByte(1, (ushort)(offset + 83)); //party name
                        else
                            PutByte(0, (ushort)(offset + 83)); //party name
                    }

                    if (value.Ring == null) {
                        PutByte(1, (ushort)(offset + 88)); //Ring name
                        PutByte(1, (ushort)(offset + 90)); //Ring master
                    }
                    else {
                        buf = Global.Unicode.GetBytes(value.Ring.Name + "\0");
                        buff = new byte[data.Length + buf.Length];
                        data.CopyTo(buff, 0);
                        data = buff;
                        PutByte((byte)buf.Length, (ushort)(offset + 88));
                        PutBytes(buf, (ushort)(offset + 89));
                        offset += (ushort)(buf.Length - 1);
                        if (value == value.Ring.Leader)
                            PutByte(1, (ushort)(offset + 90)); //party name
                        else
                            PutByte(0, (ushort)(offset + 90)); //party name
                    }

                    buf = Global.Unicode.GetBytes(value.Sign + "\0");
                    buff = new byte[data.Length + buf.Length];
                    data.CopyTo(buff, 0);
                    data = buff;
                    PutByte((byte)buf.Length, (ushort)(offset + 91)); //sign name
                    PutBytes(buf, (ushort)(offset + 92));
                    offset += (ushort)(buf.Length - 1);


                    PutByte(1, (ushort)(offset + 93)); //shop name

                    PutUInt(1000, (ushort)(offset + 96)); //size

                    PutUShort((ushort)value.Motion, (ushort)(offset + 100));

                    PutUInt(0, (ushort)(offset + 102)); //unknown
                    switch (value.Mode) {
                        case PlayerMode.NORMAL:
                            PutInt(2, (ushort)(offset + 106)); //mode1
                            PutInt(0, (ushort)(offset + 110)); //mode2
                            break;
                        case PlayerMode.COLISEUM_MODE:
                            PutInt(0x42, (ushort)(offset + 106)); //mode1
                            PutInt(1, (ushort)(offset + 110)); //mode2
                            break;
                        case PlayerMode.KNIGHT_WAR:
                            PutInt(0x22, (ushort)(offset + 106)); //mode1
                            PutInt(2, (ushort)(offset + 110)); //mode2
                            break;
                        case PlayerMode.WRP:
                            PutInt(0x102, (ushort)(offset + 106)); //mode1
                            PutInt(4, (ushort)(offset + 110)); //mode2
                            break;
                        case PlayerMode.KNIGHT_EAST:
                            PutInt(0x22, (ushort)(offset + 106)); //mode1
                            PutInt(2, (ushort)(offset + 110)); //mode2
                            PutByte(1, (ushort)(offset + 114)); //emblem; //演習時のエンブレムとか　1東2西4南8北Aヒーロー状態
                            PutByte(0, (ushort)(offset + 115)); //metamo; //メタモーバトルのチーム　1花2岩
                            break;
                        case PlayerMode.KNIGHT_WEST:
                            PutInt(0x22, (ushort)(offset + 106)); //mode1
                            PutInt(2, (ushort)(offset + 110)); //mode2
                            PutByte(2, (ushort)(offset + 114)); //emblem; //演習時のエンブレムとか　1東2西4南8北Aヒーロー状態
                            PutByte(0, (ushort)(offset + 115)); //metamo; //メタモーバトルのチーム　1花2岩
                            break;
                        case PlayerMode.KNIGHT_SOUTH:
                            PutInt(0x22, (ushort)(offset + 106)); //mode1
                            PutInt(2, (ushort)(offset + 110)); //mode2
                            PutByte(4, (ushort)(offset + 114)); //emblem; //演習時のエンブレムとか　1東2西4南8北Aヒーロー状態
                            PutByte(0, (ushort)(offset + 115)); //metamo; //メタモーバトルのチーム　1花2岩
                            break;
                        case PlayerMode.KNIGHT_NORTH:
                            PutInt(0x22, (ushort)(offset + 106)); //mode1
                            PutInt(2, (ushort)(offset + 110)); //mode2
                            PutByte(8, (ushort)(offset + 114)); //emblem; //演習時のエンブレムとか　1東2西4南8北Aヒーロー状態
                            PutByte(0, (ushort)(offset + 115)); //metamo; //メタモーバトルのチーム　1花2岩
                            break;
                        case PlayerMode.KNIGHT_FLOWER:
                            PutInt(0x22, (ushort)(offset + 106)); //mode1
                            PutInt(2, (ushort)(offset + 110)); //mode2
                            PutByte(0, (ushort)(offset + 114)); //emblem; //演習時のエンブレムとか　1東2西4南8北Aヒーロー状態
                            PutByte(1, (ushort)(offset + 115)); //metamo; //メタモーバトルのチーム　1花2岩
                            break;
                        case PlayerMode.KNIGHT_ROCK:
                            PutInt(0x22, (ushort)(offset + 106)); //mode1
                            PutInt(2, (ushort)(offset + 110)); //mode2
                            PutByte(0, (ushort)(offset + 114)); //emblem; //演習時のエンブレムとか　1東2西4南8北Aヒーロー状態
                            PutByte(2, (ushort)(offset + 115)); //metamo; //メタモーバトルのチーム　1花2岩
                            break;
                    }

                    PutByte(0, (ushort)(offset + 116));
                    PutByte(0, (ushort)(offset + 117));
                    PutByte(0, (ushort)(offset + 118));
                    PutByte(value.Level, (ushort)(offset + 120));
                    PutUInt(value.WRPRanking, (ushort)(offset + 121));
                    PutByte(0xFF, (ushort)(offset + 129));
                }

                //#endregion

                //#region Saga17

                if (Configuration.Configuration.Instance.Version >= Version.Saga17) {
                    byte[] buf, buff;
                    byte size;
                    ushort offset;

                    PutUInt(value.ActorID, 2);
                    PutUInt(value.CharID, 6);

                    ///////////////玩家角色名///////////////
                    var name = value.Name;
                    //name = "糖果_" +value.Account.AccountID.ToString() +Global.Random.Next(10000, 99999);//万圣节活动

                    /*if (value.FirstName != "")
                        name = value.FirstName + "·" + name;*/
                    buf = Global.Unicode.GetBytes(name + "\0");
                    size = (byte)buf.Length; //角色名长度
                    buff = new byte[data.Length - 1 + size];
                    data.CopyTo(buff, 0);
                    data = buff;
                    PutByte(size, 10);
                    PutBytes(buf, 11);
                    offset = (ushort)(11 + size);

                    ////////////////玩家外观////////////////


                    //Logger.getLogger().Information(appearance.Name+" "+appearance.CharID.ToString() + " " + value.TInt["幻化"].ToString());
                    //if (value.TInt["幻化"] != 0)   //玩家处于幻化状态
                    //{
                    //    var chr =
                    //        from c in Manager.MapClientManager.Instance.OnlinePlayer
                    //        where c.Character.CharID == (uint)value.TInt["幻化"]
                    //        select c;
                    //    if (chr.Count()!=0)
                    //    {

                    //        appearance = chr.First().Character;  //tranceTarger为幻化目标。
                    //        //Logger.getLogger().Information(appearance.CharID.ToString());
                    //    }
                    //}

                    //value.appearance.

                    if (value.appearance.MarionettePictID == 0 && value.Marionette == null &&
                        value.IllusionPictID == 0 && value.TranceID == 0) {
                        PutByte(
                            (byte)(value.appearance.Race == SagaLib.PcRace.NONE ? value.Race : value.appearance.Race),
                            offset);
                        PutByte((byte)(value.appearance.Form == DEM_FORM.NONE ? value.Form : value.appearance.Form),
                            offset + 1);
                        PutByte(
                            (byte)(value.appearance.Gender == PC_GENDER.NONE ? value.Gender : value.appearance.Gender),
                            offset + 2);
                        PutUShort(value.appearance.HairStyle == 0 ? value.HairStyle : value.appearance.HairStyle,
                            offset + 3);
                        PutByte(value.appearance.HairColor == 0 ? value.HairColor : value.appearance.HairColor,
                            offset + 5);
                        PutUShort(value.appearance.Wig == 0 ? value.Wig : value.appearance.Wig, offset + 6);
                        PutByte(0xff, offset + 8);
                        PutUShort(value.appearance.Face == 0 ? value.Face : value.appearance.Face, offset + 9);

                        PutByte(0x00, offset + 11); //未知
                        //3转外观

                        PutByte(
                            value.appearance.TailStyle == byte.MaxValue ? value.TailStyle : value.appearance.TailStyle,
                            offset + 12);
                        PutByte(
                            value.appearance.WingStyle == byte.MaxValue ? value.WingStyle : value.appearance.WingStyle,
                            offset + 13);
                        PutByte(
                            value.appearance.WingColor == byte.MaxValue ? value.WingColor : value.appearance.WingColor,
                            offset + 14);
                    }
                    else //如果外观目标处于木偶或是变身状态
                    {
                        PutByte(0xff, offset);
                        PutByte(0xff, offset + 1);
                        PutByte(0xff, offset + 2);
                        PutByte(0xff, offset + 3);
                        PutByte(0xff, offset + 4);
                        PutByte(0xff, offset + 5);
                        PutByte(0xff, offset + 6);
                        PutByte(0xff, offset + 7);
                        PutByte(0xff, offset + 8);
                        PutByte(0xff, offset + 9);
                        PutByte(0xff, offset + 10);
                        PutByte(0xff, offset + 11);
                        PutByte(0xff, offset + 12);
                        PutByte(0xff, offset + 13);
                        PutByte(0xff, offset + 14);
                    }

                    PutByte(14, offset + 15);

                    ////////////////玩家装备////////////////
                    Dictionary<EnumEquipSlot, SagaDB.Item.Item> equips, appequips; //幻化外观
                    if (value.Form != DEM_FORM.MACHINA_FORM)
                        equips = value.Inventory.Equipments;
                    else
                        equips = value.Inventory.Parts;

                    appequips = value.appearance.Equips;

                    if (value.Marionette == null && value.appearance.MarionettePictID == 0) {
                        if (value.TranceID == 0 && value.IllusionPictID == 0)
                            for (var j = 0; j < 14; j++) {
                                if (appequips.ContainsKey((EnumEquipSlot)j) || equips.ContainsKey((EnumEquipSlot)j)) {
                                    //取得外观装备的内容
                                    var item = appequips.ContainsKey((EnumEquipSlot)j)
                                        ? appequips[(EnumEquipSlot)j]
                                        : equips[(EnumEquipSlot)j];
                                    if (item == null || item.Stack == 0) continue;
                                    if (item.PictID == 0)
                                        PutUInt(item.BaseData.imageID, offset + 16 + j * 4);
                                    else if (item.BaseData.itemType != ItemType.PET_NEKOMATA &&
                                             item.BaseData.itemType != ItemType.PARTNER &&
                                             item.BaseData.itemType != ItemType.RIDE_PARTNER)
                                        PutUInt(item.PictID, offset + 16 + j * 4);

                                    //斥候隐藏不用的武器外观

                                    if (value.Job == PC_JOB.HAWKEYE && j == 9 && value.TInt["斥候远程模式"] != 1 &&
                                        !item.BaseData.doubleHand)
                                        PutUInt(0, offset + 16 + j * 4);
                                    if (value.Job == PC_JOB.HAWKEYE && j == 8 && value.TInt["斥候远程模式"] == 1 &&
                                        !item.BaseData.doubleHand)
                                        PutUInt(0, offset + 16 + j * 4);
                                }
                            }
                        else
                            PutUInt(value.IllusionPictID == 0 ? value.TranceID : value.IllusionPictID, offset + 16);
                    }
                    else {
                        PutUInt(
                            value.appearance.MarionettePictID == 0
                                ? value.Marionette.PictID
                                : value.appearance.MarionettePictID, offset + 16);
                    }


                    //offset += 4;
                    ////////////////左手动作////////////////
                    PutByte(3, offset + 72);
                    if ((appequips.ContainsKey(EnumEquipSlot.LEFT_HAND) ||
                         equips.ContainsKey(EnumEquipSlot.LEFT_HAND)) &&
                        value.Marionette == null && value.TranceID == 0) {
                        var leftHand = appequips.ContainsKey(EnumEquipSlot.LEFT_HAND)
                            ? appequips[EnumEquipSlot.LEFT_HAND]
                            : equips[EnumEquipSlot.LEFT_HAND];
                        if (leftHand.BaseData.handMotion > 255)
                            PutUShort(leftHand.BaseData.handMotion, offset + 73);
                        else
                            PutByte((byte)leftHand.BaseData.handMotion, offset + 74);
                        offset += 2;
                        try {
                            PutUShort(
                                (byte)(EquipSound)Enum.Parse(typeof(EquipSound), leftHand.BaseData.itemType.ToString()),
                                offset + 75);
                            //this.PutUShort((ushort)leftHand.BaseData.itemType, offset + 75);
                        }
                        catch {
                            PutUShort(leftHand.BaseData.handMotion2, offset + 75);
                        }

                        offset += 1;
                        var it = leftHand.BaseData.itemType;
                        if (value.TInt["斥候远程模式"] == 1 && value.Job == PC_JOB.HAWKEYE) {
                            byte s = 0;
                            if (it == ItemType.BOW)
                                s = 0x0A;
                            if (it == ItemType.RIFLE)
                                s = 0x0B;
                            if (it == ItemType.GUN)
                                s = 0x0C;
                            if (it == ItemType.DUALGUN)
                                s = 0x0D;
                            PutByte(s, offset + 75);
                        }
                    }
                    else {
                        offset += 3;
                    }

                    ////////////////右手动作////////////////
                    PutByte(3, offset + 76);
                    if ((appequips.ContainsKey(EnumEquipSlot.RIGHT_HAND) ||
                         equips.ContainsKey(EnumEquipSlot.RIGHT_HAND)) &&
                        value.Marionette == null && value.TranceID == 0) {
                        var rightHand = appequips.ContainsKey(EnumEquipSlot.RIGHT_HAND)
                            ? appequips[EnumEquipSlot.RIGHT_HAND]
                            : equips[EnumEquipSlot.RIGHT_HAND];
                        if (rightHand.BaseData.handMotion > 255)
                            PutUShort(rightHand.BaseData.handMotion, offset + 77);
                        else
                            PutByte((byte)rightHand.BaseData.handMotion, offset + 78);
                        offset += 2;
                        try {
                            PutByte(
                                (byte)(EquipSound)Enum.Parse(typeof(EquipSound),
                                    rightHand.BaseData.itemType.ToString()), offset + 79);
                            //this.PutUShort((ushort)rightHand.BaseData.itemType, offset + 79);
                        }
                        catch {
                            PutByte(rightHand.BaseData.handMotion2, offset + 79);
                        }

                        offset += 1;
                    }
                    else {
                        offset += 3;
                    }

                    //////////////////骑乘//////////////////
                    PutByte(3, offset + 80);
                    if (equips.ContainsKey(EnumEquipSlot.PET) && value.Pet != null &&
                        value.Pet.Ride) {
                        var pet = equips[EnumEquipSlot.PET];
                        if (pet.BaseData.handMotion > 255)
                            PutUShort(pet.BaseData.handMotion, offset + 81);
                        else
                            PutByte((byte)pet.BaseData.handMotion, offset + 82);
                        offset += 2;
                        //this.PutByte(pet.BaseData.handMotion2, offset + 83);
                        PutByte(0xff, offset + 83);
                        PutByte(0xff, offset + 84);
                        offset += 1;
                    }
                    else {
                        offset += 3;
                    }

                    if (equips.ContainsKey(EnumEquipSlot.PET) && value.Pet != null)
                        if (value.Pet.Ride)
                            PutUInt(equips[EnumEquipSlot.PET].ItemID, offset + 84);

                    //BYTE ride_color; //乗り物の染色値
                    PutByte(0, offset + 89);

                    offset += 4; //2015年12月10日，对应449版本
                    //offset += 5;//2016年11月1日

                    ////////////////队伍信息////////////////
                    if (value.Party != null) {
                        buf = Global.Unicode.GetBytes(value.Party.Name + "\0");
                        buff = new byte[data.Length + buf.Length];
                        data.CopyTo(buff, 0);
                        data = buff;
                        PutByte((byte)buf.Length, offset + 90);
                        PutBytes(buf, offset + 91);
                        offset += (ushort)(buf.Length - 1);
                        if (value == value.Party.Leader)
                            PutByte(1, offset + 92);
                        else
                            PutByte(0, offset + 92);
                    }
                    else {
                        PutByte(1, offset + 90);
                        PutByte(1, offset + 92);
                    }

                    this.offset -= 2;
                    //UINT UNKNOMW
                    ////////////////军团信息////////////////
                    if (value.PlayerTitle != "") {
                        buf = Global.Unicode.GetBytes(value.PlayerTitle + "\0");
                        buff = new byte[data.Length + buf.Length];
                        data.CopyTo(buff, 0);
                        data = buff;
                        PutByte((byte)buf.Length, offset + 97);
                        PutBytes(buf, offset + 98);
                        offset += (ushort)(buf.Length - 1);
                        if (value.Ring != null && value == value.Ring.Leader)
                            PutByte(1, offset + 99);
                        else
                            PutByte(0, offset + 99);
                    }
                    else {
                        PutByte(1, offset + 97);
                        PutByte(1, offset + 99);
                    }

                    ///////////////聊天室信息///////////////
                    buf = Global.Unicode.GetBytes(value.Sign + "\0");
                    buff = new byte[data.Length + buf.Length];
                    data.CopyTo(buff, 0);
                    data = buff;
                    PutByte((byte)buf.Length, offset + 100);
                    PutBytes(buf, offset + 101);
                    offset += (ushort)(buf.Length - 1);

                    /////////////////露天商店////////////////
                    if (!value.Fictitious) {
                        if (MapClient.FromActorPC(value).Shopswitch == 0) {
                            PutByte(1, offset + 102);
                        }
                        else {
                            buf = Global.Unicode.GetBytes(MapClient.FromActorPC(value).Shoptitle + "\0");
                            buff = new byte[data.Length + buf.Length];
                            data.CopyTo(buff, 0);
                            data = buff;
                            PutByte((byte)buf.Length, offset + 102);
                            PutBytes(buf, offset + 103);
                            offset += (ushort)(buf.Length - 1);
                            PutByte(1, offset + 104);
                        }
                    }
                    else {
                        PutByte(1, offset + 102);
                    }

                    if (value.TInt["playersize"] != 0)
                        PutUInt((uint)value.TInt["playersize"], offset + 105);
                    else
                        PutUInt(1000, offset + 105);

                    PutUShort((ushort)value.Motion, offset + 109);
                    if (value.EMotionLoop)
                        PutByte(value.EMotion, offset + 111);

                    //this.PutUInt(0, offset + 111);//unknown

                    /////////////////阵容信息////////////////
                    switch (value.Mode) {
                        case PlayerMode.NORMAL:
                            PutUInt(2, offset + 116);
                            PutUInt(0, offset + 120);
                            break;
                        case PlayerMode.COLISEUM_MODE:
                            PutInt(0x42, (ushort)(offset + 116)); //mode1
                            PutInt(1, (ushort)(offset + 120)); //mode2
                            break;
                        case PlayerMode.KNIGHT_WAR:
                            PutInt(0x22, (ushort)(offset + 116)); //mode1
                            PutInt(2, (ushort)(offset + 120)); //mode2
                            break;
                        case PlayerMode.WRP:
                            PutInt(0x102, (ushort)(offset + 116)); //mode1
                            PutInt(4, (ushort)(offset + 120)); //mode2
                            break;
                        case PlayerMode.KNIGHT_EAST:
                            PutInt(0x22, (ushort)(offset + 116)); //mode1
                            PutInt(2, (ushort)(offset + 120)); //mode2
                            PutInt(1, (ushort)(offset + 124)); //emblem; //演習時のエンブレムとか　1東2西4南8北Aヒーロー状態
                            //this.PutInt(0, (ushort)(offset + 115));//metamo; //メタモーバトルのチーム　1花2岩
                            break;
                        case PlayerMode.KNIGHT_WEST:
                            PutInt(0x22, (ushort)(offset + 116)); //mode1
                            PutInt(2, (ushort)(offset + 120)); //mode2
                            PutInt(2, (ushort)(offset + 124)); //emblem; //演習時のエンブレムとか　1東2西4南8北Aヒーロー状態
                            //this.PutInt(0, (ushort)(offset + 115));//metamo; //メタモーバトルのチーム　1花2岩
                            break;
                        case PlayerMode.KNIGHT_SOUTH:
                            PutInt(0x22, (ushort)(offset + 116)); //mode1
                            PutInt(2, (ushort)(offset + 120)); //mode2
                            PutInt(4, (ushort)(offset + 124)); //emblem; //演習時のエンブレムとか　1東2西4南8北Aヒーロー状態
                            //this.PutInt(0, (ushort)(offset + 115));//metamo; //メタモーバトルのチーム　1花2岩
                            break;
                        case PlayerMode.KNIGHT_NORTH:
                            PutInt(0x22, (ushort)(offset + 116)); //mode1
                            PutInt(2, (ushort)(offset + 120)); //mode2
                            PutInt(8, (ushort)(offset + 124)); //emblem; //演習時のエンブレムとか　1東2西4南8北Aヒーロー状態
                            //this.PutInt(0, (ushort)(offset + 115));//metamo; //メタモーバトルのチーム　1花2岩
                            break;
                    }

                    PutByte(0x00, offset + 128); //不明
                    this.offset += 1;
                    /////////////////等级信息////////////////
                    PutUInt(value.Level, offset + 128);
                    PutUInt(value.WRPRanking, offset + 132);


                    this.offset -= 1;

                    PutByte(0xFF, offset + 141); //师徒图标
                    PutByte(value.WaitType, offset + 143); //2015年12月28日加入
                    PutUShort(value.UsingPaperID, offset + 144); //2015年12月28日加入
                    PutByte(4, offset + 146);
                    PutUInt(value.PossessionPartnerSlotIDinRightHand, offset + 147);
                    PutUInt(value.PossessionPartnerSlotIDinLeftHand, offset + 151);
                    PutUInt(value.PossessionPartnerSlotIDinAccesory, offset + 155);
                    PutUInt(value.PossessionPartnerSlotIDinClothes, offset + 159);
                    PutByte(4, offset + 163);
                    PutUInt(value.PossessionPartnerSlotIDinRightHand, offset + 164);
                    PutUInt(value.PossessionPartnerSlotIDinLeftHand, offset + 168);
                    PutUInt(value.PossessionPartnerSlotIDinAccesory, offset + 172);
                    PutUInt(value.PossessionPartnerSlotIDinClothes, offset + 176);
                    //unknown
                    PutByte(4, offset + 180);
                    PutUInt(0, offset + 181);
                    PutUInt(0, offset + 185);
                    PutUInt(0, offset + 189);
                    PutUInt(0, offset + 193);
                    PutUInt(0, offset + 197); //unknown
                    //称号部分
                    PutByte(3, offset + 202);
                    PutUInt((uint)value.AInt["称号_主语"], offset + 203);
                    PutUInt((uint)value.AInt["称号_连词"], offset + 207);
                    PutUInt((uint)value.AInt["称号_谓语"], offset + 211);
                    //PutUInt((uint)value.AInt["称号_战斗"], offset + 176);
                }

                //#endregion
            }
        }


        private ActorPet SetPet {
            set {
                //#region Saga14

                if (Configuration.Configuration.Instance.Version >= Version.Saga9 &&
                    Configuration.Configuration.Instance.Version < Version.Saga14_2) {
                    byte[] buf, buff;
                    byte size;
                    ushort offset;
                    PutUInt(value.ActorID, 2);
                    PutUInt(0xFFFFFFFF, 6);

                    buf = Global.Unicode.GetBytes(value.Name + "\0");
                    size = (byte)buf.Length;
                    buff = new byte[data.Length - 1 + size];
                    data.CopyTo(buff, 0);
                    data = buff;

                    PutByte(size, 10);
                    PutBytes(buf, 11);
                    offset = (ushort)(11 + size);

                    PutByte(0xff, offset);
                    if (Configuration.Configuration.Instance.Version >= Version.Saga10) {
                        offset++;
                        PutByte(0xff, offset);
                    }

                    PutByte(0xff, (ushort)(offset + 1));
                    PutByte(0xff, (ushort)(offset + 2));
                    PutByte(0xff, (ushort)(offset + 3));
                    PutByte(0xff, (ushort)(offset + 4));
                    PutByte(0xff, (ushort)(offset + 5));
                    PutByte(0xff, (ushort)(offset + 6));
                    PutByte(0xff, (ushort)(offset + 7));
                    PutByte(0xff, (ushort)(offset + 8));
                    PutByte(0xff, (ushort)(offset + 9));
                    if (Configuration.Configuration.Instance.Version >= Version.Saga11) {
                        PutByte(0xff, (ushort)(++offset + 9));
                        PutByte(0xff, (ushort)(++offset + 9));
                        PutByte(0xff, (ushort)(++offset + 9));
                    }

                    PutByte(0x0D, (ushort)(offset + 10));

                    if (value.PictID != 0)
                        PutUInt(value.PictID, (ushort)(offset + 11));
                    else if (value.BaseData.pictid != 0)
                        PutUInt(value.BaseData.pictid, (ushort)(offset + 11));
                    else
                        PutUInt(value.PetID, (ushort)(offset + 11));


                    //left hand weapon motion
                    PutByte(3, (ushort)(offset + 63));
                    //right hand weapon motion
                    PutByte(3, (ushort)(offset + 67));
                    //riding motion
                    PutByte(3, (ushort)(offset + 71));

                    PutByte(0, (ushort)(offset + 80));
                    PutByte(1, (ushort)(offset + 81)); //party name
                    PutByte(1, (ushort)(offset + 83)); //party name

                    PutByte(1, (ushort)(offset + 88)); //Ring name

                    PutByte(1, (ushort)(offset + 90)); //Ring master

                    PutByte(1, (ushort)(offset + 91)); //Sign name

                    PutByte(1, (ushort)(offset + 93)); //shop name

                    PutUInt(1000, (ushort)(offset + 96)); //size
                    PutUInt(2, (ushort)(offset + 106)); //size

                    PutUShort(0, (ushort)(offset + 110));

                    PutByte(0, (ushort)(offset + 114)); //emblem; //演習時のエンブレムとか　1東2西4南8北Aヒーロー状態
                    PutByte(0, (ushort)(offset + 115)); //metamo; //メタモーバトルのチーム　1花2岩
                    PutByte(0, (ushort)(offset + 116));
                    PutByte(0, (ushort)(offset + 117));
                    PutByte(0, (ushort)(offset + 118));
                    PutByte(1, (ushort)(offset + 119));
                    PutUInt(0xffffffff, (ushort)(offset + 120)); //// WRP順位（ペットは -1固定。別のパケで主人の値が送られてくる
                    /*
                    DWORD unknown; // 0?
                    DWORD mode1   //2 r0fa7参照
                    DWORD mode2   //0 r0fa7参照
                    BYTE emblem; //演習時のエンブレムとか　1東2西4南8北Aヒーロー状態
                    BYTE metamo; //メタモーバトルのチーム　1花2岩
                    BYTE unknown; //1にすると/joyのモーションを取る（マリオネット変身時。）2にすると～
                    BYTE unknown; // 0?
                    BYTE guest; // ゲストIDかどうか
                    BYTE level; // レベル（ペットは1固定
                    DWORD wrp_rank; // WRP順位（ペットは -1固定。別のパケで主人の値が送られてくる
                    */
                }

                //#endregion

                //#region Saga14_2

                if (Configuration.Configuration.Instance.Version >= Version.Saga14_2 &&
                    Configuration.Configuration.Instance.Version < Version.Saga17) {
                    data = new byte[154];
                    this.offset = 2;
                    ID = 0x020E;

                    byte[] buf, buff;
                    byte size;
                    ushort offset;
                    PutUInt(value.ActorID, 2);
                    PutUInt(0xFFFFFFFF, 6);

                    buf = Global.Unicode.GetBytes(value.Name + "\0");
                    size = 22;
                    buff = new byte[data.Length - 1 + size];
                    data.CopyTo(buff, 0);
                    data = buff;

                    PutByte(size, 10);
                    PutBytes(buf, 11);
                    offset = (ushort)(11 + size);

                    PutByte(0xff, offset);
                    if (Configuration.Configuration.Instance.Version >= Version.Saga10) {
                        offset++;
                        PutByte(0xff, offset);
                    }

                    PutByte(0xff, (ushort)(offset + 1));
                    PutByte(0xff, (ushort)(offset + 2));
                    PutByte(0xff, (ushort)(offset + 3));
                    PutByte(0xff, (ushort)(offset + 4));
                    PutByte(0xff, (ushort)(offset + 5));
                    PutByte(0xff, (ushort)(offset + 6));
                    PutByte(0xff, (ushort)(offset + 7));
                    PutByte(0xff, (ushort)(offset + 8));
                    PutByte(0xff, (ushort)(offset + 9));
                    if (Configuration.Configuration.Instance.Version >= Version.Saga11) {
                        PutByte(0xff, (ushort)(++offset + 9));
                        PutByte(0xff, (ushort)(++offset + 9));
                        PutByte(0xff, (ushort)(++offset + 9));
                    }

                    if (Configuration.Configuration.Instance.Version >= Version.Saga14_2)
                        PutByte(0xff, (ushort)(++offset + 9));
                    PutByte(0x0D, (ushort)(offset + 10));

                    if (value.PictID != 0)
                        PutUInt(value.PictID, (ushort)(offset + 11));
                    else if (value.BaseData.pictid != 0)
                        PutUInt(value.BaseData.pictid, (ushort)(offset + 11));
                    else
                        PutUInt(value.PetID, (ushort)(offset + 11));


                    //left hand weapon motion
                    PutByte(3, (ushort)(offset + 63));
                    //right hand weapon motion
                    PutByte(3, (ushort)(offset + 67));
                    //riding motion
                    PutByte(3, (ushort)(offset + 71));

                    PutByte(0, (ushort)(offset + 80));
                    PutByte(1, (ushort)(offset + 81)); //party name
                    PutByte(0, (ushort)(offset + 83)); //party name

                    PutByte(1, (ushort)(offset + 88)); //Ring name

                    PutByte(0, (ushort)(offset + 90)); //Ring master

                    PutByte(1, (ushort)(offset + 91)); //Sign name

                    PutByte(1, (ushort)(offset + 93)); //shop name

                    PutUInt(1100, (ushort)(offset + 96)); //size
                    PutUInt(2, (ushort)(offset + 106)); //size

                    PutUShort(0, (ushort)(offset + 110));

                    PutByte(0, (ushort)(offset + 114)); //emblem; //演習時のエンブレムとか　1東2西4南8北Aヒーロー状態
                    PutByte(0, (ushort)(offset + 115)); //metamo; //メタモーバトルのチーム　1花2岩
                    PutByte(0, (ushort)(offset + 116));
                    PutByte(0, (ushort)(offset + 117));
                    PutByte(0, (ushort)(offset + 118));
                    PutByte(1, (ushort)(offset + 119));
                    PutUInt(0xffffffff, (ushort)(offset + 120)); //// WRP順位（ペットは -1固定。別のパケで主人の値が送られてくる
                    PutUInt(0xffffffff, (ushort)(offset + 124));
                    PutByte(0xff, (ushort)(offset + 128));
                    /*
                    DWORD unknown; // 0?
                    DWORD mode1   //2 r0fa7参照
                    DWORD mode2   //0 r0fa7参照
                    BYTE emblem; //演習時のエンブレムとか　1東2西4南8北Aヒーロー状態
                    BYTE metamo; //メタモーバトルのチーム　1花2岩
                    BYTE unknown; //1にすると/joyのモーションを取る（マリオネット変身時。）2にすると～
                    BYTE unknown; // 0?
                    BYTE guest; // ゲストIDかどうか
                    BYTE level; // レベル（ペットは1固定
                    DWORD wrp_rank; // WRP順位（ペットは -1固定。別のパケで主人の値が送られてくる
                    */
                }

                //#endregion

                //#region Saga17

                if (Configuration.Configuration.Instance.Version >= Version.Saga17) {
                    byte[] buf, buff;
                    byte size;
                    ushort offset;

                    PutUInt(value.ActorID, 2);
                    PutUInt(0xFFFFFFFF, 6);

                    ///////////////玩家角色名///////////////
                    buf = Global.Unicode.GetBytes(value.Name + "\0");
                    size = (byte)buf.Length; //角色名长度
                    buff = new byte[data.Length - 1 + size];
                    data.CopyTo(buff, 0);
                    data = buff;
                    PutByte(size, 10);
                    PutBytes(buf, 11);
                    offset = (ushort)(11 + size);

                    ////////////////玩家外观////////////////
                    PutByte(0xff, offset);
                    PutByte(0xff, offset + 1);
                    PutByte(0xff, offset + 2);
                    PutByte(0xff, offset + 3);
                    PutByte(0xff, offset + 4);
                    PutByte(0xff, offset + 5);
                    PutByte(0xff, offset + 6);
                    PutByte(0xff, offset + 7);
                    PutByte(0xff, offset + 8);
                    PutByte(0xff, offset + 9);
                    PutByte(0xff, offset + 10);
                    PutByte(0xff, offset + 11);
                    PutByte(0xff, offset + 12);
                    PutByte(0xff, offset + 13);
                    PutByte(0xff, offset + 14);


                    PutByte(0x0E, offset + 15);

                    ////////////////玩家装备////////////////
                    if (value.PictID != 0)
                        PutUInt(value.PictID, (ushort)(offset + 16));
                    else if (value.BaseData.pictid != 0)
                        PutUInt(value.BaseData.pictid, (ushort)(offset + 16));
                    else
                        PutUInt(value.PetID, (ushort)(offset + 16));

                    ////////////////左手动作////////////////
                    PutByte(3, offset + 72);

                    ////////////////右手动作////////////////
                    PutByte(3, offset + 76);

                    //////////////////骑乘//////////////////
                    PutByte(3, offset + 80);

                    //BYTE ride_color; //乗り物の染色値
                    PutByte(0, offset + 89);

                    offset += 4; //2015年12月10日，对应449版本

                    ////////////////队伍信息////////////////
                    PutByte(1, offset + 90);
                    PutByte(0, offset + 92);
                    //UINT UNKNOMW
                    ////////////////军团信息////////////////
                    PutByte(1, offset + 97);
                    PutByte(0, offset + 99);

                    ///////////////聊天室信息///////////////
                    PutByte(1, offset + 100);

                    /////////////////露天商店////////////////
                    PutByte(1, offset + 102);

                    PutUInt(1000, offset + 105);

                    offset++; //2015年12月11日，对应449版本

                    /////////////////阵容信息////////////////
                    PutUInt(2, offset + 115);
                    PutUInt(0, offset + 119);

                    /////////////////等级信息////////////////
                    PutByte(0x1, offset + 131);
                    /*if (Manager.MapManager.Instance.GetMap(value.MapID).Info.Flag.Test(SagaDB.Map.MapFlags.Dominion))
                        this.PutUInt(0xffffffff, offset + 128);
                    else
                        this.PutUInt(0xffffffff, offset + 128);*/
                    PutUInt(0xffffffff, offset + 132);
                    PutUInt(0xffffffff, offset + 136);
                    PutByte(0xff, offset + 140); //2015年12月11日，对应449版本
                    PutUInt(0xffffffff, offset + 145); //2015年12月11日，对应449版本
                    //this.PutByte(0xFF, offset + 144);//师徒图标


                    //2015年12月11日，对应449版本
                    buff = new byte[data.Length + 1];
                    data.CopyTo(buff, 0);
                    data = buff;
                }

                //#endregion
            }
        }

        private ActorPartner SetPartner {
            set {
                //#region Saga17

                if (Configuration.Configuration.Instance.Version >= Version.Saga17) {
                    byte[] buf, buff;
                    byte size;
                    ushort offset;

                    PutUInt(value.ActorID, 2);
                    PutUInt(0xFFFFFFFF, 6);

                    buf = Global.Unicode.GetBytes(value.Name + "\0");
                    size = (byte)buf.Length; //角色名长度
                    buff = new byte[data.Length - 1 + size];
                    data.CopyTo(buff, 0);
                    data = buff;
                    PutByte(size, 10);
                    PutBytes(buf, 11);
                    offset = (ushort)(11 + size);

                    PutByte(0xff, offset);
                    PutByte(0xff, offset + 1);
                    PutByte(0xff, offset + 2);
                    PutByte(0xff, offset + 3);
                    PutByte(0xff, offset + 4);
                    PutByte(0xff, offset + 5);
                    PutByte(0xff, offset + 6);
                    PutByte(0xff, offset + 7);
                    PutByte(0xff, offset + 8);
                    PutByte(0xff, offset + 9);
                    PutByte(0xff, offset + 10);
                    PutByte(0xff, offset + 11);
                    PutByte(0xff, offset + 12);
                    PutByte(0xff, offset + 13);
                    PutByte(0xff, offset + 14);
                    PutByte(14, offset + 15);
                    if (value.PictID != 0)
                        PutUInt(value.PictID, (ushort)(offset + 16));

                    else if (value.BaseData.pictid != 0)
                        PutUInt(value.BaseData.pictid, (ushort)(offset + 16));
                    PutByte(3, offset + 72);
                    PutByte(3, offset + 79);
                    PutByte(3, offset + 86);

                    PutByte(0, offset + 89);

                    //offset += 4;//2015年12月10日，对应449版本
                    PutByte(1, offset + 103);
                    //this.PutByte(0, offset + );
                    ////////////////军团信息////////////////
                    PutByte(1, offset + 110);
                    //this.PutByte(0, offset + 99);
                    PutByte(1, offset + 113);

                    PutByte(1, offset + 115);

                    PutUInt(1000, offset + 118);

                    PutUShort((ushort)value.Motion, offset + 122);
                    //offset++;//2015年12月11日，对应449版本

                    PutUInt(2, offset + 129);
                    //this.PutUInt(0, offset + 119);

                    PutByte(0x1, offset + 145);
                    PutUInt(0xffffffff, offset + 146);
                    PutUInt(0xffffffff, offset + 150);
                    PutByte(0xff, offset + 154); //2015年12月11日，对应449版本
                    PutUInt(0xffffffff, offset + 162);
                    buff = new byte[data.Length + 1];
                    data.CopyTo(buff, 0);
                    data = buff;
                }

                //#endregion
            }
        }

        public SagaDB.Actor.Actor Actor {
            set {
                if (value.type == ActorType.PC)
                    SetPC = (ActorPC)value;
                else if (value.type == ActorType.PET)
                    SetPet = (ActorPet)value;
                else if (value.type == ActorType.SHADOW)
                    SetShadow = (ActorShadow)value;
                else if (value.type == ActorType.MOB)
                    SetMob = (ActorMob)value;
                else if (value.type == ActorType.PARTNER) SetPartner = (ActorPartner)value;
            }
        }
    }
}