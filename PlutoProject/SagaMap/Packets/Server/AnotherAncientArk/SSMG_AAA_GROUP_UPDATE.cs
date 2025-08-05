using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_AAA_GROUP_UPDATE : Packet
    {
        public SSMG_AAA_GROUP_UPDATE()
        {
            data = new byte[4];
            offset = 2;
            ID = 0x0;
        }
    }
}