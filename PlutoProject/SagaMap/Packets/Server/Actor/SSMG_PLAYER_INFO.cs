using System;
using SagaDB.Actor;
using SagaDB.Item;
using SagaLib;
using SagaMap.Manager;
using Version = SagaLib.Version;

namespace SagaMap.Packets.Server
{
    public class SSMG_PLAYER_INFO : Packet
    {
        public SSMG_PLAYER_INFO()
        {
            if (Configuration.Instance.Version == Version.Saga6)
            {
                data = new byte[210];
                offset = 2;
                ID = 0x01FF;
            }

            if (Configuration.Instance.Version >= Version.Saga9)
            {
                data = new byte[219];
                offset = 2;
                ID = 0x01FF;
            }

            if (Configuration.Instance.Version >= Version.Saga10)
            {
                data = new byte[222];
                offset = 2;
                ID = 0x01FF;
            }

            if (Configuration.Instance.Version >= Version.Saga11)
            {
                data = new byte[225];
                offset = 2;
                ID = 0x01FF;
            }

            if (Configuration.Instance.Version >= Version.Saga13)
            {
                data = new byte[243];
                offset = 2;
                ID = 0x01FF;
            }

            if (Configuration.Instance.Version >= Version.Saga14)
            {
                data = new byte[252];
                offset = 2;
                ID = 0x01FF;
            }

            if (Configuration.Instance.Version >= Version.Saga14_2)
            {
                data = new byte[235];
                offset = 2;
                ID = 0x01FF;
            }

            if (Configuration.Instance.Version >= Version.Saga17)
            {
                uint length = 250;
                length += 8; //15年12月8日加入，对应版本449
                length += 4; //15年12月25日加入，对应版本452
                length += 8; //16年6月28日加入，对应版本469
                length += 9; //16年6月28日加入，对应版本482
                length += 17; //17年4月1日加入，对应版本497，增加称号系统
                data = new byte[length];
                offset = 2;
                ID = 0x01FF;
            }
        }

