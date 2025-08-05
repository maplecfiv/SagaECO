using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.Another
{
    public class CSMG_ANO_PAPER_USE : Packet
    {
        public CSMG_ANO_PAPER_USE()
        {
            offset = 2;
        }

        public byte paperID => GetByte(3);

        public byte index => GetByte(4);

        public uint slotID => GetUInt(5);

        public override Packet New()
        {
            return new CSMG_ANO_PAPER_USE();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnAnoPaperUse(this);
        }
    }
}