using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_CHAT_WAITTYPE : Packet
    {
        public CSMG_CHAT_WAITTYPE()
        {
            offset = 2;
        }

        public byte type => GetByte(3);

        public override Packet New()
        {
            return new CSMG_CHAT_WAITTYPE();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnWaitType(this);
        }
    }
}