        public ActorPC Player
        {
            set
            {
                #region Saga6

                if (Configuration.Instance.Version == Version.Saga6)
                {
                    var info = MapManager.Instance.GetMap(value.MapID);
                    PutUInt(value.ActorID, 2);
                    PutUInt(value.CharID, 6);
                    var name = value.Name;
                    name = name.Replace("\0", "");
                    var buf = Global.Unicode.GetBytes(name);
                    var buff = new byte[211 + buf.Length];
                    data.CopyTo(buff, 0);
                    data = buff;
                    var offset = (ushort)(12 + buf.Length);
                    PutByte((byte)(buf.Length + 1), 10);
                    PutBytes(buf, 11);
                    PutByte((byte)value.Race, offset);
                    PutByte((byte)value.Gender, (ushort)(offset + 1));
                    PutByte((byte)value.HairStyle, (ushort)(offset + 2));
                    PutByte(value.HairColor, (ushort)(offset + 3));
                    PutByte((byte)value.Wig, (ushort)(offset + 4));
                    PutByte(0xff, (ushort)(offset + 5));
                    PutUShort(value.Face, (ushort)(offset + 6));
                    PutUInt(value.MapID, (ushort)(offset + 10));
                    PutByte(Global.PosX16to8(value.X, info.Width), (ushort)(offset + 14));
                    PutByte(Global.PosY16to8(value.Y, info.Height), (ushort)(offset + 15));
                    PutByte((byte)(value.Dir / 45), (ushort)(offset + 16));
                    PutUInt(value.HP, (ushort)(offset + 17));
                    PutUInt(value.MaxHP, (ushort)(offset + 21));
                    PutUInt(value.MP, (ushort)(offset + 25));
                    PutUInt(value.MaxMP, (ushort)(offset + 29));
                    PutUInt(value.SP, (ushort)(offset + 33));
                    PutUInt(value.MaxSP, (ushort)(offset + 37));
                    PutByte(8, (ushort)(offset + 41));
                    PutUShort(value.Str, (ushort)(offset + 42));
                    PutUShort(value.Dex, (ushort)(offset + 44));
                    PutUShort(value.Int, (ushort)(offset + 46));
                    PutUShort(value.Vit, (ushort)(offset + 48));
                    PutUShort(value.Agi, (ushort)(offset + 50));
                    PutUShort(value.Mag, (ushort)(offset + 52));
                    PutUShort(13, (ushort)(offset + 54)); //luk
                    PutUShort(0, (ushort)(offset + 56)); //cha
                    PutByte(0x14, (ushort)(offset + 58)); //unknown
                    if (value.PossessionTarget == 0)
                    {
                        PutUInt(0xFFFFFFFF, (ushort)(offset + 101)); //possession target
                    }
                    else
                    {
                        var possession = info.GetActor(value.PossessionTarget);
                        if (possession.type != ActorType.ITEM)
                            PutUInt(value.PossessionTarget, (ushort)(offset + 101)); //possession target
                        else
                            PutUInt(value.ActorID, (ushort)(offset + 101)); //possession target                    
                    }

                    if (value.PossessionTarget == 0)
                        PutByte(0xFF, (ushort)(offset + 105)); //possession place
                    else
                        PutByte((byte)value.PossessionPosition, (ushort)(offset + 105)); //possession place
                    PutUInt((uint)value.Gold, (ushort)(offset + 106));
                    PutByte((byte)value.Status.attackType, (ushort)(offset + 110));
                    PutByte(13, (ushort)(offset + 111));
                    for (var j = 0; j < 14; j++)
                        if (value.Inventory.Equipments.ContainsKey((EnumEquipSlot)j))
                        {
                            var item = value.Inventory.Equipments[(EnumEquipSlot)j];
                            if (item.Stack == 0) continue;
                            if (item.PictID == 0)
                                PutUInt(item.BaseData.imageID, (ushort)(offset + 112 + j * 4));
                            else
                                PutUInt(item.PictID, (ushort)(offset + 112 + j * 4));
                        }

                    //left hand weapon motion
                    PutByte(3, (ushort)(offset + 164));
                    if (value.Inventory.Equipments.ContainsKey(EnumEquipSlot.LEFT_HAND))
                    {
                        var leftHand = value.Inventory.Equipments[EnumEquipSlot.LEFT_HAND];
                        PutUShort(leftHand.BaseData.handMotion, (ushort)(offset + 165));
                        PutUShort(leftHand.BaseData.handMotion2, (ushort)(offset + 166));
                    }
                    else
                    {
                        PutByte(0, (ushort)(offset + 165));
                    }

                    //right hand weapon motion
                    PutByte(3, (ushort)(offset + 168));
                    if (value.Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND))
                    {
                        var rightHand = value.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND];
                        PutUShort(rightHand.BaseData.handMotion, (ushort)(offset + 169));
                        PutUShort(rightHand.BaseData.handMotion2, (ushort)(offset + 170));
                    }
                    else
                    {
                        PutByte(0, (ushort)(offset + 169));
                    }

                    //riding motion
                    PutByte(3, (ushort)(offset + 172));
                    if (value.Inventory.Equipments.ContainsKey(EnumEquipSlot.PET) && value.Pet != null)
                        if (value.Pet.Ride)
                        {
                            var pet = value.Inventory.Equipments[EnumEquipSlot.PET];
                            PutUShort(pet.BaseData.handMotion, (ushort)(offset + 173));
                            PutUShort(pet.BaseData.handMotion2, (ushort)(offset + 174));
                        }

                    if (value.Inventory.Equipments.ContainsKey(EnumEquipSlot.PET) && value.Pet != null)
                        if (value.Pet.Ride)
                            PutUInt(value.Inventory.Equipments[EnumEquipSlot.PET].ItemID, (ushort)(offset + 176));

                    //BYTE ride_color;  //乗り物の染色値
                    PutUInt(value.Range, (ushort)(offset + 181));
                    switch (value.Mode)
                    {
                        case PlayerMode.NORMAL:
                            PutInt(2, (ushort)(offset + 189)); //mode1
                            PutInt(0, (ushort)(offset + 193)); //mode2
                            break;
                        case PlayerMode.COLISEUM_MODE:
                            PutInt(0x42, (ushort)(offset + 189)); //mode1
                            PutInt(1, (ushort)(offset + 193)); //mode2
                            break;
                        case PlayerMode.KNIGHT_WAR:
                            PutInt(0x22, (ushort)(offset + 189)); //mode1
                            PutInt(2, (ushort)(offset + 193)); //mode2
                            break;
                        case PlayerMode.WRP:
                            PutInt(0x102, (ushort)(offset + 189)); //mode1
                            PutInt(4, (ushort)(offset + 193)); //mode2
                            break;
                        case PlayerMode.KNIGHT_EAST:
                            PutInt(0x22, (ushort)(offset + 189)); //mode1
                            PutInt(2, (ushort)(offset + 193)); //mode2
                            PutByte(1, (ushort)(offset + 197)); //emblem; //演習時のエンブレムとか　1東2西4南8北Aヒーロー状態
                            PutByte(0, (ushort)(offset + 198)); //metamo; //メタモーバトルのチーム　1花2岩
                            break;
                        case PlayerMode.KNIGHT_WEST:
                            PutInt(0x22, (ushort)(offset + 189)); //mode1
                            PutInt(2, (ushort)(offset + 193)); //mode2
                            PutByte(2, (ushort)(offset + 197)); //emblem; //演習時のエンブレムとか　1東2西4南8北Aヒーロー状態
                            PutByte(0, (ushort)(offset + 198)); //metamo; //メタモーバトルのチーム　1花2岩
                            break;
                        case PlayerMode.KNIGHT_SOUTH:
                            PutInt(0x22, (ushort)(offset + 189)); //mode1
                            PutInt(2, (ushort)(offset + 193)); //mode2
                            PutByte(4, (ushort)(offset + 197)); //emblem; //演習時のエンブレムとか　1東2西4南8北Aヒーロー状態
                            PutByte(0, (ushort)(offset + 198)); //metamo; //メタモーバトルのチーム　1花2岩
                            break;
                        case PlayerMode.KNIGHT_NORTH:
                            PutInt(0x22, (ushort)(offset + 189)); //mode1
                            PutInt(2, (ushort)(offset + 193)); //mode2
                            PutByte(8, (ushort)(offset + 197)); //emblem; //演習時のエンブレムとか　1東2西4南8北Aヒーロー状態
                            PutByte(0, (ushort)(offset + 198)); //metamo; //メタモーバトルのチーム　1花2岩
                            break;
                        case PlayerMode.KNIGHT_FLOWER:
                            PutInt(0x22, (ushort)(offset + 189)); //mode1
                            PutInt(2, (ushort)(offset + 193)); //mode2
                            PutByte(0, (ushort)(offset + 197)); //emblem; //演習時のエンブレムとか　1東2西4南8北Aヒーロー状態
                            PutByte(1, (ushort)(offset + 198)); //metamo; //メタモーバトルのチーム　1花2岩
                            break;
                        case PlayerMode.KNIGHT_ROCK:
                            PutInt(0x22, (ushort)(offset + 189)); //mode1
                            PutInt(2, (ushort)(offset + 193)); //mode2
                            PutByte(0, (ushort)(offset + 197)); //emblem; //演習時のエンブレムとか　1東2西4南8北Aヒーロー状態
                            PutByte(2, (ushort)(offset + 198)); //metamo; //メタモーバトルのチーム　1花2岩
                            break;
                    }

                    /*int  ridepet_id; //(itemid
            byte ridepet_color;//ロボ用
            int range     //武器の射程
            int unknown   //0?
            int mode1   //2 r0fa7参照
            int mode2   //0 r0fa7参照
            byte  unknown //0?
            byte  guest //ゲストIDかどうか (07/11/22より)
            */
                }

