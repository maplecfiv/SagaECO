using SagaLib;

namespace SagaMap.Packets.Server.Actor
{
    public class SSMG_TEST_EVOLVE_OPEN2 : Packet
    {
        public SSMG_TEST_EVOLVE_OPEN2()
        {
            data = new byte[3];
            offset = 2;
            ID = 0x0605;
            PutByte(1, 2);
        }
    }
}