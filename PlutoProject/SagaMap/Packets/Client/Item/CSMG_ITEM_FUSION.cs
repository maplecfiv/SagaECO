using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_ITEM_FUSION : Packet
    {
        public CSMG_ITEM_FUSION()
        {
            offset = 2;
        }

        public uint EffectItem => GetUInt(6);

        public uint ViewItem => GetUInt(2);

        public override Packet New()
        {
            return new CSMG_ITEM_FUSION();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnItemFusion(this);
        }
    }
}