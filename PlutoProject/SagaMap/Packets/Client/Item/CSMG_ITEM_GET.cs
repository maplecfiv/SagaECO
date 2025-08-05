using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_ITEM_GET : Packet
    {
        public CSMG_ITEM_GET()
        {
            offset = 2;
        }

        public uint ActorID => GetUInt(2);

        public override Packet New()
        {
            return new CSMG_ITEM_GET();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnItemGet(this);
        }
    }
}