using SagaLib;

namespace SagaMap.Packets.Server.Golem
{
    public class SSMG_GOLEM_SHOP_FOOTER : Packet
    {
        public SSMG_GOLEM_SHOP_FOOTER()
        {
            data = new byte[2];
            offset = 2;
            ID = 0x1802;
        }
    }
}