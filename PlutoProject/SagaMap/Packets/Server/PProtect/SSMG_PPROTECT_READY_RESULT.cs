using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_PPROTECT_READY_RESULT : Packet
    {
        public SSMG_PPROTECT_READY_RESULT()
        {
            data = new byte[3];
            offset = 2;
            ID = 0x2375;
        }

        public byte Code
        {
            set => PutByte(value, 2);
        }
    }
}