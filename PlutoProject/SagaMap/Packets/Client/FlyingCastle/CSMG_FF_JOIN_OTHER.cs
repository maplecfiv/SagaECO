using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.FlyingCastle
{
    public class CSMG_FFGARDEN_JOIN_OTHER : Packet
    {
        public CSMG_FFGARDEN_JOIN_OTHER()
        {
            offset = 2;
        }

        public uint ff_id => GetUInt(2);

        public override Packet New()
        {
            return new CSMG_FFGARDEN_JOIN_OTHER();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnFFGardenOtherJoin(this);
        }
    }
}