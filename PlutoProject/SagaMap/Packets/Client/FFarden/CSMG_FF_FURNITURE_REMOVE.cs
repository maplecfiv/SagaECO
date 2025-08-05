using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_FF_FURNITURE_REMOVE : Packet
    {
        public CSMG_FF_FURNITURE_REMOVE()
        {
            offset = 2;
        }

        public uint ActorID => GetUInt(2);

        public override Packet New()
        {
            return new CSMG_FF_FURNITURE_REMOVE();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnFFFurnitureRemove(this);
        }
    }
}