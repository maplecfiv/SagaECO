using SagaLib;

namespace SagaMap.Packets.Server.IrisCard
{
    public class SSMG_IRIS_CARD_OPEN_RESULT : Packet
    {
        public enum Results
        {
            OK = 0,
            NO_ITEM = -1,
            CANNOT_SET_NOW = -2,
            NO_SLOT = -3
        }

        public SSMG_IRIS_CARD_OPEN_RESULT()
        {
            data = new byte[6];
            offset = 2;
            ID = 0x1DB1;
        }

        public Results Result
        {
            set => PutInt((int)value, 2);
        }
    }
}