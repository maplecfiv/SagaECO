using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.Navi
{
    public class CSMG_NAVI_OPEN : Packet
    {
        public CSMG_NAVI_OPEN()
        {
            offset = 2;
        }

        public override Packet New()
        {
            return new CSMG_NAVI_OPEN();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnNaviOpen(this);
        }
    }
}