using System;
using System.Collections.Generic;
using SagaDB.Actor;
using SagaDB.Item;
using SagaLib;
using SagaMap.ActorEventHandlers;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Bard
{
    /// <summary>
    ///     安魂曲演奏（レクイエム）
    /// </summary>
    public class Requiem : ISkill
    {
        #region Timer

        private class Activator : MultiRunTask
        {
            private readonly ActorSkill actor;
            private readonly Map map;
            private readonly Actor sActor;
            private readonly SkillArg skill;
            private int counts;
            private float factor;
            private int lifetime;

            public Activator(Actor _sActor, ActorSkill _dActor, SkillArg _args, byte level)
            {
                sActor = _sActor;
                actor = _dActor;
                skill = _args.Clone();
                //factor = 0.1f * level;
                factor = 0.37f + 0.43f * level;
                dueTime = 0;
                period = 1000;
                lifetime = 5000;
                int[] c = { 0, 75, 40, 33, 28, 25 };
                counts = c[level];
                map = MapManager.Instance.GetMap(actor.MapID);
            }

            public override void CallBack()
            {
                //同步鎖，表示之後的代碼是執行緒安全的，也就是，不允許被第二個執行緒同時訪問
                //测试去除技能同步锁ClientManager.EnterCriticalArea();
                try
                {
                    if (lifetime > 0 && counts > 0)
                    {
                        var factor = 0.37f + 0.43f * skill.skill.Level;
                        var map = MapManager.Instance.GetMap(sActor.MapID);
                        var affected = map.GetActorsArea(sActor, 200, false);
                        var realAffected = new List<Actor>();
                        foreach (var act in affected)
                            if (SkillHandler.Instance.CheckValidAttackTarget(sActor, act))
                            {
                                if (act.type == ActorType.MOB &&
                                    (act as ActorMob).BaseData.mobType.ToString().StartsWith("UNDEAD"))
                                {
                                    realAffected.Add(act);
                                    counts--;
                                    if (counts == 0) break;
                                }

                                if (act.type == ActorType.PC && (act as ActorPC).Buff.Undead)
                                {
                                    realAffected.Add(act);
                                    counts--;
                                    if (counts == 0) break;
                                }
                            }

                        SkillHandler.Instance.MagicAttack(sActor, realAffected, skill, Elements.Holy, factor);
                        lifetime -= period;
                    }
                    else
                    {
                        Deactivate();
                        //在指定地图删除技能体（技能效果结束）
                        map.DeleteActor(actor);
                    }
                }
                catch (Exception ex)
                {
                    Logger.ShowError(ex);
                }
                //解開同步鎖
                //测试去除技能同步锁ClientManager.LeaveCriticalArea();
            }
        }

        #endregion

        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            if (SkillHandler.Instance.isEquipmentRight(sActor, ItemType.STRINGS) ||
                sActor.Inventory.GetContainer(ContainerType.RIGHT_HAND2).Count > 0) return 0;
            return -5;
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