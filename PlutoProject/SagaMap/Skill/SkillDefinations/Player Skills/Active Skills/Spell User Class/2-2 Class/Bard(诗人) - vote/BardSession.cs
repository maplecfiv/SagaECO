using System;
using System.Collections.Generic;
using SagaDB.Actor;
using SagaDB.Item;
using SagaLib;
using SagaLib.Tasks;
using SagaMap.ActorEventHandlers;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._2_2_Class.Bard_诗人____vote
{
    /// <summary>
    ///     專門樂器演奏（セッション）
    /// </summary>
    public class BardSession : ISkill
    {
        //#region Timer

        private class Activator : MultiRunTask
        {
            private readonly ActorSkill actor;
            private readonly Map map;
            private readonly int rate1;
            private readonly int rate2;
            private readonly Actor sActor;
            private readonly SkillArg skill;
            private readonly int times;
            private int count;

            public Activator(Actor _sActor, ActorSkill _dActor, SkillArg _args, byte level)
            {
                sActor = _sActor;
                actor = _dActor;
                skill = _args.Clone();
                DueTime = 0;
                Period = 2000;
                rate1 = 2 + 4 * level;
                rate2 = 6 + 4 * level;
                times = level + 3;
                map = MapManager.Instance.GetMap(actor.MapID);
            }

            public override void CallBack()
            {
                //同步鎖，表示之後的代碼是執行緒安全的，也就是，不允許被第二個執行緒同時訪問
                //测试去除技能同步锁ClientManager.EnterCriticalArea();
                try
                {
                    if (count < times)
                    {
                        var map = MapManager.Instance.GetMap(sActor.MapID);
                        var affected = map.GetActorsArea(sActor, 150, false);
                        foreach (var act in affected)
                            if (SkillHandler.Instance.CheckValidAttackTarget(sActor, act))
                            {
                                if (SkillHandler.Instance.CanAdditionApply(sActor, act,
                                        SkillHandler.DefaultAdditions.Confuse, rate1))
                                {
                                    var skill5 = new Confuse(skill.skill, act, 5000);
                                    SkillHandler.ApplyAddition(act, skill5);
                                }

                                if (SkillHandler.Instance.CanAdditionApply(sActor, act,
                                        SkillHandler.DefaultAdditions.Sleep, rate2))
                                {
                                    var skill6 = new Sleep(skill.skill, act, 5000);
                                    SkillHandler.ApplyAddition(act, skill6);
                                }
                            }

                        //广播技能效果
                        map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SKILL, skill, actor, false);
                        count++;
                    }
                    else
                    {
                        Deactivate();
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

        //#endregion

        //#region ISkill Members

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

            //註冊施放者的Buff
            var skill = new DefaultBuff(args.skill, sActor, "BardSession_Caster", 2000);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(sActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.Buff.Playing = true;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.Buff.Playing = false;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        //#endregion
    }
}