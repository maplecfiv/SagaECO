using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_TEST_EVOLVE_OPEN : Packet
    {
        public SSMG_TEST_EVOLVE_OPEN()
        {
            data = new byte[3];
            offset = 2;
            ID = 0x0606;
            PutByte(0, 2);
        }
    }
}