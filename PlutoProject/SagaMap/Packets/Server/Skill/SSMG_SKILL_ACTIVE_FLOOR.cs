using System.Collections.Generic;
using SagaLib;

namespace SagaMap.Packets.Server.Skill
{
    public class SSMG_SKILL_ACTIVE_FLOOR : Packet
    {
        private readonly byte combo;
        private readonly byte set = 1;

        public SSMG_SKILL_ACTIVE_FLOOR(byte combo)
        {
            data = new byte[17 + 33 * combo];
            this.combo = combo;

            offset = 2;
            ID = 0x138D;
        }

        public ushort SkillID
        {
            set
            {
                PutUShort(value, 2);
                PutByte(combo, 4);
                for (var i = 0; i < combo; i++) PutByte(0, 5 + i);
            }
        }

        public uint ActorID
        {
            set => PutUInt(value, 5 + combo);
        }

        public List<SagaDB.Actor.Actor> AffectedID
        {
            set
            {
                PutByte(combo, set + 8 + combo);
                for (var i = 0; i < combo; i++)
                    if (value[i] != null)
                        PutUInt(value[i].ActorID, (ushort)(set + 9 + combo + i * 4));
                    else
                        PutUInt(0xFFFFFFFF, (ushort)(set + 9 + combo + i * 4));
            }
        }

        public byte X
        {
            set => PutByte(value, (ushort)(set + 9 + combo * 4 + combo));
        }

        public byte Y
        {
            set => PutByte(value, (ushort)(set + 10 + combo * 4 + combo));
        }

        public byte SkillLv
        {
            set => PutByte(value, (ushort)(set + 15 + combo * 4 + combo + combo * 24 + combo * 4));
        }

        public void SetHP(List<int> hp)
        {
            PutByte(combo, (ushort)(set + 11 + combo * 4 + combo));
            for (var i = 0; i < combo; i++)
            {
                PutInt(hp[i], (ushort)(set + 12 + combo * 4 + combo + i * 4));
                PutInt(hp[i], (ushort)(set + 12 + combo * 8 + combo + i * 4));
            }
        }

        public void SetMP(List<int> mp)
        {
            PutByte(combo, (ushort)(set + 12 + combo * 4 + combo + combo * 8));
            for (var i = 0; i < combo; i++)
            {
                PutInt(mp[i], (ushort)(set + 13 + combo * 4 + combo + combo * 8 + i * 4));
                PutInt(mp[i], (ushort)(set + 13 + combo * 8 + combo + combo * 8 + i * 4));
            }
        }

        public void SetSP(List<int> sp)
        {
            PutByte(combo, (ushort)(set + 13 + combo * 4 + combo + combo * 16));
            for (var i = 0; i < combo; i++)
            {
                PutInt(sp[i], (ushort)(set + 14 + combo * 4 + combo + combo * 16 + i * 4));
                PutInt(sp[i], (ushort)(set + 14 + combo * 8 + combo + combo * 16 + i * 4));
            }
        }

        public void AttackFlag(List<AttackFlag> flag)
        {
            PutByte(combo, (ushort)(set + 14 + combo * 4 + combo + combo * 24));
            for (var i = 0; i < combo; i++)
                PutUInt((uint)flag[i], (ushort)(set + 15 + combo * 4 + combo + combo * 24 + i * 4));
        }
    }
}