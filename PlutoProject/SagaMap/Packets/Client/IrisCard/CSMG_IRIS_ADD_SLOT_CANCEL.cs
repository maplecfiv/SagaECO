using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.IrisCard
{
    public class CSMG_IRIS_ADD_SLOT_CANCEL : Packet
    {
        public CSMG_IRIS_ADD_SLOT_CANCEL()
        {
            offset = 2;
        }

        public override Packet New()
        {
            return new CSMG_IRIS_ADD_SLOT_CANCEL();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnIrisAddSlotCancel(this);
        }
    }
}