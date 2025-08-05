using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.Chat
{
    public class CSMG_CHAT_SIT : Packet
    {
        public CSMG_CHAT_SIT()
        {
            offset = 2;
        }

        public override Packet New()
        {
            return new CSMG_CHAT_SIT();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnSit(this);
        }
    }
}