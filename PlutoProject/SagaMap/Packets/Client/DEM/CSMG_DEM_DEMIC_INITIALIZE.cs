using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_DEM_DEMIC_INITIALIZE : Packet
    {
        public CSMG_DEM_DEMIC_INITIALIZE()
        {
            offset = 2;
        }

        public byte Page => GetByte(2);

        public override Packet New()
        {
            return new CSMG_DEM_DEMIC_INITIALIZE();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnDEMDemicInitialize(this);
        }
    }
}