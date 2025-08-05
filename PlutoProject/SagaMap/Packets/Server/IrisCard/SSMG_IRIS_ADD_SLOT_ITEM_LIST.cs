using System.Collections.Generic;
using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_IRIS_ADD_SLOT_ITEM_LIST : Packet
    {
        public SSMG_IRIS_ADD_SLOT_ITEM_LIST()
        {
            data = new byte[3];
            offset = 2;
            ID = 0x13E2;
        }

        public List<uint> Items
        {
            set
            {
                data = new byte[10 + 4 * value.Count];
                offset = 2;
                ID = 0x13E2;

                PutByte((byte)value.Count);

                foreach (var item in value) PutUInt(item);
                PutUInt(0u);
                PutByte(01);
                PutShort(100);
                PutByte(0);
            }
        }
    }
}