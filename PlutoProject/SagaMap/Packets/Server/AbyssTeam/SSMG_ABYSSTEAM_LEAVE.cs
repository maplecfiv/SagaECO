using System.Text;
using SagaLib;

namespace SagaMap.Packets.Server.AbyssTeam
{
    public class SSMG_ABYSSTEAM_LEAVE : Packet
    {
        public SSMG_ABYSSTEAM_LEAVE()
        {
            data = new byte[10];
            offset = 2;
            ID = 0x22F3;
        }

        public byte Result
        {
            set => PutByte(value, 2);
        }

        public string TeamName
        {
            set
            {
                var name = Encoding.UTF8.GetBytes(value + "\0");
                var buf = new byte[data.Length + name.Length + 1];
                data.CopyTo(buf, 0);
                data = buf;
                PutByte((byte)name.Length, 3);
                PutBytes(name, 4);
            }
        }
    }
}