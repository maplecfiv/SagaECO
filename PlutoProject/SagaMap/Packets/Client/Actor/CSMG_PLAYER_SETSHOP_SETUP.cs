using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_PLAYER_SETSHOP_SETUP : Packet
    {
        public CSMG_PLAYER_SETSHOP_SETUP()
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

        public ulong[] Prices
        {
            get
            {
                var len = GetByte(2);
                var prices = new ulong[GetByte((ushort)(4 + len * 6))];
                for (var i = 0; i < prices.Length; i++) prices[i] = GetULong();

                return prices;
            }
        }

        public string Comment
        {
            get
            {
                var len = GetByte(2);
                len = GetByte((ushort)(5 + len * 14));
                return Global.Unicode.GetString(GetBytes(len, (ushort)(6 + GetByte(2) * 14))).Replace("\0", "");
            }
        }

        public override Packet New()
        {
            return new CSMG_PLAYER_SETSHOP_SETUP();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnPlayerSetShopSetup(this);
            ((MapClient)client).OnPlayerShopChange(this);
        }
    }
}