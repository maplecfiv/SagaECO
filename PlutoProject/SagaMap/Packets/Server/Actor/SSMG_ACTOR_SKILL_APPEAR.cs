using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_ACTOR_SKILL_APPEAR : Packet
    {
        public SSMG_ACTOR_SKILL_APPEAR()
        {
            data = new byte[14];
            offset = 2;
            ID = 0x13A1;
        }

        public uint ActorID
        {
            set => PutUInt(value, 2);
        }

        public ushort SkillID
        {
            set => PutUShort(value, 6);
        }

        public byte X
        {
            set => PutByte(value, 8);
        }

        public byte Y
        {
            set => PutByte(value, 9);
        }

        public ushort Speed
        {
            set => PutUShort(value, 10);
        }

        public byte SkillLv
        {
            set => PutByte(value, 12);
        }

        public byte Dir
        {
            set => PutByte(value, 13);
        }
    }
}