using System;
using System.Collections.Generic;
using SagaLib;
using SagaMap.Manager;
using SagaMap.Mob.AICommands;
using SagaMap.Skill;
using SagaMap.Tasks.Mob;
using SagaMap.Tasks.Skill;

namespace SagaMap.Mob
{
    public enum Activity
    {
        IDLE,
        LAZY,
        BUSY
    }

    public class MobAI
    {
        public bool Activated;
        private Activity aiActivity = Activity.LAZY;

        public string Announce;
        public DateTime attackStamp = DateTime.Now;

        public DateTime BackTimer = DateTime.Now;

        /// <summary>
        ///     是否可以在战斗前随意移动
        /// </summary>
        public bool Cannotmovebeforefight = false;

        public Dictionary<string, AICommand> commands = new Dictionary<string, AICommand>();

        //伤害表，掉宝归属
        public Dictionary<uint, int> DamageTable = new Dictionary<uint, int>();
        public Actor firstAttacker;
        public Dictionary<uint, uint> Hate = new Dictionary<uint, uint>();

        /// <summary>
        ///     设置被技能锁定中，避免技能释放过程行动
        /// </summary>
        public bool locked;

        public Map map;
        private Actor master;
        public short MoveRange, X_Ori, Y_Ori, X_Spawn, Y_Spawn;
        public DateTime NextUpdateTime = DateTime.Now;
        private Dictionary<int, MapNode> openedNode = new Dictionary<int, MapNode>();
        public int period;
        public int SpawnDelay;
        public short X_pb, Y_pb;

        public MobAI(Actor mob, bool idle)
        {
            period = 1000; //process 1 command every second            
            Mob = mob;
            map = MapManager.Instance.GetMap(mob.MapID);
        }

        public MobAI(Actor mob)
        {
            period = 1000; //process 1 command every second            
            Mob = mob;
            map = MapManager.Instance.GetMap(mob.MapID);
            commands.Add("Attack", new Attack(this));
        }

        /// <summary>
        ///     AI的模式
        /// </summary>
        public AIMode Mode { get; set; }

        public Actor Master
        {
            get
            {
                if (master == null) return null;
                return master;
            }
            set => master = value;
        }

        public Activity AIActivity
        {
            get => aiActivity;
            set
            {
                aiActivity = value;
                if (Mob.Speed == 0)
                    return;
                if (value == Activity.BUSY)
                    period = 100000 / Mob.Speed;
                else if (value == Activity.LAZY)
                    period = 200000 / Mob.Speed;
                else if (value == Activity.IDLE) period = 1000;
            }
        }

        public Actor Mob { get; }

        public bool CanMove =>
            !(Mode.NoMove || Mob.Buff.CannotMove || Mob.Buff.Stun || Mob.Buff.Stone || Mob.Buff.Frosen ||
              Mob.Buff.Stiff || Mob.Tasks.ContainsKey("SkillCast"));

        public bool CanAttack => !(Mode.NoAttack || Mob.Buff.Stone || Mob.Buff.Stun || Mob.Buff.Frosen ||
                                   Mob.Tasks.ContainsKey("SkillCast"));

        public bool CanUseSkill
        {
            get
            {
                if (Mob.Buff.Silence)
                    return false;
                if (Global.Random.Next(0, 100) < 20)
                    return !(Mob.Buff.Stun || Mob.Buff.Stone || Mob.Buff.Frosen);
                return true;
            }
        }

        public void Start()
        {
            AIThread.Instance.RegisterAI(this);
            Hate.Clear(); //Hate table should be cleard at respawn
            //this.mob.Actor.BattleStatus.Status = new List<uint>();
            commands = new Dictionary<string, AICommand>();
            commands.Add("Attack", new Attack(this));
            AIActivity = Activity.LAZY;
            Activated = true;

            /*if (Skill.SkillHandler.Instance.isBossMob(this.Mob))
            {
                Tasks.Mob.MobRecover MobRecover = new SagaMap.Tasks.Mob.MobRecover((ActorMob)this.Mob);
                if (!this.Mob.Tasks.ContainsKey("MobRecover"))
                    this.Mob.Tasks.Add("MobRecover", MobRecover);
                MobRecover.Activate();
            }*/ ///关闭怪物回复线程以节省资源
        }

        public void Pause()
        {
            try
            {
                foreach (var i in commands.Keys) commands[i].Dispose();
                commands.Clear();
                Mob.VisibleActors.Clear();
                Mob.Status.attackingActors.Clear();
                lastAttacker = null;
                AIThread.Instance.RemoveAI(this);
                Activated = false;
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex, null);
            }
        }

