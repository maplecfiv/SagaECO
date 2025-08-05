using SagaLib;

namespace SagaMap.Packets.Server.DualJob
{
    public class SSMG_DUALJOB_SET_DUALJOB_INFO : Packet
    {
        public SSMG_DUALJOB_SET_DUALJOB_INFO()
        {
            data = new byte[4];
            offset = 2;
            ID = 0x22D1;
        }

        public bool Result
        {
            set
            {
                if (value)
                    PutByte(0x00);
                else
                    PutByte(0x01);
            }
        }

        public byte RetType
        {
            set => PutByte(value);
        }
    }
}