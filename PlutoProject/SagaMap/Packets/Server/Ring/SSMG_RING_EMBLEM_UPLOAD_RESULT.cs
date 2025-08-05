using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_RING_EMBLEM_UPLOAD_RESULT : Packet
    {
        public enum Results
        {
            OK = 0,
            SERVER_ERROR = -1,
            WRONG_FORMAT = -2,
            FAME_NOT_ENOUGH = -3
        }

        public SSMG_RING_EMBLEM_UPLOAD_RESULT()
        {
            data = new byte[6];
            offset = 2;
            ID = 0x1ADC;
        }

        public Results Result
        {
            set => PutInt((int)value, 2);
        }
    }
}