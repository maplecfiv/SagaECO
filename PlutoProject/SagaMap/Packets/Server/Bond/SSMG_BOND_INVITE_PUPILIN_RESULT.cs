using System.Text;
using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_BOND_INVITE_PUPILIN_RESULT : Packet
    {
        public SSMG_BOND_INVITE_PUPILIN_RESULT()
        {
            data = new byte[7];
            offset = 2;
            ID = 0x1FE5;
        }

        public int Result
        {
            set => PutInt(value, 2);
        }

        public string Name
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

                var namebuf = Encoding.UTF8.GetBytes(value);
                var buf = new byte[data.Length + namebuf.Length];
                data.CopyTo(buf, 0);
                data = buf;
                PutByte((byte)namebuf.Length, 6);
                PutBytes(namebuf, 7);
                offset = (ushort)(namebuf.Length + 7);
            }
        }
    }
}