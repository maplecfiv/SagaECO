using SagaLib;

namespace SagaMap.Packets.Server.FGarden
{
    public class SSMG_FG_WARE_HEADER : Packet
    {
        public SSMG_FG_WARE_HEADER()
        {
            data = new byte[10];
            offset = 2;
            ID = 0x1c25;
        }
    }
}