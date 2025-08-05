using SagaLib;

namespace SagaMap.Packets.Server.Chat
{
    public class SSMG_CHAT_WAITTYPE : Packet
    {
        public SSMG_CHAT_WAITTYPE()
        {
            data = new byte[8];
            offset = 2;
            ID = 0x121E;
        }

        public uint ActorID
        {
            set => PutUInt(value, 2);
        }

        public ushort type
        {
            set => PutUShort(value, 6);
        }
    }
}