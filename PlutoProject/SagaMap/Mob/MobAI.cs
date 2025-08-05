using System;
using System.Collections.Generic;
using SagaDB.Actor;
using SagaLib;
using SagaMap.Manager;
using SagaMap.Mob.AICommands;

namespace SagaMap.Mob
{
    public enum Activity
    {
        IDLE,
        LAZY,
        BUSY
    }

    public partial class MobAI
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
                                if ((Math.Abs(Mob.X - X_Spawn) > 1000 || Math.Abs(Mob.Y - Y_Spawn) > 1000) &&
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
    }
}