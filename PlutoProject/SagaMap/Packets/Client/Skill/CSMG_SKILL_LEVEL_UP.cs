using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_SKILL_LEVEL_UP : Packet
    {
        public CSMG_SKILL_LEVEL_UP()
        {
            offset = 2;
        }

        public ushort SkillID => GetUShort(2);

        public override Packet New()
        {
            return new CSMG_SKILL_LEVEL_UP();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnSkillLvUP(this);
        }
    }
}