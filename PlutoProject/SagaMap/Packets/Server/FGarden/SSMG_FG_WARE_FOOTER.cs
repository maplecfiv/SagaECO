using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_FG_WARE_FOOTER : Packet
    {
        public SSMG_FG_WARE_FOOTER()
        {
            data = new byte[10];
            offset = 2;
            ID = 0x1c27;
        }
    }
}