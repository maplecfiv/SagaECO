using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.PProtect
{
    public class CSMG_PPROTECT_CREATED_INITI : Packet
    {
        public CSMG_PPROTECT_CREATED_INITI()
        {
            offset = 2;
        }

        public override Packet New()
        {
            return new CSMG_PPROTECT_CREATED_INITI();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnPProtectCreatedIniti(this);
        }
    }
}