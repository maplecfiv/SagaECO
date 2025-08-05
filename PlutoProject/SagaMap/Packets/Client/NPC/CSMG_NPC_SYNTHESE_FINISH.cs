using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.NPC
{
    public class CSMG_NPC_SYNTHESE_FINISH : Packet
    {
        public CSMG_NPC_SYNTHESE_FINISH()
        {
            offset = 2;
        }

        public override Packet New()
        {
            return new CSMG_NPC_SYNTHESE_FINISH();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnNPCSyntheseFinish(this);
        }
    }
}