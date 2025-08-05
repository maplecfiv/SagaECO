using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_GOLEM_SHOP_BUY_SET : Packet
    {
        public SSMG_GOLEM_SHOP_BUY_SET()
        {
            data = new byte[3];
            offset = 2;
            ID = 0x181E;
        }
    }
}