        public void CallBack(object o)
        {
            var deletequeue = new List<string>();
            //ClientManager.EnterCriticalArea();
            try
            {
                string[] keys;
                if (locked)
                    return;
                if (Mob.Buff.Dead)
                    return;
                if (master != null)
                {
                    if (master.MapID != Mob.MapID)
                    {
                        Mob.e.OnDie();
                        return;
                    }

                    if (master.type == ActorType.PC)
                    {
                        var pc = (ActorPC)master;
                        if (!pc.Online)
                        {
                            Mob.e.OnDie();
                            return;
                        }
                    }
                }

                if (commands.Count == 1)
                    if (commands.ContainsKey("Attack"))
                        if (Hate.Count == 0)
                        {
                            AIActivity = Activity.IDLE;
                            if (Global.Random.Next(0, 99) < 10)
                            {
                                AIActivity = Activity.LAZY;
                                if ((Math.Abs((int)(Mob.X - X_Spawn)) > 1000 ||
                                     Math.Abs((int)(Mob.Y - Y_Spawn)) > 1000) &&
                                    MoveRange != 0)
                                {
                                    short x, y;
                                    var len = GetLengthD(X_Spawn, Y_Spawn, Mob.X, Mob.Y);
                                    x = (short)(Mob.X + (X_Spawn - Mob.X) / len * Mob.Speed);
                                    y = (short)(Mob.Y + (Y_Spawn - Mob.Y) / len * Mob.Speed);

                                    var mov = new Move(this, x, y);
                                    commands.Add("Move", mov);
                                }
                                else
                                {
                                    double x, y;
                                    byte _x, _y;
                                    var counter = 0;
                                    do
                                    {
                                        x = Global.Random.Next(-100, 100);
                                        y = Global.Random.Next(-100, 100);
                                        var len = GetLengthD(0, 0, (short)x, (short)y);
                                        x = x / len * 500;
                                        y = y / len * 500;
                                        x += Mob.X;
                                        y += Mob.Y;
                                        _x = Global.PosX16to8((short)x, map.Width);
                                        _y = Global.PosY16to8((short)y, map.Height);
                                        if (_x >= map.Width)
                                            _x = (byte)(map.Width - 1);
                                        if (_y >= map.Height)
                                            _y = (byte)(map.Height - 1);
                                        counter++;
                                    } while (map.Info.walkable[_x, _y] != 2 && counter < 1000);

                                    var mov = new Move(this, (short)x, (short)y);
                                    commands.Add("Move", mov);
                                }
                            }
                        }

                keys = new string[commands.Count];
                commands.Keys.CopyTo(keys, 0);
                var count = commands.Count;
                for (var i = 0; i < count; i++)
                    try
                    {
                        string j;
                        j = keys[i];
                        AICommand command;
                        commands.TryGetValue(j, out command);
                        if (command != null)
                        {
                            if (command.Status != CommandStatus.FINISHED && command.Status != CommandStatus.DELETING &&
                                command.Status != CommandStatus.PAUSED)
                                lock (command)
                                {
                                    command.Update(null);
                                }

                            if (command.Status == CommandStatus.FINISHED)
                            {
                                deletequeue.Add(j); //删除队列
                                command.Status = CommandStatus.DELETING;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.ShowError(ex);
                    }

                lock (commands)
                {
                    foreach (var i in deletequeue) commands.Remove(i);
                }
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex, null);
                Logger.ShowError(ex.StackTrace, null);
            }
            //ClientManager.LeaveCriticalArea();
        }

        public List<MapNode> FindPath(byte x, byte y, byte x2, byte y2)
        {
            var src = new MapNode();
            var now = DateTime.Now;
            var count = 0;
            src.x = x;
            src.y = y;
            src.F = 0;
            src.G = 0;
            src.H = 0;
            var path = new List<MapNode>();
            var current = src;
            if (x2 > map.Info.width - 1 || y2 > map.Info.height - 1)
            {
                path.Add(current);
                return path;
            }

            if (map.Info.walkable[x2, y2] != 2)
            {
                path.Add(current);
                return path;
            }

            if (x == x2 && y == y2)
            {
                path.Add(current);
                return path;
            }

            openedNode = new Dictionary<int, MapNode>();
            GetNeighbor(src, x2, y2);
            while (openedNode.Count != 0)
            {
                var shortest = new MapNode();
                shortest.F = int.MaxValue;
                if (count > 1000)
                    break;
                foreach (var i in openedNode.Values)
                {
                    if (i.x == x2 && i.y == y2)
                    {
                        openedNode.Clear();
                        shortest = i;
                        break;
                    }

                    if (i.F < shortest.F)
                        shortest = i;
                }

                current = shortest;
                if (openedNode.Count == 0)
                    break;
                openedNode.Remove(shortest.x * 1000 + shortest.y);
                current = GetNeighbor(shortest, x2, y2);
                count++;
            }

            while (current.Previous != null)
            {
                path.Add(current);
                current = current.Previous;
            }

            path.Reverse();
            return path;
        }

        private int GetPathLength(MapNode node)
        {
            var count = 0;
            var src = node;
            while (src.Previous != null)
            {
                count++;
                src = src.Previous;
            }

            return count;
        }

        public static int GetLength(byte x, byte y, byte x2, byte y2)
        {
            return (int)Math.Sqrt((x2 - x) * (x2 - x) + (y2 - y) * (y2 - y));
        }

        public static double GetLengthD(short x, short y, short x2, short y2)
        {
            return Math.Sqrt((x2 - x) * (x2 - x) + (y2 - y) * (y2 - y));
        }

        public static ushort GetDir(short x, short y)
        {
            var len = GetLengthD(0, 0, x, y);
            var degree = (int)(Math.Acos(y / len) / Math.PI * 180);
            if (x < 0)
                return (ushort)degree;
            return (ushort)(360 - degree);
        }

        private MapNode GetNeighbor(MapNode node, byte x, byte y)
        {
            var res = node;
            for (var i = node.x - 1; i <= node.x + 1; i++)
                for (var j = node.y - 1; j <= node.y + 1; j++)
                {
                    if (j == -1 || i == -1)
                        continue;
                    if (j == node.y && i == node.x)
                        continue;
                    if (i >= map.Info.width || j >= map.Info.height)
                        continue;
                    if (map.Info.walkable[i, j] == 2)
                    {
                        if (!openedNode.ContainsKey(i * 1000 + j))
                        {
                            var node2 = new MapNode();
                            node2.x = (byte)i;
                            node2.y = (byte)j;
                            node2.Previous = node;
                            if (i == node.x || j == node.y)
                                node2.G = node.G + 10;
                            else
                                node2.G = node.G + 14;
                            node2.H = Math.Abs(x - node2.x) * 10 + Math.Abs(y - node2.y) * 10;
                            node2.F = node2.G + node2.H;
                            openedNode.Add(i * 1000 + j, node2);
                        }
                        else
                        {
                            var tmp = openedNode[i * 1000 + j];
                            int G;
                            if (i == node.x || j == node.y)
                                G = 10;
                            else
                                G = 14;
                            if (node.G + G > tmp.G) res = tmp;
                        }
                    }
                }

            return res;
        }

        //新增部分开始 by:TT
        private readonly Dictionary<uint, DateTime> skillCast = new Dictionary<uint, DateTime>();
        private readonly List<int> skillOfHP = new List<int>();
        private bool CastIsFinished = true;
        public Actor lastAttacker;
        private DateTime longSkillTime = DateTime.Now;
        public float needlen = 1f;

        private bool NeedNewSkillList = true;
        public uint NextSurelySkillID;
        public bool noreturn = false;
        public bool notInitialize = false;
        private AIMode.SkillList Now_SkillList = new AIMode.SkillList();

        /// <summary>
        ///     AnAI的当前顺序
        /// </summary>
        private uint NowSequence = 0;

        private int Sequence;
        private DateTime shortSkillTime = DateTime.Now;
        private int SkillDelay;
        public bool skillisok;
        private bool skillOK;
        private DateTime SkillWait = DateTime.Now;
        private List<AIMode.SkillList> Temp_skillList = new List<AIMode.SkillList>();

        public DateTime LastSkillCast { get; set; } = DateTime.Now;

        public DateTime CannotAttack { get; set; } = DateTime.Now;

        public void SkillOfHPClear()
        {
            skillOfHP.Clear();
        }

        public void OnShouldCastSkill_An(AIMode mode, Actor currentTarget)
        {
            try
            {
                if (Mob.Tasks.ContainsKey("SkillCast"))
                    return;

                #region 根据条件抽选技能列表

                if (NeedNewSkillList)
                {
                    var totalRate = 0;
                    var determinator = 0;
                    Now_SkillList = new AIMode.SkillList();
                    Temp_skillList = new List<AIMode.SkillList>();
                    foreach (var item in mode.AnAI_SkillAssemblage)
                        if (Mob.HP >= item.Value.MinHP * Mob.HP / 100 && Mob.HP <= item.Value.MaxHP * Mob.HP / 100)
                        {
                            totalRate += item.Value.Rate;
                            Temp_skillList.Add(item.Value);
                        }

                    var ran = Global.Random.Next(0, totalRate);
                    foreach (var item in Temp_skillList)
                    {
                        determinator += item.Rate;
                        if (ran <= determinator)
                        {
                            Now_SkillList = item;
                            break;
                        }
                    }

                    NeedNewSkillList = false;
                    Sequence = 1;
                }

                #endregion

                #region 按照顺序释放技能

                foreach (var item in Now_SkillList.AnAI_SkillList)
                {
                    skillisok = false;
                    var skill = SkillFactory.Instance.GetSkill(item.Value.SkillID, 1);
                    if (GetLengthD(Mob.X, Mob.Y, currentTarget.X, currentTarget.Y) <= skill.Range * 145)
                    {
                        skillisok = true;
                    }
                    else
                    {
                        needlen = skill.Range;
                        needlen -= 1f;
                        if (needlen < 1f)
                            needlen = 1f;
                    }

                    if (item.Key >= Sequence)
                    {
                        //CannotAttack = DateTime.Now.AddMilliseconds(item.Value.Delay);
                        if (skillisok)
                        {
                            SkillDelay = item.Value.Delay;
                            CastIsFinished = false;
                            CastSkill(item.Value.SkillID, 1, currentTarget);
                            Sequence++;
                        }
                        else if (SkillWait <= DateTime.Now)
                        {
                            Sequence++;
                            SkillWait = DateTime.Now.AddSeconds(5);
                        }

                        break;
                    }

                    if (Sequence > Now_SkillList.AnAI_SkillList.Count)
                    {
                        NeedNewSkillList = true;
                        break;
                    }

                    if (Now_SkillList.AnAI_SkillList.Count == 0)
                    {
                        NeedNewSkillList = true;
                        break;
                    }
                }

                #endregion
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);
            }
        }

