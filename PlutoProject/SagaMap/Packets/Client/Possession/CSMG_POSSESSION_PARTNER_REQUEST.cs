using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_POSSESSION_PARTNER_REQUEST : Packet
    {
        public CSMG_POSSESSION_PARTNER_REQUEST()
        {
            offset = 2;
        }

        public uint InventorySlot => GetUInt(2);

        public PossessionPosition PossessionPosition => (PossessionPosition)GetByte(6);

        public override Packet New()
        {
            return new CSMG_POSSESSION_PARTNER_REQUEST();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnPartnerPossessionRequest(this);
        }
    }
}