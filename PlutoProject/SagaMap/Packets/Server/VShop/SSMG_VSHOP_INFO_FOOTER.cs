using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_VSHOP_INFO_FOOTER : Packet
    {
        public SSMG_VSHOP_INFO_FOOTER()
        {
            data = new byte[2];
            offset = 2;
            ID = 0x0651;
        }
    }
}