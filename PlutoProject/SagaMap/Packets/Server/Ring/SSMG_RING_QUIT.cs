using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_RING_QUIT : Packet
    {
        public enum Reasons
        {
            DISSOLVE = 1,
            LEAVE,
            KICK
        }

        public SSMG_RING_QUIT()
        {
            data = new byte[12];
            offset = 2;
            ID = 0x1ACD;

            PutByte(1, 6);
        }

        public uint RingID
        {
            set => PutUInt(value, 2);
        }

        public Reasons Reason
        {
            set => PutInt((int)value, 8);
        }
    }
}