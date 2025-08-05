using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_PARTNER_PERK_CONFIRM : Packet
    {
        public CSMG_PARTNER_PERK_CONFIRM()
        {
            offset = 2;
        }

        public uint PartnerInventorySlot
        {
            get => GetUInt(2);
            set => PutUInt(value, 2);
        }

        public byte unknown0 => GetByte(6);

        public byte PerkListLength
        {
            get => GetByte(7);
            set => PutByte(value, 7);
        }

        public byte Perk0
        {
            get => GetByte(8);
            set => PutByte(value, 8);
        }

        public byte Perk1
        {
            get => GetByte(9);
            set => PutByte(value, 9);
        }

        public byte Perk2
        {
            get => GetByte(10);
            set => PutByte(value, 10);
        }

        public byte Perk3
        {
            get => GetByte(11);
            set => PutByte(value, 11);
        }

        public byte Perk4
        {
            get => GetByte(12);
            set => PutByte(value, 12);
        }

        public byte Perk5
        {
            get => GetByte(13);
            set => PutByte(value, 13);
        }

        public override Packet New()
        {
            return new CSMG_PARTNER_PERK_CONFIRM();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnPartnerPerkConfirm(this);
        }
    }
}