using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.Another
{
    public class CSMG_ANO_PAPER_COMPOUND : Packet
    {
        public CSMG_ANO_PAPER_COMPOUND()
        {
            offset = 2;
        }

        public byte paperID => GetByte(3);

        public uint SlotID => GetUInt(4);

        public override Packet New()
        {
            return new CSMG_ANO_PAPER_COMPOUND();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnAnoPaperCompound(this);
        }
    }
}