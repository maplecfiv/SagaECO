using SagaLib;

namespace SagaMap.Packets.Client
{
    public class CSMG_INFINITECORRIDOR_TRAP : Packet
    {
        public CSMG_INFINITECORRIDOR_TRAP()
        {
            offset = 2;
        }

        public override Packet New()
        {
            return new CSMG_INFINITECORRIDOR_TRAP();
        }

        public override void Parse(SagaLib.Client client)
        {
        }
    }
}