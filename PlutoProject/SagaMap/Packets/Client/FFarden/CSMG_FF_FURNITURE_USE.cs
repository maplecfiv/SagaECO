using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.FFGarden
{
    public class CSMG_FF_FURNITURE_USE : Packet
    {
        public CSMG_FF_FURNITURE_USE()
        {
            offset = 2;
        }

        public uint ActorID => GetUInt(2);

        public override Packet New()
        {
            return new CSMG_FF_FURNITURE_USE();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnFFGardenFurnitureUse(this);
        }
    }
}