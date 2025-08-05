using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_NPC_VOICE_STOP : Packet
    {
        public SSMG_NPC_VOICE_STOP()
        {
            data = new byte[6];
            offset = 2;
            ID = 0x0623;
        }

        public uint VoiceID
        {
            set => PutUInt(value, 2);
        }
    }
}