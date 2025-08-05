using SagaLib;

namespace SagaMap.Packets.Server.IrisCard
{
    public class SSMG_IRIS_CARD_UNLOCK_RESULT : Packet
    {
        public SSMG_IRIS_CARD_UNLOCK_RESULT()
        {
            data = new byte[6];
            offset = 2;
            ID = 0x1DCC;
        }

        public byte Result
        {
            set => PutShort(value);
        }
    }
}