using System.Collections.Generic;
using SagaDB.Actor;
using SagaMap.ActorEventHandlers;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.COF_Additions.丢弃
{
    internal class IceHole : MobISkill
    {
        //#region ISkill Members

        public void BeforeCast(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var actors = map.GetActorsArea(sActor, 1000, false, true);
            var realAffected = new List<Actor>();
            foreach (var act in actors)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, act))
                    realAffected.Add(act);
            foreach (var a in realAffected)
            {
                var 钝足 = new MoveSpeedDown(args.skill, a, 5000);
                SkillHandler.ApplyAddition(a, 钝足);
                var arg = new EffectArg();
                arg.effectID = 5078;
                arg.actorID = a.ActorID;
                MapManager.Instance.GetMap(sActor.MapID)
                    .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SHOW_EFFECT, arg, a, true);
            }

            if (sActor.type == ActorType.MOB) ((MobEventHandler)((ActorMob)sActor).e).AI.NextSurelySkillID = 20006;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var actors = map.GetActorsArea(sActor, 1000, false, true);
            var realAffected = new List<Actor>();
            foreach (var act in actors)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, act))
                    realAffected.Add(act);
            //SkillHandler.Instance.MagicAttack(sActor,realAffected,args, Elements.Water,2f);
            foreach (var act in realAffected)
            {
                var freeze = new Freeze(args.skill, act, 5000);
                SkillHandler.ApplyAddition(act, freeze);
                var arg = new EffectArg();
                arg.effectID = 5284;
                arg.actorID = act.ActorID;
                MapManager.Instance.GetMap(sActor.MapID)
                    .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SHOW_EFFECT, arg, act, true);
            }
        }

        //#endregion
    }
}