        public void OnShouldCastSkill_New(AIMode mode, Actor currentTarget)
        {
            if (Mob.Tasks.ContainsKey("SkillCast"))
                return;
            if (mode.SkillOfHP.Count > 0 && mode.SkillOfHP.Count > skillOfHP.Count)
            {
                uint id = 0;
                var hp = 0;
                foreach (var i in mode.SkillOfHP)
                    if (hp < i.Key && !skillOfHP.Contains(i.Key))
                    {
                        hp = i.Key;
                        id = i.Value;
                    }

                if (hp > 0)
                {
                    var mobHP = Mob.HP * 100 / Mob.MaxHP;
                    if (mobHP <= hp)
                    {
                        CastSkill(id, 1, currentTarget);
                        skillOfHP.Add(hp);
                        return;
                    }
                }
            }

            var len = GetLengthD(Mob.X, Mob.Y, currentTarget.X, currentTarget.Y) / 145;

            uint skillID = 0;
            var totalRate = 0;

            var temp_skillList = new Dictionary<uint, AIMode.SkilInfo>();
            var skillList = new Dictionary<uint, AIMode.SkilInfo>();
            var usedtime = (int)(DateTime.Now - Mob.BattleStartTime).TotalSeconds;

            if (mode.Distance < len && longSkillTime < DateTime.Now)
                temp_skillList = mode.SkillOfLong;
            //远程
            else if (shortSkillTime < DateTime.Now) temp_skillList = mode.SkillOfShort;
            //近身
            foreach (var i in temp_skillList)
            {
                var MaxHPLimit = (int)(Mob.MaxHP * (i.Value.MaxHP * 0.01f)) + 1;
                var MinHPLimit = (int)(Mob.MaxHP * (i.Value.MinHP * 0.01f)) + 1;
                if (MaxHPLimit >= Mob.HP && MinHPLimit <= Mob.HP)
                    if (usedtime > i.Value.OverTime)
                    {
                        if (skillCast.ContainsKey(i.Key))
                        {
                            if (skillCast[i.Key] < DateTime.Now)
                            {
                                skillList.Add(i.Key, i.Value);
                                skillCast.Remove(i.Key);
                            }
                        }
                        else
                        {
                            skillList.Add(i.Key, i.Value);
                        }
                    }
            }

            foreach (var i in skillList.Values) totalRate += i.Rate;
            var ran = 0;
            if (totalRate > 1)
                ran = Global.Random.Next(0, totalRate);
            var determinator = 0;

            foreach (var i in skillList.Keys)
            {
                determinator += skillList[i].Rate;
                if (ran <= determinator)
                {
                    skillID = i;
                    break;
                }
            }

            if (NextSurelySkillID != 0)
            {
                skillID = NextSurelySkillID;
                NextSurelySkillID = 0;
            }

            //释放技能
            if (skillID != 0)
            {
                CastSkill(skillID, 1, currentTarget);
                if (skillOK)
                {
                    try
                    {
                        skillCast.Add(skillID, DateTime.Now.AddSeconds(skillList[skillID].CD));
                    }
                    catch (Exception ex)
                    {
                        Logger.ShowError(ex);
                    }

                    longSkillTime = DateTime.Now.AddSeconds(mode.LongCD);
                    //远程
                    shortSkillTime = DateTime.Now.AddSeconds(mode.ShortCD);
                    //近身 CD在使用一个技能后同时增加。
                    skillOK = false;
                }
            }
        }
        //新增结束

