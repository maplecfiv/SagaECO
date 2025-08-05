using System.Collections.Generic;
using SagaLib;

namespace SagaMap.Packets.Server.IrisCard
{
    public class SSMG_IRIS_CARD_ASSEMBLE_LIST : Packet
    {
        public SSMG_IRIS_CARD_ASSEMBLE_LIST()
        {
            data = new byte[3];
            offset = 2;
            ID = 0x140A;
        }

        public List<SagaDB.Item.Item> Cards
        {
            set
            {
                data = new byte[10 + 4 * value.Count];
                offset = 2;
                ID = 0x140A;

                PutByte((byte)value.Count);

                foreach (var item in value) PutUInt(item.Slot);
                PutUInt(0u);
                PutByte(01);
                PutShort(0x64);
                PutByte(0);
            }
        }
    }
}