using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_PPROTECT_CREATED_OUT : Packet
    {
        public SSMG_PPROTECT_CREATED_OUT()
        {
            data = new byte[4];
            offset = 2;
            ID = 0x236E;
        }

        public byte Index
        {
            set => PutByte(value, 2);
        }

        public byte Code
        {
            set => PutByte(value, 3);
        }
    }
}