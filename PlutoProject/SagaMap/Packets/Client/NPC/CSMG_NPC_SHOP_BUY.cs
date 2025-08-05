using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.NPC
{
    public class CSMG_NPC_SHOP_BUY : Packet
    {
        public CSMG_NPC_SHOP_BUY()
        {
            offset = 2;
        }

        public uint[] Goods
        {
            get
            {
                var num = GetByte(2);
                var goods = new uint[num];
                for (var i = 0; i < num; i++) goods[i] = GetUInt((ushort)(3 + i * 4));
                return goods;
            }
        }

        public uint[] Counts
        {
            get
            {
                var num = GetByte(2);
                var goods = new uint[num];
                for (var i = 0; i < num; i++) goods[i] = GetUInt((ushort)(4 + num * 4 + i * 4));
                return goods;
            }
        }

        public override Packet New()
        {
            return new CSMG_NPC_SHOP_BUY();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnNPCShopBuy(this);
        }
    }
}