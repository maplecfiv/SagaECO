using System.Collections.Generic;
using SagaDB.Mob;
using SagaLib;

namespace SagaDB.Actor
{
    public class ActorMob : Actor, IStats
    {
        /// <summary>
        ///     Another怪物ID
        /// </summary>
        public uint AnotherID;

        /// <summary>
        ///     记录被攻击，事件用
        /// </summary>
        public byte AttackedForEvent;

        protected MobData baseData;

        public uint baseExp, jobExp;

        /// <summary>
        ///     阵营 0敌人 1友军 2中立（绿名）
        /// </summary>
        public uint Camp;

        public List<MobData.DropData> dropItems = new List<MobData.DropData>();
        public List<MobData.DropData> dropItemsSpecial = new List<MobData.DropData>();
        public bool FirstDefending = false;

        private byte level;

        /// <summary>
        ///     强制的攻击目标，挑衅使用
        /// </summary>
        public Actor PriorityTartget;

        public float range;

        /// <summary>
        ///     骑宠ID
        /// </summary>
        public uint RideID;

        private VariableHolder<string, int> tIntVar = new VariableHolder<string, int>(0);
        private VariableHolder<string, string> tStrVar = new VariableHolder<string, string>("");

        public ActorMob()
        {
        }

        public ActorMob(uint mobID)
        {
            type = ActorType.MOB;
            baseData = MobFactory.Instance.GetMobData(mobID);
            Level = BaseData.level;
            MaxHP = baseData.hp;
            HP = MaxHP;
            MaxMP = baseData.mp;
            MP = MaxMP;
            MaxSP = baseData.sp;
            SP = MaxSP;
            Name = baseData.name;
            Speed = baseData.speed;
            Status.attackType = baseData.attackType;
            Status.aspd = baseData.aspd;
            Status.cspd = baseData.cspd;
            Status.def = baseData.def;
            Status.def_add = (short)baseData.def_add;
            Status.mdef = baseData.mdef;
            Status.mdef_add = (short)baseData.mdef_add;
            Status.min_atk1 = baseData.atk_min;
            Status.max_atk1 = baseData.atk_max;
            Status.min_atk2 = baseData.atk_min;
            Status.max_atk2 = baseData.atk_max;
            Status.min_atk3 = baseData.atk_min;
            Status.max_atk3 = baseData.atk_max;
            Status.min_matk = baseData.matk_min;
            Status.max_matk = baseData.matk_max;
            Status.hit_critical = baseData.cri;
            Status.avoid_critical = baseData.criavd;
            Status.hit_magic = baseData.hit_magic;
            Status.avoid_magic = baseData.avoid_magic;
            Race = baseData.race;
            foreach (var i in baseData.elements.Keys)
            {
                Elements[i] = baseData.elements[i];
                AttackElements[i] = 0;
            }

            foreach (var i in baseData.abnormalStatus.Keys) AbnormalStatus[i] = baseData.abnormalStatus[i];
            Status.hit_melee = baseData.hit_melee;
            Status.hit_ranged = baseData.hit_ranged;
            Status.avoid_melee = baseData.avoid_melee;
            Status.avoid_ranged = baseData.avoid_ranged;

            Status.undead = baseData.undead;
        }

        public ActorMob(uint mobID, MobInfo info)
        {
            type = ActorType.MOB;
            baseData = MobFactory.Instance.GetMobData(mobID);
            if (info.level != 0)
                Level = info.level;
            MaxHP = info.maxhp;
            HP = info.maxhp;
            MaxMP = info.maxmp;
            MP = info.maxmp;
            MaxSP = info.maxsp;
            SP = info.maxsp;
            Name = info.name;
            Speed = info.speed;
            if (info.AttackType != null)
                Status.attackType = info.AttackType;
            else
                Status.attackType = baseData.attackType;
            Status.aspd = info.Aspd;
            Status.cspd = info.Cspd;
            Status.def = info.def;
            Status.def_add = (short)info.def_add;
            Status.mdef = info.mdef;
            Status.mdef_add = (short)info.mdef_add;
            Status.min_atk1 = info.atk_min;
            Status.max_atk1 = info.atk_max;
            Status.min_atk2 = info.atk_min;
            Status.max_atk2 = info.atk_max;
            Status.min_atk3 = info.atk_min;
            Status.max_atk3 = info.atk_max;
            Status.min_matk = info.matk_min;
            Status.max_matk = info.matk_max;
            Status.hit_critical = info.hit_critical;
            Status.avoid_critical = info.avoid_critical;
            Status.hit_magic = info.hit_magic;
            Status.avoid_magic = info.avoid_magic;
            Race = info.Race;

            foreach (var i in baseData.elements.Keys)
            {
                Elements[i] = info.elements[i];
                AttackElements[i] = 0;
            }

            foreach (var i in baseData.abnormalStatus.Keys) AbnormalStatus[i] = info.abnormalstatus[i];
            Status.hit_melee = info.hit_melee;
            Status.hit_ranged = info.hit_ranged;
            Status.avoid_melee = info.avoid_melee;
            Status.avoid_ranged = info.avoid_ranged;

            Status.undead = info.undead;
            range = info.range;
            baseExp = info.baseExp;
            jobExp = info.jobExp;

            dropItems = new List<MobData.DropData>();
            dropItems = info.dropItems;

            dropItemsSpecial = new List<MobData.DropData>();
            dropItemsSpecial = info.dropItemsSpecial;
        }

