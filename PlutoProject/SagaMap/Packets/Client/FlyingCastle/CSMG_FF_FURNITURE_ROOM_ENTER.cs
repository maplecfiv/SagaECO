using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.FlyingCastle
{
    public class CSMG_FF_FURNITURE_ROOM_ENTER : Packet
    {
        public CSMG_FF_FURNITURE_ROOM_ENTER()
        {
            offset = 2;
        }

        public int data => GetInt(2);

        public override Packet New()
        {
            return new CSMG_FF_FURNITURE_ROOM_ENTER();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnFFFurnitureRoomEnter(this);
        }
    }
}