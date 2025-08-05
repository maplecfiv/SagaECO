using SagaLib;
using SagaMap.Scripting;

namespace SagaMap.Scripting
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
}

namespace SagaMap.Packets.Server
{
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