using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_CHAT_EXPRESSION : Packet
    {
        public SSMG_CHAT_EXPRESSION()
        {
            data = new byte[9];
            offset = 2;
            ID = 0x1D0C;
        }

        public uint ActorID
        {
            set => PutUInt(value, 2);
        }

        public byte Motion
        {
            set => PutByte(value, 6);
        }

        public byte Loop
        {
            set => PutByte(value, 7);
        }

        public byte Special
        {
            set => PutByte(value, 8);
        }
    }
}