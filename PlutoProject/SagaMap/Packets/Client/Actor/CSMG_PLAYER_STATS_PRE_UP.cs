using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.Actor
{
    public class CSMG_PLAYER_STATS_UP : Packet
    {
        public CSMG_PLAYER_STATS_UP()
        {
            offset = 2;
        }

        public byte Type => GetByte(2);

        public ushort Str => GetUShort(3);

        public ushort Dex => GetUShort(5);

        public ushort Int => GetUShort(7);

        public ushort Vit => GetUShort(9);

        public ushort Agi => GetUShort(11);

        public ushort Mag => GetUShort(13);

        public override Packet New()
        {
            return new CSMG_PLAYER_STATS_UP();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnStatsUp(this);
        }
    }
}