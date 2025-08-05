using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_SKILL_CHANGE_BATTLE_STATUS : Packet
    {
        public CSMG_SKILL_CHANGE_BATTLE_STATUS()
        {
            offset = 2;
        }

        public byte Status => GetByte(2);

        public override Packet New()
        {
            return new CSMG_SKILL_CHANGE_BATTLE_STATUS();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnSkillChangeBattleStatus(this);
        }
    }
}