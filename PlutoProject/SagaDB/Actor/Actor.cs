using System;
using System.Collections.Generic;
using SagaLib;
using SagaLib.Tasks;

namespace SagaDB.Actor
{
    [Serializable]
    public class Actor
    {
        private byte _MuSoUCount;
        private byte _SwordACount;

        [NonSerialized]
        private Dictionary<AbnormalStatus, short> abnormalStatus = new Dictionary<AbnormalStatus, short>();

        [NonSerialized] private Dictionary<Elements, int> attackElements = new Dictionary<Elements, int>();

        /// <summary>
        ///     怪物的战斗开始时间，用于计算持续多久战斗，在Aggro时计时为DateTime.Now.
        /// </summary>
        public DateTime BattleStartTime;

        [NonSerialized] private Buff buff = new Buff();
        public bool castaway;
        private ushort dir;

        /// <summary>
        ///     Actor事件处理器
        /// </summary>
        public ActorEventHandler e;

        [NonSerialized] private Dictionary<Elements, int> elements = new Dictionary<Elements, int>();
        private uint hp, mp, sp, max_hp, max_mp, max_sp, ep, max_ep;
        private uint id;
        public uint IllusionPictID; //所有Actor都有的幻化pictID
        public bool invisble;
        private byte isseals, ishomicidal, homicidal, seals, darks, hotblade, hotblademark, plies; //圣印、暗刻、烈刃


        private int killingmarkcounter;

        private bool killingmarksouluse;
        private short lastX;
        private short lastY;
        private uint mapID;
        private string name;
        private bool noNewStatus;

        /// <summary>
        ///     外观
        /// </summary>
        public uint PictID;

        public Race Race;
        private uint range;
        public uint region;

        /// <summary>
        ///     该Actor的附属Actor，不会因为随从死亡而无法访问
        /// </summary>
        public List<Actor> SettledSlave = new List<Actor>();

        private uint shieldhp;
        public uint sightRange;
        public List<int> SkillCombo = new List<int>();

        public ActorSkill skillsong;
        [NonSerialized] private List<Actor> slaves = new List<Actor>();
        private short speed;
        private byte speedcut, attackRhythm;

        [NonSerialized] private Status status;

        [NonSerialized] private Dictionary<string, MultiRunTask> tasks = new Dictionary<string, MultiRunTask>();
        private VariableHolderA<string, BitMask> tMask = new VariableHolderA<string, BitMask>();

        public ActorType type;
        [NonSerialized] private List<uint> visibleActors = new List<uint>();
        private short x, y;
        private byte x2, y2;

        /// <summary>
        ///     不受极大化放大的技能ID
        /// </summary>
        public List<uint> ZenOutLst = new List<uint>();

        public Elements ShieldElement
        {
            get
            {
                var ele = SagaLib.Elements.Neutral;
                var atkvalue = 0;
                foreach (var item in Elements)
                    if (atkvalue < item.Value + Status.elements_item[item.Key] + Status.elements_skill[item.Key] +
                        Status.elements_iris[item.Key])
                    {
                        ele = item.Key;
                        atkvalue = item.Value + Status.elements_item[item.Key] + Status.elements_skill[item.Key] +
                                   Status.elements_iris[item.Key];
                    }

                return ele;
            }
        }

        public Elements WeaponElement
        {
            get
            {
                var ele = SagaLib.Elements.Neutral;
                var atkvalue = 0;
                foreach (var item in AttackElements)
                    if (atkvalue < item.Value + Status.attackElements_item[item.Key] +
                        Status.attackElements_skill[item.Key] + Status.attackelements_iris[item.Key])
                    {
                        ele = item.Key;
                        atkvalue = item.Value + Status.attackElements_item[item.Key] +
                                   Status.attackElements_skill[item.Key] + Status.attackelements_iris[item.Key];
                    }

                return ele;
            }
        }

        /// <summary>
        ///     临时字符串变量集
        /// </summary>
        public VariableHolder<string, string> TStr { get; } = new VariableHolder<string, string>("");

