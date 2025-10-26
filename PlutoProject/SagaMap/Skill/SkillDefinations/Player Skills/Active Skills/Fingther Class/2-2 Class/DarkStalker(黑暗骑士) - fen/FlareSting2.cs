using System;
using SagaDB.Actor;
using SagaLib;
using SagaLib.Tasks;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._2_2_Class.DarkStalker_黑暗骑士____fen {
    /// <summary>
    ///     火光蟲（フレアスティング）[接續技能]
    /// </summary>
    public class FlareSting2 : ISkill {
        //#region Timer

        private class Activator : MultiRunTask {
            private readonly Actor dActor;
            private readonly float factor;
            private readonly Map map;
            private readonly Actor sActor;
            private readonly SkillArg skill;
            private int lifetime;

            public Activator(Actor _sActor, Actor _dActor, SkillArg _args, byte level) {
                sActor = _sActor;
                dActor = _dActor;
                skill = _args.Clone();
                float[] factors = { 0f, 5.0f, 5.5f, 6.0f, 6.5f, 7.0f };
                factor = factors[level];
                DueTime = 1000;
                Period = 1000;
                lifetime = 1000;
                map = MapManager.Instance.GetMap(sActor.MapID);
                SkillHandler.Instance.MagicAttack(sActor, dActor, skill, Elements.Dark, factor);
                float[] appFactors = { 0f, 0.009f, 0.009f, 0.012f, 0.012f, 0.015f };
                factor = appFactors[level];
            }

            public override void CallBack() {
                //同步鎖，表示之後的代碼是執行緒安全的，也就是，不允許被第二個執行緒同時訪問
                //测试去除技能同步锁ClientManager.EnterCriticalArea();
                try {
                    if (lifetime > 0) {
                        var HP_Lost = (uint)(dActor.MaxHP * factor);
                        if (dActor.HP > HP_Lost) {
                            dActor.HP -= HP_Lost;
                            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.HPMPSP_UPDATE, null, dActor, true);
                        }
                        else {
                            dActor.HP = 0;
                            SkillHandler.Instance.MagicAttack(sActor, dActor, skill, Elements.Dark, 1f);
                            lifetime = 0;
                        }

                        map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SKILL, skill, dActor, false);
                        lifetime -= Period;
                    }
                    else {
                        Deactivate();
                        ////在指定地图删除技能体（技能效果结束）
                        //map.DeleteActor(actor);
                    }
                }
                catch (Exception ex) {
                    Logger.ShowError(ex);
                }
                //解開同步鎖
                //测试去除技能同步锁ClientManager.LeaveCriticalArea();
            }
        }

        //#endregion

        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args) {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level) {
            ////建立設置型技能實體
            //ActorSkill actor = new ActorSkill(args.skill, sActor);
            //Map map = Manager.MapManager.Instance.GetMap(sActor.MapID);
            ////設定技能位置
            //actor.MapID = dActor.MapID;
            //actor.X = SagaLib.Global.PosX8to16(args.x, map.Width);
            //actor.Y = SagaLib.Global.PosY8to16(args.y, map.Height);
            ////設定技能的事件處理器，由於技能體不需要得到消息廣播，因此建立空處理器
            //actor.e = new ActorEventHandlers.NullEventHandler();
            ////在指定地圖註冊技能Actor
            //map.RegisterActor(actor);
            ////設置Actor隱身屬性為False
            //actor.invisble = false;
            ////廣播隱身屬性改變事件，以便讓玩家看到技能實體
            //map.OnActorVisibilityChange(actor);
            ////建立技能效果處理物件
            var timer = new Activator(sActor, dActor, args, level);
            timer.Activate();
        }

        //#endregion
    }
}