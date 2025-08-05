using SagaLib;

namespace SagaMap.Packets.Server.Ring
{
    public class SSMG_RING_INVITE_RESULT : Packet
    {
        public enum RESULT
        {
            OK = 0,
            CANNOT_FIND_TARGET = -1,
            SERVER_ERROR = -2,
            TARGET_NO_RING_INVITE = -3,
            TARGET_ALREADY_IN_RING = -4,
            NO_RING = -5,
            NO_RIGHT = -6
        }

        public SSMG_RING_INVITE_RESULT()
        {
            data = new byte[6];
            offset = 2;
            ID = 0x1AB3;
        }

        public int Result
        {
            set => PutInt(value, 2);
        }
    }
}