        /// <summary>
        ///     临时整数变量集
        /// </summary>
        public VariableHolder<string, int> TInt { get; } = new VariableHolder<string, int>(0);

        public VariableHolderA<string, DateTime> TTime { get; } = new VariableHolderA<string, DateTime>();

        /// <summary>
        ///     Actor的名称
        /// </summary>
        public string Name
        {
            get => name;
            set => name = value;
        }

        public short LastX
        {
            get => lastX;
            set => lastX = value;
        }

        public short LastY
        {
            get => lastY;
            set => lastY = value;
        }

        /// <summary>
        ///     此Actor在服务器存在的唯一标识ID
        /// </summary>
        public uint ActorID
        {
            get => id;
            set => id = value;
        }

        /// <summary>
        ///     Actor所在地图ID
        /// </summary>
        public uint MapID
        {
            get => mapID;
            set => mapID = value;
        }

        /// <summary>
        ///     等级
        /// </summary>
        public virtual byte Level
        {
            get => 0;
            set { }
        }

        /// <summary>
        ///     X坐标
        /// </summary>
        public byte X2
        {
            get => x2;
            set => x2 = value;
        }

        /// <summary>
        ///     Y坐标
        /// </summary>
        public byte Y2
        {
            get => y2;
            set => y2 = value;
        }

        /// <summary>
        ///     X坐标
        /// </summary>
        public short X
        {
            get => x;
            set => x = value;
        }

        /// <summary>
        ///     Y坐标
        /// </summary>
        public short Y
        {
            get => y;
            set => y = value;
        }

        /// <summary>
        ///     面向方向，0－360
        /// </summary>
        public ushort Dir
        {
            get => dir;
            set => dir = value;
        }

        /// <summary>
        ///     最终移动速度，请避免在技能和其他效果中直接对pc或者mob对象赋值（伪actor没有问题），如需要请赋值_item _iris _skill
        /// </summary>
        public ushort Speed
        {
            //似乎是移动速度判定

            //#region 试图的解锁，失败

            //get
            //{
            //    if (this.type == ActorType.MOB)
            //    {
            //        if (this.speed + this.Status.speed_item + this.Status.speed_iris + this.Status.speed_skill >= 0)
            //        {
            //            return (ushort)(this.speed + this.Status.speed_item + this.Status.speed_iris + this.Status.speed_skill);
            //        }
            //    }
            //    if (this.speed + this.Status.speed_item + this.Status.speed_iris + this.Status.speed_skill >= 0)
            //    {
            //        return (ushort)(this.speed + this.Status.speed_item + this.Status.speed_iris + this.Status.speed_skill);
            //    }
            //    else
            //    {
            //        return 0;
            //    }
            //}
            //set
            //{
            //    if (this.type == ActorType.MOB)
            //    {
            //        if (this.speed + this.Status.speed_item + this.Status.speed_iris + this.Status.speed_skill >= 0)
            //        {
            //            this.speed = (short)(this.speed + this.Status.speed_skill);
            //        }
            //    }
            //    if (this.speed + this.Status.speed_item + this.Status.speed_iris + this.Status.speed_skill >= 0)
            //    {
            //        this.speed = (short)(this.speed + this.Status.speed_item + this.Status.speed_iris + this.Status.speed_skill);
            //    }
            //    else
            //    {
            //        this.speed = 0;
            //    }
            //    //暂时锁值，因为容易卡速度
            //    this.speed = (short)value;
            //    if (e != null) e.PropertyUpdate(UpdateEvent.SPEED, 0);
            //}表现非常不正常的减速效果，待议先留着

            //#endregion

            get
            {
                //if(this.type == ActorType.MOB)
                //{
                //    if (this.speed + this.Status.speed_item + this.Status.speed_iris + this.Status.speed_skill >= 0)
                //    {
                //        return (ushort)(this.speed + this.Status.speed_item + this.Status.speed_iris + this.Status.speed_skill);
                //    }
                //}
                return (ushort)speed;
                /*if (this.speed + this.Status.speed_item + this.Status.speed_iris + this.Status.speed_skill >= 0)
                {
                    return (ushort)(this.speed + this.Status.speed_item + this.Status.speed_iris + this.Status.speed_skill);
                }
                else
                {
                    return 0;
                }*/ //暂时锁值，因为容易卡速度
            }
            set
            {
                speed = (short)value;
                if (e != null) e.PropertyUpdate(UpdateEvent.SPEED, 0);
            }
        }

