using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_FG_ACTOR_APPEAR : Packet
    {
        public SSMG_FG_ACTOR_APPEAR(byte type)
        {
            data = new byte[45];
            offset = 2;
            if (type == 1)
                ID = 0x1BEF;
            else if (type == 3)
                ID = 0x2058;
            else
                ID = 0x1C03;
            PutByte(0, 14);
            PutByte(1, 6); //count always 1 in COF!!!
            PutByte(1, 11); //always!!!
            PutByte(1, 16); //always!!!
            PutByte(1, 21);
            PutByte(0, 22); //unknown
            PutByte(1, 23);
            PutByte(1, 26);
            PutByte(1, 29);
            PutByte(1, 32);
            PutByte(1, 35);
            PutByte(1, 38);
            PutByte(1, 41);
            PutByte(1, 44);
        }

        public uint MapID
        {
            set => PutUInt(value, 2);
        }

        public uint ActorID
        {
            set => PutUInt(value, 7);
        }

        public uint ItemID
        {
            set => PutUInt(value, 12);
        }

        public uint PictID
        {
            set => PutUInt(value, 17);
        }

        public short X
        {
            set => PutShort(value, 24);
        }

        public short Y
        {
            set => PutShort(value, 27);
        }

        public short Z
        {
            set => PutShort(value, 30);
        }

        public short Xaxis
        {
            set => PutShort(value, 33);
        }

        public short Yaxis
        {
            set => PutShort(value, 36);
        }

        public short Zaxis
        {
            set => PutShort(value, 39);
        }

        public ushort Motion
        {
            set => PutUShort(value, 42);
        }

        public string Name
        {
            set
            {
                var name = Global.Unicode.GetBytes(value + "\0");
                var buf = new byte[58 + name.Length];
                data.CopyTo(buf, 0);
                data = buf;
                PutByte((byte)name.Length, 45);
                PutBytes(name, 46);
            }
        }
    }
}