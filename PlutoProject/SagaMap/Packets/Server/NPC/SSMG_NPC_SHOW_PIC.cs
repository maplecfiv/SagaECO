using SagaLib;

namespace SagaMap.Packets.Server.NPC
{
    public class SSMG_NPC_SHOW_PIC : Packet
    {
        public SSMG_NPC_SHOW_PIC()
        {
            data = new byte[13];
            offset = 2;
            ID = 0x067C;
        }

        public string Path
        {
            set
            {
                var buf = Global.Unicode.GetBytes(value + "\0");
                var buff = new byte[data.Length + buf.Length];
                data.CopyTo(buff, 0);
                data = buff;

                PutByte((byte)buf.Length, 2);
                PutBytes(buf, 3);
            }
        }

        public int Unknown
        {
            set
            {
                var len = GetByte(2);
                PutInt(value, (ushort)(3 + len));
            }
        }

        public int Unknown2
        {
            set
            {
                var len = GetByte(2);
                PutInt(value, (ushort)(7 + len));
            }
        }

        public string Title
        {
            set
            {
                var len = GetByte(2);
                var buf = Global.Unicode.GetBytes(value + "\0");
                var buff = new byte[data.Length + buf.Length];
                data.CopyTo(buff, 0);
                data = buff;

                PutByte((byte)buf.Length, (ushort)(11 + len));
                PutBytes(buf, (ushort)(12 + len));
            }
        }
    }
}