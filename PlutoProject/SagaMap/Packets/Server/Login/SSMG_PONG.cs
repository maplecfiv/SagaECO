using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_PONG : Packet
    {
        public SSMG_PONG()
        {
            data = new byte[2];
            offset = 2;
            ID = 0x33;
        }
    }
}