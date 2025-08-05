using System.Text;
using SagaDB.Item;

namespace SagaMap.Packets.Server
{
    public class SSMG_POSSESSION_CATALOG : HasItemDetail
    {
        public SSMG_POSSESSION_CATALOG()
        {
            data = new byte[220];
            offset = 2;
            ID = 0x178F;
        }

        public uint ActorID
        {
            set => PutUInt(value, 2);
        }

        public string comment
        {
            set
            {
                if (value != "")
                {
                    if (value.Substring(value.Length - 1) != "\0")
                        value += "\0";
                }
                else
                {
                    value = "\0";
                }

                var commentbuf = Encoding.UTF8.GetBytes(value);
                var buf = new byte[data.Length + commentbuf.Length];
                data.CopyTo(buf, 0);
                data = buf;
                PutByte((byte)commentbuf.Length, 6);
                PutBytes(commentbuf, 7);
                offset = (ushort)(commentbuf.Length + 7);

                PutByte(0xD4);
            }
        }

        public uint Index
        {
            set => PutUInt(value);
        }

        public Item Item
        {
            set => ItemDetail = value;
        }
    }
}