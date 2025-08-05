using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_PLAYER_SHOP_GOLD_UPDATA : Packet
    {
        public SSMG_PLAYER_SHOP_GOLD_UPDATA()
        {
            data = new byte[16];
            offset = 2;
            ID = 0x190F;
        }

        public uint SlotID
        {
            set => PutUInt(value, 2);
        }

        public ushort Count
        {
            set => PutUShort(value, 6);
        }

        public ulong gold
        {
            set => PutULong(value, 8);
        }
    }
}