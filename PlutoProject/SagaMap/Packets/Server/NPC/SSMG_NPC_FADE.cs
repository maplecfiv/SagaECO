using SagaLib;

namespace SagaMap.Packets.Server.NPC
{
    public enum FadeType
    {
        In,
        Out
    }

    public enum FadeEffect
    {
        Black,
        White
    }

    public class SSMG_NPC_FADE : Packet
    {
        public SSMG_NPC_FADE()
        {
            data = new byte[10];
            offset = 2;
            ID = 0x05FE;
        }

        public FadeType FadeType
        {
            set => PutByte((byte)value, 2);
        }

        public FadeEffect FadeEffect
        {
            set => PutByte((byte)value, 3);
        }
    }
}