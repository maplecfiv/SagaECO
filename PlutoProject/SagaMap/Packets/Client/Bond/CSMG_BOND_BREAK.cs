using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.Bond
{
    public class CSMG_BOND_CANCEL : Packet
    {
        public CSMG_BOND_CANCEL()
        {
            offset = 2;
        }

        public uint TargetCharID => GetUInt(2);

        public override Packet New()
        {
            return new CSMG_BOND_CANCEL();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnBondBreak(this);
        }
    }
}