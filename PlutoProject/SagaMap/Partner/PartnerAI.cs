using System;
using System.Collections.Generic;
using SagaDB.Actor;
using SagaDB.Skill;
using SagaLib;
using SagaMap.Manager;
using SagaMap.Partner.AICommands;
using SagaMap.Skill;
using SagaMap.Tasks.Partner;
using SagaMap.Tasks.Skill;

namespace SagaMap.Partner {
    public enum Activity {
        IDLE,
        LAZY,
        BUSY
    }

    public class PartnerAI {
        //新增部分开始 by:TT
        private readonly Dictionary<uint, DateTime> skillCast = new Dictionary<uint, DateTime>();
        private bool CastIsFinished = true;
        public Actor lastAttacker;
        private DateTime longSkillTime = DateTime.Now;
        public float needlen = 1f;

        private bool NeedNewSkillList = true;
        public uint NextSurelySkillID;
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

        public void OnShouldCastSkill_An(AIMode mode, Actor currentTarget) {
            try {
                if (Partner.Tasks.ContainsKey("SkillCast"))
                    return;

                //#region 根据条件抽选技能列表

                if (NeedNewSkillList) {
                    var totalRate = 0;
                    var determinator = 0;
                    Now_SkillList = new AIMode.SkillList();
                    Temp_skillList = new List<AIMode.SkillList>();
                    foreach (var item in mode.AnAI_SkillAssemblage)
                        if (Partner.HP >= item.Value.MinHP * Partner.HP / 100 &&
                            Partner.HP <= item.Value.MaxHP * Partner.HP / 100) {
                            totalRate += item.Value.Rate;
                            Temp_skillList.Add(item.Value);
                        }

                    var ran = Global.Random.Next(0, totalRate);
                    foreach (var item in Temp_skillList) {
                        determinator += item.Rate;
                        if (ran <= determinator) {
                            Now_SkillList = item;
                            break;
                        }
                    }

                    NeedNewSkillList = false;
                    Sequence = 1;
                }

                //#endregion

                //#region 按照顺序释放技能

                foreach (var item in Now_SkillList.AnAI_SkillList) {
                    skillisok = false;
                    var skill = SkillFactory.Instance.GetSkill(item.Value.SkillID, 1);
                    if (GetLengthD(Partner.X, Partner.Y, currentTarget.X, currentTarget.Y) <= skill.Range * 145) {
                        skillisok = true;
                    }
                    else {
                        needlen = skill.Range;
                        needlen -= 1f;
                        if (needlen < 1f)
                            needlen = 1f;
                    }

                    if (item.Key >= Sequence) {
                        //CannotAttack = DateTime.Now.AddMilliseconds(item.Value.Delay);
                        if (skillisok) {
                            SkillDelay = item.Value.Delay;
                            CastIsFinished = false;
                            CastSkill(item.Value.SkillID, 1, currentTarget);
                            Sequence++;
                        }
                        else if (SkillWait <= DateTime.Now) {
                            Sequence++;
                            SkillWait = DateTime.Now.AddSeconds(5);
                        }

                        break;
                    }

                    if (Sequence > Now_SkillList.AnAI_SkillList.Count) {
                        NeedNewSkillList = true;
                        break;
                    }

                    if (Now_SkillList.AnAI_SkillList.Count == 0) {
                        NeedNewSkillList = true;
                        break;
                    }
                }

                //#endregion
            }
            catch (Exception ex) {
                Logger.ShowError(ex);
            }
        }

