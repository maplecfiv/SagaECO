using SagaLib;

namespace SagaMap.Packets.Server.Skill
{
    public class SSMG_SKILL_CAST_CANCEL : Packet
    {
        public SSMG_SKILL_CAST_CANCEL()
        {
            data = new byte[7];
            offset = 2;
            ID = 0x138A;
        }

        public uint ActorID
        {
            set => PutUInt(value, 2);
        }

        public byte result
        {
            set => PutByte(value, 6);
        }
    }
}