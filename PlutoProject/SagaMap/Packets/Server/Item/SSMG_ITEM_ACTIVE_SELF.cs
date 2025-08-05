using System.Collections.Generic;
using SagaLib;

namespace SagaMap.Packets.Server.Item
{
    public class SSMG_ITEM_ACTIVE_SELF : Packet
    {
        private readonly byte combo;

        public SSMG_ITEM_ACTIVE_SELF(byte combo)
        {
            data = new byte[17 + 4 * combo + 24 * combo + 4 * combo];
            offset = 2;
            ID = 0x09C8;
            this.combo = combo;
            PutByte(1, 6);
        }

        public uint ItemID
        {
            set => PutUInt(value, 2);
        }

        public uint ActorID
        {
            set => PutUInt(value, 8);
        }

        public List<SagaDB.Actor.Actor> AffectedID
        {
            set
            {
                PutByte(combo, 12);
                for (var i = 0; i < combo; i++) PutUInt(value[i].ActorID, (ushort)(13 + i * 4));
            }
        }

        public void SetHP(List<int> hp)
        {
            PutByte(combo, (ushort)(13 + combo * 4));
            for (var i = 0; i < combo; i++)
            {
                PutUInt((uint)hp[i], (ushort)(14 + combo * 4 + i * 4));
                PutUInt((uint)hp[i], (ushort)(14 + combo * 8 + i * 4));
            }
        }

        public void SetMP(List<int> mp)
        {
            PutByte(combo, (ushort)(14 + combo * 4 + combo * 8));
            for (var i = 0; i < combo; i++)
            {
                PutUInt((uint)mp[i], (ushort)(15 + combo * 4 + combo * 8 + i * 4));
                PutUInt((uint)mp[i], (ushort)(15 + combo * 4 + combo * 12 + i * 4));
            }
        }

        public void SetSP(List<int> sp)
        {
            PutByte(combo, (ushort)(15 + combo * 4 + combo * 16));
            for (var i = 0; i < combo; i++)
            {
                PutUInt((uint)sp[i], (ushort)(16 + combo * 4 + combo * 16 + i * 4));
                PutUInt((uint)sp[i], (ushort)(16 + combo * 4 + combo * 20 + i * 4));
            }
        }

        public void AttackFlag(List<AttackFlag> flag)
        {
            PutByte(combo, (ushort)(16 + combo * 4 + combo * 24));
            for (var i = 0; i < combo; i++) PutUInt((uint)flag[i], (ushort)(17 + combo * 4 + combo * 24 + i * 4));
        }
    }
}