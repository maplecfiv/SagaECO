using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.PProtect
{
    public class CSMG_PPROTECT_READY : Packet
    {
        public CSMG_PPROTECT_READY()
        {
            offset = 2;
        }

        public byte State => GetByte(2);

        public override Packet New()
        {
            return new CSMG_PPROTECT_READY();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnPProtectReady(this);
        }
    }
}