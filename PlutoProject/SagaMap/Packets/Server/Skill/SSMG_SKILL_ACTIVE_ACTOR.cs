using System.Collections.Generic;
using SagaLib;

namespace SagaMap.Packets.Server.Skill
{
    public class SSMG_SKILL_ACTIVE_ACTOR : Packet
    {
        private readonly byte combo;

        public SSMG_SKILL_ACTIVE_ACTOR(byte combo)
        {
            data = new byte[11 + 4 * combo + 12 * combo + 4 * combo];
            offset = 2;
            ID = 0x13B0;
            this.combo = combo;
        }

        public uint ActorID
        {
            set => PutUInt(value, 2);
        }

        public List<SagaDB.Actor.Actor> AffectedID
        {
            set
            {
                PutByte(combo, 6);
                for (var i = 0; i < combo; i++) PutUInt(value[i].ActorID, (ushort)(7 + i * 4));
            }
        }

        public void SetHP(List<int> hp)
        {
            PutByte(combo, (ushort)(7 + combo * 4)); //11
            for (var i = 0; i < combo; i++)
            {
                PutUInt((uint)hp[i], (ushort)(8 + combo * 4 + i * 2)); //12
                PutUInt((uint)hp[i], (ushort)(8 + combo * 8 + i * 2)); //16
            }
        }

        public void SetMP(List<int> mp)
        {
            PutByte(combo, (ushort)(8 + combo * 4 + combo * 8)); //20
            for (var i = 0; i < combo; i++)
            {
                PutUInt((uint)mp[i], (ushort)(9 + combo * 4 + combo * 8 + i * 4)); //21
                PutUInt((uint)mp[i], (ushort)(9 + combo * 4 + combo * 8 + combo * 4 + i * 4)); //25
            }
        }

        public void SetSP(List<int> sp)
        {
            PutByte(combo, (ushort)(9 + combo * 4 + combo * 16)); //29
            for (var i = 0; i < combo; i++)
            {
                PutUInt((uint)sp[i], (ushort)(10 + combo * 4 + combo * 16 + i * 4)); //30
                PutUInt((uint)sp[i], (ushort)(10 + combo * 4 + combo * 16 + combo * 4 + i * 4)); //34
            }
        }

        public void AttackFlag(List<AttackFlag> flag)
        {
            PutByte(combo, (ushort)(10 + combo * 4 + combo * 24)); //38
            for (var i = 0; i < combo; i++) PutUInt((uint)flag[i], (ushort)(11 + combo * 4 + combo * 24 + i * 4)); //39
        }
    }
}