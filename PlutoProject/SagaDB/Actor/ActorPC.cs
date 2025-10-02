using System;
using System.Collections.Generic;
using SagaDB.BBS;
using SagaDB.Iris;
using SagaDB.Item;
using SagaDB.Map;
using SagaDB.Quests;
using SagaDB.Tamaire;
using SagaLib;

namespace SagaDB.Actor
{
    [Serializable]

    //#region 商人商店部分..

    public class PlayerShopItem
    {
        private ushort count;
        private uint inventoryID;
        private uint itemID;
        private ulong price;

        public uint InventoryID
        {
            get => inventoryID;
            set => inventoryID = value;
        }

        public uint ItemID
        {
            get => itemID;
            set => itemID = value;
        }

        public ushort Count
        {
            get => count;
            set => count = value;
        }

        public ulong Price
        {
            get => price;
            set => price = value;
        }
    }

    //#endregion

    //#region 幻化外观部分..

    public class Appearance
    {
        //null时隐藏（设置为0），不存在时跳过

        //其他都是0时不变


        //为none时将维持原样

        //-1时不改变

        public PC_RACE Race { get; set; } = PC_RACE.NONE;

        public PC_GENDER Gender { get; set; } = PC_GENDER.NONE;

        public DEM_FORM Form { get; set; } = DEM_FORM.NONE;

        public byte WingStyle { get; set; } = byte.MaxValue;

        public byte TailStyle { get; set; } = byte.MaxValue;

        public byte WingColor { get; set; } = byte.MaxValue;

        public ushort HairStyle { get; set; }

        public byte HairColor { get; set; }

        public ushort Wig { get; set; }

        public ushort Face { get; set; }

        public uint MarionettePictID { get; set; }

        public Dictionary<EnumEquipSlot, Item.Item> Equips { get; set; } = new Dictionary<EnumEquipSlot, Item.Item>();

        public bool Illusion()
        {
            var result = true;
            result &= Race == PC_RACE.NONE && Gender == PC_GENDER.NONE && Form == DEM_FORM.NONE;
            result &= TailStyle == byte.MaxValue && WingStyle == byte.MaxValue && WingColor == byte.MaxValue;
            result &= HairStyle == 0 && HairColor == 0 && Wig == 0 && Face == 0;
            result &= MarionettePictID == 0;
            result &= Equips.Count == 0;
            return !result;
        }
    }

    //#endregion

    /// <summary>
    ///     副职基本信息
    /// </summary>
    [Serializable]
    public class PlayerDualJobInfo
    {
        public ulong DualJobExp = 0;
        public byte DualJobID = 0;
        public byte DualJobLevel = 0;
    }

    public enum PlayerUsingShopType
    {
        None,
        GShop,
        NCShop
    }

    public class ActorPC : Actor, IStats
    {
        //Iris system

        /// <summary>
        ///     拼图装备相关
        /// </summary>
        public Dictionary<uint, AnotherDetail> AnotherPapers = new Dictionary<uint, AnotherDetail>();

        public Appearance appearance; //幻化目标外观


        public bool AutoAttack = false;

        public bool canTrade = true,
            canParty = true,
            canPossession = true,
            canRing = true,
            showRevive = true,
            canWork = true,
            canMentor = true,
            showEquipment = true,
            canChangePartnerDisplay = true,
            canFriend = true;

        private int CPlimit;

        private DateTime CPLine = DateTime.Now;

        //当前副职ID
        public byte DualJobID = 0;

        //当前副职等级
        public byte DualJobLevel = 0;

        //副职技能列表
        public List<Skill.Skill> DualJobSkill = new List<Skill.Skill>();

        /// <summary>
        ///     玩家选择的副本难度
        /// </summary>
        public byte DungeonsDifc;

        /// <summary>
        ///     玩家在副本的死亡次数限制
        /// </summary>
        public byte DungeonsReviveCount;

        public byte EMotion;
        public bool EMotionLoop;

        //Fish bait

        //Navi.Navi navi = new Navi.Navi(NaviFactory.Instance.Navi);

        public uint[] equips;

        //冒险经验
        private ushort face;
        private uint fame;

        public bool Fictitious;

        /// <summary>
        ///     称号的前缀
        /// </summary>
        public string FirstName = "";

        public List<Gift> Gifts = new List<Gift>();
        private long gold;
        private int Goldlimit;

        private DateTime GoldLine = DateTime.Now;
        private byte hairColor;

        private ushort hairStyle;

        private byte jlv1;
        private byte jlv2t;
        private byte jlv2x;

        private byte jlv3;
        public uint JobLV_CARDINAL;
        public uint JobLV_FORCEMASTER;
        public uint JobLV_GLADIATOR;
        public uint JobLV_HAWKEYE;

        /// <summary>
        ///     击杀列表
        /// </summary>
        public Dictionary<uint, KillInfo> KillList = new Dictionary<uint, KillInfo>();

        public uint LastAttackActorID;
        private byte lv, dlv, djlv, jjlv;

        public List<Mail> Mails = new List<Mail>();

        public short MaxHealMpForWeapon;

        private PlayerMode mode = PlayerMode.NORMAL;

        //当前玩家旅人目錄
        public Dictionary<uint, bool> MosterGuide = new Dictionary<uint, bool>();

        public uint NowUseFurnitureID = 0;

        /// <summary>
        ///     自动显示隐藏NPC列表（地图ID，state = 0 显示  state = 1 隐藏）
        /// </summary>
        public Dictionary<uint, NPCHide> NpcShowList = new Dictionary<uint, NPCHide>();

        /// <summary>
        ///     玩家当前目标，供搭档使用。
        /// </summary>
        public Actor PartnerTartget;

        //当前玩家可用副职清单
        public Dictionary<byte, PlayerDualJobInfo> PlayerDualJobList = new Dictionary<byte, PlayerDualJobInfo>();

        /// <summary>
        ///     玩家的称号名
        /// </summary>
        public string PlayerTitle = "";

        /// <summary>
        ///     凭依在项链的宠物道具位置
        /// </summary>
        public uint PossessionPartnerSlotIDinAccesory;

        /// <summary>
        ///     凭依在衣服的宠物道具位置
        /// </summary>
        public uint PossessionPartnerSlotIDinClothes;

        /// <summary>
        ///     凭依在左手的宠物道具位置
        /// </summary>
        public uint PossessionPartnerSlotIDinLeftHand;

