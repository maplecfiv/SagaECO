using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.Golem
{
    public class CSMG_GOLEM_SHOP_BUY_SETUP : Packet
    {
        public CSMG_GOLEM_SHOP_BUY_SETUP()
        {
            offset = 2;
        }

        public uint[] InventoryIDs
        {
            get
            {
                var ids = new uint[GetByte(2)];
                for (var i = 0; i < ids.Length; i++) ids[i] = GetUInt();
                return ids;
            }
        }

        public ushort[] Counts
        {
            get
            {
                var len = GetByte(2);
                var counts = new ushort[GetByte((ushort)(3 + len * 4))];
                for (var i = 0; i < counts.Length; i++) counts[i] = GetUShort();
                return counts;
            }
        }

        public uint[] Prices
        {
            get
            {
                var len = GetByte(2);
                var prices = new uint[GetByte((ushort)(4 + len * 6))];
                for (var i = 0; i < prices.Length; i++) prices[i] = GetUInt();
                return prices;
            }
        }

        public uint BuyLimit
        {
            get
            {
                var len = GetByte(2);
                return GetUInt((ushort)(5 + len * 10));
            }
        }

        public string Comment
        {
            get
            {
                var len = GetByte(2);
                len = GetByte((ushort)(9 + len * 10));
                return Global.Unicode.GetString(GetBytes(len, (ushort)(10 + GetByte(2) * 10))).Replace("\0", "");
            }
        }

        public override Packet New()
        {
            return new CSMG_GOLEM_SHOP_BUY_SETUP();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnGolemShopBuySetup(this);
        }
    }
}