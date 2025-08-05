using SagaLib;

namespace SagaMap.Packets.Server.NPC
{
    public class SSMG_NPC_SYNTHESE_NEWINFO : Packet
    {
        public SSMG_NPC_SYNTHESE_NEWINFO()
        {
            data = new byte[7];
            offset = 2;
            ID = 0x13B5;
        }

        public ushort SkillID
        {
            set => PutUShort(value, 2);
        }

        public byte SkillLevel
        {
            set => PutByte(value, 4);
        }

        public byte Unknown1
        {
            set => PutByte(value, 5);
        }

        public byte Unknown2
        {
            set => PutByte(value, 6);
        }
    }
}