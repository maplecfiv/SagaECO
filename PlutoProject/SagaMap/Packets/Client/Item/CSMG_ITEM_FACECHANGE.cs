using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.Item
{
    public class CSMG_ITEM_FACECHANGE : Packet
    {
        public CSMG_ITEM_FACECHANGE()
        {
            offset = 2;
        }

        public uint SlotID => GetUInt(2);

        public ushort FaceID => GetUShort(6);

        public override Packet New()
        {
            return new CSMG_ITEM_FACECHANGE();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnPlayerFaceChange(this);
        }
    }
}