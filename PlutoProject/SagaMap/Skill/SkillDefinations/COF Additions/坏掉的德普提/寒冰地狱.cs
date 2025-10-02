using System.Collections.Generic;
using SagaDB.Actor;
using SagaLib;
using SagaMap.ActorEventHandlers;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.COF_Additions.坏掉的德普提
{
    internal class IceInfernal : MobISkill
    {
        //#region ISkill Members

        public void BeforeCast(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            if (sActor.type == ActorType.MOB)
            {
                var map = MapManager.Instance.GetMap(sActor.MapID);
                var mob = (ActorMob)sActor;
                var mobe = (MobEventHandler)mob.e;
                var ids = new List<uint>();
                if (mobe.AI.Hate.Count > 0)
                {
                    foreach (var aid in mobe.AI.Hate.Keys) ids.Add(aid);
                    var id = ids[SagaLib.Global.Random.Next(0, ids.Count)];
                    var da = map.GetActor(id);
                    if (da != null)
                        dActor = da;
                }
            }
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 5f;
            var actorS = new ActorSkill(args.skill, sActor);
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var actors = map.GetActorsArea(SagaLib.Global.PosX8to16(args.x, map.Width),
                SagaLib.Global.PosY8to16(args.y, map.Height), 300, null);
            var affected = new List<Actor>();
            foreach (var i in actors)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, i))
                {
                    var freeze = new Freeze(args.skill, i, 5000);
                    SkillHandler.ApplyAddition(i, freeze);
                    affected.Add(i);
                }

            SkillHandler.Instance.MagicAttack(sActor, affected, args, Elements.Water, factor);
        }

        //#endregion
    }
}