using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.Chat
{
    public class CSMG_CHAT_GIFT_TAKE : Packet
    {
        public CSMG_CHAT_GIFT_TAKE()
        {
            offset = 2;
        }

        public uint GiftID => GetUInt(2);

        public byte type => GetByte(6);

        public override Packet New()
        {
            return new CSMG_CHAT_GIFT_TAKE();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnTakeGift(this);
        }
    }
}