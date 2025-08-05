using SagaLib;

namespace SagaMap.Packets.Server.Item
{
    public class SSMG_ITEM_CHANGE_ADD : Packet
    {
        public SSMG_ITEM_CHANGE_ADD()
        {
            data = new byte[9];
            offset = 2;
            ID = 0x022D;
            PutByte(3, 2);
            PutUShort(10100, 3);
            PutUShort(10101, 5);
            PutUShort(10102, 7);
        }
    }
}