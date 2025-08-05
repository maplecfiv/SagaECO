using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_PLAYER_SETTITLE : Packet
    {
        public CSMG_PLAYER_SETTITLE()
        {
            offset = 2;
        }

        public uint GetTSubID => GetUInt(3);

        public uint GetTConjID => GetUInt(7);

        public uint GetTPredID => GetUInt(11);

        public uint GetTBattleID => GetUInt(15);

        public override Packet New()
        {
            return new CSMG_PLAYER_SETTITLE();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnPlayerSetTitle(this);
        }
    }
}