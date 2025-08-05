using SagaLib;

namespace SagaMap.Packets.Server.Skill
{
    public class SSMG_SKILL_CHANGE_BATTLE_STATUS : Packet
    {
        public SSMG_SKILL_CHANGE_BATTLE_STATUS()
        {
            data = new byte[7];
            offset = 2;
            ID = 0x0FA6;
        }

        public uint ActorID
        {
            set => PutUInt(value, 2);
        }

        public byte Status
        {
            set => PutByte(value, 6);
        }
    }
}