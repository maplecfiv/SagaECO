using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_TEST_EVOLVE_OPEN3 : Packet
    {
        public SSMG_TEST_EVOLVE_OPEN3()
        {
            data = new byte[4];
            offset = 2;
            ID = 0x1EDC;
            PutUShort(0x6E, 2);
        }
    }
}