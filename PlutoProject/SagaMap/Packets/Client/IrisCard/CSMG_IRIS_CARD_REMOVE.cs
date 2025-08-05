using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_IRIS_CARD_REMOVE : Packet
    {
        public CSMG_IRIS_CARD_REMOVE()
        {
            offset = 2;
        }

        public short CardSlot => GetShort(2);

        public byte Unknown => GetByte(4);

        public override Packet New()
        {
            return new CSMG_IRIS_CARD_REMOVE();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnIrisCardRemove(this);
        }
    }
}