        /// <summary>
        ///     圣印标记（0为不触发，1为触发）
        /// </summary>
        public byte IsSeals
        {
            get => isseals;
            set => isseals = value;
        }

        public byte IsHomicidal
        {
            get => ishomicidal;
            set => ishomicidal = value;
        }

        /// <summary>
        ///     冰棍层数
        /// </summary>
        public byte Plies
        {
            get => plies;
            set => plies = value;
        }

        /// <summary>
        ///     圣印层数
        /// </summary>
        public byte Seals
        {
            get => seals;
            set => seals = value;
        }

        /// <summary>
        ///     殺意层数
        /// </summary>
        public byte Homicidal
        {
            get => homicidal;
            set => homicidal = value;
        }

        /// <summary>
        ///     减速层数
        /// </summary>
        public byte SpeedCut
        {
            get => speedcut;
            set => speedcut = value;
        }

        public byte AttackRhythm
        {
            get => attackRhythm;
            set => attackRhythm = value;
        }

        /// <summary>
        ///     暗刻状态
        /// </summary>
        public byte Darks
        {
            get => darks;
            set => darks = value;
        }

        /// <summary>
        ///     烈刃标记
        /// </summary>
        public byte HotBladeMark
        {
            get => hotblademark;
            set => hotblademark = value;
        }

        /// <summary>
        ///     烈刃层数
        /// </summary>
        public byte HotBlade
        {
            get => hotblade;
            set => hotblade = value;
        }

        /// <summary>
        ///     无双斩击HIT数
        /// </summary>
        public byte MuSoUCount
        {
            get => _MuSoUCount;
            set => _MuSoUCount = value;
        }

        /// <summary>
        ///     利剑连袭HIT数
        /// </summary>
        public byte SwordACount
        {
            get => _SwordACount;
            set => _SwordACount = value;
        }

        /// <summary>
        ///     杀戮标记计数器
        /// </summary>
        public int KillingMarkCounter
        {
            get => killingmarkcounter;
            set => killingmarkcounter = value;
        }

        /// <summary>
        ///     是否零件使用的杀戮标记
        /// </summary>
        public bool KillingMarkSoulUse
        {
            get => killingmarksouluse;
            set => killingmarksouluse = value;
        }

        /// <summary>
        ///     护盾值（受伤优先扣除）
        /// </summary>
        public uint SHIELDHP
        {
            get => shieldhp;
            set => shieldhp = value;
        }

        /// <summary>
        ///     生命
        /// </summary>
        public uint HP
        {
            get => hp;
            set => hp = value;
        }

        /// <summary>
        ///     魔法
        /// </summary>
        public uint MP
        {
            get => mp;
            set => mp = value;
        }

        /// <summary>
        ///     体力
        /// </summary>
        public uint SP
        {
            get => sp;
            set => sp = value;
        }

        /// <summary>
        ///     最大生命
        /// </summary>
        public uint MaxHP
        {
            get => max_hp;
            set => max_hp = value;
        }

        /// <summary>
        ///     最大魔法
        /// </summary>
        public uint MaxMP
        {
            get => max_mp;
            set => max_mp = value;
        }

        /// <summary>
        ///     最大体力
        /// </summary>
        public uint MaxSP
        {
            get => max_sp;
            set => max_sp = value;
        }

        public uint EP
        {
            get => ep;
            set => ep = value;
        }

        public uint MaxEP
        {
            get => max_ep;
            set => max_ep = value;
        }

