using SagaLib;

namespace SagaMap.Packets.Server.IrisCard
{
    public class SSMG_IRIS_CARD_INSERT_RESULT : Packet
    {
        public enum Results
        {
            OK = 0,
            CANNOT_SET_NOW = -1,
            CANNOT_SET = -2,
            SLOT_OVER = -3
        }

        public SSMG_IRIS_CARD_INSERT_RESULT()
        {
            data = new byte[6];
            offset = 2;
            ID = 0x1DB7;
        }

        public Results Result
        {
            set => PutInt((int)value, 2);
        }
    }
}