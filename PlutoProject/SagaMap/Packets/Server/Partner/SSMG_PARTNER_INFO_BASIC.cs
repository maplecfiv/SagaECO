using SagaLib;

namespace SagaMap.Packets.Server.Partner
{
    public class SSMG_PARTNER_INFO_BASIC : Packet
    {
        public SSMG_PARTNER_INFO_BASIC()
        {
            data = new byte[45];
            offset = 2;
            ID = 0x217A;
            PutByte(6, 30);
        }

        public uint InventorySlot
        {
            set => PutUInt(value, 2);
        }

        public byte Level
        {
            set => PutByte(value, 6);
        }

        public uint EXPPercentage
        {
            set => PutUInt(value, 7);
        }

        public byte Rebirth
        {
            set => PutByte(value, 11);
        }

        public byte Rank //1perbarslot 10per rank level at least 1
        {
            set => PutByte(value, 12);
        }

        public byte ReliabilityColor //max is 9,more will collapse the client at least 0
        {
            set => PutByte(value, 13);
        }

        public ushort ReliabilityUpRate
        {
            set => PutUShort(value, 14);
        }

        /// <summary>
        ///     seconds
        /// </summary>
        public uint NextFeedTime
        {
            set => PutUInt(value, 16);
        }

        public byte AIMode
        {
            set => PutByte(value, 20);
        }

        public uint MaxNextFeedTime //infinity feed time to show --:--
        {
            set => PutUInt(value, 21);
        }

        /// <summary>
        ///     0 for 1 sheet, 1 for 2 sheet
        /// </summary>
        public byte CustomAISheet
        {
            set => PutByte(value, 25);
        }

        public byte AICommandCount1
        {
            set => PutByte(value, 26);
        }

        public byte AICommandCount2
        {
            set => PutByte(value, 27);
        }

        public ushort PerkPoint
        {
            set => PutUShort(value, 28);
        }

        public byte PerkListCount
        {
            set => PutByte(6, 30);
        }

        public byte Perk0
        {
            set => PutByte(value, 31);
        }

        public byte Perk1
        {
            set => PutByte(value, 32);
        }

        public byte Perk2
        {
            set => PutByte(value, 33);
        }

        public byte Perk3
        {
            set => PutByte(value, 34);
        }

        public byte Perk4
        {
            set => PutByte(value, 35);
        }

        public byte Perk5
        {
            set => PutByte(value, 36);
        }

        public uint WeaponID
        {
            set => PutUInt(value, 37);
        }

        public uint ArmorID
        {
            set => PutUInt(value, 41);
        }
    }
}