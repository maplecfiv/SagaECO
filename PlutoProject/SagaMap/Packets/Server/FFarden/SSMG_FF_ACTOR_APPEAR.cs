using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_FF_ACTOR_APPEAR : Packet
    {
        public SSMG_FF_ACTOR_APPEAR(byte type)
        {
            data = new byte[34];
            offset = 2;
            if (type == 1)
                ID = 0x1BEF;
            else if (type == 3)
                ID = 0x2058;
            else
                ID = 0x1C03;
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

        public uint UnknownID
        {
            set => PutUInt(value, 14);
        }

        public short X
        {
            set => PutShort(value, 18);
        }

        public short Y
        {
            set => PutShort(value, 20);
        }

        public short Z
        {
            set => PutShort(value, 22);
        }

        public short Xaxis
        {
            set => PutShort(value, 24);
        }

        public short Yaxis
        {
            set => PutShort(value, 26);
        }

        public short Zaxis
        {
            set => PutShort(value, 28);
        }


        public ushort Motion
        {
            set => PutUShort(value, 31);
        }

        public string Name
        {
            set
            {
                var name = Global.Unicode.GetBytes(value + "\0");
                var buf = new byte[34 + name.Length + 5];
                data.CopyTo(buf, 0);
                data = buf;
                PutByte((byte)name.Length, 33);
                PutBytes(name, 34);
            }
        }
    }
}