                #endregion

                #region Saga9/Saga9_2

                if (Configuration.Instance.Version >= Version.Saga9 &&
                    Configuration.Instance.Version < Version.Saga14_2)
                {
                    var info = MapManager.Instance.GetMap(value.MapID);
                    PutUInt(value.ActorID, 2);
                    PutUInt(value.CharID, 6);
                    var name = value.Name;
                    name = name.Replace("\0", "");
                    var buf = Global.Unicode.GetBytes(name);
                    var buff = new byte[data.Length + buf.Length];
                    data.CopyTo(buff, 0);
                    data = buff;
                    var offset = (ushort)(12 + buf.Length);
                    PutByte((byte)(buf.Length + 1), 10);
                    PutBytes(buf, 11);
                    PutByte((byte)value.Race, offset);
                    if (Configuration.Instance.Version >= Version.Saga10)
                    {
                        offset++;
                        PutByte((byte)value.Form, offset);
                    }

                    PutByte((byte)value.Gender, (ushort)(offset + 1));
                    if (Configuration.Instance.Version >= Version.Saga11)
                    {
                        offset++;
                        PutByte(0, (ushort)(offset + 1)); //Unknown
                    }

                    PutByte((byte)value.HairStyle, (ushort)(offset + 2));
                    PutByte(value.HairColor, (ushort)(offset + 3));
                    PutUShort(value.Wig, (ushort)(offset + 4));
                    //this.PutByte(0xff, (ushort)(offset + 5));
                    if (Configuration.Instance.Version >= Version.Saga11)
                    {
                        offset++;
                        PutByte(0xFF, (ushort)(offset + 5)); //Unknown
                    }

                    PutUShort(value.Face, (ushort)(offset + 6));
                    if (Configuration.Instance.Version >= Version.Saga11)
                    {
                        offset++;
                        if (value.Rebirth || value.Job == value.Job3)
                            PutByte(0x1, (ushort)(offset + 6)); //Unknown
                        else
                            PutByte(0x0, (ushort)(offset + 6)); //Unknown
                        PutByte(value.TailStyle, (ushort)(offset + 7)); //3轉外觀
                        PutByte(value.WingStyle, (ushort)(offset + 8)); //3轉外觀
                        PutByte(value.WingColor, (ushort)(offset + 9)); //3轉外觀
                    }

                    PutUInt(value.MapID, (ushort)(offset + 10));
                    PutByte(Global.PosX16to8(value.X, info.Width), (ushort)(offset + 14));
                    PutByte(Global.PosY16to8(value.Y, info.Height), (ushort)(offset + 15));
                    PutByte((byte)(value.Dir / 45), (ushort)(offset + 16));
                    PutUInt(value.HP, (ushort)(offset + 17));
                    PutUInt(value.MaxHP, (ushort)(offset + 21));
                    PutUInt(value.MP, (ushort)(offset + 25));
                    PutUInt(value.MaxMP, (ushort)(offset + 29));
                    PutUInt(value.SP, (ushort)(offset + 33));
                    PutUInt(value.MaxSP, (ushort)(offset + 37));
                    PutUInt(value.EP, (ushort)(offset + 41)); //ep
                    PutUInt(value.MaxEP, (ushort)(offset + 45)); //max ep
                    if (Configuration.Instance.Version >= Version.Saga10)
                    {
                        PutShort(value.CL, (ushort)(offset + 49));
                        offset += 2;
                    }

                    PutByte(8, (ushort)(offset + 49));
                    PutUShort(value.Str, (ushort)(offset + 50));
                    PutUShort(value.Dex, (ushort)(offset + 52));
                    PutUShort(value.Int, (ushort)(offset + 54));
                    PutUShort(value.Vit, (ushort)(offset + 56));
                    PutUShort(value.Agi, (ushort)(offset + 58));
                    PutUShort(value.Mag, (ushort)(offset + 60));
                    PutUShort(13, (ushort)(offset + 62)); //luk
                    PutUShort(0, (ushort)(offset + 64)); //cha
                    PutByte(0x14, (ushort)(offset + 66)); //unknown
                    if (value.PossessionTarget == 0)
                    {
                        PutUInt(0xFFFFFFFF, (ushort)(offset + 109)); //possession target
                    }
                    else
                    {
                        var possession = info.GetActor(value.PossessionTarget);
                        if (possession.type != ActorType.ITEM)
                            PutUInt(value.PossessionTarget, (ushort)(offset + 109)); //possession target
                        else
                            PutUInt(value.ActorID, (ushort)(offset + 109)); //possession target                    
                    }

                    if (value.PossessionTarget == 0)
                        PutByte(0xFF, (ushort)(offset + 113)); //possession place
                    else
                        PutByte((byte)value.PossessionPosition, (ushort)(offset + 113)); //possession place
                    PutUInt((uint)value.Gold, (ushort)(offset + 114));
                    PutByte((byte)value.Status.attackType, (ushort)(offset + 118));
                    PutByte(13, (ushort)(offset + 119));
                    for (var j = 0; j < 1; j++)
                        if (value.Inventory.Equipments.ContainsKey((EnumEquipSlot)j))
                        {
                            var item = value.Inventory.Equipments[(EnumEquipSlot)j];
                            if (item.Stack == 0) continue;
                            if (item.PictID == 0)
                                PutUInt(item.BaseData.imageID, (ushort)(offset + 120 + j * 4));
                            else
                                PutUInt(item.PictID, (ushort)(offset + 120 + j * 4));
                        }

                    //left hand weapon motion
                    PutByte(3, (ushort)(offset + 172));
                    if (value.Inventory.Equipments.ContainsKey(EnumEquipSlot.LEFT_HAND))
                    {
                        var leftHand = value.Inventory.Equipments[EnumEquipSlot.LEFT_HAND];
                        PutUShort(leftHand.BaseData.handMotion, (ushort)(offset + 173));
                        PutUShort(leftHand.BaseData.handMotion2, (ushort)(offset + 173));
                    }
                    else
                    {
                        PutByte(0, (ushort)(offset + 173));
                    }

                    //right hand weapon motion
                    PutByte(3, (ushort)(offset + 176));
                    if (value.Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND))
                    {
                        var rightHand = value.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND];
                        PutUShort(rightHand.BaseData.handMotion, (ushort)(offset + 177));
                        PutUShort(rightHand.BaseData.handMotion2, (ushort)(offset + 178));
                        if (rightHand.BaseData.itemType == ItemType.SHORT_SWORD ||
                            rightHand.BaseData.itemType == ItemType.SWORD)
                            PutByte(1, (ushort)(offset + 179)); //匕首
                        else if (rightHand.BaseData.itemType == ItemType.SWORD)
                            PutByte(2, (ushort)(offset + 179)); //剑
                        else if (rightHand.BaseData.itemType == ItemType.RAPIER)
                            PutByte(3, (ushort)(offset + 179)); //长剑
                        else if (rightHand.BaseData.itemType == ItemType.CLAW)
                            PutByte(4, (ushort)(offset + 179)); //爪
                        else if (rightHand.BaseData.itemType == ItemType.HAMMER)
                            PutByte(6, (ushort)(offset + 179)); //锤
                        else if (rightHand.BaseData.itemType == ItemType.AXE)
                            PutByte(7, (ushort)(offset + 179)); //斧
                        else if (rightHand.BaseData.itemType == ItemType.SPEAR)
                            PutByte(8, (ushort)(offset + 179)); //矛
                        else if (rightHand.BaseData.itemType == ItemType.STAFF)
                            PutByte(9, (ushort)(offset + 179)); //杖
                    }
                    else
                    {
                        PutByte(0, (ushort)(offset + 177));
                    }


