using System;
using SagaDB.Actor;
using SagaLib;
using SagaLib.Tasks;
using SagaMap.ActorEventHandlers;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Global.Active {
    /// <summary>
    ///     陷阱的基礎類別
    /// </summary>
    public class Trap : ISkill {
        /// <summary>
        ///     座標跟誰相同
        /// </summary>
        public enum PosType {
            sActor,
            args
        }

        public float factor = 1f;
        public int LifeTime;
        public bool OneTimes;
        public PosType posType;
        public uint Range;

        public Trap(bool OneTimes, uint Range, PosType posType) {
            LifeTime = 0;
            this.OneTimes = OneTimes;
            this.Range = Range;
            this.posType = posType;
        }

        public Trap(bool OneTimes, PosType posType) {
            LifeTime = 0;
            this.OneTimes = OneTimes;
            Range = 100;
            this.posType = posType;
        }

        public virtual void ProcSkill(Actor sActor, Actor mActor, ActorSkill actor, SkillArg args, Map map, int level,
            float factor) {
        }

        public virtual void BeforeProc(Actor sActor, Actor dActor, SkillArg args, byte level) {
        }

        public virtual void OnTimer(Activator timer) {
        }

        //#region Timer

        public class Activator : MultiRunTask {
            public delegate void OnTimerHandler(Activator timer);

            public delegate void ProcSkillHandler(Actor sActor, Actor mActor, ActorSkill actor, SkillArg args, Map map,
                int level, float factor);

            public ActorSkill actor;
            public float factor;
            public int level;
            public int lifetime;
            public Map map;
            public bool OneTimes;
            public Actor sActor;
            public SkillArg skill;
            public int State = 0;

            public Activator(Actor _sActor, ActorSkill _dActor, SkillArg _args, byte level, int lifetime, bool OneTimes,
                float factor) {
                sActor = _sActor;
                actor = _dActor;
                skill = _args.Clone();
                DueTime = 0;
                Period = 1000;
                this.lifetime = lifetime;
                this.level = level;
                map = MapManager.Instance.GetMap(sActor.MapID);
                this.OneTimes = OneTimes;
                this.factor = factor;
            }

            public event ProcSkillHandler ProcSkill;
            public event OnTimerHandler OnTimer;

            public override void CallBack() {
                //同步鎖，表示之後的代碼是執行緒安全的，也就是，不允許被第二個執行緒同時訪問
                //测试去除技能同步锁ClientManager.EnterCriticalArea();
                try {
                    if (lifetime > 0) {
                        if (OnTimer != null) OnTimer.Invoke(this);
                        lifetime -= Period;
                    }
                    else {
                        Deactivate();
                        //在指定地图删除技能体（技能效果结束）
                        map.DeleteActor(actor);
                    }
                }
                catch (Exception ex) {
                    Logger.ShowError(ex);
                }
                //解開同步鎖
                //测试去除技能同步锁ClientManager.LeaveCriticalArea();
            }

            public void ActorMoveEvent(Actor mActor, short[] pos, ushort dir, ushort speed) {
                //ClientManager.EnterCriticalArea();
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, mActor))
                    if (ProcSkill != null) {
                        ProcSkill.Invoke(sActor, mActor, actor, skill, map, level, factor);
                        map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SKILL, skill, actor, false);
                        if (OneTimes) {
                            Deactivate();
                            //在指定地图删除技能体（技能效果结束）
                            map.DeleteActor(actor);
                        }
                    }
                //ClientManager.LeaveCriticalArea();
            }
        }

        //#endregion

        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args) {
            uint itemID = 10021900; //陷阱
            var pc = sActor;
            if (SkillHandler.Instance.CountItem(pc, itemID) > 0) {
                SkillHandler.Instance.TakeItem(pc, itemID, 1);
                return 0;
            }

            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level) {
            BeforeProc(sActor, dActor, args, level);
            factor = 1f;
            // 陷阱傷害增加（罠ダメージ上昇）
            if (sActor.Status.Additions.ContainsKey("TrapDamUp")) {
                var TrapDamUp = (DefaultPassiveSkill)sActor.Status.Additions["TrapDamUp"];
                factor = 1.10f + 0.02f * TrapDamUp.skill.Level;
            }

            //建立設置型技能實體
            var actor = new ActorSkill(args.skill, sActor);
            var map = MapManager.Instance.GetMap(sActor.MapID);
            //建置處理器
            var timer = new Activator(sActor, actor, args, level, LifeTime, OneTimes, factor);
            //設定技能位置
            if (posType == PosType.sActor) {
                actor.MapID = sActor.MapID;
                actor.X = sActor.X;
                actor.Y = sActor.Y;
            }
            else {
                actor.MapID = sActor.MapID;
                actor.X = SagaLib.Global.PosX8to16(args.x, map.Width);
                actor.Y = SagaLib.Global.PosY8to16(args.y, map.Height);
            }

            //設定技能的事件處理器，由於技能體不需要得到消息廣播，因此建立空處理器
            var sh = new SkillEventHandler();
            sh.ActorMove += timer.ActorMoveEvent;
            actor.e = sh;
            actor.sightRange = Range;
            //在指定地圖註冊技能Actor
            map.RegisterActor(actor);
            //設置Actor隱身屬性為False
            actor.invisble = false;
            //廣播隱身屬性改變事件，以便讓玩家看到技能實體
            map.OnActorVisibilityChange(actor);
            timer.ProcSkill += ProcSkill;
            timer.OnTimer += OnTimer;
            timer.Activate();
        }

        //#endregion
    }
}