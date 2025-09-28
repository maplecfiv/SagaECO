using System.Collections.Generic;
using SagaLib;

namespace SagaMap.Packets.Server.Skill
{
    public class SSMG_SKILL_COMBO_ATTACK_RESULT : Packet
    {
        private readonly byte combo;

        public SSMG_SKILL_COMBO_ATTACK_RESULT(byte combo)
        {
            data = new byte[26 + 33 * combo];
            offset = 2;
            ID = 0x0FA2;
            this.combo = combo;
        }

        public uint ActorID
        {
            set => PutUInt(value, 2);
        }

        public List<SagaDB.Actor.Actor> TargetID
        {
            set
            {
                PutByte(combo, 6);
                for (var i = 0; i < combo; i++) PutUInt(value[i].ActorID, (ushort)(7 + i * 4));
            }
        }

        public ATTACK_TYPE AttackType
        {
            set => PutByte((byte)value, (ushort)(7 + combo * 4));
        }

        public byte Unknown2
        {
            set
            {
                PutByte(combo, (ushort)(12 + combo * 32));
                for (var i = 0; i < combo; i++) PutByte(value, (ushort)(13 + combo * 32 + i));
            }
        }

        public uint Delay
        {
            set => PutUInt(value, (short)(13 + combo * 33));
        }

        public uint Unknown
        {
            set => PutUInt(value, (short)(17 + combo * 33));
        }

        public uint SkillID
        {
            set => PutUInt(value, (short)(21 + combo * 33));
        }

        public byte SkillLevel
        {
            set => PutByte(value, (short)(25 + combo * 33));
        }

        public void SetHP(List<int> hp)
        {
            PutByte(combo, (ushort)(8 + combo * 4));
            for (var i = 0; i < combo; i++)
            {
                PutInt(hp[i], (ushort)(9 + combo * 4 + i * 4));
                PutInt(hp[i], (ushort)(9 + combo * 8 + i * 4));
            }
        }

        public void SetMP(List<int> mp)
        {
            PutByte(combo, (ushort)(9 + combo * 12));
            for (var i = 0; i < combo; i++)
            {
                PutInt(mp[i], (ushort)(10 + combo * 12 + i * 4));
                PutInt(mp[i], (ushort)(10 + combo * 16 + i * 4));
            }
        }

        public void SetSP(List<int> sp)
        {
            PutByte(combo, (ushort)(10 + combo * 20));
            for (var i = 0; i < combo; i++)
            {
                PutInt(sp[i], (ushort)(11 + combo * 20 + i * 4));
                PutInt(sp[i], (ushort)(11 + combo * 24 + i * 4));
            }
        }

        public void AttackFlag(List<AttackFlag> flag)
        {
            PutByte(combo, (ushort)(11 + combo * 28));
            for (var i = 0; i < combo; i++) PutUInt((uint)flag[i], (ushort)(12 + combo * 28 + i * 4));
        }
    }
}