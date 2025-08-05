using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_TRADE_STATUS : Packet
    {
        public SSMG_TRADE_STATUS()
        {
            data = new byte[6];
            offset = 2;
            ID = 0x0A19;
            PutByte(1, 2);
            PutByte(1, 4);
        }

        public bool Confirm
        {
            set
            {
                if (value)
                    PutByte(0, 3);
                else
                    PutByte(0xFF, 3);
            }
        }

        public bool Perform
        {
            set
            {
                if (value)
                    PutByte(0, 5);
                else
                    PutByte(0xFF, 5);
            }
        }
    }
}