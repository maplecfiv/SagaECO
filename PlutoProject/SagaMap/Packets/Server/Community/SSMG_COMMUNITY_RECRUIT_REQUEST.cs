using SagaLib;

namespace SagaMap.Packets.Server.Community
{
    public class SSMG_COMMUNITY_RECRUIT_REQUEST : Packet
    {
        public SSMG_COMMUNITY_RECRUIT_REQUEST()
        {
            data = new byte[7];
            offset = 2;
            ID = 0x1BAD;
        }

        public uint CharID
        {
            set => PutUInt(value, 2);
        }

        public string CharName
        {
            set
            {
                var name = Global.Unicode.GetBytes(value + "\0");
                var buf = new byte[7 + name.Length];
                data.CopyTo(buf, 0);
                data = buf;
                PutByte((byte)name.Length, 6);
                PutBytes(name, 7);
            }
        }
    }
}