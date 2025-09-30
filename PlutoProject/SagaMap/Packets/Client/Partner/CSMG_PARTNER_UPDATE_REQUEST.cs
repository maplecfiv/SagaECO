using SagaLib;

namespace SagaMap.Packets.Client.Partner
{
    public class CSMG_PARTNER_UPDATE_REQUEST : Packet
    {
        public CSMG_PARTNER_UPDATE_REQUEST()
        {
            offset = 2;
        }

        public uint PartnerItemID
        {
            get => GetUInt(2);
            set => PutUInt(value, 2);
        }

        public ushort unknown0
        {
            get => GetUShort(6);
            set => PutUShort(value, 6);
        }

        public override Packet New()
        {
            return new CSMG_PARTNER_UPDATE_REQUEST();
        }

        public override void Parse(SagaLib.Client client)
        {
            //((MapClient)(client)).NetIo.Disconnect();
        }
    }
}