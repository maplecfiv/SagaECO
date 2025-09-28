using System.Collections.Generic;
using SagaLib;
using SagaMap.ActorEventHandlers;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.COF_Additions.领主骑士
{
    public class IceHeart : MobISkill
    {
        #region ISkill Members

        public void BeforeCast(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var actors = map.GetActorsArea(dActor, 3000, false, true);
            var realAffected = new List<Actor>();
            var ec = new ActorPC();
            foreach (var act in actors)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, act))
                    realAffected.Add(act);
            foreach (var acs in realAffected)
                if (acs.type == ActorType.PC)
                {
                    var ac = (ActorPC)acs;
                    if (ac.Job == PC_JOB.VATES || ac.Job == PC_JOB.DRUID || ac.Job == PC_JOB.CARDINAL ||
                        ac.Job == PC_JOB.BARD)
                        ec = ac;
                }

            if (dActor.type == ActorType.PC)
                if (ec.CharID == 0)
                    ec = (ActorPC)dActor;
            if (ec != null && sActor.type == ActorType.MOB)
            {
                var mob = (ActorMob)sActor;
                var hate = ((MobEventHandler)mob.e).AI.Hate[dActor.ActorID] + 500;
                if (((MobEventHandler)mob.e).AI.Hate.ContainsKey(ec.ActorID))
                    ((MobEventHandler)mob.e).AI.Hate[ec.ActorID] = hate;
                else
                    ((MobEventHandler)mob.e).AI.Hate.Add(ec.ActorID, hate);
                ((MobEventHandler)mob.e).AI.NextSurelySkillID = 20012;
            }

            var pos = new short[2];
            pos[0] = ec.X;
            pos[1] = ec.Y;
            map.MoveActor(Map.MOVE_TYPE.START, sActor, pos, 20000, 20000, true);

            var stun = new Stun(args.skill, ec, 5000);
            SkillHandler.ApplyAddition(ec, stun);

            SkillHandler.Instance.MagicAttack(sActor, ec, args, Elements.Neutral, 3f);
        }

        #endregion
    }
}