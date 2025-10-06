using SagaDB.Actor;
using SagaMap.ActorEventHandlers;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Event
{
    /// <summary>
    ///     招喚怪物並且使用技能
    /// </summary>
    public class SumMobCastSkill : ISkill
    {
        private readonly uint MobID;
        private readonly uint NextSkillID;

        public SumMobCastSkill(uint NextSkillID, uint MobID)
        {
            this.NextSkillID = NextSkillID;
            this.MobID = MobID;
        }

        public class SumMobCastSkillBuff : DefaultBuff
        {
            private readonly ActorMob mob;

            public SumMobCastSkillBuff(SagaDB.Skill.Skill skill, Actor actor, ActorMob mob, int lifetime)
                : base(skill, actor, "SumMobCastSkillBuff", lifetime)
            {
                OnAdditionStart += StartEvent;
                OnAdditionEnd += EndEvent;
                this.mob = mob;
            }

            private void StartEvent(Actor actor, DefaultBuff skill)
            {
            }

            private void EndEvent(Actor actor, DefaultBuff skill)
            {
                mob.ClearTaskAddition();
                var map = MapManager.Instance.GetMap(mob.MapID);
                map.DeleteActor(mob);
            }
        }

        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 5000;
            var map = MapManager.Instance.GetMap(sActor.MapID);
            map.GetRandomPosAroundActor(sActor);
            var mob = map.SpawnMob(MobID, SagaLib.Global.PosX8to16(args.x, map.Width),
                SagaLib.Global.PosY8to16(args.y, map.Height), 100, sActor);
            var skill = new SumMobCastSkillBuff(args.skill, dActor, mob, lifetime);
            SkillHandler.ApplyAddition(dActor, skill);
            var mh = (MobEventHandler)mob.e;
            mh.AI.CastSkill(NextSkillID, 1, sActor);
        }

        //#endregion
    }
}