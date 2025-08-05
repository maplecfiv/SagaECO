using SagaLib;

namespace SagaMap.Packets.Server.Possession
{
    public class SSMG_POSSESSION_CANCEL : Packet
    {
        public SSMG_POSSESSION_CANCEL()
        {
            data = new byte[14];
            offset = 2;
            ID = 0x1780;
        }

        public uint FromID
        {
            set => PutUInt(value, 2);
        }

        public uint ToID
        {
            set => PutUInt(value, 6);
        }

        public PossessionPosition Position
        {
            set => PutByte((byte)value, 10);
        }

        public byte X
        {
            set => PutByte(value, 11);
        }

        public byte Y
        {
            set => PutByte(value, 12);
        }

        public byte Dir
        {
            set => PutByte(value, 13);
        }
    }
}