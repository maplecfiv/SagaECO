using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_DAILY_STAMP_OPEN : Packet
    {
        public CSMG_DAILY_STAMP_OPEN()
        {
            offset = 2;
        }

        public override Packet New()
        {
            return new CSMG_DAILY_STAMP_OPEN();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnPlayerOpenDailyStamp(this);
        }
    }
}