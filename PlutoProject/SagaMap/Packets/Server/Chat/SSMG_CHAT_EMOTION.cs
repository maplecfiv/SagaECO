using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_CHAT_EMOTION : Packet
    {
        public SSMG_CHAT_EMOTION()
        {
            data = new byte[10];
            offset = 2;
            ID = 0x1217;
        }

        public uint ActorID
        {
            set => PutUInt(value, 2);
        }

        public uint Emotion
        {
            set => PutUInt(value, 6);
        }
    }
}