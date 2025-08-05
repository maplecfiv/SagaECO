using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_NPC_JOB_SWITCH : Packet
    {
        public CSMG_NPC_JOB_SWITCH()
        {
            offset = 2;
        }

        public int Unknown => GetInt(2);

        public uint ItemUseCount => GetUInt(6);

        public ushort[] Skills
        {
            get
            {
                var count = GetByte(10);
                var skills = new ushort[count];
                for (var i = 0; i < count; i++) skills[i] = GetUShort((ushort)(11 + i * 2));
                return skills;
            }
        }

        public override Packet New()
        {
            return new CSMG_NPC_JOB_SWITCH();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnNPCJobSwitch(this);
        }
    }
}