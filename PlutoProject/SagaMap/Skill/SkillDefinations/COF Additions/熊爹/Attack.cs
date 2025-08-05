using System;
using System.Collections.Generic;
using SagaDB.Actor;
using SagaLib;
using SagaMap.Manager;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.X
{
    public class Attack : MobISkill
    {
        #region ISkill Members

        public void BeforeCast(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var actors = map.GetActorsArea(sActor, 800, false, true);
            var FrosenActors = new List<Actor>();
            var realAffected = new List<Actor>();
            var realAffected2 = new List<Actor>();
            var ParryAffected = new List<Actor>();
            foreach (var act in actors)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, act))
                {
                    realAffected.Add(act);
                    if (act.Status.Additions.ContainsKey("Parry") && 3 > Math.Max(Math.Abs(act.X - sActor.X) / 100,
                            Math.Abs(act.Y - sActor.Y) / 100))
                    {
                        ParryAffected.Add(act);
                    }
                    else
                    {
                        if (act.Status.Additions.ContainsKey("Frosen"))
                        {
                            FrosenActors.Add(act);
                            act.Status.Additions["Frosen"].AdditionEnd();
                            act.Status.Additions.Remove("Frosen");
                        }
                        else
                        {
                            var 钝足 = new MoveSpeedDown(args.skill, act, 5000 + dActor.SpeedCut * 300);
                            SkillHandler.ApplyAddition(act, 钝足);
                        }
                    }
                }

            var arg2 = new SkillArg();
            arg2 = args.Clone();
            var arg3 = new SkillArg();
            arg3 = args.Clone();
            SkillHandler.Instance.PhysicalAttack(sActor, realAffected, arg2, Elements.Neutral, 1f);
            SkillHandler.Instance.MagicAttack(sActor, ParryAffected, arg3, Elements.Neutral, 2f);

            arg2.skill.BaseData.id = 100;
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SKILL, arg2, sActor, true);
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SKILL, arg3, sActor, true);
            arg2.skill.BaseData.id = 20009;


            if (FrosenActors.Count > 0)
            {
                //SkillHandler.Instance.ActorSpeak(sActor, "被我逮到了就让你哭爹喊娘！");
                foreach (var act in FrosenActors)
                {
                    actors = map.GetActorsArea(act, 300, true, true);

                    var xy = map.GetRandomPosAroundActor2(sActor);
                    if (sActor.Slave.Count < 3)
                        sActor.Slave.Add(map.SpawnMob(82000001, act.X, act.Y, 2500, sActor));
                    else if (sActor.Slave[0].Buff.Dead)
                        sActor.Slave[0] = map.SpawnMob(82000001, act.X, act.Y, 2500, sActor);
                    else if (sActor.Slave[1].Buff.Dead)
                        sActor.Slave[1] = map.SpawnMob(82000001, act.X, act.Y, 2500, sActor);
                    else if (sActor.Slave[2].Buff.Dead)
                        sActor.Slave[2] = map.SpawnMob(82000001, act.X, act.Y, 2500, sActor);


                    foreach (var act2 in actors)
                        if (SkillHandler.Instance.CheckValidAttackTarget(sActor, act2) && !realAffected2.Contains(act))
                            realAffected2.Add(act);
                }

                SkillHandler.Instance.MagicAttack(sActor, realAffected2, args, SkillHandler.DefType.IgnoreAll,
                    Elements.Earth, 10f);
            }
        }

        #endregion
    }
}