        public void OnShouldCastSkill(Dictionary<uint, int> skillList, Actor currentTarget)
        {
            if (!Mob.Tasks.ContainsKey("SkillCast") && skillList.Count > 0)
            {
                //确定释放的技能
                uint skillID = 0;
                var totalRate = 0;
                foreach (var i in skillList.Values) totalRate += i;
                var ran = Global.Random.Next(0, totalRate);
                var determinator = 0;

                foreach (var i in skillList.Keys)
                {
                    determinator += skillList[i];
                    if (ran <= determinator)
                    {
                        skillID = i;
                        break;
                    }
                }

                //释放技能
                if (skillID != 0) CastSkill(skillID, 1, currentTarget);
            }
        }

        /// <summary>
        ///     检查是施放者能否施放技能或功擊
        /// </summary>
        /// <param name="sActor">攻击者</param>
        /// <param name="type">0=魔法功擊,1=物理功擊,2=技能施放</param>
        /// <returns></returns>
        public bool CheckStatusCanBeAttact(Actor sActor, int type)
        {
            switch (type)
            {
                case 0:
                    //Type 0 = Magic
                    //Slienced Confused Frozen Sleep stone stun paralyse
                    if (
                        sActor.Status.Additions.ContainsKey("Silence") ||
                        sActor.Status.Additions.ContainsKey("Confused") ||
                        sActor.Status.Additions.ContainsKey("Frosen") ||
                        sActor.Status.Additions.ContainsKey("Stone") ||
                        sActor.Status.Additions.ContainsKey("Stun") ||
                        sActor.Status.Additions.ContainsKey("Sleep") ||
                        sActor.Status.Additions.ContainsKey("Paralyse") ||
                        sActor.Status.Additions.ContainsKey("SkillForbid")
                    )
                        return false;
                    break;
                case 1: //Type 1 == Phy
                    //Confused Frozen Sleep stone stun paralyse +斷腕
                    if (
                        sActor.Status.Additions.ContainsKey("Confused") ||
                        sActor.Status.Additions.ContainsKey("Frosen") ||
                        sActor.Status.Additions.ContainsKey("Stone") ||
                        sActor.Status.Additions.ContainsKey("Stun") ||
                        sActor.Status.Additions.ContainsKey("Sleep") ||
                        sActor.Status.Additions.ContainsKey("Paralyse")
                    )
                        return false;
                    break;
                case 2:
                    //檢查能否施放
                    //Slienced Confused Frozen Sleep stone stun paralyse

                    if (
                        sActor.Status.Additions.ContainsKey("Silence") ||
                        sActor.Status.Additions.ContainsKey("Confused") ||
                        sActor.Status.Additions.ContainsKey("Frosen") ||
                        sActor.Status.Additions.ContainsKey("Stone") ||
                        sActor.Status.Additions.ContainsKey("Stun") ||
                        sActor.Status.Additions.ContainsKey("Sleep") ||
                        sActor.Status.Additions.ContainsKey("Paralyse") ||
                        sActor.Status.Additions.ContainsKey("SkillForbid")
                    )
                        return false;

                    break;
            }

            return true;
        }

