using SagaLib;

namespace SagaMap.Packets.Client.InfiniteCorridor
{
    public class CSMG_INFINITECORRIDOR_WARP : Packet
    {
        public CSMG_INFINITECORRIDOR_WARP()
        {
            offset = 2;
        }

        public override Packet New()
        {
            return new CSMG_INFINITECORRIDOR_WARP();
        }

        public override void Parse(SagaLib.Client client)
        {
        }
    }
}