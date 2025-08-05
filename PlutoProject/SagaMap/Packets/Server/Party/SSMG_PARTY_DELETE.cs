using SagaLib;

namespace SagaMap.Packets.Server.Party
{
    public class SSMG_PARTY_DELETE : Packet
    {
        public enum Result
        {
            DISMISSED = 1,
            QUIT = 2,
            KICKED = 3
        }

        public SSMG_PARTY_DELETE()
        {
            data = new byte[11];
            offset = 2;
            ID = 0x19DD;
        }

        public uint PartyID
        {
            set => PutUInt(value, 2);
        }

        public string PartyName
        {
            set
            {
                var buf = Global.Unicode.GetBytes(value + "\0");
                var buff = new byte[11 + buf.Length];
                data.CopyTo(buff, 0);
                data = buff;
                PutByte((byte)buf.Length, 6);
                PutBytes(buf, 7);
            }
        }

        public Result Reason
        {
            set
            {
                var size = GetByte(6);
                PutInt((int)value, (ushort)(7 + size));
            }
        }
    }
}