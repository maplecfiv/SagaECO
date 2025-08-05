using System.Collections.Generic;
using SagaDB.Actor;
using SagaLib;

namespace SagaDB.Mob
{
    public class MobData
    {
        public Dictionary<AbnormalStatus, short> abnormalStatus = new Dictionary<AbnormalStatus, short>();
        public int aiMode;
        public short aspd, cspd;

        public ushort atk_min,
            atk_max,
            matk_min,
            matk_max,
            def,
            def_add,
            mdef,
            mdef_add,
            str,
            mag,
            vit,
            dex,
            agi,
            intel,
            cri,
            criavd,
            hit_melee,
            hit_ranged,
            hit_magic,
            avoid_melee,
            avoid_ranged,
            avoid_magic;

        public ATTACK_TYPE attackType;
        public uint baseExp, jobExp;
        public bool boss;

        public List<DropData> dropItems = new List<DropData>();
        public List<DropData> dropItemsSpecial = new List<DropData>();

        public string[] dropRate;
        public Dictionary<Elements, int> elements = new Dictionary<Elements, int>();
        public bool fly;

        public byte guideFlag;
        public short guideID;
        public uint hp, mp, sp;
        public uint id, pictid;
        public byte level;
        public float magicreduce;
        public float mobSize;
        public MobType mobType;
        public string name;

        public float physicreduce;
        public Race race;

        public float range;
        public int resilience;
        public ushort speed;
        public DropData stampDrop;
        public bool undead;

        public override string ToString()
        {
            return name;
        }

        public class DropData
        {
            public ushort count = 1;
            public int GreaterThanTime;
            public uint ItemID;
            public int LessThanTime;
            public ushort MinCount, MaxCount;
            public bool Party;
            public bool Public;
            public bool Public20;
            public int Rate;
            public bool Roll;
            public string TreasureGroup;
        }
    }
}