        public uint MobID => baseData.id;

        public override byte Level
        {
            get
            {
                if (level != baseData.level)
                    return level;
                return baseData.level;
            }
            set => level = value;
        }

        public Actor Owner { get; set; }

        public MobData BaseData => baseData;

        public ushort Str
        {
            get => baseData.str;
            set { }
        }

        public ushort Dex
        {
            get => baseData.dex;
            set { }
        }

        public ushort Int
        {
            get => baseData.intel;
            set { }
        }

        public ushort Vit
        {
            get => baseData.vit;
            set { }
        }

        public ushort Agi
        {
            get => baseData.agi;
            set { }
        }

        public ushort Mag
        {
            get => baseData.mag;
            set { }
        }

        public class MobInfo
        {
            public Dictionary<AbnormalStatus, short> abnormalstatus = new Dictionary<AbnormalStatus, short>();
            public short Aspd, Cspd;
            public ATTACK_TYPE AttackType;
            public uint baseExp, jobExp;
            public List<MobData.DropData> dropItems = new List<MobData.DropData>();
            public List<MobData.DropData> dropItemsSpecial = new List<MobData.DropData>();
            public Dictionary<Elements, int> elements = new Dictionary<Elements, int>();
            public byte level = 0;
            public uint maxhp, maxmp, maxsp;
            public string name;
            public Race Race;
            public float range = 0;

            public ushort speed,
                atk_min,
                atk_max,
                matk_min,
                matk_max,
                def,
                mdef,
                def_add,
                mdef_add,
                hit_magic,
                hit_melee,
                hit_ranged,
                hit_critical,
                avoid_magic,
                avoid_melee,
                avoid_ranged,
                avoid_critical;

            public bool undead;

            public MobInfo()
            {
                elements.Add(SagaLib.Elements.Neutral, 0);
                elements.Add(SagaLib.Elements.Fire, 0);
                elements.Add(SagaLib.Elements.Water, 0);
                elements.Add(SagaLib.Elements.Wind, 0);
                elements.Add(SagaLib.Elements.Earth, 0);
                elements.Add(SagaLib.Elements.Holy, 0);
                elements.Add(SagaLib.Elements.Dark, 0);

                abnormalstatus.Add(SagaLib.AbnormalStatus.Confused, 0);
                abnormalstatus.Add(SagaLib.AbnormalStatus.Frosen, 0);
                abnormalstatus.Add(SagaLib.AbnormalStatus.Paralyse, 0);
                abnormalstatus.Add(SagaLib.AbnormalStatus.Poisen, 0);
                abnormalstatus.Add(SagaLib.AbnormalStatus.Silence, 0);
                abnormalstatus.Add(SagaLib.AbnormalStatus.Sleep, 0);
                abnormalstatus.Add(SagaLib.AbnormalStatus.Stone, 0);
                abnormalstatus.Add(SagaLib.AbnormalStatus.Stun, 0);
                abnormalstatus.Add(SagaLib.AbnormalStatus.MoveSpeedDown, 0);
            }
            /*public uint MaxHP { get { return this.maxhp; } set { } }
            public uint MaxMP { get { return this.maxmp; } set { } }
            public uint MaxSP { get { return this.maxsp; } set { } }
            public string Name { get { return this.name; } set { } }
            public ushort Speed { get { return this.speed; } set { } }
            public ushort Atk_min { get { return this.atk_min; } set { } }
            public ushort Atk_max { get { return this.atk_max; } set { } }
            public ushort Matk_min { get { return this.matk_min; } set { } }
            public ushort Matk_max { get { return this.matk_max; } set { } }

            public Dictionary<SagaLib.Elements, int> Elements { get { return this.elements; } set { } }
            public Dictionary<SagaLib.AbnormalStatus, short> Abnormalstatus { get { return this.abnormalstatus; } set { } }*/
        }
    }
}