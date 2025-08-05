using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_SKILL_ATTACK : Packet
    {
        public uint acid;

        public CSMG_SKILL_ATTACK()
        {
            offset = 2;
        }

        public uint ActorID
        {
            set => acid = value;
            get
            {
                if (data != null)
                    acid = GetUInt(2);
                return acid;
            }
        }

        public short Random => GetShort(6);

        public override Packet New()
        {
            return new CSMG_SKILL_ATTACK();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnSkillAttack(this, false);
        }
    }
}