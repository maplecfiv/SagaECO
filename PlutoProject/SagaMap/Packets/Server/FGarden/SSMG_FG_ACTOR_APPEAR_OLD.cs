using SagaLib;


namespace SagaMap.Packets.Server
{
    public class SSMG_FG_ACTOR_APPEAR_OLD : Packet
    {
        public SSMG_FG_ACTOR_APPEAR_OLD(byte type)
        {
            data = new byte[29];
            offset = 2;
            if (type == 1)
                ID = 0x1BEF;
            else if (type == 3)
                ID = 0x2058;
            else
                ID = 0x1C03;
            PutByte(0, 14);
        }

        public uint ActorID
        {
            set
            {
                PutUInt(value, 2);
            }
        }

        public uint ItemID
        {
            set
            {
                PutUInt(value, 6);
            }
        }

        public uint PictID
        {
            set
            {
                PutUInt(value, 10);
            }
        }

        public short X
        {
            set
            {
                PutShort(value, 15);
            }
        }

        public short Y
        {
            set
            {
                PutShort(value, 17);
            }
        }

        public short Z
        {
            set
            {
                PutShort(value, 19);
            }
        }

        public short Xaxis
        {
            set
            {
                PutShort(value, 21);
            }
        }
        public short Yaxis
        {
            set
            {
                PutShort(value, 23);
            }
        }
        public short Zaxis
        {
            set
            {
                PutShort(value, 25);
            }
        }

        public ushort Motion
        {
            set
            {
                PutUShort(value, 27);
            }
        }

        public string Name
        {
            set
            {
                byte[] name = Global.Unicode.GetBytes(value + "\0");
                byte[] buf = new byte[30 + name.Length];
                data.CopyTo(buf, 0);
                data = buf;
                PutByte((byte)name.Length, 29);
                PutBytes(name, 30);
            }
        }
    }
}

