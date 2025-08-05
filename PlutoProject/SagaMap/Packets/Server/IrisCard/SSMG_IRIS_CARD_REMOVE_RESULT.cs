using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_IRIS_CARD_REMOVE_RESULT : Packet
    {
        public enum Results
        {
            OK = 0,
            CANNOT_REMOVE_NOW = -1,
            FAILED = -2
        }

        public SSMG_IRIS_CARD_REMOVE_RESULT()
        {
            data = new byte[6];
            offset = 2;
            ID = 0x1DBC;
        }

        public Results Result
        {
            set => PutInt((int)value, 2);
        }
    }
}