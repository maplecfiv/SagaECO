﻿using System;
using System.Collections.Generic;
using SagaDB.Actor;
using SagaLib;
using SagaLib.Tasks;
using SagaMap.ActorEventHandlers;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Joint_Class.Breeder_驯兽师_
{
    /// <summary>
    ///     癒し火（癒し火）
    /// </summary>
    public class HealFire : ISkill
    {
        //#region Timer

        private class Activator : MultiRunTask
        {
            private readonly ActorSkill actor;
            private readonly Map map;
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
                DueTime = 0;
                Period = 3000;
                lifetime = 60000;
                map = MapManager.Instance.GetMap(actor.MapID);
            }

            public override void CallBack()
            {
                //同步鎖，表示之後的代碼是執行緒安全的，也就是，不允許被第二個執行緒同時訪問
                //测试去除技能同步锁ClientManager.EnterCriticalArea();
                try
                {
                    if (lifetime > 0)
                    {
                        var actors = map.GetActorsArea(actor, 250, false);
                        var pcs = new List<Actor>();
                        var pets = new List<Actor>();
                        foreach (var act in actors)
                            if (act.type == ActorType.PC)
                                pcs.Add(act);
                            else if (act.type == ActorType.PET) pets.Add(act);

                        if (pcs.Count > 0) SkillHandler.Instance.FixAttack(sActor, pcs, skill, Elements.Holy, -20f);
                        //广播技能效果
                        map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SKILL, skill, actor, false);
                        if (pets.Count > 0) SkillHandler.Instance.FixAttack(sActor, pets, skill, Elements.Holy, -50f);

                        //广播技能效果
                        map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SKILL, skill, actor, false);

                        lifetime -= Period;
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
                    Logger.getLogger().Error(ex, ex.Message);
                }
                //解開同步鎖
                //测试去除技能同步锁ClientManager.LeaveCriticalArea();
            }
        }

        //#endregion

        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            var map = MapManager.Instance.GetMap(sActor.MapID);
            if (map.CheckActorSkillInRange(SagaLib.Global.PosX8to16(args.x, map.Width),
                    SagaLib.Global.PosY8to16(args.y, map.Height), 200)) return -17;
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            //建立設置型技能實體
            var actor = new ActorSkill(args.skill, sActor);
            var map = MapManager.Instance.GetMap(sActor.MapID);

            actor.Stackable = false;
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

        //#endregion
    }
}