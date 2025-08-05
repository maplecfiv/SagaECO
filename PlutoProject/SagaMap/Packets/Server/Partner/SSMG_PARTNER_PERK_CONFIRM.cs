using SagaLib;

namespace SagaMap.Packets.Server.Partner
{
    public class SSMG_PARTNER_PERK_CONFIRM : Packet
    {
        private readonly byte perklistlen = 6;

        public SSMG_PARTNER_PERK_CONFIRM()
        {
            data = new byte[68];
            offset = 2;
            ID = 0x217F;
            PutByte(3, 9);
            PutByte(0x13, 29);
            PutByte(perklistlen, 22);
        }

        public byte unknown0
        {
            set => PutByte(0, 2);
        }

        public uint InventorySlot
        {
            set => PutUInt(value, 3);
        }

        public ushort Perkpoints
        {
            set => PutUShort(value, 7);
        }

        public byte unknown1 //has sth to do with part of data length
        {
            set => PutByte(3, 9);
        }

        public uint MaxHP
        {
            set => PutUInt(value, 10);
        }

        public uint MaxMP
        {
            set => PutUInt(value, 14);
        }

        public uint MaxSP
        {
            set => PutUInt(value, 18);
        }

        public byte Perk0
        {
            set => PutByte(value, 23);
        }

        public byte Perk1
        {
            set => PutByte(value, 24);
        }

        public byte Perk2
        {
            set => PutByte(value, 25);
        }

        public byte Perk3
        {
            set => PutByte(value, 26);
        }

        public byte Perk4
        {
            set => PutByte(value, 27);
        }

        public byte Perk5
        {
            set => PutByte(value, 28);
        }

        public ushort MoveSpeed
        {
            set => PutUShort(value, 30);
        }

        public ushort MinPhyATK
        {
            set
            {
                PutUShort(value, 32);
                PutUShort(value, 34); //没显示
                PutUShort(value, 36); //没显示
            }
        }

        public ushort MaxPhyATK
        {
            set
            {
                PutUShort(value, 38);
                PutUShort(value, 40); //没显示
                PutUShort(value, 42); //没显示
            }
        }

        public ushort MinMAGATK
        {
            set => PutUShort(value, 44);
        }

        public ushort MaxMAGATK
        {
            set => PutUShort(value, 46);
        }

        public ushort DEF
        {
            set => PutUShort(value, 48);
        }

        public ushort DEFAdd
        {
            set => PutUShort(value, 50);
        }

        public ushort MDEF
        {
            set => PutUShort(value, 52);
        }

        public ushort MDEFAdd
        {
            set => PutUShort(value, 54);
        }

        public ushort ShortHit
        {
            set => PutUShort(value, 56);
        }

        public ushort LongHit
        {
            set => PutUShort(value, 58);
        }

        public ushort ShortAvoid
        {
            set => PutUShort(value, 60);
        }

        public ushort LongAvoid
        {
            set => PutUShort(value, 62);
        }

        public short ASPD
        {
            set => PutShort(value, 64);
        }

        public short CSPD
        {
            set => PutShort(value, 66);
        }
    }
}