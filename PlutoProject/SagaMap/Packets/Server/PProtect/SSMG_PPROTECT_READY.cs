using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_PPROTECT_READY : Packet
    {
        public SSMG_PPROTECT_READY()
        {
            data = new byte[4];
            offset = 2;
            ID = 0x2370;
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