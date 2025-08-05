using System;
using SagaDB.Item;
using SagaLib;
using Version = SagaLib.Version;

namespace SagaMap.Packets.Server
{
    public abstract class HasItemDetail : Packet
    {
        private ulong price;
        private ushort shopCount;

        public ulong Price
        {
            set => price = value;
        }

        public ushort ShopCount
        {
            set => shopCount = value;
        }

        protected Item ItemDetail
        {
            set
            {
                /*
                 * DWORD(物品ID) uint
                 * DWORD(外觀ID 沒有則0) uint
                 * BYTE(位置 02背包內 06頭 08面 0B衣服 0D背 0E武 0F盾 10鞋子 11襪 13特)
                 * DWORD(背包內順位 1開始) uint
                 * 00 00 ushort
                 * BYTE(二進位 左→1性能開放  右→8寵已partner 4? 2舊物品標示 1潛在強化)
                 * BYTE(二進位 左→8change變暗 4change物白框 2插槽鎖 1不明  右→2損壞 1鑑定)
                 * WORD(耐久)
                 * WORD(最大耐久)
                 */
                if (value.Refine > 0)
                    if (value.EquipSlot.Count > 0)
                        if (value.EquipSlot[0] == EnumEquipSlot.CHEST_ACCE ||
                            value.EquipSlot[0] == EnumEquipSlot.RIGHT_HAND ||
                            value.EquipSlot[0] == EnumEquipSlot.UPPER_BODY)
                            ItemFactory.Instance.CalcRefineBouns(value);
                PutUInt(value.ItemID);
                PutUInt(value.PictID);
                //this.PutByte(0x00); //物品现在存在的位置 02背包內 06頭 08面 0B衣服 0D背 0E武 0F盾 10鞋子 11襪 13特
                //this.PutUInt(0x00);
                //this.PutUShort(0x0);
                offset += 1; //4 bytes fusion + 1 byte place for containtype in packet details
                ushort identify = value.identified;
                if (value.maxDurability != 0 && value.Durability == 0)
                    identify |= 0x02;
                if (value.Locked)
                    identify |= 0x20;
                if (value.ChangeMode2)
                    identify |= 0x40;
                if (value.ChangeMode)
                    identify |= 0x80;
                if (value.Potential)
                    identify |= 0x100;
                if (value.Old)
                    identify |= 0x200;
                if (value.Release)
                    identify |= 0x1000;

                //flag
                //0未鉴定
                //1已鉴定
                //2已损坏（半透明）

                //00卡槽全未锁
                //20卡槽锁定
                //40可以change
                //80已经change

                //000未潜在强化
                //100已潜在强化
                //200旧物标记

                //0000性能未开放
                //1000性能已开放
                //this.PutUShort(identify);
                PutInt(0); //???
                PutInt(identify); //新增??? not partner or not partner initialized then 01 if has partner initialed then 0801
                PutUShort(value.Durability);
                PutUShort(value.maxDurability);
                if (value.BaseData.itemType != ItemType.PET && value.BaseData.itemType != ItemType.PET_NEKOMATA &&
                    value.BaseData.itemType != ItemType.RIDE_PET
                    && value.BaseData.itemType != ItemType.BACK_DEMON && value.BaseData.itemType != ItemType.PARTNER &&
                    value.BaseData.itemType != ItemType.RIDE_PARTNER)
                    PutUShort(value.Refine); // 残り強化回数
                else
                    PutUShort(0);
                //Iris Cards
                if (Configuration.Instance.Version >= Version.Saga9_Iris)
                {
                    PutUShort(value.CurrentSlot);
                    for (var i = 0; i < 10; i++)
                        if (i < value.Cards.Count)
                            PutUInt(value.Cards[i].ID);
                        else
                            PutUInt(0);
                }

                PutByte(value.Dye); //  染色
                PutUShort(value.Stack);
                if (price == 0)
                    PutULong(value.BaseData.price);
                else
                    PutULong(price);
                PutUShort(shopCount); // 商品個数
                //this.PutUShort(value.BaseData.possessionWeight);
                //this.offset += 8;
                //this.PutShort(100);//unknown2016年2月21日
                //this.PutShort(110);//unknown2016年2月21日
                PutShort((short)(value.BaseData.weightUp + value.WeightUp + value.atk_refine * 10));
                PutShort((short)(value.BaseData.volumeUp + value.VolumeUp + value.matk_refine * 10));
                //this.PutUShort(value.BaseData.possibleSkill);
                //this.PutUShort(value.BaseData.possessionWeight);
                /*this.PutUShort(value.BaseData.passiveSkill);
                this.PutUShort(value.BaseData.possessionSkill);
                this.PutUShort(value.BaseData.possessionPassiveSkill);*/
                PutShort((short)(value.BaseData.str + value.Str));
                PutShort((short)(value.BaseData.mag + value.Mag));
                PutShort((short)(value.BaseData.vit + value.Vit));
                PutShort((short)(value.BaseData.dex + value.Dex));
                PutShort((short)(value.BaseData.agi + value.Agi));
                PutShort((short)(value.BaseData.intel + value.Int));
                PutShort((short)(value.BaseData.luk + value.Luk));
                PutShort((short)(value.BaseData.cha + value.Cha));
                PutShort((short)(value.BaseData.hp + value.HP));
                PutShort((short)(value.BaseData.sp + value.SP));
                PutShort((short)(value.BaseData.mp + value.MP));
                PutShort((short)(value.BaseData.speedUp + value.SpeedUp));
                PutShort((short)(value.BaseData.atk1 + value.Atk1));
                PutShort((short)(value.BaseData.atk2 + value.Atk2));
                PutShort((short)(value.BaseData.atk3 + value.Atk3));
                PutShort((short)(value.BaseData.matk + value.MAtk));
                PutShort((short)(value.BaseData.def + value.Def));
                PutShort((short)(value.BaseData.mdef + value.MDef));
                PutShort((short)(value.BaseData.hitMelee + value.HitMelee));
                PutShort((short)(value.BaseData.hitRanged + value.HitRanged));
                PutShort((short)(value.BaseData.hitMagic + value.HitMagic));
                PutShort((short)(value.BaseData.avoidMelee + value.AvoidMelee));
                PutShort((short)(value.BaseData.avoidRanged + value.AvoidRanged));
                PutShort((short)(value.BaseData.avoidMagic + value.AvoidMagic));
                PutShort((short)(value.BaseData.hitCritical + value.HitCritical));
                PutShort((short)(value.BaseData.avoidCritical + value.AvoidCritical));
                PutShort((short)(value.BaseData.hpRecover + value.HPRecover)); //recovery both???
                PutShort((short)(value.BaseData.mpRecover + value.MPRecover));
                PutShort((short)(value.BaseData.spRecover + value.SPRecover));
                for (var i = 0; i < 7; i++)
                    if (value.BaseData.element.ContainsKey((Elements)i))
                        PutShort(value.BaseData.element[(Elements)i]);

                for (var i = 1; i <= 9; i++)
                    if (value.BaseData.abnormalStatus.ContainsKey((AbnormalStatus)i))
                        PutShort(value.BaseData.abnormalStatus[(AbnormalStatus)i]);

                PutShort(0); //unknown2016年2月21日
                PutShort((short)(value.ASPD + value.spd_refine)); // ペットステ（攻撃速度
                PutShort((short)(value.CSPD + value.spd_refine)); // ペットステ（詠唱速度


                PutShort(0); // ペットステ？（スタミナ回復力？倉では参照されない。
                PutInt(0); //unknown6
                PutInt(0); //unknown7

                PutULong(price); //price 商品の値段（露天商）（上の方のpriceの値と一致するとは限らない
                PutUShort(shopCount); //num 販売個数（露天商）（上の方の個数と一緒?

                var buff = new byte[data.Length + Global.Unicode.GetBytes(value.Name).Length];

                data.CopyTo(buff, 0);
                data = buff;

                PutShort((short)(Global.Unicode.GetBytes(value.Name).Length + 1)); //name_length 这里不是错误，就是要单独写一次tstr长度！
                PutString(value.Name); //name

                //this.PutByte((byte)(this.size-3), 2); //回头补封包前面的偏移

                if (Configuration.Instance.Version >= Version.Saga9_Iris)
                {
                    if (value.Rental)
                    {
                        PutByte(1);
                        PutInt((int)(value.RentalTime - DateTime.Now).TotalSeconds);
                    }
                    else
                    {
                        PutByte(0); //貸出アイテムフラグ
                        PutInt(-1); //貸出品のとき残り貸出期間(秒)、それ以外-1　
                    }
                }


                PutByte(0); //unknown
                PutByte(value.PartnerLevel); //partner level if not initialized or not partner then 0
                PutByte(value.PartnerRebirth); //partner rebirth mark if not rebirth then 0
                /*
                  short  atk_speed;
short  mgk_speed;
short  heal_stamina?;

DWORD  unknown6;       //
 WORD  unknown7;       //
DWORD  price;          // 商品の値段（露天商）（上の方のpriceの値と一致するとは限らない
 WORD  num;            // 販売個数（露天商）（上の方の個数と一緒?
DWORD  unknown8;       //
 WORD  unknown9;       //

 WORD  name_length;    // 次の名前の文字列長と同じ？
 TSTR  name;           // 固有ネーム（ペットの名前とか
                         //（";ab";という名前ならname_length = 0003, name = 03 'a' 'b' '\0'
 BYTE  unknown10;      // 0固定？
                DWORD? unknown11;      // 貸出品のとき残り貸出期間(秒)、それ以外-1
 BYTE  unkwnon12;      // 貸出アイテムフラグ

                 */
            }
        }
    }
}