                    //riding motion
                    PutByte(3, (ushort)(offset + 180));
                    if (value.Inventory.Equipments.ContainsKey(EnumEquipSlot.PET) && value.Pet != null)
                        if (value.Pet.Ride)
                        {
                            var pet = value.Inventory.Equipments[EnumEquipSlot.PET];
                            PutUShort(pet.BaseData.handMotion, (ushort)(offset + 181));
                            PutUShort(pet.BaseData.handMotion2, (ushort)(offset + 182));
                        }

                    if (value.Inventory.Equipments.ContainsKey(EnumEquipSlot.PET) && value.Pet != null)
                        if (value.Pet.Ride)
                            PutUInt(value.Inventory.Equipments[EnumEquipSlot.PET].ItemID, (ushort)(offset + 184));

                    PutUInt(value.Range, (ushort)(offset + 189));
                    switch (value.Mode)
                    {
                        case PlayerMode.NORMAL:
                            PutInt(2, (ushort)(offset + 197)); //mode1
                            PutInt(0, (ushort)(offset + 201)); //mode2
                            break;
                        case PlayerMode.COLISEUM_MODE:
                            PutInt(0x42, (ushort)(offset + 197)); //mode1
                            PutInt(1, (ushort)(offset + 201)); //mode2
                            break;
                        case PlayerMode.KNIGHT_WAR:
                            PutInt(0x22, (ushort)(offset + 197)); //mode1
                            PutInt(2, (ushort)(offset + 201)); //mode2
                            break;
                        case PlayerMode.WRP:
                            PutInt(0x102, (ushort)(offset + 197)); //mode1
                            PutInt(4, (ushort)(offset + 201)); //mode2
                            break;
                    }

