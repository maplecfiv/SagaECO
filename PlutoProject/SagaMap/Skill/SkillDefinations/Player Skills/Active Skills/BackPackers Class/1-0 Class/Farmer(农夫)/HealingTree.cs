using System;
using SagaDB.Actor;
using SagaLib;
using SagaLib.Tasks;
using SagaMap.ActorEventHandlers;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._1_0_Class.Farmer_农夫_
{
    /// <summary>
    ///     樹精靈之手（ヒーリングツリー）
    /// </summary>
    public class HealingTree : ISkill
    {
        #region Timer

        private class Activator : MultiRunTask
        {
            private readonly ActorSkill actor;
            private readonly Map map;
            private readonly ActorMob mob;
            private readonly Actor sActor;
            private readonly SkillArg skill;
            private float factor;
            private int lifetime;

            public Activator(Actor _sActor, ActorSkill _dActor, SkillArg _args, byte level)
            {
                sActor = _sActor;
                actor = _dActor;
                skill = _args.Clone();
                factor = 0.1f * level;
                dueTime = 1000;
                period = 1000;
                lifetime = 5000 * level;
                map = MapManager.Instance.GetMap(actor.MapID);
                mob = map.SpawnMob(30480000, actor.X, actor.Y, 2500, sActor);
            }

            public override void CallBack()
            {
                //同步鎖，表示之後的代碼是執行緒安全的，也就是，不允許被第二個執行緒同時訪問
                //测试去除技能同步锁ClientManager.EnterCriticalArea();
                var HP_ADD = (uint)(10 * skill.skill.Level);
                try
                {
                    skill.affectedActors.Clear();
                    if (lifetime > 0)
                    {
                        lifetime -= period;
                        var affected = map.GetActorsArea(actor, 200, false);
                        foreach (var act in affected)
                            if (act.type == ActorType.PC || act.type == ActorType.PET || act.type == ActorType.SHADOW)
                                RecoverHP(act, HP_ADD);

                        skill.Init();
                    }
                    else
                    {
                        Deactivate();
                        map.DeleteActor(actor);
                        map.DeleteActor(mob);
                    }
                }
                catch (Exception ex)
                {
                    Logger.ShowError(ex);
                }
                //解開同步鎖
                //测试去除技能同步锁 ClientManager.LeaveCriticalArea();
            }

            public void RecoverHP(Actor act, uint HP_Add)
            {
                SkillHandler.Instance.FixAttack(sActor, act, skill, Elements.Holy, -HP_Add);
                map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SKILL, skill, actor, false);
            }
        }

        #endregion

        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            //建立設置型技能實體
            var actor = new ActorSkill(args.skill, sActor);
            var map = MapManager.Instance.GetMap(sActor.MapID);
            //設定技能位置
            actor.MapID = dActor.MapID;
            actor.X = SagaLib.Global.PosX8to16(args.x, map.Width);
            actor.Y = SagaLib.Global.PosY8to16(args.y, map.Height);
            //設定技能的事件處理器，由於技能體不需要得到消息廣播，因此建立空處理器
            actor.e = new NullEventHandler();
            //在指定地圖註冊技能Actor
            map.RegisterActor(actor);
            //設置Actor隱身屬性為False
            actor.invisble = false;
            //廣播隱身屬性改變事件，以便讓玩家看到技能實體
            map.OnActorVisibilityChange(actor);
            //建立技能效果處理物件
            var timer = new Activator(sActor, actor, args, level);
            timer.Activate();
        }

        #endregion
    }
}