        /// <summary>
        ///     所有属性相关
        /// </summary>
        public Status Status
        {
            get
            {
                if (status == null && !noNewStatus)
                {
                    status = new Status(this);
                    noNewStatus = true;
                }

                return status;
            }
            set => status = value;
        }

        /// <summary>
        ///     射程
        /// </summary>
        public uint Range
        {
            get => range;
            set => range = value;
        }


        /// <summary>
        ///     该Actor执行中的系统任务
        /// </summary>
        public Dictionary<string, MultiRunTask> Tasks => tasks;

        /// <summary>
        ///     该Actor的附加状态
        /// </summary>
        public Buff Buff => buff;

        /// <summary>
        ///     此Actor可见的Actor
        /// </summary>
        public List<uint> VisibleActors => visibleActors;

        /// <summary>
        ///     防御元素属性值
        /// </summary>
        public Dictionary<Elements, int> Elements
        {
            get
            {
                if (elements.Count == 0)
                {
                    elements.Add(SagaLib.Elements.Neutral, 0);
                    elements.Add(SagaLib.Elements.Fire, 0);
                    elements.Add(SagaLib.Elements.Water, 0);
                    elements.Add(SagaLib.Elements.Wind, 0);
                    elements.Add(SagaLib.Elements.Earth, 0);
                    elements.Add(SagaLib.Elements.Holy, 0);
                    elements.Add(SagaLib.Elements.Dark, 0);
                }

                return elements;
            }
        }

        /// <summary>
        ///     攻击元素属性值
        /// </summary>
        public Dictionary<Elements, int> AttackElements
        {
            get
            {
                if (attackElements.Count == 0)
                {
                    attackElements.Add(SagaLib.Elements.Neutral, 0);
                    attackElements.Add(SagaLib.Elements.Fire, 0);
                    attackElements.Add(SagaLib.Elements.Water, 0);
                    attackElements.Add(SagaLib.Elements.Wind, 0);
                    attackElements.Add(SagaLib.Elements.Earth, 0);
                    attackElements.Add(SagaLib.Elements.Holy, 0);
                    attackElements.Add(SagaLib.Elements.Dark, 0);
                }

                return attackElements;
            }
        }

        public Dictionary<AbnormalStatus, short> AbnormalStatus
        {
            get
            {
                if (abnormalStatus.Count == 0)
                {
                    abnormalStatus.Add(SagaLib.AbnormalStatus.Confused, 0);
                    abnormalStatus.Add(SagaLib.AbnormalStatus.Frosen, 0);
                    abnormalStatus.Add(SagaLib.AbnormalStatus.Paralyse, 0);
                    abnormalStatus.Add(SagaLib.AbnormalStatus.Poisen, 0);
                    abnormalStatus.Add(SagaLib.AbnormalStatus.Silence, 0);
                    abnormalStatus.Add(SagaLib.AbnormalStatus.Sleep, 0);
                    abnormalStatus.Add(SagaLib.AbnormalStatus.Stone, 0);
                    abnormalStatus.Add(SagaLib.AbnormalStatus.Stun, 0);
                    abnormalStatus.Add(SagaLib.AbnormalStatus.MoveSpeedDown, 0);
                    abnormalStatus.Add(SagaLib.AbnormalStatus.SkillForbid, 0);
                }

                return abnormalStatus;
            }
        }

        /// <summary>
        ///     该Actor的附属Actor
        /// </summary>
        public List<Actor> Slave => slaves;

        /// <summary>
        ///     清除该Actor所有任务以及状态
        /// </summary>
        public void ClearTaskAddition()
        {
            foreach (var i in Tasks.Values)
                try
                {
                    i.Deactivate();
                }
                catch (Exception exception)
                {
                    Logger.ShowError(exception, null);
                }

            var additionlist = new Addition[Status.Additions.Count];
            Status.Additions.Values.CopyTo(additionlist, 0);
            foreach (var i in additionlist)
                try
                {
                    if (i.Activated) i.AdditionEnd();
                }
                catch
                {
                }

            Status.Additions.Clear();
            Tasks.Clear();
        }
    }
}