using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_GOLEM_WAREHOUSE_GET : Packet
    {
        public SSMG_GOLEM_WAREHOUSE_GET()
        {
            data = new byte[6];
            offset = 2;
            ID = 0x17F9;
        }
    }
}