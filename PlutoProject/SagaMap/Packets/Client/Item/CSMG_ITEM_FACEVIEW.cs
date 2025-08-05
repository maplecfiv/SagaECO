using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_ITEM_FACEVIEW : Packet
    {
        public CSMG_ITEM_FACEVIEW()
        {
            offset = 2;
        }

        public override Packet New()
        {
            return new CSMG_ITEM_FACEVIEW();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnPlayerFaceView(this);
        }
    }
}