using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.Bond
{
    public class CSMG_BOND_REQUEST_MASTER_ANSWER : Packet
    {
        public CSMG_BOND_REQUEST_MASTER_ANSWER()
        {
            offset = 2;
        }

        public bool Rejected => GetByte(2) == 1;

        public uint PupilinCharID => GetUInt(3);

        public override Packet New()
        {
            return new CSMG_BOND_REQUEST_MASTER_ANSWER();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnBondMasterAnswer(this);
        }
    }
}