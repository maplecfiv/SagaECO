using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_PARTY_MEMBER : Packet
    {
        public SSMG_PARTY_MEMBER()
        {
            data = new byte[12];
            offset = 2;
            ID = 0x19E1;
        }

        public int PartyIndex
        {
            set => PutInt(value, 2);
        }

        public uint CharID
        {
            set => PutUInt(value, 6);
        }

        public string CharName
        {
            set
            {
                var buf = Global.Unicode.GetBytes(value + "\0");
                var buff = new byte[12 + buf.Length];
                data.CopyTo(buff, 0);
                data = buff;
                PutByte((byte)buf.Length, 10);
                PutBytes(buf, 11);
            }
        }

        public bool Leader
        {
            set
            {
                var size = GetByte(10);
                if (value)
                    PutByte(1, (ushort)(11 + size));
                else
                    PutByte(0, (ushort)(11 + size));
            }
        }
    }
}