        public void OnShouldCastSkill_New(AIMode mode, Actor currentTarget) {
            if (Partner.Tasks.ContainsKey("SkillCast"))
                return;
            var len = GetLengthD(Partner.X, Partner.Y, currentTarget.X, currentTarget.Y) / 145;

            uint skillID = 0;
            var totalRate = 0;

            var temp_skillList = new Dictionary<uint, AIMode.SkilInfo>();
            var skillList = new Dictionary<uint, AIMode.SkilInfo>();

            if (mode.Distance < len && longSkillTime < DateTime.Now)
                temp_skillList = mode.SkillOfLong;
            //远程
            else if (shortSkillTime < DateTime.Now) temp_skillList = mode.SkillOfShort;
            //近身
            foreach (var i in temp_skillList) {
                var MaxHPLimit = (int)(Partner.MaxHP * (i.Value.MaxHP * 0.01f)) + 1;
                var MinHPLimit = (int)(Partner.MaxHP * (i.Value.MinHP * 0.01f)) + 1;
                if (MaxHPLimit >= Partner.HP && MinHPLimit <= Partner.HP) {
                    if (skillCast.ContainsKey(i.Key)) {
                        if (skillCast[i.Key] < DateTime.Now) {
                            skillList.Add(i.Key, i.Value);
                            skillCast.Remove(i.Key);
                        }
                    }
                    else {
                        skillList.Add(i.Key, i.Value);
                    }
                }
            }

            foreach (var i in skillList.Values) totalRate += i.Rate;
            var ran = 0;
            if (totalRate > 1)
                ran = Global.Random.Next(0, totalRate);
            var determinator = 0;

            foreach (var i in skillList.Keys) {
                determinator += skillList[i].Rate;
                if (ran <= determinator) {
                    skillID = i;
                    break;
                }
            }

            if (NextSurelySkillID != 0) {
                skillID = NextSurelySkillID;
                NextSurelySkillID = 0;
            }

            //释放技能
            if (skillID != 0) {
                CastSkill(skillID, 1, currentTarget);
                if (skillOK) {
                    try {
                        skillCast.Add(skillID, DateTime.Now.AddSeconds(skillList[skillID].CD));
                    }
                    catch {
                    }

                    if (mode.Distance < len)
                        longSkillTime = DateTime.Now.AddSeconds(mode.LongCD);
                    //远程
                    else
                        shortSkillTime = DateTime.Now.AddSeconds(mode.ShortCD);
                    //近身
                    skillOK = false;
                }
            }
        }
        //新增结束

        public void OnShouldCastSkill(Dictionary<uint, int> skillList, Actor currentTarget) {
            if (!Partner.Tasks.ContainsKey("SkillCast") && skillList.Count > 0) {
                //确定释放的技能
                uint skillID = 0;
                var totalRate = 0;
                foreach (var i in skillList.Values) totalRate += i;
                var ran = Global.Random.Next(0, totalRate);
                var determinator = 0;

                foreach (var i in skillList.Keys) {
                    determinator += skillList[i];
                    if (ran <= determinator) {
                        skillID = i;
                        break;
                    }
                }

                //释放技能
                if (skillID != 0)
                    //Partner.TTime["攻击僵直"] = DateTime.Now + new TimeSpan(0, 0, 0, 3);
                    CastSkill(skillID, 1, currentTarget);
            }
        }

