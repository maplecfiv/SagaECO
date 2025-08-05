using SagaDB.Actor;
using SagaLib;

namespace SagaMap.Packets.Server.Stamp
{
    public class SSMG_STAMP_USE : Packet
    {
        public SSMG_STAMP_USE()
        {
            data = new byte[8];
            offset = 2;
            ID = 0x1BC1;
        }

        public uint Page
        {
            set => PutUInt((byte)value, 2);
        }

        public StampGenre Genre
        {
            set => PutByte((byte)value, 6);
        }

        public StampSlot slot
        {
            set
            {
                switch (value)
                {
                    case StampSlot.One:
                        PutByte(0, 7);
                        break;
                    case StampSlot.Two:
                        PutByte(1, 7);
                        break;
                    case StampSlot.Three:
                        PutByte(2, 7);
                        break;
                    case StampSlot.Four:
                        PutByte(3, 7);
                        break;
                    case StampSlot.Five:
                        PutByte(4, 7);
                        break;
                    case StampSlot.Six:
                        PutByte(5, 7);
                        break;
                    case StampSlot.Seven:
                        PutByte(6, 7);
                        break;
                    case StampSlot.Eight:
                        PutByte(7, 7);
                        break;
                    case StampSlot.Nine:
                        PutByte(8, 7);
                        break;
                    case StampSlot.Ten:
                        PutByte(9, 7);
                        break;
                }
            }
        }
    }
}