        /// <summary>
        ///     凭依在右手的宠物道具位置
        /// </summary>
        public uint PossessionPartnerSlotIDinRightHand;

        private ushort questRemaining;

        /// <summary>
        ///     显示前缀开关
        /// </summary>
        public byte ShowFirstName = 0;

        //END

        private uint cp, ecoin;

        private ushort skillpoint3;

        /// <summary>
        ///     1转职业技能
        /// </summary>
        public Dictionary<uint, Skill.Skill> Skills = new Dictionary<uint, Skill.Skill>();

        /// <summary>
        ///     2转职业技能
        /// </summary>
        public Dictionary<uint, Skill.Skill> Skills2 = new Dictionary<uint, Skill.Skill>();

        //TT添加的部分

        /// <summary>
        ///     2-1职业技能
        /// </summary>
        public Dictionary<uint, Skill.Skill> Skills2_1 = new Dictionary<uint, Skill.Skill>();

        /// <summary>
        ///     2-2职业技能
        /// </summary>
        public Dictionary<uint, Skill.Skill> Skills2_2 = new Dictionary<uint, Skill.Skill>();

        /// <summary>
        ///     3转职业技能
        /// </summary>
        public Dictionary<uint, Skill.Skill> Skills3 = new Dictionary<uint, Skill.Skill>();

        /// <summary>
        ///     2转职业保留技能
        /// </summary>
        public Dictionary<uint, Skill.Skill> SkillsReserve = new Dictionary<uint, Skill.Skill>();

        private ushort statspoints, skillpoint, skillpoint2x, skillpoint2t, dstatspoints;

        private byte tailStyle; //尾巴形狀

        private VariableHolder<string, int> tIntVar = new VariableHolder<string, int>(0);

        /// <summary>
        ///     變身圖片ID
        /// </summary>
        private uint tranceID;

        private VariableHolder<string, string> tStrVar = new VariableHolder<string, string>("");


        public PlayerUsingShopType UsingShopType = PlayerUsingShopType.None;

        private uint vpoints, usedVPoints;
        private ushort wig;
        private byte wingColor; //翅膀顏色
        private byte wingStyle; //翅膀形狀

        private int wrp;

        public ActorPC()
        {
            type = ActorType.PC;
            sightRange = Global.MAX_SIGHT_RANGE;
            Speed = 410;
            Inventory = new Inventory(this);
            appearance = new Appearance();
        }

        /// <summary>
        ///     PC's Iris Ability Value Points
        /// </summary>
        public Dictionary<AbilityVector, int> IrisAbilityValues { get; } = new Dictionary<AbilityVector, int>();

        /// <summary>
        ///     PC's Iris Ability Value Levels
        /// </summary>
        public Dictionary<AbilityVector, int> IrisAbilityLevels { get; } = new Dictionary<AbilityVector, int>();

        /// <summary>
        ///     站姿
        /// </summary>
        public byte WaitType
        {
            get => (byte)CInt["WaitType"];
            set => CInt["WaitType"] = value;
        }

        /// <summary>
        ///     伪造ACTOR用装备栏
        /// </summary>
        public uint[] Equips
        {
            get => equips;
            set => equips = value;
        }

        /// <summary>
        ///     正在使用的拼图ID
        /// </summary>
        public ushort UsingPaperID { set; get; }

        /// <summary>
        ///     正在使用的称号ID
        /// </summary>
        public ushort PlayerTitleID { set; get; }

        /// <summary>
        ///     剩余3技能点
        /// </summary>
        public ushort SkillPoint3
        {
            get => skillpoint3;
            set
            {
                skillpoint3 = value;
                if (e != null) e.PropertyUpdate(UpdateEvent.STAT_POINT, 0);
            }
        }

        /// <summary>
        ///     3职业等级
        /// </summary>
        public byte JobLevel3
        {
            get => jlv3;
            set
            {
                jlv3 = value;
                if (e != null)
                    e.PropertyUpdate(UpdateEvent.LEVEL, 0);
            }
        }

        /// <summary>
        ///     转生前等级
        /// </summary>
        public byte Level1 { get; set; }

        /// <summary>
        ///     转生标记
        /// </summary>
        public bool Rebirth => Job == Job3;

        /// <summary>
        ///     尾巴形狀
        /// </summary>
        public byte TailStyle
        {
            get => tailStyle;
            set
            {
                tailStyle = value;
                if (e != null) e.PropertyUpdate(UpdateEvent.CHAR_INFO, 0);
            }
        }

        /// <summary>
        ///     翅膀形狀
        /// </summary>
        public byte WingStyle
        {
            get => wingStyle;
            set
            {
                wingStyle = value;
                if (e != null) e.PropertyUpdate(UpdateEvent.CHAR_INFO, 0);
            }
        }

        /// <summary>
        ///     翅膀顏色
        /// </summary>
        public byte WingColor
        {
            get => wingColor;
            set
            {
                wingColor = value;
                if (e != null) e.PropertyUpdate(UpdateEvent.CHAR_INFO, 0);
            }
        }

        /// <summary>
        ///     當前已裝備的魚餌
        /// </summary>
        public uint EquipedBaitID { get; set; }

        /// <summary>
        ///     体积
        /// </summary>
        public uint Size { get; set; }

        /// <summary>
        ///     存在于数据库的CharID
        /// </summary>
        public uint CharID { get; set; }

        /// <summary>
        ///     该玩家的帐号信息
        /// </summary>
        [field: NonSerialized]
        public Account Account { get; set; }

        /// <summary>
        ///     种族
        /// </summary>
        public PC_RACE Race { get; set; }

        /// <summary>
        ///     性别
        /// </summary>
        public PC_GENDER Gender { get; set; }

        /// <summary>
        ///     发型
        /// </summary>
        public ushort HairStyle
        {
            get => hairStyle;
            set
            {
                hairStyle = value;
                if (e != null) e.PropertyUpdate(UpdateEvent.CHAR_INFO, 0);
            }
        }

        /// <summary>
        ///     头发颜色
        /// </summary>
        public byte HairColor
        {
            get => hairColor;
            set
            {
                hairColor = value;
                if (e != null) e.PropertyUpdate(UpdateEvent.CHAR_INFO, 0);
            }
        }

        /// <summary>
        ///     假发
        /// </summary>
        public ushort Wig
        {
            get => wig;
            set
            {
                wig = value;
                if (e != null) e.PropertyUpdate(UpdateEvent.CHAR_INFO, 0);
            }
        }

