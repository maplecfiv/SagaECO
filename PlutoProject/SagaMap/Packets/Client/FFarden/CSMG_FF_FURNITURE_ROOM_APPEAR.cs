using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.FFGarden
{
    public class CSMG_FF_FURNITURE_ROOM_APPEAR : Packet
    {
        public CSMG_FF_FURNITURE_ROOM_APPEAR()
        {
            offset = 2;
        }

        public override Packet New()
        {
            return new CSMG_FF_FURNITURE_ROOM_APPEAR();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnFFFurnitureRoomAppear();
        }
    }
}