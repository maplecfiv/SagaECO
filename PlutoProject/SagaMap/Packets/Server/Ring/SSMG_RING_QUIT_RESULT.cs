using SagaLib;

namespace SagaMap.Packets.Server.Ring
{
    public class SSMG_RING_QUIT_RESULT : Packet
    {
        public SSMG_RING_QUIT_RESULT()
        {
            data = new byte[6];
            offset = 2;
            ID = 0x1ABE;
        }

        public int Result
        {
            set => PutInt(value, 2);
        }
    }
}