        public void CastSkill(uint skillID, byte lv, uint target, short x, short y) {
            var skill = SkillFactory.Instance.GetSkill(skillID, lv);
            if (skill == null)
                return;
            if (!CanUseSkill)
                return;
            var arg = new SkillArg();
            arg.sActor = Partner.ActorID;

            if (target != 0xFFFFFFFF) {
                var dactor = map.GetActor(target);
                if (dactor == null) {
                    if (Partner.Tasks.ContainsKey("AutoCast")) {
                        Partner.Tasks.Remove("AutoCast");
                        Partner.Buff.CannotMove = false;
                    }

                    return;
                }

                if (GetLengthD(Partner.X, Partner.Y, dactor.X, dactor.Y) <= skill.Range * 145) {
                    if (skill.Target == 2) {
                        //如果是辅助技能
                        if (skill.Support) {
                            if (Partner.type == ActorType.PET) {
                                var pet = (ActorPet)Partner;
                                if (pet.Owner != null)
                                    arg.dActor = pet.Owner.ActorID;
                                else
                                    arg.dActor = Partner.ActorID;
                            }
                            else {
                                if (Master == null)
                                    arg.dActor = Partner.ActorID;
                                else
                                    arg.dActor = Master.ActorID;
                            }
                        }
                        else {
                            arg.dActor = target;
                        }
                    }
                    else if (skill.Target == 1) {
                        if (Partner.type == ActorType.PET) {
                            var pet = (ActorPet)Partner;
                            if (pet.Owner != null)
                                arg.dActor = pet.Owner.ActorID;
                            else
                                arg.dActor = Partner.ActorID;
                        }
                        else {
                            arg.dActor = Partner.ActorID;
                        }
                    }
                    else {
                        arg.dActor = 0xFFFFFFFF;
                    }

                    if (arg.dActor != 0xFFFFFFFF) {
                        var dst = map.GetActor(arg.dActor);
                        if (dst != null) {
                            if (dst.Buff.Dead != skill.DeadOnly) {
                                if (Partner.Tasks.ContainsKey("AutoCast")) {
                                    Partner.Tasks.Remove("AutoCast");
                                    Partner.Buff.CannotMove = false;
                                }

                                return;
                            }
                        }
                        else {
                            if (Partner.Tasks.ContainsKey("AutoCast")) {
                                Partner.Tasks.Remove("AutoCast");
                                Partner.Buff.CannotMove = false;
                            }

                            return;
                        }
                    }

                    if (Master != null)
                        if (Master.ActorID == target && !skill.Support) {
                            if (Partner.Tasks.ContainsKey("AutoCast")) {
                                Partner.Tasks.Remove("AutoCast");
                                Partner.Buff.CannotMove = false;
                            }

                            return;
                        }

                    arg.skill = skill;
                    arg.x = Global.PosX16to8(x, map.Width);
                    arg.y = Global.PosY16to8(y, map.Height);
                    arg.argType = SkillArg.ArgType.Cast;

                    //arg.delay = (uint)(skill.CastTime * (1f - this.Mob.Status.cspd / 1000f));//怪物技能吟唱时间
                    arg.delay = (uint)skill.CastTime;
                }
                else if (skill.Range == -1) {
                    if (skill.Target == 2) {
                        //如果是辅助技能
                        if (skill.Support) {
                            if (Partner.type == ActorType.PET) {
                                var pet = (ActorPet)Partner;
                                if (pet.Owner != null)
                                    arg.dActor = pet.Owner.ActorID;
                                else
                                    arg.dActor = Partner.ActorID;
                            }
                            else {
                                if (Master == null)
                                    arg.dActor = Partner.ActorID;
                                else
                                    arg.dActor = Master.ActorID;
                            }
                        }
                        else {
                            arg.dActor = target;
                        }
                    }
                    else if (skill.Target == 1) {
                        if (Partner.type == ActorType.PET) {
                            var pet = (ActorPet)Partner;
                            if (pet.Owner != null)
                                arg.dActor = pet.Owner.ActorID;
                            else
                                arg.dActor = Partner.ActorID;
                        }
                        else {
                            arg.dActor = Partner.ActorID;
                        }
                    }
                    else {
                        arg.dActor = 0xFFFFFFFF;
                    }

                    if (arg.dActor != 0xFFFFFFFF) {
                        var dst = map.GetActor(arg.dActor);
                        if (dst != null) {
                            if (dst.Buff.Dead != skill.DeadOnly) {
                                if (Partner.Tasks.ContainsKey("AutoCast")) {
                                    Partner.Tasks.Remove("AutoCast");
                                    Partner.Buff.CannotMove = false;
                                }

                                return;
                            }
                        }
                        else {
                            if (Partner.Tasks.ContainsKey("AutoCast")) {
                                Partner.Tasks.Remove("AutoCast");
                                Partner.Buff.CannotMove = false;
                            }

                            return;
                        }
                    }

                    if (Master != null)
                        if (Master.ActorID == target && !skill.Support) {
                            if (Partner.Tasks.ContainsKey("AutoCast")) {
                                Partner.Tasks.Remove("AutoCast");
                                Partner.Buff.CannotMove = false;
                            }

                            return;
                        }

                    arg.skill = skill;
                    arg.x = Global.PosX16to8(x, map.Width);
                    arg.y = Global.PosY16to8(y, map.Height);
                    arg.argType = SkillArg.ArgType.Cast;

                    //arg.delay = (uint)(skill.CastTime * (1f - this.Mob.Status.cspd / 1000f));//怪物技能吟唱时间
                    arg.delay = (uint)skill.CastTime;
                }
                else {
                    if (Partner.Tasks.ContainsKey("AutoCast")) {
                        Partner.Tasks.Remove("AutoCast");
                        Partner.Buff.CannotMove = false;
                    }

                    return;
                }
            }
            else {
                arg.dActor = 0xFFFFFFFF;
                if (GetLengthD(Partner.X, Partner.Y, x, y) <= skill.CastRange * 145) {
                    arg.skill = skill;
                    arg.x = Global.PosX16to8(x, map.Width);
                    arg.y = Global.PosY16to8(x, map.Height);
                    arg.argType = SkillArg.ArgType.Cast;

                    //arg.delay = (uint)(skill.CastTime * (1f - this.Mob.Status.cspd / 1000f));
                    arg.delay = (uint)skill.CastTime;
                }
                else {
                    if (Partner.Tasks.ContainsKey("AutoCast")) {
                        Partner.Tasks.Remove("AutoCast");
                        Partner.Buff.CannotMove = false;
                    }

                    return;
                }
            }

            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SKILL, arg, Partner, false);
            if (skill.CastTime > 0) {
                if (SkillHandler.Instance.MobskillHandlers.ContainsKey(arg.skill.ID)) {
                    var dactor = map.GetActor(target);
                    SkillHandler.Instance.MobskillHandlers[arg.skill.ID].BeforeCast(Partner, dactor, arg, lv);
                }

                var task = new SkillCast(this, arg);
                Partner.Tasks.Add("SkillCast", task);

                task.Activate();
            }
            else {
                OnSkillCastComplete(arg);
            }

