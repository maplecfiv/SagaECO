using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_FG_WARE_GET_RESULT : Packet
    {
        public SSMG_FG_WARE_GET_RESULT()
        {
            data = new byte[10];
            offset = 2;
            ID = 0x1c2b;
        }
    }
}