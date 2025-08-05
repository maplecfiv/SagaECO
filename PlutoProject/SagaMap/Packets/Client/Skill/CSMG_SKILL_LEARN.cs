using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_SKILL_LEARN : Packet
    {
        public CSMG_SKILL_LEARN()
        {
            offset = 2;
        }

        public ushort SkillID => GetUShort(2);

        public override Packet New()
        {
            return new CSMG_SKILL_LEARN();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnSkillLearn(this);
        }
    }
}