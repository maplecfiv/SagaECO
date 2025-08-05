using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_FF_UNIT_APPEAR : Packet
    {
        public SSMG_FF_UNIT_APPEAR()
        {
            data = new byte[29];
            offset = 2;
            ID = 0x20D1;
        }

        public uint ActorID
        {
            set => PutUInt(value, 2);
        }

        public uint ItemID
        {
            set => PutUInt(value, 6);
        }

        public uint PictID
        {
            set => PutUInt(value, 10);
        }

        public short unknown
        {
            set => PutShort(8, 14);
        }

        public short X
        {
            set => PutShort(value, 16);
        }

        public short Z
        {
            set => PutShort(value, 18);
        }

        public short Yaxis
        {
            set => PutShort(value, 20);
        }

        public short unknown2
        {
            set => PutShort(186, 22);
        }

        public uint unknown3
        {
            set => PutUInt(0, 24);
        }


        public byte unknown4
        {
            set => PutByte(2, 28);
        }
    }
}