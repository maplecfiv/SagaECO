using SagaLib;

namespace SagaMap.Packets.Server.IrisCard
{
    public class SSMG_IRIS_CARD_LOCK_RESULT : Packet
    {
        public SSMG_IRIS_CARD_LOCK_RESULT()
        {
            data = new byte[6];
            offset = 2;
            ID = 0x1DCA;
        }
    }
}