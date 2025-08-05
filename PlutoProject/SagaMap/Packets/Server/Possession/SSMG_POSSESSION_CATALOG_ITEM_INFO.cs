using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_POSSESSION_CATALOG_ITEM_INFO : Packet
    {
        public SSMG_POSSESSION_CATALOG_ITEM_INFO()
        {
            data = new byte[56];
            offset = 2;
            ID = 0x1792;

            PutByte(0x03, 12);
            PutByte(0x0E, 25);
        }

        public byte Result
        {
            set => PutByte(value, 2);
        }

        public uint ActorID
        {
            set => PutUInt(value, 3);
        }

        public uint ItemID
        {
            set => PutUInt(value, 7);
        }

        public byte Level
        {
            set => PutByte(value, 11);
        }

        public int HP
        {
            set => PutInt(value, 13);
        }

        public int MP
        {
            set => PutInt(value, 17);
        }

        public int SP
        {
            set => PutInt(value, 21);
        }

        public short MinATK1
        {
            set => PutShort(value, 26);
        }

        public short MaxATK1
        {
            set => PutShort(value, 28);
        }

        public short MinATK2
        {
            set => PutShort(value, 30);
        }

        public short MaxATK2
        {
            set => PutShort(value, 32);
        }

        public short MinATK3
        {
            set => PutShort(value, 34);
        }

        public short MaxATK3
        {
            set => PutShort(value, 36);
        }

        public short MinMATK
        {
            set => PutShort(value, 38);
        }

        public short MaxMATK
        {
            set => PutShort(value, 40);
        }

        public short SHIT
        {
            set => PutShort(value, 42);
        }

        public short LHIT
        {
            set => PutShort(value, 44);
        }

        public short DEF
        {
            set => PutShort(value, 46);
        }

        public short MDEF
        {
            set => PutShort(value, 48);
        }

        public short SAVOID
        {
            set => PutShort(value, 50);
        }

        public short LAVOID
        {
            set => PutShort(value, 52);
        }

        public byte X
        {
            set => PutByte(value, 54);
        }

        public byte Y
        {
            set => PutByte(value, 55);
        }
    }
}