using System.Collections.Generic;
using SagaLib;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.COF_Additions.巨大咕咕鸡
{
    internal class BlackHole : MobISkill
    {
        #region ISkill Members

        public void BeforeCast(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var actors = map.GetActorsArea(sActor, 900, false, true);
            var realAffected = new List<Actor>();
            foreach (var act in actors)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, act))
                    realAffected.Add(act);
            foreach (var a in realAffected)
            {
                var pos = new short[2] { sActor.X, sActor.Y };
                map.MoveActor(Map.MOVE_TYPE.START, a, pos, 1000, 1000, true, MoveType.QUICKEN);
                var 钝足 = new MoveSpeedDown(args.skill, a, 4000);
                SkillHandler.ApplyAddition(a, 钝足);
            }
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var actors = map.GetActorsArea(sActor, 600, false, true);
            var realAffected = new List<Actor>();
            foreach (var act in actors)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, act))
                    realAffected.Add(act);
            SkillHandler.Instance.MagicAttack(sActor, realAffected, args, Elements.Neutral, 3f);


            //if (sActor.Slave.Count <= 9)
            {
                for (var i = 0; i < 3; i++)
                {
                    var xy = map.GetRandomPosAroundActor2(sActor);
                    sActor.Slave.Add(map.SpawnMob(82000000, xy[0], xy[1], 2500, sActor));
                }
            }
        }

        #endregion
    }
}