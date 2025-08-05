using SagaLib;

namespace SagaMap.Packets.Server.Partner
{
    public class SSMG_PARTNER_PERK_PREVIEW : Packet
    {
        private readonly byte perklistlen = 6;

        public SSMG_PARTNER_PERK_PREVIEW()
        {
            data = new byte[67];
            offset = 2;
            ID = 0x217D;
            PutByte(3, 6);
            PutByte(0x13, 28);
            PutByte(perklistlen, 21);
        }

        public uint InventorySlot
        {
            set => PutUInt(value, 2);
        }

        public byte unknown0 //has sth to do with part of data length
        {
            set => PutByte(value, 6);
        }

        public uint MaxHP
        {
            set => PutUInt(value, 7);
        }

        public uint MaxMP
        {
            set => PutUInt(value, 11);
        }

        public uint MaxSP
        {
            set => PutUInt(value, 15);
        }

        public ushort Perkpoints
        {
            set => PutUShort(value, 19);
        }

        public byte Perk0
        {
            set => PutByte(value, 22);
        }

        public byte Perk1
        {
            set => PutByte(value, 23);
        }

        public byte Perk2
        {
            set => PutByte(value, 24);
        }

        public byte Perk3
        {
            set => PutByte(value, 25);
        }

        public byte Perk4
        {
            set => PutByte(value, 26);
        }

        public byte Perk5
        {
            set => PutByte(value, 27);
        }

        public ushort MoveSpeed
        {
            set => PutUShort(value, 29);
        }

        public ushort MinPhyATK
        {
            set
            {
                PutUShort(value, 31);
                PutUShort(value, 33); //没显示
                PutUShort(value, 35); //没显示
            }
        }

        public ushort MaxPhyATK
        {
            set
            {
                PutUShort(value, 37);
                PutUShort(value, 39); //没显示
                PutUShort(value, 41); //没显示
            }
        }

        public ushort MinMAGATK
        {
            set => PutUShort(value, 43);
        }

        public ushort MaxMAGATK
        {
            set => PutUShort(value, 45);
        }

        public ushort DEF
        {
            set => PutUShort(value, 47);
        }

        public ushort DEFAdd
        {
            set => PutUShort(value, 49);
        }

        public ushort MDEF
        {
            set => PutUShort(value, 51);
        }

        public ushort MDEFAdd
        {
            set => PutUShort(value, 53);
        }

        public ushort ShortHit
        {
            set => PutUShort(value, 55);
        }

        public ushort LongHit
        {
            set => PutUShort(value, 57);
        }

        public ushort ShortAvoid
        {
            set => PutUShort(value, 59);
        }

        public ushort LongAvoid
        {
            set => PutUShort(value, 61);
        }

        public short ASPD
        {
            set => PutShort(value, 63);
        }

        public short CSPD
        {
            set => PutShort(value, 65);
        }
    }
}