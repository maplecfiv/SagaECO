using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.FGarden
{
    public class CSMG_FGARDEN_FURNITURE_USE : Packet
    {
        public CSMG_FGARDEN_FURNITURE_USE()
        {
            offset = 2;
        }

        public uint ActorID => GetUInt(2);

        public override Packet New()
        {
            return new CSMG_FGARDEN_FURNITURE_USE();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnFGardenFurnitureUse(this);
        }
    }
}