using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.PProtect
{
    public class CSMG_PPROTECT_CREATED_OUT : Packet
    {
        public CSMG_PPROTECT_CREATED_OUT()
        {
            offset = 2;
        }

        public override Packet New()
        {
            return new CSMG_PPROTECT_CREATED_OUT();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnPProtectCreatedOut(this);
        }
    }
}