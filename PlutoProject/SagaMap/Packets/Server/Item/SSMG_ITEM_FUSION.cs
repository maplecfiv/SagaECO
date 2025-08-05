using SagaLib;

namespace SagaMap.Packets.Server.Item
{
    public class SSMG_ITEM_FUSION : Packet
    {
        public SSMG_ITEM_FUSION()
        {
            data = new byte[2];
            offset = 2;
            ID = 0x13D8;
        }
    }
}