using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_ITEM_IDENTIFY : Packet
    {
        public SSMG_ITEM_IDENTIFY()
        {
            data = new byte[10];
            offset = 2;
            ID = 0x09D1;
        }

        public uint InventorySlot
        {
            set => PutUInt(value, 2);
        }

        public bool Identify
        {
            set
            {
                if (value)
                    PutInt(GetInt(6) | 1, 6);
            }
        }

        public bool Lock
        {
            set
            {
                if (value)
                    PutInt(GetInt(6) | 0x20, 6);
            }
        }
    }
}