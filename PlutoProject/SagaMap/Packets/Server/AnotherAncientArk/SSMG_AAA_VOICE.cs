using SagaLib;

namespace SagaMap.Packets.Server.AnotherAncientArk
{
    public class SSMG_AAA_VOICE : Packet
    {
        public SSMG_AAA_VOICE()
        {
            data = new byte[3];
            offset = 2;
            ID = 0x1FB4;
        }

        public ushort VoiceID
        {
            set => PutUShort(value, 2);
        }
    }
}