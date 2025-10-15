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
    ///     混合演奏（フュージョン）
    /// </summary>
    public class Fusion : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            if (SkillHandler.Instance.isEquipmentRight(sActor, ItemType.STRINGS) ||
                sActor.Inventory.GetContainer(ContainerType.RIGHT_HAND2).Count > 0) return 0;
            return -5;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            //创建设置型技能技能体
            var actor = new ActorSkill(args.skill, sActor);
            var map = MapManager.Instance.GetMap(sActor.MapID);
            //设定技能体位置
            actor.MapID = sActor.MapID;
            actor.X = sActor.X;
            actor.Y = sActor.Y;
            //设定技能体的事件处理器，由于技能体不需要得到消息广播，因此创建个空处理器
            actor.e = new NullEventHandler();
            //在指定地图注册技能体Actor
            map.RegisterActor(actor);
            //设置Actor隐身属性为非
            actor.invisble = false;
            //广播隐身属性改变事件，以便让玩家看到技能体
            map.OnActorVisibilityChange(actor);
            //設置系
            actor.Stackable = false;
            //创建技能效果处理对象
            var timer = new Activator(sActor, actor, args, level);
            timer.Activate();

            //註冊施放者的Buff
            var skill = new DefaultBuff(args.skill, sActor, "Fusion_Caster", 30000 + 30000 * level);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(sActor, skill);
            if (sActor.skillsong != null) map.DeleteActor(sActor.skillsong);
            sActor.skillsong = actor;
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

        private class Activator : MultiRunTask
        {
            private readonly ActorSkill _actor;
            private readonly Actor _caster;
            private readonly int _countMax = 3;
            private readonly int _lifeTime;
            private readonly Map _map;
            private readonly SkillArg _skill;
            private int _count;

            public Activator(Actor caster, ActorSkill actor, SkillArg args, byte level)
            {
                _actor = actor;
                _caster = caster;
                _skill = args.Clone();
                _map = MapManager.Instance.GetMap(actor.MapID);
                Period = 500;
                DueTime = 0;
                _lifeTime = (30 + level * 30) * 1000;
                _countMax = _lifeTime / Period;
            }

            public override void CallBack()
            {
                //同步锁，表示之后的代码是线程安全的，也就是，不允许被第二个线程同时访问
                //测试去除技能同步锁ClientManager.EnterCriticalArea();
                try
                {
                    if (_count < _countMax)
                    {
                        //取得设置型技能，技能体周围7x7范围的怪（范围300，300代表3格，以自己为中心的3格范围就是7x7）
                        var actors = _map.GetActorsArea(_actor, 200, false);
                        //取得有效Actor

                        _skill.affectedActors.Clear();
                        foreach (var act in actors)
                            if (!SkillHandler.Instance.CheckValidAttackTarget(_caster, act))
                                if (!act.Status.Additions.ContainsKey("Fusion"))
                                {
                                    var skill2 = new DefaultBuff(_skill.skill, act, "Fusion",
                                        _lifeTime - _count * Period,
                                        200);
                                    skill2.OnAdditionStart += StartEventHandler;
                                    skill2.OnAdditionEnd += EndEventHandler;
                                    skill2.OnUpdate += TimerEventHandler; //
                                    SkillHandler.ApplyAddition(act, skill2);
                                }

                        //广播技能效果
                        _map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SKILL, _skill, _actor, false);
                        _count++;
                    }
                    else
                    {
                        Deactivate();
                        //在指定地图删除技能体（技能效果结束）
                        _map.DeleteActor(_actor);
                    }
                }
                catch (Exception ex)
                {
                    Logger.GetLogger().Error(ex, ex.Message);
                }
                //解开同步锁
                //测试去除技能同步锁ClientManager.LeaveCriticalArea();
            }

            private void StartEventHandler(Actor actor, DefaultBuff skill)
            {
                int level = skill.skill.Level;
                short[] str = { 3, 4, 5, 6, 7 };
                short[] vit = { 3, 4, 5, 6, 7 };
                short[] agi = { 5, 6, 7, 8, 9 };

                //STR
                if (skill.Variable.ContainsKey("Fusion_STR"))
                    skill.Variable.Remove("Fusion_STR");
                skill.Variable.Add("Fusion_STR", str[level - 1]);
                actor.Status.str_skill += str[level - 1];
                //VIT
                if (skill.Variable.ContainsKey("Fusion_VIT"))
                    skill.Variable.Remove("Fusion_VIT");
                skill.Variable.Add("Fusion_VIT", vit[level - 1]);
                actor.Status.vit_skill += vit[level - 1];
                //AGI
                if (skill.Variable.ContainsKey("Fusion_AGI"))
                    skill.Variable.Remove("Fusion_AGI");
                skill.Variable.Add("Fusion_AGI", agi[level - 1]);
                actor.Status.agi_skill += agi[level - 1];
                MapManager.Instance.GetMap(actor.MapID)
                    .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.CHANGE_STATUS, null, actor, true);
            }

            private void EndEventHandler(Actor actor, DefaultBuff skill)
            {
                //STR
                actor.Status.str_skill -= (short)skill.Variable["Fusion_STR"];
                //VIT
                actor.Status.vit_skill -= (short)skill.Variable["Fusion_VIT"];
                //AGI
                actor.Status.agi_skill -= (short)skill.Variable["Fusion_AGI"];
                MapManager.Instance.GetMap(actor.MapID)
                    .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.CHANGE_STATUS, null, actor, true);
            }

            private void TimerEventHandler(Actor actor, DefaultBuff skill)
            {
                int ranges = Map.Distance(_actor, actor);
                if (ranges > 200) skill.AdditionEnd();
            }
        }

        //#endregion
    }
}