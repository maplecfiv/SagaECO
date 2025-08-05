using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_ACTOR_REQUEST_PC_INFO : Packet
    {
        public CSMG_ACTOR_REQUEST_PC_INFO()
        {
            offset = 2;
        }

        public uint ActorID => GetUInt(2);

        public override Packet New()
        {
            return new CSMG_ACTOR_REQUEST_PC_INFO();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnRequestPCInfo(this);
        }
    }
}