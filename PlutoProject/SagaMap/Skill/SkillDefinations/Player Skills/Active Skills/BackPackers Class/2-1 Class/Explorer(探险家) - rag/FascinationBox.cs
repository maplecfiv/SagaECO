using System;
using System.Collections.Generic;
using SagaDB.Actor;
using SagaDB.Mob;
using SagaLib;
using SagaLib.Tasks;
using SagaMap.Manager;
using static SagaMap.Skill.SkillHandler;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._2_1_Class.Explorer_探险家____rag
{
    /// <summary>
    ///     魅惑之箱（ファシネイションボックス）
    /// </summary>
    public class FascinationBox : ISkill
    {
        //#region Timer

        public class Activator : MultiRunTask
        {
            public delegate void OnTimerHandler(Activator timer);

            public delegate void ProcSkillHandler(Actor sActor, Actor mActor, Actor actor, SkillArg args, Map map,
                int level, float factor);

            public ActorMob actor;
            public float factor;
            public int level;
            public int lifetime;
            public Map map;
            public bool OneTimes;
            public Actor sActor;
            public SkillArg skill;
            public int State = 0;

            public Activator(Actor _sActor, ActorMob _dActor, SkillArg _args, byte level, int lifetime, float _factor)
            {
                sActor = _sActor;
                actor = _dActor;
                skill = _args;
                DueTime = 0;
                Period = 1;
                this.lifetime = lifetime;
                this.level = level;
                factor = _factor;
                map = MapManager.Instance.GetMap(sActor.MapID);
            }

            public event ProcSkillHandler ProcSkill;
            public event OnTimerHandler OnTimer;

            public override void CallBack()
            {
                //同步鎖，表示之後的代碼是執行緒安全的，也就是，不允許被第二個執行緒同時訪問
                //测试去除技能同步锁
                //ClientManager.EnterCriticalArea();
                try
                {
                    if (lifetime > 0)
                    {
                        if (OnTimer != null) OnTimer.Invoke(this);
                        lifetime -= Period;
                    }
                    else if (actor.HP <= 0)
                    {
                        var map = MapManager.Instance.GetMap(sActor.MapID);
                        var arg2 = new EffectArg();
                        arg2.effectID = 5345;
                        arg2.actorID = actor.ActorID;
                        map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SHOW_EFFECT, arg2, actor, true);

                        var affected = map.GetActorsArea(actor, 200, false);
                        var realAffected = new List<Actor>();
                        foreach (var act in affected)
                            if (SkillHandler.Instance.CheckValidAttackTarget(actor, act))
                            {
                                //realAffected.Add(act);
                                var demage = SkillHandler.Instance.CalcDamage(false, actor, act, skill, DefType.MDef,
                                    Elements.Neutral, 0, factor);
                                SkillHandler.Instance.FixAttack(actor, act, skill, Elements.Neutral, demage);
                                SkillHandler.Instance.ShowVessel(act, demage);
                            }

                        realAffected.Add(actor);
                        Deactivate();
                        map.DeleteActor(actor);
                        actor.HP = 0;
                        actor.e.OnDie();
                    }
                    else
                    {
                        var map = MapManager.Instance.GetMap(sActor.MapID);
                        var arg2 = new EffectArg();
                        arg2.effectID = 5345;
                        arg2.actorID = actor.ActorID;
                        map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SHOW_EFFECT, arg2, actor, true);

                        var affected = map.GetActorsArea(actor, 200, false);
                        foreach (var act in affected)
                            if (SkillHandler.Instance.CheckValidAttackTarget(actor, act))
                            {
                                //realAffected.Add(act);
                                var demage = SkillHandler.Instance.CalcDamage(false, actor, act, skill, DefType.MDef,
                                    Elements.Neutral, 0, factor);
                                SkillHandler.Instance.FixAttack(actor, act, skill, Elements.Neutral, demage);
                                SkillHandler.Instance.ShowVessel(act, demage);
                            }

                        Deactivate();
                        map.DeleteActor(actor);
                        actor.HP = 0;
                        actor.e.OnDie();
                    }
                }
                catch (Exception ex)
                {
                    Logger.GetLogger().Error(ex, ex.Message);
                }
                //解開同步鎖
                //测试去除技能同步锁
                //ClientManager.LeaveCriticalArea();
            }
        }

        //#endregion

        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            if (sActor.Slave.Count < 5) return 0;
            return 13;
        }


        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var factor = new[] { 0, 2.0f, 3.0f, 4.0f }[level];
            var m = new ActorMob();
            m = map.SpawnMob(90010037, sActor.X, sActor.Y, 0, sActor);
            m.BaseData.mobType = MobType.NONE_NOTOUCH;
            m.Status.min_matk = 2000;
            m.Status.max_matk = 2800;
            sActor.Slave.Add(m);
            var LifeTime = 4000;
            var arg = new SkillArg();
            arg = args;
            var timer = new Activator(sActor, m, arg, level, LifeTime, factor);
            timer.Activate();
        }

        //#endregion
    }
}