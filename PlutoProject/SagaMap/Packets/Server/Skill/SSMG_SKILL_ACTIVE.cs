using System.Collections.Generic;
using SagaLib;

namespace SagaMap.Packets.Server.Skill
{
    public class SSMG_SKILL_ACTIVE : Packet
    {
        private readonly byte combo;

        public SSMG_SKILL_ACTIVE(byte combo)
        {
            if (Configuration.Configuration.Instance.Version <= Version.Saga9)
                data = new byte[22 + 4 * combo + 6 * combo + 4 * combo];
            if (Configuration.Configuration.Instance.Version >= Version.Saga9_2)
                //this.data = new byte[23 + 4 * combo + 12 * combo + 4 * combo + 12 * combo];
                data = new byte[22 + combo * 4 + combo * 28];
            offset = 2;
            ID = 0x1392;
            this.combo = combo;
            PutByte(1, 4);
        }

        public ushort SkillID
        {
            set => PutUShort(value, 2);
        }

        public uint ActorID
        {
            set => PutUInt(value, 6);
        }

        public uint TargetID
        {
            set => PutUInt(value, 10);
        }

        public List<SagaDB.Actor.Actor> AffectedID
        {
            set
            {
                PutByte(combo, 14);
                for (var i = 0; i < combo; i++) PutUInt(value[i].ActorID, (ushort)(15 + i * 4));
            }
        }

        public byte X
        {
            set => PutByte(value, (ushort)(15 + combo * 4));
        }

        public byte Y
        {
            set => PutByte(value, (ushort)(16 + combo * 4));
        }

        public byte SkillLv
        {
            set
            {
                if (Configuration.Configuration.Instance.Version <= Version.Saga9)
                    PutByte(value, (ushort)(21 + combo * 4 + combo * 6 + combo * 4));

                if (Configuration.Configuration.Instance.Version >= Version.Saga9_2)
                    PutByte(value, (ushort)(21 + combo * 4 + combo * 28));
            }
        }

        public void SetHP(List<int> hp)
        {
            if (Configuration.Configuration.Instance.Version <= Version.Saga9)
            {
                PutByte(combo, (ushort)(17 + combo * 4));
                for (var i = 0; i < combo; i++) PutShort((short)hp[i], (ushort)(18 + combo * 4 + i * 2));
            }

            if (Configuration.Configuration.Instance.Version >= Version.Saga9_2)
            {
                PutByte(combo, (ushort)(17 + combo * 4));
                for (var i = 0; i < combo; i++) PutLong(hp[i], (ushort)(18 + combo * 4 + i * 8));
                //this.PutInt(hp[i], (ushort)(18 + combo * 8 + i * 4));
            }
        }

        public void SetMP(List<int> mp)
        {
            if (Configuration.Configuration.Instance.Version <= Version.Saga9)
            {
                PutByte(combo, (ushort)(18 + combo * 4 + combo * 2));
                for (var i = 0; i < combo; i++) PutShort((short)mp[i], (ushort)(19 + combo * 4 + combo * 2 + i * 2));
            }

            if (Configuration.Configuration.Instance.Version >= Version.Saga9_2)
            {
                PutByte(combo, (ushort)(18 + combo * 4 + combo * 8));
                for (var i = 0; i < combo; i++)
                {
                    PutInt(mp[i], (ushort)(19 + combo * 4 + combo * 12 + i * 4));
                    PutInt(mp[i], (ushort)(19 + combo * 4 + combo * 16 + i * 4));
                }
            }
        }

        public void SetSP(List<int> sp)
        {
            if (Configuration.Configuration.Instance.Version <= Version.Saga9)
            {
                PutByte(combo, (ushort)(19 + combo * 4 + combo * 4));
                for (var i = 0; i < combo; i++) PutShort((short)sp[i], (ushort)(20 + combo * 4 + combo * 4 + i * 2));
            }

            if (Configuration.Configuration.Instance.Version >= Version.Saga9_2)
            {
                PutByte(combo, (ushort)(19 + combo * 4 + combo * 16));
                for (var i = 0; i < combo; i++)
                {
                    PutInt(sp[i], (ushort)(20 + combo * 4 + combo * 20 + i * 4));
                    PutInt(sp[i], (ushort)(20 + combo * 4 + combo * 24 + i * 4));
                }
            }
        }

        public void AttackFlag(List<AttackFlag> flag)
        {
            if (Configuration.Configuration.Instance.Version <= Version.Saga9)
            {
                PutByte(combo, (ushort)(20 + combo * 4 + combo * 6));
                for (var i = 0; i < combo; i++) PutUInt((uint)flag[i], (ushort)(21 + combo * 4 + combo * 6 + i * 4));
            }

            if (Configuration.Configuration.Instance.Version >= Version.Saga9_2)
            {
                PutByte(combo, (ushort)(20 + combo * 4 + combo * 24));
                for (var i = 0; i < combo; i++) PutUInt((uint)flag[i], (ushort)(21 + combo * 4 + combo * 24 + i * 4));
            }
        }
    }
}