using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_ITEM_DELETE : Packet
    {
        public SSMG_ITEM_DELETE()
        {
            data = new byte[6];
            offset = 2;
            ID = 0x09CE;
        }

        public uint InventorySlot
        {
            set => PutUInt(value, 2);
        }
    }
}