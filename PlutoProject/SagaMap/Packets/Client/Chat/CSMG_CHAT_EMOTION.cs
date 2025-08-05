using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.Chat
{
    public class CSMG_CHAT_EMOTION : Packet
    {
        public CSMG_CHAT_EMOTION()
        {
            offset = 2;
        }

        public uint Emotion => GetUInt(2);

        public override Packet New()
        {
            return new CSMG_CHAT_EMOTION();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnEmotion(this);
        }
    }
}