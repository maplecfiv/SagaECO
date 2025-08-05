using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_LOGIN_ALLOWED : Packet
    {
        public SSMG_LOGIN_ALLOWED()
        {
            data = new byte[10];
            offset = 14;
            ID = 0x000F;
        }

        public uint FrontWord
        {
            set => PutUInt(value, 2);
        }

        public uint BackWord
        {
            set => PutUInt(value, 6);
        }
    }
}