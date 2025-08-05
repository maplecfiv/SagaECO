using SagaDB.Actor;
using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_GOLEM_ACTOR_APPEAR : Packet
    {
        public SSMG_GOLEM_ACTOR_APPEAR()
        {
            data = new byte[30];
            offset = 2;
            ID = 0x17D4;
        }

        public uint PictID
        {
            set => PutUInt(value, 2);
        }

        public uint ActorID
        {
            set => PutUInt(value, 6);
        }

        public byte X
        {
            set => PutByte(value, 10);
        }

        public byte Y
        {
            set => PutByte(value, 11);
        }

        public ushort Speed
        {
            set => PutUShort(value, 12);
        }

        public byte Dir
        {
            set => PutByte(value, 14);
        }

        public uint GolemID
        {
            set => PutUInt(value, 15);
        }

        public GolemType GolemType
        {
            set => PutByte((byte)value, 19);
        }

        public string CharName
        {
            set
            {
                var name = Global.Unicode.GetBytes(value + "\0");
                var buf = new byte[30 + name.Length];
                data.CopyTo(buf, 0);
                data = buf;

                PutByte((byte)name.Length, 20);
                PutBytes(name, 21);
            }
        }

        public string Title
        {
            set
            {
                var title = Global.Unicode.GetBytes(value + "\0");
                var len = GetByte(20);
                var buf = new byte[30 + len + title.Length];
                data.CopyTo(buf, 0);
                data = buf;

                PutByte((byte)title.Length, (ushort)(21 + len));
                PutBytes(title, (ushort)(22 + len));
            }
        }

        public uint Unknown
        {
            set
            {
                var len = GetByte(20);
                len += GetByte((ushort)(21 + len));
                PutUInt(value, (ushort)(22 + len));
                PutUInt(value, (ushort)(26 + len));
            }
        }
    }
}