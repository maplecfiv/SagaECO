using SagaLib;

namespace SagaMap.Packets.Server.NPC
{
    public class SSMG_NPC_VOICE_PLAY : Packet
    {
        public SSMG_NPC_VOICE_PLAY()
        {
            data = new byte[7];
            offset = 2;
            ID = 0x0622;
        }

        public uint VoiceID
        {
            set => PutUInt(value, 2);
        }

        public byte Loop
        {
            set => PutByte(value, 6);
        }
    }
}