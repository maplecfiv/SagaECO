using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_SKILL_CAST : Packet
    {
        public CSMG_SKILL_CAST()
        {
            offset = 2;
            data = new byte[14];
        }

        public ushort SkillID
        {
            get => GetUShort(2);
            set => PutUShort(value, 2);
        }

        public uint ActorID
        {
            get => GetUInt(4);
            set => PutUInt(value, 4);
        }

        public byte X
        {
            get => GetByte(8);
            set => PutByte(value, 8);
        }

        public byte Y
        {
            get => GetByte(9);
            set => PutByte(value, 9);
        }

        public byte SkillLv
        {
            get => GetByte(10);
            set => PutByte(value, 10);
        }

        public short Random
        {
            get => GetShort(11);
            set => PutShort(value, 11);
        }

        public override Packet New()
        {
            return new CSMG_SKILL_CAST();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnSkillCast(this);
        }
    }
}