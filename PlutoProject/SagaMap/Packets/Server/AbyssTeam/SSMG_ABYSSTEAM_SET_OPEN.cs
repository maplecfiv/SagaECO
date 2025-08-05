using SagaLib;

namespace SagaMap.Packets.Server.AbyssTeam
{
    public class SSMG_ABYSSTEAM_SET_OPEN : Packet
    {
        public SSMG_ABYSSTEAM_SET_OPEN()
        {
            data = new byte[3];
            offset = 2;
            ID = 0x22E7;
        }

        public byte Result
        {
            set => PutByte(value, 2);
        }

        public int Floor
        {
            set
            {
                var buf = new byte[data.Length + 4];
                data.CopyTo(buf, 0);
                data = buf;
                PutInt(value, 3);
            }
        }
    }
}