        public void CastSkill(uint skillID, byte lv, uint target, short x, short y)
        {
            var skill = SkillFactory.Instance.GetSkill(skillID, lv);
            if (skill == null)
                return;
            if (!CanUseSkill)
                return;
            var arg = new SkillArg();
            arg.sActor = Mob.ActorID;

            if (!CheckStatusCanBeAttact(Mob, 2))
                return;


            if (target != 0xFFFFFFFF)
            {
                var dactor = map.GetActor(target);
                if (dactor == null)
                {
                    if (Mob.Tasks.ContainsKey("AutoCast"))
                    {
                        Mob.Tasks.Remove("AutoCast");
                        Mob.Buff.CannotMove = false;
                    }

                    return;
                }

                if (GetLengthD(Mob.X, Mob.Y, dactor.X, dactor.Y) <= Math.Abs(skill.Range) * 145)
                {
                    if (skill.Target == 2)
                    {
                        //如果是辅助技能
                        if (skill.Support)
                        {
                            if (Mob.type == ActorType.PET)
                            {
                                var pet = (ActorPet)Mob;
                                if (pet.Owner != null)
                                    arg.dActor = pet.Owner.ActorID;
                                else
                                    arg.dActor = Mob.ActorID;
                            }
                            else
                            {
                                if (master == null)
                                    arg.dActor = Mob.ActorID;
                                else
                                    arg.dActor = master.ActorID;
                            }
                        }
                        else
                        {
                            arg.dActor = target;
                        }
                    }
                    else if (skill.Target == 1)
                    {
                        if (Mob.type == ActorType.PET)
                        {
                            var pet = (ActorPet)Mob;
                            if (pet.Owner != null)
                                arg.dActor = pet.Owner.ActorID;
                            else
                                arg.dActor = Mob.ActorID;
                        }
                        else
                        {
                            arg.dActor = Mob.ActorID;
                        }
                    }
                    else
                    {
                        arg.dActor = 0xFFFFFFFF;
                    }

                    if (arg.dActor != 0xFFFFFFFF)
                    {
                        var dst = map.GetActor(arg.dActor);
                        if (dst != null)
                        {
                            if (dst.Buff.Dead != skill.DeadOnly)
                            {
                                if (Mob.Tasks.ContainsKey("AutoCast"))
                                {
                                    Mob.Tasks.Remove("AutoCast");
                                    Mob.Buff.CannotMove = false;
                                }

                                return;
                            }
                        }
                        else
                        {
                            if (Mob.Tasks.ContainsKey("AutoCast"))
                            {
                                Mob.Tasks.Remove("AutoCast");
                                Mob.Buff.CannotMove = false;
                            }

                            return;
                        }
                    }

                    if (master != null)
                        if (master.ActorID == target && !skill.Support)
                        {
                            if (Mob.Tasks.ContainsKey("AutoCast"))
                            {
                                Mob.Tasks.Remove("AutoCast");
                                Mob.Buff.CannotMove = false;
                            }

                            return;
                        }

                    arg.skill = skill;
                    arg.x = Global.PosX16to8(x, map.Width);
                    arg.y = Global.PosY16to8(y, map.Height);
                    arg.argType = SkillArg.ArgType.Cast;

                    arg.delay = (uint)(skill.CastTime *
                                       (1.0f - Math.Min(850,
                                           (Mob.Status.cspd + Mob.Status.cspd_skill) / 1000f))); //怪物技能吟唱时间
                }
                else
                {
                    if (Mob.Tasks.ContainsKey("AutoCast"))
                    {
                        Mob.Tasks.Remove("AutoCast");
                        Mob.Buff.CannotMove = false;
                    }

                    return;
                }
            }
            else
            {
                arg.dActor = 0xFFFFFFFF;
                if (GetLengthD(Mob.X, Mob.Y, x, y) <= skill.CastRange * 145)
                {
                    arg.skill = skill;
                    arg.x = Global.PosX16to8(x, map.Width);
                    arg.y = Global.PosY16to8(x, map.Height);
                    arg.argType = SkillArg.ArgType.Cast;

                    arg.delay = (uint)(skill.CastTime *
                                       (1.0f - Math.Min(850,
                                           (Mob.Status.cspd + Mob.Status.cspd_skill) / 1000f))); //怪物技能吟唱时间
                }
                else
                {
                    if (Mob.Tasks.ContainsKey("AutoCast"))
                    {
                        Mob.Tasks.Remove("AutoCast");
                        Mob.Buff.CannotMove = false;
                    }

                    return;
                }
            }

            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SKILL, arg, Mob, false);
            if (skill.CastTime > 0)
            {
                if (SkillHandler.Instance.MobskillHandlers.ContainsKey(arg.skill.ID))
                {
                    var dactor = map.GetActor(target);
                    SkillHandler.Instance.MobskillHandlers[arg.skill.ID].BeforeCast(Mob, dactor, arg, lv);

                    if (arg.result != 0)
                    {
                        skillOK = false;
                        return;
                    }
                }

                if (skill.BaseData.flag.Test(SkillFlags.NO_INTERRUPT))
                    Mob.TInt["CanNotInterrupted"] = 1;
                var task = new SkillCast(this, arg);
                Mob.Tasks.Add("SkillCast", task);

                task.Activate();
            }
            else
            {
                if (SkillHandler.Instance.MobskillHandlers.ContainsKey(arg.skill.ID))
                {
                    var dactor = map.GetActor(target);
                    SkillHandler.Instance.MobskillHandlers[arg.skill.ID].BeforeCast(Mob, dactor, arg, lv);

                    if (arg.result != 0)
                    {
                        skillOK = false;
                        return;
                    }
                }

                OnSkillCastComplete(arg);
            }

