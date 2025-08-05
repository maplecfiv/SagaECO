using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.Another
{
    public class CSMG_ANO_UI_OPEN : Packet
    {
        public CSMG_ANO_UI_OPEN()
        {
            offset = 2;
        }

        public byte index => GetByte(2);

        public override Packet New()
        {
            return new CSMG_ANO_UI_OPEN();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnAnoUIOpen(this);
        }
    }
}