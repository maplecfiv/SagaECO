using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_ANO_PAPER_COMPOUND_RESULT : Packet
    {
        public SSMG_ANO_PAPER_COMPOUND_RESULT()
        {
            data = new byte[6];
            offset = 2;
            ID = 0x23A9;
        }

        public byte paperID
        {
            set => PutByte(value, 4);
        }

        public byte lv
        {
            set => PutByte(value, 5);
        }
    }
}