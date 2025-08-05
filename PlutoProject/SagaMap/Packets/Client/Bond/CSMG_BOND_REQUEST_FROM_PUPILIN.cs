using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.Bond
{
    public class CSMG_BOND_REQUEST_FROM_PUPILIN : Packet
    {
        public CSMG_BOND_REQUEST_FROM_PUPILIN()
        {
            offset = 2;
        }

        public uint CharID => GetUInt(2);

        public override Packet New()
        {
            return new CSMG_BOND_REQUEST_FROM_PUPILIN();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnBondRequestFromPupilin(this);
        }
    }
}