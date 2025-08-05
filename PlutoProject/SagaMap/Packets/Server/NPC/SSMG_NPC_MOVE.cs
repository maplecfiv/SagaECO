using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_NPC_MOVE : Packet
    {
        public SSMG_NPC_MOVE()
        {
            data = new byte[20];
            offset = 2;
            ID = 0x05E9;

            //PutShort(0x12, 11);//unknown
            //PutShort(0x0A, 15);//unknown
            PutByte(0xFF, 18); //unknown
            PutByte(0xFF, 19); //unknown
        }

        public uint NPCID
        {
            set => PutUInt(value, 2);
        }

        public byte X
        {
            set => PutByte(value, 6);
        }

        public byte Y
        {
            set => PutByte(value, 7);
        }

        public ushort Speed
        {
            set => PutUShort(value, 8);
        }

        public byte Dir
        {
            set => PutByte(value, 10);
        }

        public ushort ShowType
        {
            set => PutUShort(value, 11);
        }

        public ushort Motion
        {
            set => PutUShort(value, 13);
        }

        public ushort MotionSpeed
        {
            set => PutUShort(value, 15);
        }

        public byte Type
        {
            set => PutByte(value, 17);
        }
    }
}