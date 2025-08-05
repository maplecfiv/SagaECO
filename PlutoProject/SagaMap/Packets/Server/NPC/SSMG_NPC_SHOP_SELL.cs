using SagaLib;

namespace SagaMap.Packets.Server.NPC
{
    public class SSMG_NPC_SHOP_SELL : Packet
    {
        public SSMG_NPC_SHOP_SELL()
        {
            data = new byte[27];
            offset = 2;
            ID = 0x0603;
        }

        public uint Rate
        {
            set => PutUInt(value, 2);
        }

        public uint ShopLimit
        {
            set => PutUInt(value, 6);
        }

        public uint Bank
        {
            set => PutUInt(value, 10);
        }
    }
}