using System.Collections.Generic;
using SagaLib;

namespace SagaMap.Packets.Server.Item
{
    public class SSMG_ITEM_EFFECT : Packet
    {
        //DWORD  item_id;          // アイテムID
        //ABYTE  unknown1;         // 
        //DWORD  from_chara_id;    // アイテム使用者のサーバキャラID？
        //ADWORD target_chara_id?; // 
        //AWORD  hp;               // HPダメージ(マイナスの場合回復)
        //AWORD  mp;               // MPダメージ(マイナスの場合回復)
        //AWORD  sp;               // SPダメージ(マイナスの場合回復)
        //ADWORD color_flag;       // 数字の色(MISS Avoid Criticalも？

        private readonly byte combo;

        public SSMG_ITEM_EFFECT(byte combo)
        {
            data = new byte[21 + 4 * combo + 6 * combo + 4 * combo];
            offset = 2;
            ID = 0x09c8;
            this.combo = combo;
            PutByte(1, 4);
        }

        public uint ItemID
        {
            set => PutUInt(value, 2);
        }

        public uint ActorID
        {
            set => PutUInt(value, 6);
        }


        public List<SagaDB.Actor.Actor> AffectedID
        {
            set
            {
                PutByte(combo, 10);
                for (var i = 0; i < combo; i++) PutUInt(value[i].ActorID, (ushort)(11 + i * 4));
            }
        }


        public void SetHP(short[] hp)
        {
            PutByte(combo, (ushort)(11 + combo * 4));
            for (var i = 0; i < combo; i++) PutShort(hp[i], (ushort)(12 + combo * 4 + i * 2));
        }

        public void SetMP(short[] mp)
        {
            PutByte(combo, (ushort)(12 + combo * 4 + combo * 2));
            for (var i = 0; i < combo; i++) PutShort(mp[i], (ushort)(13 + combo * 4 + combo * 2 + i * 2));
        }

        public void SetSP(short[] sp)
        {
            PutByte(combo, (ushort)(13 + combo * 4 + combo * 4));
            for (var i = 0; i < combo; i++) PutShort(sp[i], (ushort)(14 + combo * 4 + combo * 4 + i * 2));
        }

        public void AttackFlag(List<AttackFlag> flag)
        {
            PutByte(combo, (ushort)(14 + combo * 4 + combo * 6));
            for (var i = 0; i < combo; i++) PutUInt((uint)flag[i], (ushort)(15 + combo * 4 + combo * 6 + i * 4));
        }
    }
}