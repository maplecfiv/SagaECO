using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Event
{
    /// <summary>
    ///     MOB召喚
    /// </summary>
    public class SumMob : ISkill
    {
        private readonly uint MobID;

        public SumMob(uint MobID)
        {
            this.MobID = MobID;
        }

        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var mob = map.SpawnMob(MobID, (short)(sActor.X + SagaLib.Global.Random.Next(1, 10))
                , (short)(sActor.Y + SagaLib.Global.Random.Next(1, 10))
                , 50, sActor);
            sActor.Slave.Add(mob);
        }

        #endregion
    }
}