using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.FFarden
{
    public class CSMG_FFGARDEN_JOIN : Packet
    {
        public CSMG_FFGARDEN_JOIN()
        {
            offset = 2;
        }

        public override Packet New()
        {
            return new CSMG_FFGARDEN_JOIN();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnFFardenJoin(this);
        }
    }
}