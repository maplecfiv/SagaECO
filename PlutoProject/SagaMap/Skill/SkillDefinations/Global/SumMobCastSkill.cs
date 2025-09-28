using System.Collections.Generic;
using SagaMap.ActorEventHandlers;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Global
{
    /// <summary>
    ///     招換怪物之後使用技能
    /// </summary>
    public class SumMobCastSkill : ISkill
    {
        private readonly uint MobID;
        private readonly Dictionary<uint, int> NextSkill = new Dictionary<uint, int>();

        public SumMobCastSkill(uint MobID, uint SkillID)
        {
            this.MobID = MobID;
            NextSkill.Add(SkillID, 100);
        }

        public SumMobCastSkill(uint MobID, uint SkillID1, int rate1, uint SkillID2, int rate2)
        {
            this.MobID = MobID;
            NextSkill.Add(SkillID1, rate1);
            NextSkill.Add(SkillID2, rate2);
        }

        #region ISkill

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var m = map.SpawnMob(MobID,
                SagaLib.Global.PosX8to16(args.x, map.Width)
                , SagaLib.Global.PosY8to16(args.y, map.Height), 50, sActor);
            var mh = (MobEventHandler)m.e;
            uint CastSkillID = 0;
            var totalrate = 0;
            foreach (var p in NextSkill) totalrate += p.Value;
            var rate = SagaLib.Global.Random.Next(0, totalrate);
            totalrate = 0;
            foreach (var p in NextSkill)
            {
                totalrate += p.Value;
                if (totalrate > rate) CastSkillID = p.Key;
            }

            mh.AI.CastSkill(CastSkillID, 1, sActor);
            sActor.Slave.Add(m);
        }

        #endregion
    }
}