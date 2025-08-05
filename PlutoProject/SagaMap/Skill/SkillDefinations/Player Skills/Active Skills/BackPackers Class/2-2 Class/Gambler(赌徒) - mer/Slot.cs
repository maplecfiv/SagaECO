using System;
using SagaDB.Actor;
using SagaLib;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Gambler
{
    /// <summary>
    ///     幸運數字（スリーセブン）
    /// </summary>
    public class Slot : ISkill
    {
        #region Timer

        private class Activator : MultiRunTask
        {
            private readonly ActorSkill actor;
            private float factor;
            private Map map;
            private Actor sActor;
            private SkillArg skill;

            public Activator(Actor _sActor, ActorSkill _dActor, SkillArg _args, byte level)
            {
                sActor = _sActor;
                actor = _dActor;
                skill = _args.Clone();
                factor = 0.1f * level;
                dueTime = 5000 * level;
                period = 1000;
                map = MapManager.Instance.GetMap(actor.MapID);
            }

            public override void CallBack()
            {
                //同步鎖，表示之後的代碼是執行緒安全的，也就是，不允許被第二個執行緒同時訪問
                //测试去除技能同步锁ClientManager.EnterCriticalArea();
                try
                {
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
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
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
            //Activator timer = new Activator(sActor, actor, args, level);
            //timer.Activate();
        }

        #endregion
    }
}