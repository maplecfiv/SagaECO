using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_CHAT_MOTION : Packet
    {
        public SSMG_CHAT_MOTION()
        {
            data = new byte[14];
            offset = 2;
            ID = 0x121C;
        }

        public uint ActorID
        {
            set => PutUInt(value, 2);
        }

        public MotionType Motion
        {
            set => PutUShort((ushort)value, 6);
        }

        public byte Loop
        {
            set => PutByte(value, 8);
        }

        public uint motionspeed
        {
            set => PutUInt(value, 10);
        }
    }
}