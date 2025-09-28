using System;
using System.Collections.Generic;
using SagaLib;
using SagaLib.Tasks;
using SagaMap.ActorEventHandlers;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.COF_Additions.BOSS朋朋
{
    internal class BlackHoleOfPP : MobISkill
    {
        #region ISkill Members

        public void BeforeCast(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var actors = map.GetActorsArea(sActor, 900, false, true);
            var realAffected = new List<Actor>();
            foreach (var act in actors)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, act))
                    realAffected.Add(act);
            foreach (var a in realAffected)
            {
                var pos = new short[2]
                    { SagaLib.Global.PosX8to16(args.x, map.Width), SagaLib.Global.PosY8to16(args.y, map.Height) };
                map.MoveActor(Map.MOVE_TYPE.START, a, pos, 1000, 1000, true, MoveType.QUICKEN);
                var 钝足 = new MoveSpeedDown(args.skill, a, 4000);
                SkillHandler.ApplyAddition(a, 钝足);
            }

            var actor = new ActorSkill(args.skill, sActor);
            map = MapManager.Instance.GetMap(sActor.MapID);
            actor.MapID = sActor.MapID;
            actor.X = SagaLib.Global.PosX8to16(args.x, map.Width);
            actor.Y = SagaLib.Global.PosY8to16(args.y, map.Height);
            actor.e = new NullEventHandler();
            map.RegisterActor(actor);
            actor.invisble = false;
            map.OnActorVisibilityChange(actor);
            actor.Stackable = false;
            var timer = new Activator(sActor, actor, args, level);
            timer.Activate();
        }

        private class Activator : MultiRunTask
        {
            private readonly ActorSkill actor;
            private readonly Actor caster;
            private readonly int countMax = 5;
            private readonly float factor = 3.0f;
            private readonly Map map;
            private readonly SkillArg skill;
            private readonly byte x;
            private readonly byte y;
            private int count;

            public Activator(Actor caster, ActorSkill actor, SkillArg args, byte level)
            {
                this.actor = actor;
                this.caster = caster;
                skill = args.Clone();
                map = MapManager.Instance.GetMap(actor.MapID);
                period = 1000;
                dueTime = 0;
                x = args.x;
                y = args.y;
            }

            public override void CallBack()
            {
                ClientManager.EnterCriticalArea();
                try
                {
                    if (count < countMax)
                    {
                        count++;
                    }
                    else
                    {
                        var actors = map.GetActorsArea(actor, 300, false);
                        var affected = new List<Actor>();
                        skill.affectedActors.Clear();
                        foreach (var i in actors)
                            if (SkillHandler.Instance.CheckValidAttackTarget(caster, i))
                                affected.Add(i);

                        SkillHandler.Instance.MagicAttack(caster, affected, skill, Elements.Dark, factor);
                        map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SKILL, skill, actor, false);
                        SkillHandler.Instance.ShowEffect(map, actor, x, y, 5300);

                        Deactivate();
                        map.DeleteActor(actor);
                    }
                }
                catch (Exception ex)
                {
                    Logger.ShowError(ex);
                }

                //解开同步锁
                ClientManager.LeaveCriticalArea();
            }
        }

        #endregion
    }
}