                    if (Configuration.Instance.Version >= Version.Saga14)
                        PutByte(1, 251); //unknown
                    /*int  ridepet_id; //(itemid
            byte ridepet_color;//ロボ用
            int range     //武器の射程
            int unknown   //0?
            int mode1   //2 r0fa7参照
            int mode2   //0 r0fa7参照
            byte  unknown //0?
            byte  guest //ゲストIDかどうか (07/11/22より)*/
                }

                #endregion

                #region Saga14_2

                if (Configuration.Instance.Version >= Version.Saga14_2 &&
                    Configuration.Instance.Version < Version.Saga17)
                {
                    var info = MapManager.Instance.GetMap(value.MapID);
                    PutUInt(value.ActorID, 2);
                    PutUInt(value.CharID, 6);
                    var name = value.Name;
                    name = name.Replace("\0", "");
                    var buf = Global.Unicode.GetBytes(name);
                    var buff = new byte[data.Length + buf.Length];
                    data.CopyTo(buff, 0);
                    data = buff;
                    var offset = (ushort)(12 + buf.Length);
                    PutByte((byte)(buf.Length + 1), 10);
                    PutBytes(buf, 11);
                    PutByte((byte)value.Race, offset);
                    offset++;
                    PutByte((byte)value.Form, offset);
                    PutByte((byte)value.Gender, (ushort)(offset + 1));
                    offset++;
                    PutByte(0, (ushort)(offset + 1)); //Unknown
                    PutUShort(value.HairStyle, (ushort)(offset + 2));
                    PutByte(value.HairColor, (ushort)(offset + 3));
                    PutUShort(value.Wig, (ushort)(offset + 4));
                    offset++;
                    PutByte(0xFF, (ushort)(offset + 5)); //Unknown
                    PutUShort(value.Face, (ushort)(offset + 6));
                    offset++;

                    offset++;
                    PutByte(value.TailStyle, (ushort)(offset + 7)); //3轉外觀
                    PutByte(value.WingStyle, (ushort)(offset + 8)); //3轉外觀
                    PutByte(value.WingColor, (ushort)(offset + 9)); //3轉外觀
                    PutUInt(value.MapID, (ushort)(offset + 10));
                    PutByte(Global.PosX16to8(value.X, info.Width), (ushort)(offset + 14));
                    PutByte(Global.PosY16to8(value.Y, info.Height), (ushort)(offset + 15));
                    PutByte((byte)(value.Dir / 45), (ushort)(offset + 16));
                    PutUInt(value.HP, (ushort)(offset + 17));
                    PutUInt(value.MaxHP, (ushort)(offset + 21));
                    PutUInt(value.MP, (ushort)(offset + 25));
                    PutUInt(value.MaxMP, (ushort)(offset + 29));
                    PutUInt(value.SP, (ushort)(offset + 33));
                    PutUInt(value.MaxSP, (ushort)(offset + 37));
                    PutUInt(value.EP, (ushort)(offset + 41)); //ep
                    PutUInt(value.MaxEP, (ushort)(offset + 45)); //max ep
                    PutShort(value.CL, (ushort)(offset + 49));
                    offset += 2;

                    PutByte(8, (ushort)(offset + 49));
                    PutUShort(value.Str, (ushort)(offset + 50));
                    PutUShort(value.Dex, (ushort)(offset + 52));
                    PutUShort(value.Int, (ushort)(offset + 54));
                    PutUShort(value.Vit, (ushort)(offset + 56));
                    PutUShort(value.Agi, (ushort)(offset + 58));
                    PutUShort(value.Mag, (ushort)(offset + 60));
                    PutUShort(13, (ushort)(offset + 62)); //luk
                    PutUShort(0, (ushort)(offset + 64)); //cha
                    PutByte(0x14, (ushort)(offset + 66)); //unknown
                    if (value.PossessionTarget == 0)
                    {
                        PutUInt(0xFFFFFFFF, (ushort)(offset + 109)); //possession target
                    }
                    else
                    {
                        var possession = info.GetActor(value.PossessionTarget);
                        if (possession.type != ActorType.ITEM)
                            PutUInt(value.PossessionTarget, (ushort)(offset + 109)); //possession target
                        else
                            PutUInt(value.ActorID, (ushort)(offset + 109)); //possession target                    
                    }

                    if (value.PossessionTarget == 0)
                        PutByte(0xFF, (ushort)(offset + 113)); //possession place
                    else
                        PutByte((byte)value.PossessionPosition, (ushort)(offset + 113)); //possession place
                    PutUInt((uint)value.Gold, (ushort)(offset + 114));
                    PutByte((byte)value.Status.attackType, (ushort)(offset + 118));
                    PutByte(13, (ushort)(offset + 119));
                    for (var j = 0; j < 14; j++)
                        if (value.Inventory.Equipments.ContainsKey((EnumEquipSlot)j))
                        {
                            var item = value.Inventory.Equipments[(EnumEquipSlot)j];
                            if (item.Stack == 0) continue;
                            if (item.PictID == 0)
                                PutUInt(item.BaseData.imageID, (ushort)(offset + 120 + j * 4));
                            else
                                PutUInt(item.PictID, (ushort)(offset + 120 + j * 4));
                        }

                    //left hand weapon motion
                    PutByte(3, (ushort)(offset + 172));
                    if (value.Inventory.Equipments.ContainsKey(EnumEquipSlot.LEFT_HAND))
                    {
                        var leftHand = value.Inventory.Equipments[EnumEquipSlot.LEFT_HAND];
                        PutUShort(leftHand.BaseData.handMotion, (ushort)(offset + 171));
                        PutUShort(leftHand.BaseData.handMotion2, (ushort)(offset + 173));
                    }
                    else
                    {
                        PutByte(0, (ushort)(offset + 173));
                    }

                    //right hand weapon motion
                    PutByte(3, (ushort)(offset + 176));
                    if (value.Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND))
                    {
                        var rightHand = value.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND];
                        PutUShort(rightHand.BaseData.handMotion, (ushort)(offset + 177));
                        PutUShort(rightHand.BaseData.handMotion2, (ushort)(offset + 178));
                        if (rightHand.BaseData.itemType == ItemType.SHORT_SWORD ||
                            rightHand.BaseData.itemType == ItemType.SWORD)
                            PutByte(1, (ushort)(offset + 179)); //匕首
                        else if (rightHand.BaseData.itemType == ItemType.SWORD)
                            PutByte(2, (ushort)(offset + 179)); //剑
                        else if (rightHand.BaseData.itemType == ItemType.RAPIER)
                            PutByte(3, (ushort)(offset + 179)); //长剑
                        else if (rightHand.BaseData.itemType == ItemType.CLAW)
                            PutByte(4, (ushort)(offset + 179)); //爪
                        else if (rightHand.BaseData.itemType == ItemType.HAMMER)
                            PutByte(6, (ushort)(offset + 179)); //锤
                        else if (rightHand.BaseData.itemType == ItemType.AXE)
                            PutByte(7, (ushort)(offset + 179)); //斧
                        else if (rightHand.BaseData.itemType == ItemType.SPEAR)
                            PutByte(8, (ushort)(offset + 179)); //矛
                        else if (rightHand.BaseData.itemType == ItemType.STAFF)
                            PutByte(9, (ushort)(offset + 179)); //杖
                    }
                    else
                    {
                        PutByte(0, (ushort)(offset + 177));
                    }


                    //riding motion
                    PutByte(3, (ushort)(offset + 180));
                    if (value.Inventory.Equipments.ContainsKey(EnumEquipSlot.PET) && value.Pet != null)
                        if (value.Pet.Ride)
                        {
                            var pet = value.Inventory.Equipments[EnumEquipSlot.PET];
                            PutUShort(pet.BaseData.handMotion, (ushort)(offset + 181));
                            PutUShort(pet.BaseData.handMotion2, (ushort)(offset + 182));
                        }

                    if (value.Inventory.Equipments.ContainsKey(EnumEquipSlot.PET) && value.Pet != null)
                        if (value.Pet.Ride)
                            PutUInt(value.Inventory.Equipments[EnumEquipSlot.PET].ItemID, (ushort)(offset + 184));

                    PutUInt(value.Range, (ushort)(offset + 189));
                    switch (value.Mode)
                    {
                        case PlayerMode.NORMAL:
                            PutInt(2, (ushort)(offset + 197)); //mode1
                            PutInt(0, (ushort)(offset + 201)); //mode2
                            break;
                        case PlayerMode.COLISEUM_MODE:
                            PutInt(0x42, (ushort)(offset + 197)); //mode1
                            PutInt(1, (ushort)(offset + 201)); //mode2
                            break;
                        case PlayerMode.KNIGHT_WAR:
                            PutInt(0x22, (ushort)(offset + 197)); //mode1
                            PutInt(2, (ushort)(offset + 201)); //mode2
                            break;
                        case PlayerMode.WRP:
                            PutInt(0x102, (ushort)(offset + 197)); //mode1
                            PutInt(4, (ushort)(offset + 201)); //mode2
                            break;
                    }
                }

                #endregion

                #region Saga17

                if (Configuration.Instance.Version >= Version.Saga17)
                {
                    var info = MapManager.Instance.GetMap(value.MapID);
                    PutUInt(value.ActorID, 2);
                    PutUInt(value.CharID, 6);

                    PutUInt(1, 10); //15年12月8日加入，对应版本449

                    var name = value.Name;
                    //name = "糖果_" + value.Account.AccountID.ToString() + Global.Random.Next(10000, 99999);//万圣节活动

                    /*if (value.FirstName != "")
                        name = value.FirstName + "·" + name;*/
                    name = name.Replace("\0", "");
                    var buf = Global.Unicode.GetBytes(name);
                    var buff = new byte[data.Length + buf.Length];
                    data.CopyTo(buff, 0);
                    data = buff;
                    PutUShort((ushort)(buf.Length + 1), 14);
                    PutBytes(buf, 16);
                    var offset = (ushort)(17 + buf.Length);

                    PutByte((byte)value.Race, offset);
                    PutByte((byte)value.Form, offset + 1);
                    PutByte((byte)value.Gender, offset + 2);

                    PutUShort(value.HairStyle, offset + 3);
                    PutByte(value.HairColor, offset + 5);
                    PutUShort(value.Wig, offset + 6);

                    PutByte(0xFF, offset + 8);
                    PutUShort(value.Face, offset + 9);
                    byte rblv = 0x00;
                    if (value.Rebirth)
                        rblv = 0x6e;
                    else
                        rblv = 0x00;
                    PutByte(rblv, offset + 11);
                    PutByte(value.TailStyle, offset + 12);
                    PutByte(value.WingStyle, offset + 13);
                    PutByte(value.WingColor, offset + 14);

                    PutUInt(value.MapID, offset + 15);
                    offset += 3;
                    PutByte(Global.PosX16to8(value.X, info.Width), offset + 16);
                    PutByte(Global.PosY16to8(value.Y, info.Height), offset + 17);
                    PutByte((byte)(value.Dir / 45), offset + 18);

                    PutUInt(value.HP, offset + 19);
                    PutUInt(value.MaxHP, offset + 23);
                    PutUInt(value.MP, offset + 27);
                    PutUInt(value.MaxMP, offset + 31);
                    PutUInt(value.SP, offset + 35);
                    PutUInt(value.MaxSP, offset + 39);
                    PutUInt(value.EP, offset + 43);
                    PutUInt(0, offset + 47);
                    PutShort(value.CL, offset + 51);

                    PutByte(8, offset + 53);
                    PutUShort(value.Str, offset + 54);
                    PutUShort(value.Dex, offset + 56);
                    PutUShort(value.Int, offset + 58);
                    PutUShort(value.Vit, offset + 60);
                    PutUShort(value.Agi, offset + 62);
                    PutUShort(value.Mag, offset + 64);
                    PutUShort(100, offset + 66);
                    PutUShort(100, offset + 68);


                    PutByte(0x14, offset + 70);

                    if (value.PossessionTarget == 0)
                    {
                        PutUInt(0xFFFFFFFF, offset + 113);
                    }
                    else
                    {
                        var possession = info.GetActor(value.PossessionTarget);
                        if (possession.type != ActorType.ITEM)
                            PutUInt(value.PossessionTarget, offset + 113);
                        else
                            PutUInt(value.ActorID, offset + 113);
                    }

                    if (value.PossessionTarget == 0)
                        PutByte(0xFF, offset + 117);
                    else
                        PutByte((byte)value.PossessionPosition, offset + 117);

                    ////offset += 4;//15年12月8日加入，对应版本449
                    ////解放金币上限到QWORD
                    //this.PutULong((uint)value.Gold, (ushort)(offset + 118));


                    PutUInt(0, offset + 118); //15年12月8日加入，对应版本449
                    offset += 4; //15年12月8日加入，对应版本449
                    PutUInt((uint)value.Gold, offset + 118);
                    PutByte((byte)value.Status.attackType, offset + 122);
                    PutUInt(0, offset + 123);
                    offset += 4;
                    offset += 4; //15年12月8日加入，对应版本452
                    PutByte(14, offset + 123);
                    for (var j = 0; j < 15; j++)
                        if (value.Inventory.Equipments.ContainsKey((EnumEquipSlot)j))
                        {
                            var item = value.Inventory.Equipments[(EnumEquipSlot)j];
                            if (item.Stack == 0) continue;
                            if (item.PictID == 0)
                                PutUInt(item.BaseData.imageID, offset + 124 + j * 4);
                            else
                                PutUInt(item.PictID, offset + 124 + j * 4);
                            if (value.Job == PC_JOB.HAWKEYE && j == 9 && value.TInt["斥候远程模式"] != 1 &&
                                !item.BaseData.doubleHand)
                                PutUInt(0, offset + 124 + j * 4);
                            if (value.Job == PC_JOB.HAWKEYE && j == 8 && value.TInt["斥候远程模式"] == 1 &&
                                !item.BaseData.doubleHand)
                                PutUInt(0, offset + 124 + j * 4);
                        }

                    offset += 4;
                    ////////////////左手动作////////////////
                    PutByte(3, offset + 176);
                    if (value.Inventory.Equipments.ContainsKey(EnumEquipSlot.LEFT_HAND) &&
                        value.Marionette == null && value.TranceID == 0)
                    {
                        var leftHand = value.Inventory.Equipments[EnumEquipSlot.LEFT_HAND];
                        /*if (leftHand.BaseData.handMotion < 255)
                            this.PutByte((byte)leftHand.BaseData.handMotion, offset + 177);
                        else//*/
                        PutUShort(leftHand.BaseData.handMotion, offset + 177);
                        offset += 2; //v282
                        try
                        {
                            PutUShort(
                                (byte)(EquipSound)Enum.Parse(typeof(EquipSound), leftHand.BaseData.itemType.ToString()),
                                offset + 179);
                            //this.PutUShort((ushort)leftHand.BaseData.itemType, offset + 179);
                        }
                        catch
                        {
                            PutUShort(leftHand.BaseData.handMotion2, offset + 179);
                        }

                        offset += 1;
                    }
                    else
                    {
                        offset += 3;
                    }

                    ////////////////右手动作////////////////
                    PutByte(3, offset + 180);
                    if (value.Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND) &&
                        value.Marionette == null && value.TranceID == 0)
                    {
                        var rightHand = value.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND];
                        /*/if(rightHand.BaseData.handMotion < 255)
                            this.PutByte((byte)rightHand.BaseData.handMotion, offset + 181);
                        else//*/
                        PutUShort(rightHand.BaseData.handMotion, offset + 181);
                        offset += 2; //v282
                        try
                        {
                            PutUShort(
                                (byte)(EquipSound)Enum.Parse(typeof(EquipSound),
                                    rightHand.BaseData.itemType.ToString()), offset + 183);
                            //this.PutUShort((ushort)rightHand.BaseData.itemType, offset + 183);
                        }
                        catch
                        {
                            PutUShort(rightHand.BaseData.handMotion2, offset + 183);
                        }

                        offset += 1;
                    }
                    else
                    {
                        offset += 3;
                    }

                    //////////////////骑乘//////////////////
                    PutByte(3, offset + 184);
                    if (value.Inventory.Equipments.ContainsKey(EnumEquipSlot.PET) && value.Pet != null)
                        if (value.Pet.Ride)
                        {
                            var pet = value.Inventory.Equipments[EnumEquipSlot.PET];
                            /*/
                            if(pet.BaseData.handMotion > 255)
                            this.PutUShort(pet.BaseData.handMotion, offset + 185);
                            else
                                this.PutByte((byte)pet.BaseData.handMotion, offset + 185);//*/
                            PutUShort(pet.BaseData.handMotion, offset + 185);

                            //this.PutUShort(pet.BaseData.handMotion2, offset + 187);
                            PutByte(0xff, offset + 189);
                            PutByte(0xff, offset + 190);
                        }

                    //*/
                    if (value.Inventory.Equipments.ContainsKey(EnumEquipSlot.PET) && value.Pet != null)
                        if (value.Pet.Ride)
                            PutUInt(value.Inventory.Equipments[EnumEquipSlot.PET].ItemID, offset + 191);

                    //*/
                    offset += 3; //v282


                    PutUInt(value.Range, (ushort)(offset + 193));
                    switch (value.Mode)
                    {
                        case PlayerMode.NORMAL:
                            PutInt(2, (ushort)(offset + 201)); //mode1
                            PutInt(0, (ushort)(offset + 205)); //mode2
                            break;
                        case PlayerMode.COLISEUM_MODE:
                            PutInt(0x42, (ushort)(offset + 201)); //mode1
                            PutInt(1, (ushort)(offset + 205)); //mode2
                            break;
                        case PlayerMode.KNIGHT_WAR:
                            PutInt(0x22, (ushort)(offset + 201)); //mode1
                            PutInt(2, (ushort)(offset + 205)); //mode2
                            break;
                        case PlayerMode.WRP:
                            PutInt(0x102, (ushort)(offset + 201)); //mode1
                            PutInt(4, (ushort)(offset + 205)); //mode2
                            break;
                        case PlayerMode.KNIGHT_EAST:
                            PutInt(0x22, (ushort)(offset + 201)); //mode1
                            PutInt(2, (ushort)(offset + 205)); //mode2
                            PutByte(1, (ushort)(offset + 209)); //emblem; //演習時のエンブレムとか　1東2西4南8北Aヒーロー状態
                            PutByte(0, (ushort)(offset + 210)); //metamo; //メタモーバトルのチーム　1花2岩
                            break;
                        case PlayerMode.KNIGHT_WEST:
                            PutInt(0x22, (ushort)(offset + 201)); //mode1
                            PutInt(2, (ushort)(offset + 205)); //mode2
                            PutByte(2, (ushort)(offset + 209)); //emblem; //演習時のエンブレムとか　1東2西4南8北Aヒーロー状態
                            PutByte(0, (ushort)(offset + 210)); //metamo; //メタモーバトルのチーム　1花2岩
                            break;
                        case PlayerMode.KNIGHT_SOUTH:
                            PutInt(0x22, (ushort)(offset + 201)); //mode1
                            PutInt(2, (ushort)(offset + 205)); //mode2
                            PutByte(4, (ushort)(offset + 209)); //emblem; //演習時のエンブレムとか　1東2西4南8北Aヒーロー状態
                            PutByte(0, (ushort)(offset + 210)); //metamo; //メタモーバトルのチーム　1花2岩
                            break;
                        case PlayerMode.KNIGHT_NORTH:
                            PutInt(0x22, (ushort)(offset + 201)); //mode1
                            PutInt(2, (ushort)(offset + 205)); //mode2
                            PutByte(8, (ushort)(offset + 209)); //emblem; //演習時のエンブレムとか　1東2西4南8北Aヒーロー状態
                            PutByte(0, (ushort)(offset + 210)); //metamo; //メタモーバトルのチーム　1花2岩
                            break;
                        case PlayerMode.KNIGHT_FLOWER:
                            PutInt(0x22, (ushort)(offset + 201)); //mode1
                            PutInt(2, (ushort)(offset + 205)); //mode2
                            PutByte(0, (ushort)(offset + 209)); //emblem; //演習時のエンブレムとか　1東2西4南8北Aヒーロー状態
                            PutByte(1, (ushort)(offset + 210)); //metamo; //メタモーバトルのチーム　1花2岩
                            break;
                        case PlayerMode.KNIGHT_ROCK:
                            PutInt(0x22, (ushort)(offset + 201)); //mode1
                            PutInt(2, (ushort)(offset + 205)); //mode2
                            PutByte(0, (ushort)(offset + 209)); //emblem; //演習時のエンブレムとか　1東2西4南8北Aヒーロー状態
                            PutByte(2, (ushort)(offset + 210)); //metamo; //メタモーバトルのチーム　1花2岩
                            break;
                    }

                    PutByte(value.WaitType, offset + 225);
                    /*this.PutShort(0x28a7, offset + 213);
                    this.PutShort(0x2b63, offset + 217);
                    this.PutByte(2, offset + 219);*/
                    PutByte(1, offset + 219);
                    PutByte(15, offset + 223);
                    PutUShort(value.UsingPaperID, offset + 226);
                    PutUShort(0, (ushort)(offset + 228)); //unknown ver469
                    PutUInt(132150, (ushort)(offset + 230)); //unknown ver469
                    PutByte(4, offset + 234);
                    if (value.type == ActorType.PC)
                    {
                        PutUInt((uint)value.AInt["称号_主语"], offset + 235);
                        PutUInt((uint)value.AInt["称号_连词"], offset + 239);
                        PutUInt((uint)value.AInt["称号_谓语"], offset + 243);
                        PutUInt((uint)value.AInt["称号_战斗"], offset + 247);
                    }
                }

                #endregion
            }
        }
    }
}