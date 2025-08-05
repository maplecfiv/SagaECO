using SagaLib;

namespace SagaDB.Skill
{
    public enum SkillFlags
    {
        NONE = 0,
        NO_INTERRUPT = 0x1,
        MAGIC = 0x2,
        PHYSIC = 0x4,
        PARTY_ONLY = 0x8,
        ATTACK = 0x10,
        CAN_HAS_TARGET = 0x20,
        SUPPORT = 0x40,
        HOLY = 0x80,

        DEAD_ONLY = 0x200,
        KIT_RELATED = 0x400,
        NO_POSSESSION = 0x800,
        NOT_BEEN_POSSESSED = 0x1000,
        HEART_SKILL = 0x2000
    }

    public enum EquipFlags
    {
        HAND = 0x1,
        SWORD = 0x2,
        SHORT_SWORD = 0x4,
        HAMMER = 0x8,
        AXE = 0x10,
        SPEAR = 0x20,
        THROW = 0x40,
        BOW = 0x80,
        SHIELD = 0x100,
        GUN = 0x200,
        BAG = 0x400,
        CLAW = 0x800,
        RAPIER = 0x1000,
        KNUCKLE = 0x2000,
        DUALGUN = 0x4000,
        RIFLE = 0x8000,
        STRINGS = 0x10000,
        INSTRUMENT2 = 0x20000,
        ROPE = 0x40000,
        CARD = 0x80000,
        NONE = 0x100000,
        BOOK = 0x200000,
        STAFF = 0x400000,
        ETC_WEAPON = 0x800000,
        EQ_ALLSLOT = 0x1000000,
        HANDBAG = 0x2000000,

        LEFT_HANDBAGHANDBAG = 0x4000000
        //DEM打 0x800000,
        //DEM斩 0x1000000,
        //DEM刺 0x2000000,
        //DEM远 0x4000000,
    }

    public class SkillData
    {
        public bool active;
        public int castTime, delay, SingleCD;
        public string description;
        public uint effect;
        public int eFlag1, eFlag2, eFlag3;
        public BitMask<EquipFlags> equipFlag = new BitMask<EquipFlags>();
        public BitMask<SkillFlags> flag = new BitMask<SkillFlags>();
        public ushort icon;
        public uint id;
        public byte joblv;
        public byte maxLv, lv;
        public ushort mp, sp, ep;
        public string name;
        public ushort nHumei4, nHumei5, nHumei6, nHumei7, nHumei9, nHumei10, nAnim1, nAnim2, nAnim3;
        public sbyte range;

        public int skillFlag,
            nHumei8,
            effect1,
            effect2,
            effect3,
            effect4,
            effect5,
            effect6,
            effect7,
            effect8,
            effect9,
            nHumei2;

        public byte target, target2, effectRange, castRange;

        public override string ToString()
        {
            return name;
        }
    }

    public class Skill
    {
        public SkillData BaseData { get; set; }

        public uint ID => BaseData.id;
        public string Name => BaseData.name;

        public bool NoSave { get; set; }

        public byte MaxLevel => BaseData.maxLv;

        public byte Level { get; set; }

        public byte JobLv { get; set; }

        public ushort MP => BaseData.mp;
        public ushort SP => BaseData.sp;
        public ushort EP => BaseData.ep;
        public sbyte Range => BaseData.range;

        public uint Effect
        {
            get => BaseData.effect;
            set => BaseData.effect = value;
        }

        public byte EffectRange
        {
            get => BaseData.effectRange;
            set => BaseData.effectRange = value;
        }

        public byte CastRange => BaseData.castRange;

        public byte Target
        {
            get => BaseData.target;
            set => BaseData.target = value;
        }

        public byte Target2
        {
            get => BaseData.target2;
            set => BaseData.target2 = value;
        }

        public int CastTime => BaseData.castTime;
        public int Delay => BaseData.delay;
        public int SinglgCD => BaseData.SingleCD;
        public bool Magical => BaseData.flag.Test(SkillFlags.MAGIC);
        public bool Physical => BaseData.flag.Test(SkillFlags.PHYSIC);
        public bool PartyOnly => BaseData.flag.Test(SkillFlags.PARTY_ONLY);
        public bool Attack => BaseData.flag.Test(SkillFlags.ATTACK);
        public bool CanHasTarget => BaseData.flag.Test(SkillFlags.CAN_HAS_TARGET);
        public bool Support => BaseData.flag.Test(SkillFlags.SUPPORT);
        public bool DeadOnly => BaseData.flag.Test(SkillFlags.DEAD_ONLY);
        public bool NoPossession => BaseData.flag.Test(SkillFlags.NO_POSSESSION);
        public bool NotBeenPossessed => BaseData.flag.Test(SkillFlags.NOT_BEEN_POSSESSED);


        public override string ToString()
        {
            return BaseData.name;
        }
    }
}