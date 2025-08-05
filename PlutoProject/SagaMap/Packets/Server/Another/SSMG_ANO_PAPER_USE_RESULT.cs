using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_ANO_PAPER_USE_RESULT : Packet
    {
        public SSMG_ANO_PAPER_USE_RESULT()
        {
            data = new byte[13];
            offset = 2;
            ID = 0x23A7;
        }

        public byte paperID
        {
            set => PutByte(value, 4);
        }

        public ulong value
        {
            set => PutULong(value, 5);
        }
    }
}