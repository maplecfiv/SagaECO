using SagaLib;

namespace SagaLogin.Packets.Server
{
    public class SSMG_FRIEND_ADD : Packet
    {
        public SSMG_FRIEND_ADD()
        {
            data = new byte[7];
            ID = 0x00D3;
        }

        public uint CharID
        {
            set => PutUInt(value, 2);
        }

        public string Name
        {
            set
            {
                var buf = Global.Unicode.GetBytes(value + "\0");
                var buff = new byte[7 + buf.Length];
                data.CopyTo(buff, 0);
                data = buff;
                PutByte((byte)buf.Length, 6);
                PutBytes(buf, 7);
            }
        }
    }
}