        /// <summary>
        ///     脸
        /// </summary>
        public ushort Face
        {
            get => face;
            set
            {
                face = value;
                if (e != null) e.PropertyUpdate(UpdateEvent.CHAR_INFO, 0);
            }
        }

        /// <summary>
        ///     职业
        /// </summary>
        public PC_JOB Job { get; set; }

        /// <summary>
        ///     等级
        /// </summary>
        public override byte Level
        {
            get => lv;
            set
            {
                lv = value;
                if (e != null)
                    e.PropertyUpdate(UpdateEvent.LEVEL, 0);
            }
        }

        /// <summary>
        ///     恶魔界的基础等级
        /// </summary>
        public byte DominionLevel
        {
            get => dlv;
            set
            {
                dlv = value;
                if (e != null)
                    e.PropertyUpdate(UpdateEvent.LEVEL, 0);
            }
        }

        /// <summary>
        ///     恶魔界的职业等级
        /// </summary>
        public byte DominionJobLevel
        {
            get => djlv;
            set
            {
                djlv = value;
                if (e != null)
                    e.PropertyUpdate(UpdateEvent.LEVEL, 0);
            }
        }

        /// <summary>
        ///     联合职业等级
        /// </summary>
        public byte JointJobLevel
        {
            get => jjlv;
            set
            {
                jjlv = value;
                if (e != null)
                    e.PropertyUpdate(UpdateEvent.LEVEL, 0);
            }
        }

        /// <summary>
        ///     当前职业等级
        /// </summary>
        public byte CurrentJobLevel
        {
            get
            {
                if (DualJobID != 0)
                    return PlayerDualJobList[DualJobID].DualJobLevel;
                if (Job == JobBasic)
                    return JobLevel1;
                if (Job == Job2X)
                    return JobLevel2X;
                if (Job == Job2T)
                    return JobLevel2T;
                if (Job == Job3)
                    return JobLevel3;
                return JobLevel1;
            }
        }

        /// <summary>
        ///     1转职业等级
        /// </summary>
        public byte JobLevel1
        {
            get => jlv1;
            set
            {
                jlv1 = value;
                if (e != null)
                    e.PropertyUpdate(UpdateEvent.LEVEL, 0);
            }
        }

        /// <summary>
        ///     剩余任务数
        /// </summary>
        public ushort QuestRemaining
        {
            get => (ushort)AInt["剩余任务点数"];
            set => AInt["剩余任务点数"] = value;
            /*if (e != null)
                    e.PropertyUpdate(UpdateEvent.QUEST_POINT, 0);*/
        }

        /// <summary>
        ///     2-1职业等级
        /// </summary>
        public byte JobLevel2X
        {
            get => jlv2x;
            set
            {
                jlv2x = value;
                if (e != null)
                    e.PropertyUpdate(UpdateEvent.LEVEL, 0);
            }
        }

        /// <summary>
        ///     2－2职业等级
        /// </summary>
        public byte JobLevel2T
        {
            get => jlv2t;
            set
            {
                jlv2t = value;
                if (e != null)
                    e.PropertyUpdate(UpdateEvent.LEVEL, 0);
            }
        }

        /*
        /// <summary>
        /// 物理熟练度等级
        /// </summary>
        public byte DefLv
        {
            get
            {
                return deflv;
            }
            set
            {
                deflv = value;
            }
        }
        /// <summary>
        /// 魔法熟练度等级
        /// </summary>
        public byte MDefLv
        {
            get
            {
                return mdeflv;
            }
            set
            {
                mdeflv = value;
            }
        }
        /// <summary>
        /// 物理熟练度点数
        /// </summary>
        public uint DefPoint
        {
            get
            {
                return defpoint;
            }
            set
            {
                defpoint = value;
            }
        }
        /// <summary>
        /// 物理熟练度点数
        /// </summary>
        public uint MDefPoint
        {
            get
            {
                return mdefpoint;
            }
            set
            {
                mdefpoint = value;
            }
        }*/
        /// <summary>
        ///     人物所占用的存储槽
        /// </summary>
        public byte Slot { get; set; }

        public bool InDominionWorld
        {
            get
            {
                if (MapInfoFactory.Instance.MapInfo.ContainsKey(MapID))
                {
                    var map = MapInfoFactory.Instance.MapInfo[MapID];
                    if (map.Flag.Test(MapFlags.Dominion))
                        return true;
                    return false;
                }

                var oriMap = MapID / 1000 * 1000;
                if (MapInfoFactory.Instance.MapInfo.ContainsKey(oriMap))
                {
                    var map = MapInfoFactory.Instance.MapInfo[oriMap];
                    if (map.Flag.Test(MapFlags.Dominion))
                        return true;
                    return false;
                }

                return false;
            }
        }

