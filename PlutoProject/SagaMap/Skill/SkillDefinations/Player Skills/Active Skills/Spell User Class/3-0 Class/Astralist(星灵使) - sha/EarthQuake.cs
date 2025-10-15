using System;
using System.Collections.Generic;
using System.Linq;
using SagaDB.Actor;
using SagaLib;
using SagaLib.Tasks;
using SagaMap.ActorEventHandlers;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._3_0_Class.Astralist_星灵使____sha
{
    public class EarthQuake : ISkill
    {
        //#region Timer

        private class Activator : MultiRunTask
        {
            private readonly ActorSkill actor;
            private readonly Actor caster;
            private readonly int countMax = 3;
            private readonly float factor = 1.0f;
            private readonly Map map;
            private readonly SkillArg skill;
            private readonly int TotalLv;
            private int count;

            public Activator(Actor caster, ActorSkill actor, SkillArg args, byte level)
            {
                this.actor = actor;
                this.caster = caster;
                skill = args.Clone();
                map = MapManager.Instance.GetMap(actor.MapID);
                Period = 400;
                DueTime = 0;

                factor = 0.95f + level * 0.05f;
                int[] LvCount = { 0, 5, 5, 6, 6, 7 };
                countMax = LvCount[level];
                if (caster.type == ActorType.PC)
                {
                    var pc = (ActorPC)caster;
                    if (pc.Skills2_1.ContainsKey(3049) || pc.DualJobSkill.Exists(x => x.ID == 3049))
                    {
                        var duallv = 0;
                        if (pc.DualJobSkill.Exists(x => x.ID == 3049))
                            duallv = pc.DualJobSkill.FirstOrDefault(x => x.ID == 3049).Level;

                        var mainlv = 0;
                        if (pc.Skills2_1.ContainsKey(3049))
                            mainlv = pc.Skills2_1[3049].Level;

                        TotalLv = Math.Max(duallv, mainlv);
                        switch (level)
                        {
                            case 1:
                                factor += 0f;
                                break;
                            case 2:
                                factor += 0.3f;
                                break;
                            case 3:
                                factor += 0.5f;
                                break;
                            case 4:
                                factor += 0.7f;
                                break;
                            case 5:
                                factor += 0.9f;
                                break;
                        }
                    }

                    if (pc.Skills2_1.ContainsKey(3025) || pc.DualJobSkill.Exists(x => x.ID == 3025))
                    {
                        var duallv = 0;
                        if (pc.DualJobSkill.Exists(x => x.ID == 3025))
                            duallv = pc.DualJobSkill.FirstOrDefault(x => x.ID == 3025).Level;

                        var mainlv = 0;
                        if (pc.Skills2_1.ContainsKey(3025))
                            mainlv = pc.Skills2_1[3025].Level;

                        TotalLv = Math.Max(duallv, mainlv);
                        switch (TotalLv)
                        {
                            case 1:
                                switch (level)
                                {
                                    case 1:
                                        countMax = 5;
                                        break;
                                    case 2:
                                        countMax = 6;
                                        break;
                                    case 3:
                                        countMax = 6;
                                        break;
                                    case 4:
                                        countMax = 7;
                                        break;
                                    case 5:
                                        countMax = 7;
                                        break;
                                }

                                break;
                            case 2:
                                switch (level)
                                {
                                    case 1:
                                        countMax = 6;
                                        break;
                                    case 2:
                                        countMax = 6;
                                        break;
                                    case 3:
                                        countMax = 7;
                                        break;
                                    case 4:
                                        countMax = 7;
                                        break;
                                    case 5:
                                        countMax = 8;
                                        break;
                                }

                                break;
                            case 3:
                                switch (level)
                                {
                                    case 1:
                                        countMax = 7;
                                        break;
                                    case 2:
                                        countMax = 7;
                                        break;
                                    case 3:
                                        countMax = 8;
                                        break;
                                    case 4:
                                        countMax = 8;
                                        break;
                                    case 5:
                                        countMax = 9;
                                        break;
                                }

                                break;
                            case 4:
                                switch (level)
                                {
                                    case 1:
                                        countMax = 7;
                                        break;
                                    case 2:
                                        countMax = 8;
                                        break;
                                    case 3:
                                        countMax = 8;
                                        break;
                                    case 4:
                                        countMax = 9;
                                        break;
                                    case 5:
                                        countMax = 10;
                                        break;
                                }

                                break;
                            case 5:
                                switch (level)
                                {
                                    case 1:
                                        countMax = 8;
                                        break;
                                    case 2:
                                        countMax = 9;
                                        break;
                                    case 3:
                                        countMax = 10;
                                        break;
                                    case 4:
                                        countMax = 11;
                                        break;
                                    case 5:
                                        countMax = 12;
                                        break;
                                }

                                break;
                        }
                    }
                }
            }


            public override void CallBack()
            {
                //同步锁，表示之后的代码是线程安全的，也就是，不允许被第二个线程同时访问
                //测试去除技能同步锁ClientManager.EnterCriticalArea();
                try
                {
                    if (count < countMax)
                    {
                        //取得设置型技能，技能体周围7x7范围的怪（范围300，300代表3格，以自己为中心的3格范围就是7x7）
                        var actors = map.GetActorsArea(actor, 300, false);
                        var affected = new List<Actor>();
                        //取得有效Actor（即怪物）

                        //施加火属性魔法伤害
                        skill.affectedActors.Clear();
                        foreach (var i in actors)
                            if (SkillHandler.Instance.CheckValidAttackTarget(caster, i))
                            {
                                var Stiff = new Stiff(skill.skill, i, 100); //Mob can not move as soon as attacked.
                                SkillHandler.ApplyAddition(i, Stiff);
                                affected.Add(i);
                            }

                        //SkillHandler.Instance.MagicAttack(caster, affected, skill, Elements.Fire, factor);
                        SkillHandler.Instance.MagicAttack(caster, affected, skill, Elements.Earth, factor);

                        //广播技能效果
                        map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SKILL, skill, actor, false);
                        count++;
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
                    Logger.GetLogger().Error(ex, ex.Message);
                }
                //解开同步锁
                //测试去除技能同步锁ClientManager.LeaveCriticalArea();
            }
        }

        //#endregion

        //#region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            var map = MapManager.Instance.GetMap(pc.MapID);
            if (map.CheckActorSkillInRange(SagaLib.Global.PosX8to16(args.x, map.Width),
                    SagaLib.Global.PosY8to16(args.y, map.Height), 300)) return -17;
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            //创建设置型技能技能体
            var actor = new ActorSkill(args.skill, sActor);
            var map = MapManager.Instance.GetMap(sActor.MapID);
            //设定技能体位置
            actor.MapID = sActor.MapID;
            actor.X = dActor.X;
            actor.Y = dActor.Y;
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
        }

        //#endregion
    }
}