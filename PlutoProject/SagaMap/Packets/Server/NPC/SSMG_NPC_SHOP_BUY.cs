using System.Collections.Generic;
using SagaLib;

namespace SagaMap.Packets.Server.NPC
{
    public class SSMG_NPC_SHOP_BUY : Packet
    {
        private readonly int num;

        public SSMG_NPC_SHOP_BUY(int num)
        {
            data = new byte[24 + num * 4];
            offset = 2;
            ID = 0x0601;
            this.num = num;
        }

        public uint Rate
        {
            set => PutUInt(value, 2);
        }

        public List<uint> Goods
        {
            set
            {
                PutByte((byte)value.Count, 6);
                for (var i = 0; i < value.Count; i++) PutUInt(value[i], (ushort)(7 + i * 4));
            }
        }

        public uint Gold
        {
            set => PutUInt(value, (ushort)(11 + num * 4));
        }

        public uint Bank
        {
            set => PutUInt(value, (ushort)(15 + num * 4));
        }

        public ShopType Type
        {
            set => PutByte((byte)value, (ushort)(23 + num * 4));
        }
    }
}