using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_PLAYER_SHOP_COUNT_UPDATE : Packet
    {
        public SSMG_PLAYER_SHOP_COUNT_UPDATE()
        {
            data = new byte[8];
            offset = 2;
            ID = 0x191C;
        }

        public uint InventorySlot
        {
            set => PutUInt(value, 2);
        }

        public short ShopCount
        {
            set => PutShort(value, 6);
        }
    }
}