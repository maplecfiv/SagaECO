using SagaLib;

namespace SagaMap.Packets.Client
{
    public class CSMG_0064 : Packet
    {
        public CSMG_0064()
        {
            offset = 2;
        }

        public override Packet New()
        {
            return new CSMG_0064();
        }

        public override void Parse(SagaLib.Client client)
        {
            ;
        }
    }
}