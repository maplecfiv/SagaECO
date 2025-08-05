using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.Partner
{
    public class CSMG_PARTNER_PERK_PREVIEW : Packet
    {
        public CSMG_PARTNER_PERK_PREVIEW()
        {
            offset = 2;
        }

        public uint PartnerInventorySlot
        {
            get => GetUInt(2);
            set => PutUInt(value, 2);
        }

        public byte PerkListLength
        {
            get => GetByte(6);
            set => PutByte(value, 6);
        }

        public byte Perk0
        {
            get => GetByte(7);
            set => PutByte(value, 7);
        }

        public byte Perk1
        {
            get => GetByte(8);
            set => PutByte(value, 8);
        }

        public byte Perk2
        {
            get => GetByte(9);
            set => PutByte(value, 9);
        }

        public byte Perk3
        {
            get => GetByte(10);
            set => PutByte(value, 10);
        }

        public byte Perk4
        {
            get => GetByte(11);
            set => PutByte(value, 11);
        }

        public byte Perk5
        {
            get => GetByte(12);
            set => PutByte(value, 12);
        }

        public override Packet New()
        {
            return new CSMG_PARTNER_PERK_PREVIEW();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnPartnerPerkPreview(this);
        }
    }
}