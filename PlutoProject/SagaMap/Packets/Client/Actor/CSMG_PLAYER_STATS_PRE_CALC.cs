using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_PLAYER_STATS_PRE_CALC : Packet
    {
        public CSMG_PLAYER_STATS_PRE_CALC()
        {
            offset = 2;
        }

        public ushort Str => GetUShort(3);

        public ushort Dex => GetUShort(5);

        public ushort Int => GetUShort(7);

        public ushort Vit => GetUShort(9);

        public ushort Agi => GetUShort(11);

        public ushort Mag => GetUShort(13);

        public override Packet New()
        {
            return new CSMG_PLAYER_STATS_PRE_CALC();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnStatsPreCalc(this);
        }
    }
}