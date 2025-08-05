using SagaLib;

namespace SagaMap.Packets.Server.Chat
{
    public class SSMG_CHAT_EXEMOTION_UNLOCK : Packet
    {
        public SSMG_CHAT_EXEMOTION_UNLOCK()
        {
            data = new byte[67];
            offset = 2;
            ID = 0x1CE8;
            PutByte(0x10, 2);
        }

        public uint List1
        {
            set => PutUInt(value, 3);
        }

        public uint List2
        {
            set => PutUInt(value, 7);
        }

        public uint List3
        {
            set => PutUInt(value, 11);
        }

        public uint List4
        {
            set => PutUInt(value, 15);
        }

        public uint List5
        {
            set => PutUInt(value, 19);
        }
    }
}