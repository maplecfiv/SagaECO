using SagaLib;

namespace SagaMap.Packets.Server.Partner
{
    public class SSMG_PARTNER_INFO_DETAIL : Packet
    {
        public SSMG_PARTNER_INFO_DETAIL()
        {
            data = new byte[58];
            offset = 2;
            ID = 0x217B;
            PutByte(3, 6);
            PutByte(0x13, 19);
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

        public ushort MoveSpeed
        {
            set => PutUShort(value, 20);
        }

        public ushort MinPhyATK
        {
            set
            {
                PutUShort(value, 22);
                PutUShort(value, 24); //没显示
                PutUShort(value, 26); //没显示
            }
        }

        public ushort MaxPhyATK
        {
            set
            {
                PutUShort(value, 28);
                PutUShort(value, 30); //没显示
                PutUShort(value, 32); //没显示
            }
        }

        public ushort MinMAGATK
        {
            set => PutUShort(value, 34);
        }

        public ushort MaxMAGATK
        {
            set => PutUShort(value, 36);
        }

        public ushort DEF
        {
            set => PutUShort(value, 38);
        }

        public short DEFAdd
        {
            set => PutShort(value, 40);
        }

        public ushort MDEF
        {
            set => PutUShort(value, 42);
        }

        public short MDEFAdd
        {
            set => PutShort(value, 44);
        }

        public ushort ShortHit
        {
            set => PutUShort(value, 46);
        }

        public ushort LongHit
        {
            set => PutUShort(value, 48);
        }

        public ushort ShortAvoid
        {
            set => PutUShort(value, 50);
        }

        public ushort LongAvoid
        {
            set => PutUShort(value, 52);
        }

        public short ASPD
        {
            set => PutShort(value, 54);
        }

        public short CSPD
        {
            set => PutShort(value, 56);
        }
    }
}