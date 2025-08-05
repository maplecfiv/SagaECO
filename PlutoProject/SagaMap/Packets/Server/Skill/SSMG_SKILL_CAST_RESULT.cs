using SagaLib;

namespace SagaMap.Packets.Server.Skill
{
    public class SSMG_SKILL_CAST_RESULT : Packet
    {
        public SSMG_SKILL_CAST_RESULT()
        {
            data = new byte[21];
            offset = 2;
            ID = 0x1389;
        }

        public ushort SkillID
        {
            set => PutUShort(value, 2);
        }

        public byte Result
        {
            set => PutByte(value, 4);
        }

        public uint ActorID
        {
            set => PutUInt(value, 5);
        }

        public uint CastTime
        {
            set => PutUInt(value, 9);
        }

        public uint TargetID
        {
            set => PutUInt(value, 13);
        }

        public byte X
        {
            set => PutByte(value, 17);
        }

        public byte Y
        {
            set => PutByte(value, 18);
        }

        public byte SkillLv
        {
            set => PutByte(value, 19);
        }
    }
}