            skillOK = true;
        }

        public void CastSkill(uint skillID, byte lv, Actor currentTarget) {
            CastSkill(skillID, lv, currentTarget.ActorID, currentTarget.X, currentTarget.Y);
        }

        public void CastSkill(uint skillID, byte lv, short x, short y) {
            CastSkill(skillID, lv, 0xFFFFFFFF, x, y);
        }

        public void AttackActor(uint actorID) {
            if (Hate.ContainsKey(actorID))
                Hate[actorID] = Partner.MaxHP;
            else
                Hate.Add(actorID, Partner.MaxHP);
        }

        public void StopAttacking() {
            Hate.Clear();
        }

        public void OnSkillCastComplete(SkillArg skill) {
            if (Mode.isAnAI) {
                CannotAttack = DateTime.Now.AddMilliseconds(SkillDelay);
                SkillDelay = 0;
                CastIsFinished = true;
            }

            if (skill.dActor != 0xFFFFFFFF) {
                var dActor = map.GetActor(skill.dActor);
                if (dActor != null) {
                    skill.argType = SkillArg.ArgType.Active;
                    SkillHandler.Instance.SkillCast(Partner, dActor, skill);
                }
            }
            else {
                skill.argType = SkillArg.ArgType.Active;
                SkillHandler.Instance.SkillCast(Partner, Partner, skill);
            }

            if (Partner.type == ActorType.PET)
                SkillHandler.Instance.ProcessPetGrowth(Partner, PetGrowthReason.UseSkill);
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SKILL, skill, Partner, false);
            if (skill.skill.Effect != 0) {
                var eff = new EffectArg();
                eff.actorID = skill.dActor;
                eff.effectID = skill.skill.Effect;
                eff.x = skill.x;
                eff.y = skill.y;
                map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SHOW_EFFECT, eff, Partner, false);
            }

            Partner.TTime["攻击僵直"] = DateTime.Now + new TimeSpan(0, 0, 0, 0, 500);
            if (Partner.Tasks.ContainsKey("AutoCast")) {
                Partner.Tasks["AutoCast"].Activate();
            }
            else {
                if (skill.autoCast.Count != 0) {
                    Partner.Buff.CannotMove = true;
                    map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, Partner, true);
                    var task = new AutoCast(Partner, skill);
                    Partner.Tasks.Add("AutoCast", task);
                    task.Activate();
                }
            }
        }

        public void OnPathInterupt() {
            if (commands.ContainsKey("Move")) {
                var command = (Move)commands["Move"];
                command.FindPath();
            }

            if (commands.ContainsKey("Chase")) {
                var command = (Chase)commands["Chase"];
                command.FindPath();
            }
        }

        public void OnAttacked(Actor sActor, int damage) {
            if (Partner.Buff.Dead)
                return;
            if (Activated == false) Start();
            if (sActor.ActorID == Partner.ActorID)
                return;
            lastAttacker = sActor;
            var tmp = (uint)Math.Abs(damage);
            if (sActor.type == ActorType.PC) {
                if (((ActorPC)sActor).Skills2.ContainsKey(612)) {
                    var rate_add = 0.1f * ((ActorPC)sActor).Skills2[612].Level;
                    tmp += (uint)(tmp * rate_add);
                }

                if (sActor.Status.Nooheito_rate > 0) //弓3转13级技能
                    tmp -= (uint)(tmp * (sActor.Status.Nooheito_rate / 100));
                if (sActor.Status.HatredUp_rate > 0) //骑士3转13级技能
                    tmp += (uint)(tmp * (sActor.Status.HatredUp_rate / 100));
            }

            if (Hate.ContainsKey(sActor.ActorID)) {
                if (tmp == 0)
                    tmp = 1;
                Hate[sActor.ActorID] += tmp;
            }
            else {
                if (tmp == 0)
                    tmp = 1;
                if (Hate.Count == 0) //保存怪物战斗前位置
                {
                    X_pb = Partner.X;
                    Y_pb = Partner.Y;
                }

                Hate.Add(sActor.ActorID, tmp);
            }

            if (damage > 0) {
                if (DamageTable.ContainsKey(sActor.ActorID))
                    DamageTable[sActor.ActorID] += damage;
                else DamageTable.Add(sActor.ActorID, damage);
                if (DamageTable[sActor.ActorID] > Partner.MaxHP)
                    DamageTable[sActor.ActorID] = (int)Partner.MaxHP;
            }

            if (firstAttacker != null) {
                if (firstAttacker == sActor) {
                    attackStamp = DateTime.Now;
                }
                else {
                    if ((DateTime.Now - attackStamp).TotalMinutes > 15) {
                        firstAttacker = sActor;
                        attackStamp = DateTime.Now;
                    }
                }
            }
            else {
                firstAttacker = sActor;
                attackStamp = DateTime.Now;
            }
        }

        public void OnSeenSkillUse(SkillArg arg) {
            if (map == null) {
                Logger.GetLogger().Warning($"Mob:{Partner.ActorID}({Partner.Name})'s map is null!");
                return;
            }

            if (Master != null) {
                for (var i = 0; i < arg.affectedActors.Count; i++) {
                    if (arg.affectedActors[i].ActorID != Master.ActorID) {
                        continue;
                    }

                    var actor = map.GetActor(arg.sActor);
                    if (actor == null) {
                        continue;
                    }

                    OnAttacked(actor, arg.hp[i]);
                    if (Hate.Count == 1) {
                        SendAggroEffect();
                    }
                }
            }

            if (Mode.HateHeal) {
                var actor = map.GetActor(arg.sActor);
                if (actor != null && arg.skill != null && Hate.Count > 0 && arg.skill.Support &&
                    actor.type == ActorType.PC) {
                    var damage = 0;
                    foreach (var i in arg.hp) {
                        damage += -i;
                    }

                    if (damage > 0) {
                        if (Hate.Count == 0) {
                            SendAggroEffect();
                        }

                        OnAttacked(actor, damage);
                    }
                }
            }

            if (arg.skill != null) {
                if (arg.skill.Support && !Mode.HateHeal) {
                    var actor = map.GetActor(arg.sActor);
                    if (actor.type == ActorType.PC) {
                        var damage = 0;
                        foreach (var i in arg.hp) damage += -i * 2;
                        if (DamageTable.ContainsKey(actor.ActorID)) {
                            DamageTable[actor.ActorID] += damage;
                        }
                        else {
                            DamageTable.Add(actor.ActorID, damage);
                        }

                        if (DamageTable[actor.ActorID] > Partner.MaxHP) {
                            DamageTable[actor.ActorID] = (int)Partner.MaxHP;
                        }
                    }
                }
                else if (arg.skill.ID == 3055) //复活
                {
                    var actor = map.GetActor(arg.sActor);
                    var dActor = map.GetActor(arg.dActor);
                    if (actor.type == ActorType.PC && dActor.type == ActorType.PC && actor != null && dActor != null) {
                        var damage = (int)dActor.MaxHP * 2;
                        if (DamageTable.ContainsKey(actor.ActorID)) {
                            DamageTable[actor.ActorID] += damage;
                        }
                        else {
                            DamageTable.Add(actor.ActorID, damage);
                        }

                        if (DamageTable[actor.ActorID] > Partner.MaxHP) {
                            DamageTable[actor.ActorID] = (int)Partner.MaxHP;
                        }
                    }
                }
            }

            if (Mode.HateMagic) {
                var actor = map.GetActor(arg.sActor);
                if (actor != null && arg.skill != null && arg.skill.Magical && actor.type == ActorType.PC) {
                    if (Hate.Count == 0) {
                        SendAggroEffect();
                    }

                    OnAttacked(actor, (int)(Partner.MaxHP / 10));
                }
            }
        }

        private void SendAggroEffect() {
            var arg = new EffectArg {
                actorID = Partner.ActorID,
                effectID = 4539
            };
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SHOW_EFFECT, arg, Partner, false);
        }

        public bool Activated;
        private Activity aiActivity = Activity.LAZY;

        public string Announce;
        public DateTime attackStamp = DateTime.Now;

        public DateTime BackTimer = DateTime.Now;
        public Dictionary<string, AICommand> commands = new Dictionary<string, AICommand>();

        //伤害表，掉宝归属
        public Dictionary<uint, int> DamageTable = new Dictionary<uint, int>();
        public Actor firstAttacker;
        public Dictionary<uint, uint> Hate = new Dictionary<uint, uint>();
        public Map map;
        public short MoveRange, X_Ori, Y_Ori, X_Spawn, Y_Spawn;
        public DateTime NextUpdateTime = DateTime.Now;
        private Dictionary<int, MapNode> openedNode = new Dictionary<int, MapNode>();
        public int Period { set; get; }
        public int SpawnDelay;
        public short X_pb, Y_pb;

        public PartnerAI(ActorPartner partner, bool idle) {
            Period = 1000; //process 1 command every second            
            Partner = partner;
            map = MapManager.Instance.GetMap(partner.MapID);
        }

        public PartnerAI(ActorPartner partner) {
            Period = 1000; //process 1 command every second            
            Partner = partner;
            map = MapManager.Instance.GetMap(Partner.MapID);
            //this.commands.Add("Attack", new AICommands.Attack(this));
        }

        /// <summary>
        ///     AI的模式
        /// </summary>
        public AIMode Mode { get; set; }

        public Actor Master { get; set; }

        public Activity AIActivity {
            get => aiActivity;
            set {
                aiActivity = value;
                if (Partner.Speed == 0) {
                    return;
                }

                switch (value) {
                    case Activity.BUSY:
                        Period = 100000 / Partner.Speed;
                        break;
                    case Activity.LAZY:
                        Period = 200000 / Partner.Speed;
                        break;
                    case Activity.IDLE:
                        Period = 1000;
                        break;
                }
            }
        }

        public Actor Partner { get; }

        public bool CanMove =>
            !(Mode.NoMove || Partner.Buff.CannotMove || Partner.Buff.Stun || Partner.Buff.Stone ||
              Partner.Buff.Frosen ||
              Partner.Buff.Stiff || Partner.Tasks.ContainsKey("SkillCast"));

        public bool CanAttack => !(Mode.NoAttack || Partner.Buff.Stone || Partner.Buff.Stun || Partner.Buff.Frosen ||
                                   Partner.Tasks.ContainsKey("SkillCast"));

        public bool CanUseSkill =>
            !(Partner.Buff.Silence || Partner.Buff.Stun || Partner.Buff.Stone || Partner.Buff.Frosen);

        public void Start() {
            AIThread.Instance.RegisterAI(this);
            Hate.Clear(); //Hate table should be cleard at respawn
            //this.mob.Actor.BattleStatus.Status = new List<uint>();
            commands = new Dictionary<string, AICommand>();
            commands.Add("Attack", new Attack(this));
            AIActivity = Activity.LAZY;
            Activated = true;
        }

        public void Pause() {
            try {
                foreach (var i in commands.Keys) {
                    commands[i].Dispose();
                }

                commands.Clear();
                Partner.VisibleActors.Clear();
                Partner.Status.attackingActors.Clear();
                lastAttacker = null;
                AIThread.Instance.RemoveAI(this);
                Activated = false;
            }
            catch (Exception ex) {
                Logger.ShowError(ex);
            }
        }

        public void CallBack(object o) {
            var deletequeue = new List<string>();
            //ClientManager.EnterCriticalArea();
            try {
                string[] keys;
                if (Partner.Buff.Dead)
                    return;
                if (Master != null) {
                    if (Master.MapID != Partner.MapID) {
                        Partner.e.OnDie();
                        return;
                    }

                    if (Master.type == ActorType.PC) {
                        var pc = (ActorPC)Master;
                        if (!pc.Online) {
                            Partner.e.OnDie();
                            return;
                        }
                    }
                }

                if (commands.Count == 1 && commands.ContainsKey("Attack") && Hate.Count == 0) {
                    if (Global.Random.Next(0, 99) < 10) {
                        AIActivity = Activity.LAZY;
                        if ((Math.Abs((int)(Partner.X - X_Spawn)) > 1000 ||
                             Math.Abs((int)(Partner.Y - Y_Spawn)) > 1000) &&
                            MoveRange != 0) {
                            var len = GetLengthD(X_Spawn, Y_Spawn, Partner.X, Partner.Y);
                            short x = (short)(Partner.X + (X_Spawn - Partner.X) / len * Partner.Speed);
                            short y = (short)(Partner.Y + (Y_Spawn - Partner.Y) / len * Partner.Speed);

                            var mov = new Move(this, x, y);
                            commands.Add("Move", mov);
                        }
                        else {
                            double x, y;
                            byte _x, _y;
                            var counter = 0;
                            do {
                                x = Global.Random.Next(-100, 100);
                                y = Global.Random.Next(-100, 100);
                                var len = GetLengthD(0, 0, (short)x, (short)y);
                                x = x / len * 500;
                                y = y / len * 500;
                                x += Partner.X;
                                y += Partner.Y;
                                _x = Global.PosX16to8((short)x, map.Width);
                                _y = Global.PosY16to8((short)y, map.Height);
                                if (_x >= map.Width) {
                                    _x = (byte)(map.Width - 1);
                                }

                                if (_y >= map.Height) {
                                    _y = (byte)(map.Height - 1);
                                }

                                counter++;
                            } while (map.Info.walkable[_x, _y] != 2 && counter < 1000);

                            var mov = new Move(this, (short)x, (short)y);
                            commands.Add("Move", mov);
                        }
                    }
                    else {
                        AIActivity = Activity.IDLE;
                    }
                }

                keys = new string[commands.Count];
                commands.Keys.CopyTo(keys, 0);
                var count = commands.Count;
                for (var i = 0; i < count; i++) {
                    try {
                        string j = keys[i];
                        commands.TryGetValue(j, out var command);
                        if (command == null) {
                            continue;
                        }

                        if (command.Status != CommandStatus.FINISHED && command.Status != CommandStatus.DELETING &&
                            command.Status != CommandStatus.PAUSED) {
                            lock (command) {
                                command.Update(null);
                            }
                        }

                        if (command.Status == CommandStatus.FINISHED) {
                            deletequeue.Add(j); //删除队列
                            command.Status = CommandStatus.DELETING;
                        }
                    }
                    catch (Exception ex) {
                        Logger.ShowError(ex);
                    }
                }

                lock (commands) {
                    foreach (var i in deletequeue) {
                        commands.Remove(i);
                    }
                }
            }
            catch (Exception ex) {
                Logger.ShowError(ex);
                Logger.ShowError(ex.StackTrace, null);
            }
            //ClientManager.LeaveCriticalArea();
        }

        public List<MapNode> FindPath(byte x, byte y, byte x2, byte y2) {
            var src = new MapNode {
                x = x,
                y = y,
                F = 0,
                G = 0,
                H = 0
            };
            var count = 0;
            var path = new List<MapNode>();
            var current = src;
            if (x2 > map.Info.width - 1 || y2 > map.Info.height - 1) {
                path.Add(current);
                return path;
            }

            if (map.Info.walkable[x2, y2] != 2) {
                path.Add(current);
                return path;
            }

            if (x == x2 && y == y2) {
                path.Add(current);
                return path;
            }

            openedNode = new Dictionary<int, MapNode>();
            GetNeighbor(src, x2, y2);
            while (openedNode.Count != 0) {
                var shortest = new MapNode();
                shortest.F = int.MaxValue;
                if (count > 1000) {
                    break;
                }

                foreach (var i in openedNode.Values) {
                    if (i.x == x2 && i.y == y2) {
                        openedNode.Clear();
                        shortest = i;
                        break;
                    }

                    if (i.F < shortest.F) {
                        shortest = i;
                    }
                }

                current = shortest;
                if (openedNode.Count == 0)
                    break;
                openedNode.Remove(shortest.x * 1000 + shortest.y);
                current = GetNeighbor(shortest, x2, y2);
                count++;
            }

            while (current.Previous != null) {
                path.Add(current);
                current = current.Previous;
            }

            path.Reverse();
            return path;
        }

        private int GetPathLength(MapNode node) {
            var count = 0;
            var src = node;
            while (src.Previous != null) {
                count++;
                src = src.Previous;
            }

            return count;
        }

        public static int GetLength(byte x, byte y, byte x2, byte y2) {
            return (int)Math.Sqrt((x2 - x) * (x2 - x) + (y2 - y) * (y2 - y));
        }

        public static double GetLengthD(short x, short y, short x2, short y2) {
            return Math.Sqrt((x2 - x) * (x2 - x) + (y2 - y) * (y2 - y));
        }

        public static ushort GetDir(short x, short y) {
            var len = GetLengthD(0, 0, x, y);
            var degree = (int)(Math.Acos(y / len) / Math.PI * 180);
            return (x < 0) ? (ushort)degree : (ushort)(360 - degree);
        }

        private MapNode GetNeighbor(MapNode node, byte x, byte y) {
            var res = node;
            for (var i = node.x - 1; i <= node.x + 1; i++) {
                for (var j = node.y - 1; j <= node.y + 1; j++) {
                    if (j == -1 || i == -1)
                        continue;
                    if (j == node.y && i == node.x)
                        continue;
                    if (i >= map.Info.width || j >= map.Info.height)
                        continue;
                    if (map.Info.walkable[i, j] != 2) {
                        continue;
                    }

                    if (!openedNode.ContainsKey(i * 1000 + j)) {
                        var node2 = new MapNode {
                            x = (byte)i,
                            y = (byte)j,
                            Previous = node,
                            G = node.G + ((i == node.x || j == node.y) ? 10 : 14)
                        };
                        node2.H = Math.Abs(x - node2.x) * 10 + Math.Abs(y - node2.y) * 10;
                        node2.F = node2.G + node2.H;
                        openedNode.Add(i * 1000 + j, node2);
                    }
                    else {
                        var tmp = openedNode[i * 1000 + j];
                        int G = (i == node.x || j == node.y) ? 10 : 14;
                        if (node.G + G > tmp.G) {
                            res = tmp;
                        }
                    }
                }
            }

            return res;
        }
    }
}