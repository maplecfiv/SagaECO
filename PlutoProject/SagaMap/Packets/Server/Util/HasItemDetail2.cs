using System;
using SagaLib;
using Version = SagaLib.Version;

namespace SagaMap.Packets.Server.Util
{
    public abstract class HasItemDetail2 : Packet
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


        protected SagaDB.Item.Item ItemDetail
        {
            set
            {
                if (value.Refine > 0)
                    if (value.EquipSlot.Count > 0)
                        if (value.EquipSlot[0] == EnumEquipSlot.CHEST_ACCE ||
                            value.EquipSlot[0] == EnumEquipSlot.RIGHT_HAND ||
                            value.EquipSlot[0] == EnumEquipSlot.UPPER_BODY)
                            ItemFactory.Instance.CalcRefineBouns(value);
                PutUInt(value.ItemID);
                PutUInt(value.PictID);
                offset += 1; //4 bytes fusion + 1 byte place
                int identify = value.identified;
                if (value.Locked)
                    identify |= 0x20;
                if (value.ChangeMode)
                    identify |= 0x81;
                if (value.ChangeMode2)
                    identify |= 0x41;
                PutInt(0);
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
                if (Configuration.Configuration.Instance.Version >= Version.Saga9_Iris)
                {
                    PutUShort(value.CurrentSlot);
                    for (var i = 0; i < 10; i++)
                        if (i < value.Cards.Count)
                            PutUInt(value.Cards[i].ID);
                        else
                            PutUInt(0);
                }

                PutByte(0); //  染色
                PutUShort(value.Stack);
                if (price == 0)
                    PutUInt(value.BaseData.price);
                else
                    PutInt(0);
                PutShort(0); // 商品個数
                //this.PutUShort(value.BaseData.possessionWeight);
                //this.offset += 8;
                PutShort(100); //unknown2016年2月21日
                PutShort(110); //unknown2016年2月21日
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
                PutShort((short)(value.BaseData.hp + value.HP + value.hp_refine));
                PutShort((short)(value.BaseData.sp + value.SP));
                PutShort((short)(value.BaseData.mp + value.MP));
                PutShort((short)(value.BaseData.speedUp + value.SpeedUp));
                PutShort((short)(value.BaseData.atk1 + value.Atk1 + value.atkrate_refine));
                PutShort((short)(value.BaseData.atk2 + value.Atk2 + value.atkrate_refine));
                PutShort((short)(value.BaseData.atk3 + value.Atk3 + value.atkrate_refine));
                PutShort((short)(value.BaseData.matk + value.MAtk + value.matkrate_refine));
                PutShort((short)(value.BaseData.def + value.Def + value.defrate_refine));
                PutShort((short)(value.BaseData.mdef + value.MDef + value.mdefrate_refine));
                PutShort((short)(value.BaseData.hitMelee + value.HitMelee));
                PutShort((short)(value.BaseData.hitRanged + value.HitRanged));
                PutShort((short)(value.BaseData.hitMagic + value.HitMagic));
                PutShort((short)(value.BaseData.avoidMelee + value.AvoidMelee));
                PutShort((short)(value.BaseData.avoidRanged + value.AvoidRanged));
                PutShort((short)(value.BaseData.avoidMagic + value.AvoidMagic));
                PutShort((short)(value.BaseData.hitCritical + value.HitCritical + value.cri_refine));
                PutShort((short)(value.BaseData.avoidCritical + value.AvoidCritical));
                PutShort((short)(value.BaseData.hpRecover + value.HPRecover + value.recover_refine)); //recovery both???
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
                //this.PutShort(0);
                PutULong(price); //price 商品の値段（露天商）（上の方のpriceの値と一致するとは限らない
                PutUShort(shopCount); //num 販売個数（露天商）（上の方の個数と一緒?
                PutShort(0);
                //this.PutInt(0);//unknown8
                PutShort(0); //unknown9
                if (Configuration.Configuration.Instance.Version >= Version.Saga9_Iris)
                {
                    if (value.Rental)
                    {
                        PutInt((int)(value.RentalTime - DateTime.Now).TotalSeconds);
                        PutByte(1);
                    }
                    else
                    {
                        PutInt(-1); //貸出品のとき残り貸出期間(秒)、それ以外-1　
                        PutByte(0); //貸出アイテムフラグ
                    }
                }
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