        public List<ActorPC> PossesionedActors
        {
            get
            {
                var list = new List<ActorPC>();
                if (Inventory != null)
                {
                    if (Inventory.Equipments.ContainsKey(EnumEquipSlot.CHEST_ACCE))
                        if (Inventory.Equipments[EnumEquipSlot.CHEST_ACCE].PossessionedActor != null)
                            if (!list.Contains(Inventory.Equipments[EnumEquipSlot.CHEST_ACCE].PossessionedActor))
                                list.Add(Inventory.Equipments[EnumEquipSlot.CHEST_ACCE].PossessionedActor);
                    if (Inventory.Equipments.ContainsKey(EnumEquipSlot.UPPER_BODY))
                        if (Inventory.Equipments[EnumEquipSlot.UPPER_BODY].PossessionedActor != null)
                            if (!list.Contains(Inventory.Equipments[EnumEquipSlot.UPPER_BODY].PossessionedActor))
                                list.Add(Inventory.Equipments[EnumEquipSlot.UPPER_BODY].PossessionedActor);
                    if (Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND))
                        if (Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].PossessionedActor != null)
                            if (!list.Contains(Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].PossessionedActor))
                                list.Add(Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].PossessionedActor);
                    if (Inventory.Equipments.ContainsKey(EnumEquipSlot.LEFT_HAND))
                        if (Inventory.Equipments[EnumEquipSlot.LEFT_HAND].PossessionedActor != null)
                            if (!list.Contains(Inventory.Equipments[EnumEquipSlot.LEFT_HAND].PossessionedActor))
                                list.Add(Inventory.Equipments[EnumEquipSlot.LEFT_HAND].PossessionedActor);
                }

                return list;
            }
        }

        /// <summary>
        ///     该玩家对应的联合职业
        /// </summary>
        public PC_JOB JobJoint { get; set; } = PC_JOB.NONE;

        /// <summary>
        ///     该玩家对应的1转职业
        /// </summary>
        public PC_JOB JobBasic
        {
            get
            {
                switch (Job)
                {
                    case PC_JOB.SWORDMAN:
                    case PC_JOB.BLADEMASTER:
                    case PC_JOB.BOUNTYHUNTER:
                    case PC_JOB.GLADIATOR:
                        return PC_JOB.SWORDMAN;
                    case PC_JOB.FENCER:
                    case PC_JOB.KNIGHT:
                    case PC_JOB.DARKSTALKER:
                    case PC_JOB.GUARDIAN:
                        return PC_JOB.FENCER;
                    case PC_JOB.SCOUT:
                    case PC_JOB.ASSASSIN:
                    case PC_JOB.COMMAND:
                    case PC_JOB.ERASER:
                        return PC_JOB.SCOUT;
                    case PC_JOB.ARCHER:
                    case PC_JOB.STRIKER:
                    case PC_JOB.GUNNER:
                    case PC_JOB.HAWKEYE:
                        return PC_JOB.ARCHER;
                    case PC_JOB.WIZARD:
                    case PC_JOB.SORCERER:
                    case PC_JOB.SAGE:
                    case PC_JOB.FORCEMASTER:
                        return PC_JOB.WIZARD;
                    case PC_JOB.SHAMAN:
                    case PC_JOB.ELEMENTER:
                    case PC_JOB.ENCHANTER:
                    case PC_JOB.ASTRALIST:
                        return PC_JOB.SHAMAN;
                    case PC_JOB.VATES:
                    case PC_JOB.DRUID:
                    case PC_JOB.BARD:
                    case PC_JOB.CARDINAL:
                        return PC_JOB.VATES;
                    case PC_JOB.WARLOCK:
                    case PC_JOB.CABALIST:
                    case PC_JOB.NECROMANCER:
                    case PC_JOB.SOULTAKER:
                        return PC_JOB.WARLOCK;
                    case PC_JOB.TATARABE:
                    case PC_JOB.BLACKSMITH:
                    case PC_JOB.MACHINERY:
                    case PC_JOB.MAESTRO:
                        return PC_JOB.TATARABE;
                    case PC_JOB.FARMASIST:
                    case PC_JOB.ALCHEMIST:
                    case PC_JOB.MARIONEST:
                    case PC_JOB.HARVEST:
                        return PC_JOB.FARMASIST;
                    case PC_JOB.RANGER:
                    case PC_JOB.EXPLORER:
                    case PC_JOB.TREASUREHUNTER:
                    case PC_JOB.STRIDER:
                        return PC_JOB.RANGER;
                    case PC_JOB.MERCHANT:
                    case PC_JOB.TRADER:
                    case PC_JOB.GAMBLER:
                    case PC_JOB.ROYALDEALER:
                        return PC_JOB.MERCHANT;
                    default:
                        return PC_JOB.NOVICE;
                }
                /*
                int intJob = (int)this.job;
                if (intJob > 0 && intJob < 120)
                    return (PC_JOB)((intJob / 10) * 10 + 1);
                else
                    return PC_JOB.NOVICE;
                 //*/
            }
        }

        /// <summary>
        ///     该玩家对应的2－1职业
        /// </summary>
        public PC_JOB Job2X
        {
            get
            {
                switch (Job)
                {
                    case PC_JOB.SWORDMAN:
                    case PC_JOB.BLADEMASTER:
                    case PC_JOB.BOUNTYHUNTER:
                    case PC_JOB.GLADIATOR:
                        return PC_JOB.BLADEMASTER;
                    case PC_JOB.FENCER:
                    case PC_JOB.KNIGHT:
                    case PC_JOB.DARKSTALKER:
                    case PC_JOB.GUARDIAN:
                        return PC_JOB.KNIGHT;
                    case PC_JOB.SCOUT:
                    case PC_JOB.ASSASSIN:
                    case PC_JOB.COMMAND:
                    case PC_JOB.ERASER:
                        return PC_JOB.ASSASSIN;
                    case PC_JOB.ARCHER:
                    case PC_JOB.STRIKER:
                    case PC_JOB.GUNNER:
                    case PC_JOB.HAWKEYE:
                        return PC_JOB.STRIKER;
                    case PC_JOB.WIZARD:
                    case PC_JOB.SORCERER:
                    case PC_JOB.SAGE:
                    case PC_JOB.FORCEMASTER:
                        return PC_JOB.SORCERER;
                    case PC_JOB.SHAMAN:
                    case PC_JOB.ELEMENTER:
                    case PC_JOB.ENCHANTER:
                    case PC_JOB.ASTRALIST:
                        return PC_JOB.ELEMENTER;
                    case PC_JOB.VATES:
                    case PC_JOB.DRUID:
                    case PC_JOB.BARD:
                    case PC_JOB.CARDINAL:
                        return PC_JOB.DRUID;
                    case PC_JOB.WARLOCK:
                    case PC_JOB.CABALIST:
                    case PC_JOB.NECROMANCER:
                    case PC_JOB.SOULTAKER:
                        return PC_JOB.CABALIST;
                    case PC_JOB.TATARABE:
                    case PC_JOB.BLACKSMITH:
                    case PC_JOB.MACHINERY:
                    case PC_JOB.MAESTRO:
                        return PC_JOB.BLACKSMITH;
                    case PC_JOB.FARMASIST:
                    case PC_JOB.ALCHEMIST:
                    case PC_JOB.MARIONEST:
                    case PC_JOB.HARVEST:
                        return PC_JOB.ALCHEMIST;
                    case PC_JOB.RANGER:
                    case PC_JOB.EXPLORER:
                    case PC_JOB.TREASUREHUNTER:
                    case PC_JOB.STRIDER:
                        return PC_JOB.EXPLORER;
                    case PC_JOB.MERCHANT:
                    case PC_JOB.TRADER:
                    case PC_JOB.GAMBLER:
                    case PC_JOB.ROYALDEALER:
                        return PC_JOB.TRADER;
                    default:
                        return PC_JOB.NOVICE;
                }
                /*
                int intJob = (int)this.job;
                if (intJob > 0 && intJob < 120)
                    return (PC_JOB)((intJob / 10) * 10 + 3);
                else
                    return PC_JOB.NOVICE;//*/
            }
        }

        /// <summary>
        ///     该玩家对应的2－2职业
        /// </summary>
        public PC_JOB Job2T
        {
            get
            {
                switch (Job)
                {
                    case PC_JOB.SWORDMAN:
                    case PC_JOB.BLADEMASTER:
                    case PC_JOB.BOUNTYHUNTER:
                    case PC_JOB.GLADIATOR:
                        return PC_JOB.BOUNTYHUNTER;
                    case PC_JOB.FENCER:
                    case PC_JOB.KNIGHT:
                    case PC_JOB.DARKSTALKER:
                    case PC_JOB.GUARDIAN:
                        return PC_JOB.DARKSTALKER;
                    case PC_JOB.SCOUT:
                    case PC_JOB.ASSASSIN:
                    case PC_JOB.COMMAND:
                    case PC_JOB.ERASER:
                        return PC_JOB.COMMAND;
                    case PC_JOB.ARCHER:
                    case PC_JOB.STRIKER:
                    case PC_JOB.GUNNER:
                    case PC_JOB.HAWKEYE:
                        return PC_JOB.GUNNER;
                    case PC_JOB.WIZARD:
                    case PC_JOB.SORCERER:
                    case PC_JOB.SAGE:
                    case PC_JOB.FORCEMASTER:
                        return PC_JOB.SAGE;
                    case PC_JOB.SHAMAN:
                    case PC_JOB.ELEMENTER:
                    case PC_JOB.ENCHANTER:
                    case PC_JOB.ASTRALIST:
                        return PC_JOB.ENCHANTER;
                    case PC_JOB.VATES:
                    case PC_JOB.DRUID:
                    case PC_JOB.BARD:
                    case PC_JOB.CARDINAL:
                        return PC_JOB.BARD;
                    case PC_JOB.WARLOCK:
                    case PC_JOB.CABALIST:
                    case PC_JOB.NECROMANCER:
                    case PC_JOB.SOULTAKER:
                        return PC_JOB.NECROMANCER;
                    case PC_JOB.TATARABE:
                    case PC_JOB.BLACKSMITH:
                    case PC_JOB.MACHINERY:
                    case PC_JOB.MAESTRO:
                        return PC_JOB.MACHINERY;
                    case PC_JOB.FARMASIST:
                    case PC_JOB.ALCHEMIST:
                    case PC_JOB.MARIONEST:
                    case PC_JOB.HARVEST:
                        return PC_JOB.MARIONEST;
                    case PC_JOB.RANGER:
                    case PC_JOB.EXPLORER:
                    case PC_JOB.TREASUREHUNTER:
                    case PC_JOB.STRIDER:
                        return PC_JOB.TREASUREHUNTER;
                    case PC_JOB.MERCHANT:
                    case PC_JOB.TRADER:
                    case PC_JOB.GAMBLER:
                    case PC_JOB.ROYALDEALER:
                        return PC_JOB.GAMBLER;
                    default:
                        return PC_JOB.NOVICE;
                }
                /*
                int intJob = (int)this.job;
                if (intJob > 0 && intJob < 120)
                    return (PC_JOB)((intJob / 10) * 10 + 5);
                else
                    return PC_JOB.NOVICE;
                //*/
            }
        }

        /// <summary>
        ///     该玩家对应的3职业
        /// </summary>
        public PC_JOB Job3
        {
            get
            {
                switch (Job)
                {
                    case PC_JOB.SWORDMAN:
                    case PC_JOB.BLADEMASTER:
                    case PC_JOB.BOUNTYHUNTER:
                    case PC_JOB.GLADIATOR:
                        return PC_JOB.GLADIATOR;
                    case PC_JOB.FENCER:
                    case PC_JOB.KNIGHT:
                    case PC_JOB.DARKSTALKER:
                    case PC_JOB.GUARDIAN:
                        return PC_JOB.GUARDIAN;
                    case PC_JOB.SCOUT:
                    case PC_JOB.ASSASSIN:
                    case PC_JOB.COMMAND:
                    case PC_JOB.ERASER:
                        return PC_JOB.ERASER;
                    case PC_JOB.ARCHER:
                    case PC_JOB.STRIKER:
                    case PC_JOB.GUNNER:
                    case PC_JOB.HAWKEYE:
                        return PC_JOB.HAWKEYE;
                    case PC_JOB.WIZARD:
                    case PC_JOB.SORCERER:
                    case PC_JOB.SAGE:
                    case PC_JOB.FORCEMASTER:
                        return PC_JOB.FORCEMASTER;
                    case PC_JOB.SHAMAN:
                    case PC_JOB.ELEMENTER:
                    case PC_JOB.ENCHANTER:
                    case PC_JOB.ASTRALIST:
                        return PC_JOB.ASTRALIST;
                    case PC_JOB.VATES:
                    case PC_JOB.DRUID:
                    case PC_JOB.BARD:
                    case PC_JOB.CARDINAL:
                        return PC_JOB.CARDINAL;
                    case PC_JOB.WARLOCK:
                    case PC_JOB.CABALIST:
                    case PC_JOB.NECROMANCER:
                    case PC_JOB.SOULTAKER:
                        return PC_JOB.SOULTAKER;
                    case PC_JOB.TATARABE:
                    case PC_JOB.BLACKSMITH:
                    case PC_JOB.MACHINERY:
                    case PC_JOB.MAESTRO:
                        return PC_JOB.MAESTRO;
                    case PC_JOB.FARMASIST:
                    case PC_JOB.ALCHEMIST:
                    case PC_JOB.MARIONEST:
                    case PC_JOB.HARVEST:
                        return PC_JOB.HARVEST;
                    case PC_JOB.RANGER:
                    case PC_JOB.EXPLORER:
                    case PC_JOB.TREASUREHUNTER:
                    case PC_JOB.STRIDER:
                        return PC_JOB.STRIDER;
                    case PC_JOB.MERCHANT:
                    case PC_JOB.TRADER:
                    case PC_JOB.GAMBLER:
                    case PC_JOB.ROYALDEALER:
                        return PC_JOB.ROYALDEALER;
                    default:
                        return PC_JOB.JOKER;
                }
                /*
                int intJob = (int)this.job;
                if (intJob > 0 && intJob < 120)
                    return (PC_JOB)((intJob / 10) * 10 + 5);
                else
                    return PC_JOB.NOVICE;
                //*/
            }
        }

        /// <summary>
        ///     JobType
        /// </summary>
        public JobType JobType
        {
            get
            {
                switch (Job)
                {
                    case PC_JOB.SWORDMAN:
                    case PC_JOB.BLADEMASTER:
                    case PC_JOB.BOUNTYHUNTER:
                    case PC_JOB.GLADIATOR:
                    case PC_JOB.FENCER:
                    case PC_JOB.KNIGHT:
                    case PC_JOB.DARKSTALKER:
                    case PC_JOB.GUARDIAN:
                    case PC_JOB.SCOUT:
                    case PC_JOB.ASSASSIN:
                    case PC_JOB.COMMAND:
                    case PC_JOB.ERASER:
                    case PC_JOB.ARCHER:
                    case PC_JOB.STRIKER:
                    case PC_JOB.GUNNER:
                    case PC_JOB.HAWKEYE:
                        return JobType.FIGHTER;
                    case PC_JOB.WIZARD:
                    case PC_JOB.SORCERER:
                    case PC_JOB.SAGE:
                    case PC_JOB.FORCEMASTER:
                    case PC_JOB.SHAMAN:
                    case PC_JOB.ELEMENTER:
                    case PC_JOB.ENCHANTER:
                    case PC_JOB.ASTRALIST:
                    case PC_JOB.VATES:
                    case PC_JOB.DRUID:
                    case PC_JOB.BARD:
                    case PC_JOB.CARDINAL:
                    case PC_JOB.WARLOCK:
                    case PC_JOB.CABALIST:
                    case PC_JOB.NECROMANCER:
                    case PC_JOB.SOULTAKER:
                        return JobType.SPELLUSER;
                    case PC_JOB.TATARABE:
                    case PC_JOB.BLACKSMITH:
                    case PC_JOB.MACHINERY:
                    case PC_JOB.MAESTRO:
                    case PC_JOB.FARMASIST:
                    case PC_JOB.ALCHEMIST:
                    case PC_JOB.MARIONEST:
                    case PC_JOB.HARVEST:
                    case PC_JOB.RANGER:
                    case PC_JOB.EXPLORER:
                    case PC_JOB.TREASUREHUNTER:
                    case PC_JOB.STRIDER:
                    case PC_JOB.MERCHANT:
                    case PC_JOB.TRADER:
                    case PC_JOB.GAMBLER:
                    case PC_JOB.ROYALDEALER:
                        return JobType.BACKPACKER;
                    default:
                        return JobType.NOVICE;
                }
                /*
                int intJob = (int)this.job;
                if (intJob > 0 && intJob < 40)
                    return JobType.FIGHTER;
                else if (intJob > 40 && intJob < 80)
                    return JobType.SPELLUSER;
                else if (intJob > 80 && intJob < 120)
                    return JobType.BACKPACKER;
                else
                    return JobType.NOVICE;
                //*/
            }
        }

        public ushort DominionStr { get; set; }

        public ushort DominionDex { get; set; }

        public ushort DominionInt { get; set; }

        public ushort DominionVit { get; set; }

        public ushort DominionAgi { get; set; }

        public ushort DominionMag { get; set; }

        public long Gold
        {
            get => gold;
            set
            {
                if (value > 999999999999)
                    value = 999999999999;
                if (value < 0)
                    value = 0;
                if (value - gold != 0)
                {
                    if (gold != 0)
                    {
                        Logger.LogGoldChange(Name + "(" + CharID + ")", (int)(value - gold));
                        if (value > 100000)
                            Logger.LogGoldChange("[金钱异常收入！]:" + Name + "(" + CharID + ")", (int)(value - gold));
                    }

                    if (GoldLine + new TimeSpan(0, 0, 15, 0) > CPLine)
                        Goldlimit = 0;
                    Goldlimit += (int)(value - gold);
                    if (Goldlimit > 500000)
                        Logger.LogGoldChange("[Gold15分钟内收入过多警告！]:" + Name + "(" + CharID + ")", Goldlimit);
                    GoldLine = DateTime.Now;
                }

                var gg = (int)(value - gold);
                if (gg > 0)
                    CInt[Name + "角色收入"] += gg;
                else
                    CInt[Name + "角色支出"] += -gg;
                gold = value;
                if (e != null)
                    e.PropertyUpdate(UpdateEvent.GOLD, 0);
            }
        }

        public uint CP
        {
            get => cp;
            set
            {
                if (value > 99999999)
                    value = 99999999;
                var balance = (int)(value - cp);
                if (value - cp != 0)
                {
                    if (cp != 0)
                    {
                        Logger.LogGoldChange("[CP]:" + Name + "(" + CharID + ")", balance);
                        if (value > 5000)
                            Logger.LogGoldChange("[CP异常收入！]:" + Name + "(" + CharID + ")", balance);
                    }

                    if (CPLine + new TimeSpan(0, 0, 15, 0) > CPLine)
                        CPlimit = 0;
                    CPlimit += balance;
                    if (CPlimit > 30000)
                        Logger.LogGoldChange("[CP15分钟内收入过多警告！]:" + Name + "(" + CharID + ")", CPlimit);
                    CPLine = DateTime.Now;
                }

                cp = value;

                if (e != null)
                    e.PropertyUpdate(UpdateEvent.CP, balance);
            }
        }

        public uint ECoin
        {
            get => ecoin;
            set
            {
                if (value > 99999999)
                    value = 99999999;
                var balance = (int)(value - ecoin);

                ecoin = value;

                if (e != null)
                    e.PropertyUpdate(UpdateEvent.ECoin, balance);
            }
        }

        /// <summary>
        ///     玩家已消费的EP
        /// </summary>
        public short EPUsed { get; set; }

        /// <summary>
        ///     玩家在恶魔界已消费的EP
        /// </summary>
        public short DominionEPUsed { get; set; }

        /// <summary>
        ///     DEM族的Cost Limit
        /// </summary>
        public short CL { get; set; }

        /// <summary>
        ///     玩家恶魔界的Cost Limit
        /// </summary>
        public short DominionCL { get; set; }

        public ulong CEXP { get; set; }

        public ulong JEXP { get; set; }


        /// <summary>
        ///     冒险阶级经验值
        /// </summary>
        public ulong ExplorerEXP { get; set; }

        /// <summary>
        ///     恶魔界的基础经验值
        /// </summary>
        public ulong DominionCEXP { get; set; }

        /// <summary>
        ///     恶魔界的职业经验值
        /// </summary>
        public ulong DominionJEXP { get; set; }

        /// <summary>
        ///     联合职业经验值
        /// </summary>
        public ulong JointJEXP { get; set; }

        /// <summary>
        ///     WRP
        /// </summary>
        public int WRP
        {
            get => wrp;
            set
            {
                var balance = value - wrp;
                wrp = value;
                if (e != null)
                    e.PropertyUpdate(UpdateEvent.WRP, balance);
            }
        }

        /// <summary>
        ///     玩家在线时长
        /// </summary>
        public List<DateTime> TimeOnline { get; set; }

        /// <summary>
        ///     玩家是否在线
        /// </summary>
        public bool Online { get; set; }

        /// <summary>
        ///     记录点地图ID
        /// </summary>
        public uint SaveMap { get; set; }

        /// <summary>
        ///     记录点X坐标
        /// </summary>
        public byte SaveX { get; set; }

        /// <summary>
        ///     记录点Y坐标
        /// </summary>
        public byte SaveY { get; set; }

        /// <summary>
        ///     DEM形态
        /// </summary>
        public DEM_FORM Form { get; set; }

        /// <summary>
        ///     玩家是否处于战斗状态
        /// </summary>
        public byte BattleStatus { get; set; }

        /// <summary>
        ///     剩余人物属性点
        /// </summary>
        public ushort StatsPoint
        {
            get => statspoints;
            set
            {
                statspoints = value;
                if (e != null) e.PropertyUpdate(UpdateEvent.STAT_POINT, 0);
            }
        }

        /// <summary>
        ///     EX属性点
        /// </summary>
        public ushort EXStatPoint { get; set; }

        /// <summary>
        ///     EX技能点
        /// </summary>
        public byte EXSkillPoint { get; set; }

        /// <summary>
        ///     恶魔界的人物属性点
        /// </summary>
        public ushort DominionStatsPoint
        {
            get => dstatspoints;
            set
            {
                dstatspoints = value;
                if (e != null) e.PropertyUpdate(UpdateEvent.STAT_POINT, 0);
            }
        }

        /// <summary>
        ///     剩余1转技能点
        /// </summary>
        public ushort SkillPoint
        {
            get => skillpoint;
            set
            {
                skillpoint = value;
                if (e != null) e.PropertyUpdate(UpdateEvent.STAT_POINT, 0);
            }
        }

        /// <summary>
        ///     剩余2－1技能点
        /// </summary>
        public ushort SkillPoint2X
        {
            get => skillpoint2x;
            set
            {
                skillpoint2x = value;
                if (e != null) e.PropertyUpdate(UpdateEvent.STAT_POINT, 0);
            }
        }

        /// <summary>
        ///     剩余2－2技能点
        /// </summary>
        public ushort SkillPoint2T
        {
            get => skillpoint2t;
            set
            {
                skillpoint2t = value;
                if (e != null) e.PropertyUpdate(UpdateEvent.STAT_POINT, 0);
            }
        }

        /// <summary>
        ///     道具栏
        /// </summary>
        public Inventory Inventory { get; set; }

        /// <summary>
        ///     动作
        /// </summary>
        public MotionType Motion { get; set; }

        public bool MotionLoop { get; set; }

        /// <summary>
        ///     帐号专有字符串变量集
        /// </summary>
        public VariableHolder<string, string> AStr { get; private set; } = new VariableHolder<string, string>("");

        /// <summary>
        ///     帐号专有整数变量集
        /// </summary>
        public VariableHolder<string, int> AInt { get; private set; } = new VariableHolder<string, int>(0);

        /// <summary>
        ///     帐号专有长整数变量集
        /// </summary>
        public VariableHolder<string, long> ALong { get; } = new VariableHolder<string, long>(0);

        /// <summary>
        ///     人物专有字符串变量集
        /// </summary>
        public VariableHolder<string, string> CStr { get; private set; } = new VariableHolder<string, string>("");

        /// <summary>
        ///     人物专有整数变量集
        /// </summary>
        public VariableHolder<string, int> CInt { get; private set; } = new VariableHolder<string, int>(0);
        /// <summary>
        /// 临时字符串变量集
        /// </summary>
        //public VariableHolder<string, string> TStr { get { return this.tStrVar; } }
        /// <summary>
        /// 临时整数变量集
        /// </summary>
        //public VariableHolder<string, int> TInt { get { return this.tIntVar; } }

        /// <summary>
        ///     临时标识变量集
        /// </summary>
        public VariableHolderA<string, BitMask> TMask { get; private set; } = new VariableHolderA<string, BitMask>();

        /// <summary>
        ///     人物专有标识变量集
        /// </summary>
        public VariableHolderA<string, BitMask> CMask { get; private set; } = new VariableHolderA<string, BitMask>();

        /// <summary>
        ///     帐号专有标识变量集
        /// </summary>
        public VariableHolderA<string, BitMask> AMask { get; private set; } = new VariableHolderA<string, BitMask>();

        public VariableHolderA<string, DateTime> TTime { get; } = new VariableHolderA<string, DateTime>();

        public VariableHolderA<string, VariableHolderA<string, int>> Adict { get; } =
            new VariableHolderA<string, VariableHolderA<string, int>>();

        /// <summary>
        ///     人物专有双Int字典变量集
        /// </summary>
        public VariableHolderA<string, VariableHolderA<int, int>> CIDict { get; } =
            new VariableHolderA<string, VariableHolderA<int, int>>();

        //public EmotionType Emotion { get { return this.emotion; } set { this.emotion = value; } }

        /// <summary>
        ///     玩家目前的变身木偶形态
        /// </summary>
        public Marionette.Marionette Marionette { get; set; }

        /// <summary>
        ///     下一次可以使用变身木偶的时间
        /// </summary>
        public DateTime NextMarionetteTime { get; set; } = DateTime.Now;

        /// <summary>
        ///     玩家目前放出的宠物
        /// </summary>
        public ActorPet Pet { get; set; }

        /// <summary>
        ///     玩家目前放出的partner
        /// </summary>
        public ActorPartner Partner { get; set; }

        /// <summary>
        ///     凭依对象
        /// </summary>
        public uint PossessionTarget { get; set; }

        /// <summary>
        ///     凭依位置
        /// </summary>
        public PossessionPosition PossessionPosition { get; set; }

        /// <summary>
        ///     目前执行中的任务
        /// </summary>
        public Quest Quest { get; set; }

        /// <summary>
        ///     任务点下次重置的时间
        /// </summary>
        public DateTime QuestNextResetTime { get; set; }

        /// <summary>
        ///     EP登陆回复重置时间
        /// </summary>
        public DateTime EPLoginTime { get; set; }

        /// <summary>
        ///     EP打招呼回复重置时间
        /// </summary>
        public DateTime EPGreetingTime { get; set; }

        /// <summary>
        ///     声望
        /// </summary>
        public uint Fame
        {
            get => fame;
            set
            {
                if (value > int.MaxValue) fame = 0;
                else fame = value;
            }
        }

        /// <summary>
        ///     队伍
        /// </summary>
        public Party.Party Party { get; set; }

        /// <summary>
        ///     军团
        /// </summary>
        public Ring.Ring Ring { get; set; }

        /// <summary>
        ///     團隊
        /// </summary>
        public Team.Team Team { get; set; }

        /// <summary>
        ///     小組
        /// </summary>
        public Group.Group Group { get; set; }

        /// <summary>
        ///     看板
        /// </summary>
        public string Sign { get; set; }

        /// <summary>
        ///     玩家的模式
        /// </summary>
        public PlayerMode Mode
        {
            get => mode;
            set
            {
                mode = value;
                if (e != null) e.PropertyUpdate(UpdateEvent.MODE, 0);
            }
        }

        /// <summary>
        ///     玩家的飞空庭
        /// </summary>
        public FlyingGarden.FlyingGarden FlyingGarden { get; set; }

        /// <summary>
        ///     帐篷Actor
        /// </summary>
        public ActorEvent TenkActor { get; set; }


        /// <summary>
        ///     玩家在虚拟商城的点券
        /// </summary>
        public uint VShopPoints
        {
            get
            {
                if (e != null)
                    e.PropertyRead(UpdateEvent.VCASH_POINT);
                return vpoints;
            }
            set
            {
                vpoints = value;
                if (e != null)
                    e.PropertyUpdate(UpdateEvent.VCASH_POINT, 0);
            }
        }

        /// <summary>
        ///     玩家已用掉的点券
        /// </summary>
        public uint UsedVShopPoints
        {
            get
            {
                if (e != null)
                    e.PropertyRead(UpdateEvent.VCASH_POINT);
                return usedVPoints;
            }
            set
            {
                usedVPoints = value;
                if (e != null)
                    e.PropertyUpdate(UpdateEvent.VCASH_POINT, 0);
            }
        }

        /// <summary>
        ///     玩家目前活动中的石像
        /// </summary>
        public ActorGolem Golem { get; set; }

        /// <summary>
        ///     玩家创建的DungeonID
        /// </summary>
        public uint DungeonID { get; set; }

        /// <summary>
        ///     玩家收集的印章
        /// </summary>
        public Stamp Stamp { get; } = new Stamp();

        /// <summary>
        ///     是否解放恶魔界保留技能列表
        /// </summary>
        public bool DominionReserveSkill { get; set; }

        /// <summary>
        ///     WRP排行
        /// </summary>
        public uint WRPRanking { get; set; }

        /// <summary>
        ///     變身的圖片ID
        /// </summary>
        public uint TranceID
        {
            get => tranceID;
            set => tranceID = value;
        }

        /// <summary>
        ///     NPC显示/隐藏状态
        /// </summary>
        public Dictionary<uint, bool> NPCStates { get; } = new Dictionary<uint, bool>();


        public uint FurnitureID { get; set; }

        public uint FurnitureID_old { get; set; }

        /// <summary>
        ///     防御战窗口是否可见
        /// </summary>
        public bool DefWarShow
        {
            get => TInt["DefWarShow"] != 0;
            set
            {
                if (value)
                    TInt["DefWarShow"] = 1;
                else
                    TInt["DefWarShow"] = 0;
            }
        }

        /// <summary>
        ///     玩家借出的"心"
        /// </summary>
        public TamaireLending TamaireLending { get; set; }

        /// <summary>
        ///     玩家租用的"心"
        /// </summary>
        public TamaireRental TamaireRental { get; set; }

        /// <summary>
        ///     玩家儲存的奈落階層
        /// </summary>
        public int AbyssFloor { get; set; }

        /// <summary>
        ///     玩家的師父
        /// </summary>
        public uint Master { get; set; }

        /// <summary>
        ///     玩家的徒弟
        /// </summary>
        public List<uint> Pupilins { get; set; } = new List<uint>();

        /// <summary>
        ///     玩家的徒弟上限
        /// </summary>
        public byte PupilinLimit { get; set; } = 1;

        /// <summary>
        ///     玩家的Str
        /// </summary>
        public ushort Str { get; set; }

        public ushort Dex { get; set; }

        public ushort Int { get; set; }

        public ushort Vit { get; set; }

        public ushort Agi { get; set; }

        public ushort Mag { get; set; }

        /// <summary>
        ///     Reset PC's Iris Abilities
        /// </summary>
        public void ClearIrisAbilities()
        {
            IrisAbilityValues.Clear();
            IrisAbilityLevels.Clear();
        }

        /// <summary>
        ///     清楚所有变量集，玩家下线后用于释放资源
        /// </summary>
        public void ClearVarialbes()
        {
            AInt = null;
            AStr = null;
            AMask = null;
            CInt = null;
            CStr = null;
            CMask = null;
            tIntVar = null;
            tStrVar = null;
            TMask = null;
        }

        public class KillInfo
        {
            public bool isFinish = false;
            public int Count { set; get; }
            public int TotalCount { set; get; }
        }

        public class NPCHide
        {
            public int NPCID { set; get; }
            public byte state { set; get; }
        }

        /// <summary>
        ///     任务标记 (byte为列表ID)
        /// </summary>
        //public Navi.Navi Navi { get { return this.navi; } set { this.navi = value; } }

        //#region 商人商店部分..

        private readonly Dictionary<uint, PlayerShopItem> playershoplist = new Dictionary<uint, PlayerShopItem>();

        /// <summary>
        ///     玩家贩卖的道具
        /// </summary>
        public Dictionary<uint, PlayerShopItem> Playershoplist => playershoplist;

        //#endregion
    }
}