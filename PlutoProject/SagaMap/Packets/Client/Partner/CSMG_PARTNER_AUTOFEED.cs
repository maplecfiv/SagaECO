using SagaLib;

namespace SagaMap.Packets.Client
{
    public class CSMG_PARTNER_AUTOFEED : Packet
    {
        public CSMG_PARTNER_AUTOFEED()
        {
            offset = 2;
        }

        public uint PartnerInventorySlot
        {
            get => GetUInt(2);
            set => PutUInt(value, 2);
        }

        public uint FoodInventorySlot
        {
            get => GetUInt(6);
            set => PutUInt(value, 6);
        }

        public override Packet New()
        {
            return new CSMG_PARTNER_AUTOFEED();
        }

        public override void Parse(SagaLib.Client client)
        {
            //((MapClient)(client)).netIO.Disconnect();
        }
    }
}