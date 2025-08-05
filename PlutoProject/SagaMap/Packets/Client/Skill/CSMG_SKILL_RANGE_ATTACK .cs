using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.Skill
{
    public class CSMG_SKILL_RANGE_ATTACK : Packet
    {
        public CSMG_SKILL_RANGE_ATTACK()
        {
            offset = 2;
        }

        public uint ActorID => GetUInt(2);

        public override Packet New()
        {
            return new CSMG_SKILL_RANGE_ATTACK();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnSkillRangeAttack(this);
        }
    }
}