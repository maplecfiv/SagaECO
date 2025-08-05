using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_PARTNER_RANK_UPDATE : Packet
    {
        public SSMG_PARTNER_RANK_UPDATE()
        {
            data = new byte[9];
            offset = 2;
            ID = 0x2192;
        }

        public uint PartnerInventorySlot
        {
            set => PutUInt(value, 2);
        }

        public byte PartnerRank
        {
            set => PutByte(value, 6);
        }

        public ushort PerkPoint
        {
            set => PutUShort(value, 7);
        }
    }
}