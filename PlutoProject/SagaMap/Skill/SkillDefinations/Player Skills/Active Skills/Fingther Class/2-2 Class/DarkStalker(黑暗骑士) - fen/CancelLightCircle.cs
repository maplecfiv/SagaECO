using System;
using SagaDB.Actor;
using SagaLib;
using SagaMap.ActorEventHandlers;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.DarkStalker
{
    /// <summary>
    ///     月蝕（ホーリーキャンセレーション）
    /// </summary>
    public class CancelLightCircle : ISkill
    {
        #region Timer

        private class Activator : MultiRunTask
        {
            private readonly ActorSkill actor;
            private readonly Map map;
            private readonly byte[,] olight = new byte[3, 3];
            private readonly SkillArg skill;
            private float factor;
            private int lifetime;
            private Actor sActor;
            private int times;

            public Activator(Actor _sActor, ActorSkill _dActor, SkillArg _args, byte level)
            {
                sActor = _sActor;
                actor = _dActor;
                skill = _args.Clone();
                factor = 0.1f * level;
                dueTime = 0;
                period = 1000;
                lifetime = 25000 + 5000 * level;
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
                        if (times == 0)
                            for (var x = (byte)(skill.x - 1); x < skill.x + 1; x++)
                            for (var y = (byte)(skill.y - 1); y < skill.y + 1; y++)
                            {
                                olight[x - skill.x + 1, y - skill.y + 1] = map.Info.holy[x, y];
                                map.Info.holy[x, y] = 0;
                            }

                        times++;
                        lifetime -= period;
                    }
                    else
                    {
                        for (var x = (byte)(skill.x - 1); x < skill.x + 1; x++)
                        for (var y = (byte)(skill.y - 1); y < skill.y + 1; y++)
                            map.Info.holy[x, y] = olight[x - skill.x + 1, y - skill.y + 1];

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

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            var map = MapManager.Instance.GetMap(pc.MapID);
            if (map.CheckActorSkillInRange(SagaLib.Global.PosX8to16(args.x, map.Width),
                    SagaLib.Global.PosY8to16(args.y, map.Height), 100)) return -17;
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
            actor.Stackable = false;
            //建立技能效果處理物件
            var timer = new Activator(sActor, actor, args, level);
            timer.Activate();
        }

        #endregion
    }
}