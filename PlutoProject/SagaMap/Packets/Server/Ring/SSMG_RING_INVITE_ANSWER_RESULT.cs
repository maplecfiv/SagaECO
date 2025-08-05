using SagaLib;

namespace SagaMap.Packets.Server.Ring
{
    public class SSMG_RING_INVITE_ANSWER_RESULT : Packet
    {
        public enum RESULTS
        {
            OK = 2,
            CANNOT_FIND_TARGET = -2,
            ALREADY_IN_RING = -11,
            MEMBER_EXCEED = -12
        }

        public SSMG_RING_INVITE_ANSWER_RESULT()
        {
            data = new byte[6];
            offset = 2;
            ID = 0x1AB3;
        }

        public RESULTS Result
        {
            set => PutInt((int)value, 2);
        }
    }
}