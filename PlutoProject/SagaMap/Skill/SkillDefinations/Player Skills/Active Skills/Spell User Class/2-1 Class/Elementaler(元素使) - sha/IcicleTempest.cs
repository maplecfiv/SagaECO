using System;
using System.Collections.Generic;
using SagaDB.Actor;
using SagaLib;
using SagaLib.Tasks;
using SagaMap.ActorEventHandlers;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._2_1_Class.Elementaler_元素使____sha
{
    internal class IcicleTempest : ISkill
    {
        #region Timer

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
                period = 500;
                dueTime = 0;
                var Me = (ActorPC)caster;

                switch (level)
                {
                    case 1:
                        factor *= 1f;
                        countMax = 3;
                        break;
                    case 2:
                        factor *= 1.2f;
                        countMax = 4;
                        break;
                    case 3:
                        factor *= 1.4f;
                        countMax = 4;
                        break;
                    case 4:
                        factor *= 1.6f;
                        countMax = 5;
                        break;
                    case 5:
                        factor *= 1.8f;
                        countMax = 5;
                        break;
                }

                if (Me.Skills2.ContainsKey(3036))
                {
                    TotalLv = Me.Skills2[3036].BaseData.lv;
                    if (TotalLv == 2 || TotalLv == 1)
                        factor += 0.3f;
                    else if (TotalLv == 4 || TotalLv == 3)
                        factor += 0.6f;
                    else if (TotalLv == 5)
                        factor += 0.9f;
                }

                if (Me.SkillsReserve.ContainsKey(3036))
                {
                    TotalLv = Me.SkillsReserve[3036].BaseData.lv;
                    if (TotalLv == 2 || TotalLv == 1)
                        factor += 0.3f;
                    else if (TotalLv == 4 || TotalLv == 3)
                        factor += 0.6f;
                    else if (TotalLv == 5)
                        factor += 0.9f;
                }

                if (Me.Skills2.ContainsKey(3025))
                {
                    TotalLv = Me.Skills2[3025].BaseData.lv;
                    if (TotalLv == 3 || TotalLv == 2)
                        factor += 0.3f;
                    else if (TotalLv == 5 || TotalLv == 4)
                        factor += 0.6f;
                }

                if (Me.SkillsReserve.ContainsKey(3025))
                {
                    TotalLv = Me.SkillsReserve[3025].BaseData.lv;
                    if (TotalLv == 3 || TotalLv == 2)
                        factor += 0.3f;
                    else if (TotalLv == 5 || TotalLv == 4)
                        factor += 0.6f;
                }
            }

            public override void CallBack()
            {
                //测试去除技能同步锁ClientManager.EnterCriticalArea();
                try
                {
                    if (count < countMax)
                    {
                        var actors = map.GetActorsArea(actor, 300, false);
                        var affected = new List<Actor>();
                        skill.affectedActors.Clear();
                        foreach (var i in actors)
                            if (SkillHandler.Instance.CheckValidAttackTarget(caster, i))
                            {
                                var Stiff = new Stiff(skill.skill, i, 400); //Mob can not move as soon as attacked.
                                SkillHandler.ApplyAddition(i, Stiff);
                                affected.Add(i);
                            }

                        SkillHandler.Instance.MagicAttack(caster, affected, skill, Elements.Water, factor);
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
                //测试去除技能同步锁ClientManager.LeaveCriticalArea();
            }
        }

        #endregion

        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            var map = MapManager.Instance.GetMap(pc.MapID);
            if (map.CheckActorSkillInRange(SagaLib.Global.PosX8to16(args.x, map.Width),
                    SagaLib.Global.PosY8to16(args.y, map.Height), 300)) return -17;
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var actor = new ActorSkill(args.skill, sActor);
            var map = MapManager.Instance.GetMap(sActor.MapID);
            actor.MapID = sActor.MapID;
            actor.X = SagaLib.Global.PosX8to16(args.x, map.Width);
            actor.Y = SagaLib.Global.PosY8to16(args.y, map.Height);
            actor.e = new NullEventHandler();
            //設置系
            actor.Stackable = false;
            map.RegisterActor(actor);
            actor.invisble = false;
            map.OnActorVisibilityChange(actor);
            var timer = new Activator(sActor, actor, args, level);
            timer.Activate();
        }

        #endregion
    }
}