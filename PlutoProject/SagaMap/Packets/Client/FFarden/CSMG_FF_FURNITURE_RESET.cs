using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.FFGarden
{
    public class CSMG_FF_FURNITURE_RESET : Packet
    {
        public CSMG_FF_FURNITURE_RESET()
        {
            offset = 2;
        }

        public uint ActorID => GetUInt(2);

        public byte Type => GetByte(6);

        public override Packet New()
        {
            return new CSMG_FF_FURNITURE_RESET();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnFFFurnitureReset(this);
        }
    }
}