            skillOK = true;
        }

        public void CastSkill(uint skillID, byte lv, Actor currentTarget)
        {
            CastSkill(skillID, lv, currentTarget.ActorID, currentTarget.X, currentTarget.Y);
        }

        public void CastSkill(uint skillID, byte lv, short x, short y)
        {
            CastSkill(skillID, lv, 0xFFFFFFFF, x, y);
        }

        public void AttackActor(uint actorID)
        {
            if (Hate.ContainsKey(actorID))
                Hate[actorID] = Mob.MaxHP;
            else
                Hate.Add(actorID, Mob.MaxHP);
        }

        public Actor HighestActor()
        {
            try
            {
                uint id = 0;
                uint hate = 0;
                Actor tmp = null;
                var ids = new uint[Hate.Keys.Count];
                Hate.Keys.CopyTo(ids, 0);
                for (uint i = 0; i < Hate.Keys.Count; i++) //Find out the actorPC with the highest hate value
                {
                    if (ids[i] == 0) continue;
                    if (ids[i] == Mob.ActorID)
                        continue;
                    if (Master != null)
                        if (ids[i] == Master.ActorID && Hate.Count > 1)
                            continue;
                    if (!Hate.ContainsKey(ids[i]))
                        continue;
                    if (hate < Hate[ids[i]])
                    {
                        hate = Hate[ids[i]];
                        id = ids[i];
                        tmp = map.GetActor(id);
                        if (tmp == null)
                        {
                            Hate.Remove(id);
                            id = 0;
                            hate = 0;
                            i = 0;
                            continue;
                        }

                        if (tmp.Status.Additions.ContainsKey("Hiding"))
                        {
                            Hate.Remove(id);
                            continue;
                        }

                        if (tmp.Status.Additions.ContainsKey("Through"))
                        {
                            Hate.Remove(id);
                            continue;
                        }

                        if (tmp.Status.Additions.ContainsKey("IAmTree"))
                        {
                            Hate.Remove(id);
                            continue;
                        }

                        if (tmp.type == ActorType.PC && Mob.type != ActorType.PET)
                            if (((ActorPC)tmp).PossessionTarget != 0)
                            {
                                Hate.Remove(id);
                                id = 0;
                                hate = 0;
                                i = 0;
                            }
                    }
                }

                if (id != 0) //Now the id is refer to the PC with the highest hate to the Mob.现在这个ID是怪物对最高仇恨者的ID
                {
                    tmp = map.GetActor(id);
                    if (tmp != null)
                        if (tmp.HP == 0)
                        {
                            Hate.Remove(tmp.ActorID);
                            id = 0;
                        }
                }

                if (id == 0) return null;
                return tmp;
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);
                return null;
            }
        }

        public void StopAttacking()
        {
            Hate.Clear();
        }

        public void OnSkillCastComplete(SkillArg skill)
        {
            if (Mode.isAnAI)
            {
                CannotAttack = DateTime.Now.AddMilliseconds(SkillDelay);
                SkillDelay = 0;
                CastIsFinished = true;
            }

            if (skill.dActor != 0xFFFFFFFF)
            {
                var dActor = map.GetActor(skill.dActor);
                if (dActor != null)
                {
                    skill.argType = SkillArg.ArgType.Active;
                    SkillHandler.Instance.SkillCast(Mob, dActor, skill);
                }
            }
            else
            {
                skill.argType = SkillArg.ArgType.Active;
                SkillHandler.Instance.SkillCast(Mob, Mob, skill);
            }

            if (Mob.type == ActorType.PET)
                SkillHandler.Instance.ProcessPetGrowth(Mob, PetGrowthReason.UseSkill);
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SKILL, skill, Mob, false);
            if (skill.skill.Effect != 0)
            {
                var eff = new EffectArg();
                eff.actorID = skill.dActor;
                eff.effectID = skill.skill.Effect;
                eff.x = skill.x;
                eff.y = skill.y;
                map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SHOW_EFFECT, eff, Mob, false);
            }

            if (Mob.Tasks.ContainsKey("AutoCast"))
            {
                Mob.Tasks["AutoCast"].Activate();
            }
            else
            {
                if (skill.autoCast.Count != 0)
                {
                    Mob.Buff.CannotMove = true;
                    map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, Mob, true);
                    var task = new AutoCast(Mob, skill);
                    Mob.Tasks.Add("AutoCast", task);
                    task.Activate();
                }
            }
        }

        public void OnPathInterupt()
        {
            if (commands.ContainsKey("Move"))
            {
                var command = (Move)commands["Move"];
                command.FindPath();
            }

            if (commands.ContainsKey("Chase"))
            {
                var command = (Chase)commands["Chase"];
                command.FindPath();
            }
        }

        public void OnAttacked(Actor sActor, int damage)
        {
            if (Mob.Buff.Dead)
                return;
            if (Activated == false) Start();
            if (sActor.ActorID == Mob.ActorID)
                return;
            lastAttacker = sActor;
            var tmp = (uint)Math.Abs(damage);
            if (sActor.type == ActorType.PC)
                tmp = (uint)(tmp * sActor.Status.HateRate);

            var HateTargetId = sActor.ActorID; //误导状态下，这个ID将会是误导目标ID，否则为攻击者ID。

            if (sActor.Status.Additions.ContainsKey("误导") && sActor.TInt["误导"] != 0 &&
                map.GetActor((uint)sActor.TInt["误导"]) != null) //如果误导的目标存在的话。
                //这部分很可能需要更详细的逻辑，例如 误导的目标不能是partner，误导的目标必须可以被攻击等
                HateTargetId = (uint)sActor.TInt["误导"];

            if (Hate.ContainsKey(HateTargetId))
            {
                if (tmp == 0)
                    tmp = 1;
                Hate[HateTargetId] += tmp;
            }
            else
            {
                if (tmp == 0)
                    tmp = 1;
                if (Hate.Count == 0) //保存怪物战斗前位置
                {
                    Mob.BattleStartTime = DateTime.Now;
                    X_pb = Mob.X;
                    Y_pb = Mob.Y;
                }

                Hate.Add(HateTargetId, tmp);
            }

            if (damage > 0)
            {
                if (DamageTable.ContainsKey(sActor.ActorID))
                    DamageTable[sActor.ActorID] += damage;
                else DamageTable.Add(sActor.ActorID, damage);
                if (DamageTable[sActor.ActorID] > Mob.MaxHP)
                    DamageTable[sActor.ActorID] = (int)Mob.MaxHP;
            }

            var fa = sActor;
            if (sActor.type == ActorType.PARTNER)
                fa = ((ActorPartner)sActor).Owner;
            if (firstAttacker != null)
            {
                if (firstAttacker == fa)
                {
                    attackStamp = DateTime.Now;
                    if (firstAttacker.type != ActorType.GOLEM)
                        firstAttacker = fa;
                }
                else
                {
                    if ((DateTime.Now - attackStamp).TotalMinutes > 15)
                    {
                        firstAttacker = fa;
                        attackStamp = DateTime.Now;
                    }
                }
            }
            else
            {
                firstAttacker = fa;
                attackStamp = DateTime.Now;
            }
        }

        public void OnSeenSkillUse(SkillArg arg)
        {
            if (map == null)
            {
                Logger.ShowWarning(string.Format("Mob:{0}({1})'s map is null!", Mob.ActorID, Mob.Name));
                return;
            }

            if (master != null)
                for (var i = 0; i < arg.affectedActors.Count; i++)
                    if (arg.affectedActors[i].ActorID == master.ActorID)
                    {
                        var actor = map.GetActor(arg.sActor);
                        if (actor != null)
                        {
                            OnAttacked(actor, arg.hp[i]);
                            if (Hate.Count == 1)
                                SendAggroEffect();
                        }
                    }

            if (Mode.HelpSameType)
            {
                Actor actor;
                ActorMob mob;
                if (Mob.type == ActorType.MOB)
                {
                    mob = (ActorMob)Mob;
                    for (var i = 0; i < arg.affectedActors.Count; i++)
                    {
                        actor = arg.affectedActors[i];
                        if (actor.type == ActorType.MOB)
                        {
                            var tar = (ActorMob)actor;

                            if (tar.BaseData.mobType == mob.BaseData.mobType)
                            {
                                actor = map.GetActor(arg.sActor);
                                if (actor != null)
                                    if (actor.type == ActorType.PC)
                                    {
                                        if (Hate.Count == 0)
                                            SendAggroEffect();
                                        OnAttacked(actor, arg.hp[i]);
                                    }
                            }
                        }
                    }

                    actor = map.GetActor(arg.sActor);
                    if (actor != null)
                        if (actor.type == ActorType.MOB)
                        {
                            var tar = (ActorMob)actor;
                            if (tar.BaseData.mobType == mob.BaseData.mobType)
                                foreach (var i in arg.affectedActors)
                                {
                                    if (i.type != ActorType.PC)
                                        continue;
                                    if (Hate.Count == 0)
                                        SendAggroEffect();
                                    OnAttacked(i, 10);
                                }
                        }
                }
            }

            if (Mode.HateHeal)
            {
                var actor = map.GetActor(arg.sActor);
                if (actor != null && arg.skill != null && Hate.Count > 0)
                    if (arg.skill.Support && actor.type == ActorType.PC)
                    {
                        var damage = 0;
                        foreach (var i in arg.hp) damage += -i;
                        if (damage > 0)
                        {
                            if (Hate.Count == 0)
                                SendAggroEffect();
                            OnAttacked(actor, damage);
                        }
                    }
            }

            if (arg.skill != null)
            {
                if (arg.skill.Support && !Mode.HateHeal)
                {
                    var actor = map.GetActor(arg.sActor);
                    if (actor.type == ActorType.PC)
                    {
                        var damage = 0;
                        foreach (var i in arg.hp) damage += -i * 2;
                        if (DamageTable.ContainsKey(actor.ActorID))
                        {
                            DamageTable[actor.ActorID] += damage;
                            if (DamageTable[actor.ActorID] > Mob.MaxHP)
                                DamageTable[actor.ActorID] = (int)Mob.MaxHP;
                        }
                        //else this.DamageTable.Add(actor.ActorID, damage);
                    }
                }
                else if (arg.skill.ID == 3055) //复活
                {
                    var actor = map.GetActor(arg.sActor);
                    var dActor = map.GetActor(arg.dActor);
                    if (actor.type == ActorType.PC && dActor.type == ActorType.PC && actor != null && dActor != null)
                    {
                        var damage = 0;
                        damage = (int)dActor.MaxHP * 2;
                        if (DamageTable.ContainsKey(actor.ActorID))
                            DamageTable[actor.ActorID] += damage;
                        else DamageTable.Add(actor.ActorID, damage);
                        if (DamageTable[actor.ActorID] > Mob.MaxHP)
                            DamageTable[actor.ActorID] = (int)Mob.MaxHP;
                    }
                }
            }

            if (Mode.HateMagic)
            {
                var actor = map.GetActor(arg.sActor);
                if (actor != null && arg.skill != null)
                    if (arg.skill.Magical)
                        if (actor.type == ActorType.PC)
                        {
                            if (Hate.Count == 0)
                                SendAggroEffect();
                            OnAttacked(actor, (int)(Mob.MaxHP / 10));
                        }
            }
        }

        private void SendAggroEffect()
        {
            var arg = new EffectArg();
            arg.actorID = Mob.ActorID;
            arg.effectID = 4539;
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SHOW_EFFECT, arg, Mob, false);
        }
    }
}