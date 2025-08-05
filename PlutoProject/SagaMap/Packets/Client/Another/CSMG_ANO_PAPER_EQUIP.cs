using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.Another
{
    public class CSMG_ANO_PAPER_EQUIP : Packet
    {
        public CSMG_ANO_PAPER_EQUIP()
        {
            offset = 2;
        }

        public byte paperID => GetByte(3);

        public Packet build()
        {
            return new CSMG_ANO_PAPER_EQUIP();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnAnoPaperEquip(this);
        }
    }
}