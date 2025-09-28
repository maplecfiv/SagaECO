using System.Text;
using SagaLib;

namespace SagaMap.Packets.Server.AbyssTeam
{
    public class SSMG_ABYSSTEAM_REGIST_APPROVAL : Packet
    {
        public SSMG_ABYSSTEAM_REGIST_APPROVAL()
        {
            data = new byte[12];
            offset = 2;
            ID = 0x22EE;
        }

        public uint CharID
        {
            set => PutUInt(value, 3);
        }

        public string Name
        {
            set
            {
                var name = Encoding.UTF8.GetBytes(value + "\0");
                var buf = new byte[data.Length + name.Length + 1];
                data.CopyTo(buf, 0);
                data = buf;
                offset = 7;
                PutByte((byte)name.Length);
                PutBytes(name);
            }
        }

        public byte Level
        {
            set
            {
                offset++;
                PutByte(value);
            }
        }

        public PC_JOB Job
        {
            set
            {
                offset++;
                PutShort((short)value);
            }
        }
    }
}