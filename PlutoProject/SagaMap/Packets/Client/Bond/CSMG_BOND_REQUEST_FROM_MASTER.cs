using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_BOND_REQUEST_FROM_MASTER : Packet
    {
        public CSMG_BOND_REQUEST_FROM_MASTER()
        {
            offset = 2;
        }

        public uint CharID => GetUInt(2);

        public override Packet New()
        {
            return new CSMG_BOND_REQUEST_FROM_MASTER();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnBondRequestFromMaster(this);
        }
    }
}