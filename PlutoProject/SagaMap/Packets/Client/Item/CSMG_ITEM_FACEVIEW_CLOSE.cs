using SagaLib;

namespace SagaMap.Packets.Client.Item
{
    public class CSMG_ITEM_FACEVIEW_CLOSE : Packet
    {
        public CSMG_ITEM_FACEVIEW_CLOSE()
        {
            offset = 2;
        }

        public override Packet New()
        {
            return new CSMG_ITEM_FACEVIEW_CLOSE();
        }

        public override void Parse(SagaLib.Client client)
        {
        }
    }
}