using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.VShop
{
    public class CSMG_VSHOP_BUY : Packet
    {
        public CSMG_VSHOP_BUY()
        {
            offset = 2;
        }

        public uint[] Items
        {
            get
            {
                var count = GetByte(2);
                var items = new uint[count];
                for (var i = 0; i < count; i++) items[i] = GetUInt((ushort)(3 + i * 4));
                return items;
            }
        }

        public uint[] Counts
        {
            get
            {
                var count = GetByte(2);
                var items = new uint[count];
                for (var i = 0; i < count; i++) items[i] = GetUInt((ushort)(4 + count * 4 + i * 4));
                return items;
            }
        }

        public override Packet New()
        {
            return new CSMG_VSHOP_BUY();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnVShopBuy(this);
        }
    }
}