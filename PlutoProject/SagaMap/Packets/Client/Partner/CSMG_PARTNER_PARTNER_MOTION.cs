using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_PARTNER_PARTNER_MOTION : Packet
    {
        public CSMG_PARTNER_PARTNER_MOTION()
        {
            offset = 2;
        }

        public uint id
        {
            get => GetUInt(2);
            set => PutUInt(value, 2);
        }

        public override Packet New()
        {
            return new CSMG_PARTNER_PARTNER_MOTION();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnPartnerMotion(this);
        }
    }
}