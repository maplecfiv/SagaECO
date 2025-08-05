using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_DEM_STATS_PRE_CALC : Packet
    {
        public CSMG_DEM_STATS_PRE_CALC()
        {
            offset = 2;
        }

        public ushort Str => GetUShort(5);

        public ushort Dex => GetUShort(7);

        public ushort Int => GetUShort(9);

        public ushort Vit => GetUShort(11);

        public ushort Agi => GetUShort(13);

        public ushort Mag => GetUShort(15);

        public override Packet New()
        {
            return new CSMG_DEM_STATS_PRE_CALC();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnDEMStatsPreCalc(this);
        }
    }
}