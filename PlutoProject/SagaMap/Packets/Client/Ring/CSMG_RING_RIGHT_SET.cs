using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_RING_RIGHT_SET : Packet
    {
        public CSMG_RING_RIGHT_SET()
        {
            offset = 2;
        }

        public uint CharID => GetUInt(2);

        public int Right => GetInt(6);

        public override Packet New()
        {
            return new CSMG_RING_RIGHT_SET();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnRingRightSet(this);
        }
    }
}