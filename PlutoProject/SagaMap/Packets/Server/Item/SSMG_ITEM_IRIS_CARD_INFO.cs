using SagaLib;

namespace SagaMap.Packets.Server.Item
{
    public class SSMG_ITEM_IRIS_CARD_INFO : Packet
    {
        public SSMG_ITEM_IRIS_CARD_INFO()
        {
            data = new byte[9];
            offset = 2;
            ID = 0x09D5;
        }

        public SagaDB.Item.Item Item
        {
            set
            {
                PutUInt(value.Slot, 2);
                var buf = new byte[9 + 4 * value.CurrentSlot];
                data.CopyTo(buf, 0);
                data = buf;

                PutByte(value.CurrentSlot, 6);
                for (var i = 0; i < value.CurrentSlot; i++)
                    if (i < value.Cards.Count)
                        PutUInt(value.Cards[i].ID);
                    else
                        PutUInt(0);

                PutShort(value.CurrentSlot);
            }
        }
    }
}