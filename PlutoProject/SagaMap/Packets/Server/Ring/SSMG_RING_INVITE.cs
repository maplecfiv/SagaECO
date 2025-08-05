using SagaLib;

namespace SagaMap.Packets.Server.Ring
{
    public class SSMG_RING_INVITE : Packet
    {
        public SSMG_RING_INVITE()
        {
            data = new byte[8];
            offset = 2;
            ID = 0x1AAF;
        }

        public uint CharID
        {
            set => PutUInt(value, 2);
        }

        public string CharName
        {
            set
            {
                var buf = Global.Unicode.GetBytes(value + "\0");
                var buff = new byte[8 + buf.Length];
                data.CopyTo(buff, 0);
                data = buff;

                PutByte((byte)buf.Length, 6);
                PutBytes(buf, 7);
            }
        }

        public string RingName
        {
            set
            {
                var offset = GetByte(6);
                var buf = Global.Unicode.GetBytes(value + "\0");
                var buff = new byte[8 + offset + buf.Length];
                data.CopyTo(buff, 0);
                data = buff;

                PutByte((byte)buf.Length, (ushort)(7 + offset));
                PutBytes(buf, (ushort)(8 + offset));
            }
        }
    }
}