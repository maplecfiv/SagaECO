using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_ITEM_WARE_PAGE : Packet
    {
        public CSMG_ITEM_WARE_PAGE()
        {
            offset = 2;
        }

        public uint PageID => GetUInt(2);


        public override Packet New()
        {
            return new CSMG_ITEM_WARE_PAGE();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnItemWarePage(this);
        }
    }
}