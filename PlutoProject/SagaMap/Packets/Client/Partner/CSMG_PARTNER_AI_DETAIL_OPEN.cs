using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_PARTNER_AI_DETAIL_OPEN : Packet
    {
        public CSMG_PARTNER_AI_DETAIL_OPEN()
        {
            offset = 2;
        }

        public uint PartnerInventorySlot
        {
            get => GetUInt(2);
            set => PutUInt(value, 2);
        }

        public override Packet New()
        {
            return new CSMG_PARTNER_AI_DETAIL_OPEN();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnPartnerAIOpen();
        }
    }
}