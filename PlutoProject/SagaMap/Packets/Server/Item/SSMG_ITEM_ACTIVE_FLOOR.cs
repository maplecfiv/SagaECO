using System.Collections.Generic;
using SagaLib;

namespace SagaMap.Packets.Server.Item
{
    public class SSMG_ITEM_ACTIVE_FLOOR : Packet
    {
        private readonly byte combo;

        public SSMG_ITEM_ACTIVE_FLOOR(byte combo)
        {
            data = new byte[19 + 4 * combo + 6 * combo + 4 * combo];
            offset = 2;
            ID = 0x09C6;
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

        public byte X
        {
            set => PutByte(value, (ushort)(13 + combo * 4));
        }

        public byte Y
        {
            set => PutByte(value, (ushort)(14 + combo * 4));
        }

        public void SetHP(List<int> hp)
        {
            PutByte(combo, (ushort)(15 + combo * 4));
            for (var i = 0; i < combo; i++) PutShort((short)hp[i], (ushort)(16 + combo * 4 + i * 2));
        }

        public void SetMP(List<int> mp)
        {
            PutByte(combo, (ushort)(16 + combo * 4 + combo * 2));
            for (var i = 0; i < combo; i++) PutShort((short)mp[i], (ushort)(17 + combo * 4 + combo * 2 + i * 2));
        }

        public void SetSP(List<int> sp)
        {
            PutByte(combo, (ushort)(17 + combo * 4 + combo * 4));
            for (var i = 0; i < combo; i++) PutShort((short)sp[i], (ushort)(18 + combo * 4 + combo * 4 + i * 2));
        }

        public void AttackFlag(List<AttackFlag> flag)
        {
            PutByte(combo, (ushort)(18 + combo * 4 + combo * 6));
            for (var i = 0; i < combo; i++) PutUInt((uint)flag[i], (ushort)(19 + combo * 4 + combo * 6 + i * 4));
        }
    }
}