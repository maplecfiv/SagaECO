﻿using SagaDB.Actor;
using SagaMap.ActorEventHandlers;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Event
{
    /// <summary>
    ///     I Love You
    /// </summary>
    public class ILoveYou : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var map = MapManager.Instance.GetMap(sActor.MapID);
            uint[] MobID = { 19010025, 19010026, 19010027 };
            uint[] SkillID = { 9152, 9153, 9154 };
            var id = SagaLib.Global.Random.Next(0, MobID.Length - 1);
            var m = map.SpawnMob(MobID[id],
                SagaLib.Global.PosX8to16(args.x, map.Width)
                , SagaLib.Global.PosY8to16(args.y, map.Height), 50, sActor);
            var mh = (MobEventHandler)m.e;
            mh.AI.CastSkill(SkillID[id], 1, sActor);
        }

        #endregion
    }
}