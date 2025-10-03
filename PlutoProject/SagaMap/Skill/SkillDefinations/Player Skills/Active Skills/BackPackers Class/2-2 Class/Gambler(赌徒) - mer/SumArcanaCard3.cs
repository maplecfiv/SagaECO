using SagaDB.Actor;
using SagaMap.ActorEventHandlers;
using SagaMap.Manager;
using SagaMap.Mob;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._2_2_Class.Gambler_赌徒____mer
{
    /// <summary>
    ///     艾卡卡（アルカナカード）[接續技能]
    /// </summary>
    public class SumArcanaCard3 : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 8000;
            uint MobID = 10310006; //艾卡納J牌
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var mob = map.SpawnMob(MobID,
                (short)(sActor.X + SagaLib.Global.Random.Next(1, 11)),
                (short)(sActor.Y + SagaLib.Global.Random.Next(1, 11)),
                2500, sActor);
            var mh = (MobEventHandler)mob.e;
            mh.AI.Mode = new AIMode(1);
            var skill = new SumArcanaCardBuff(args.skill, sActor, mob, lifetime);
            SkillHandler.ApplyAddition(sActor, skill);
        }

        public class SumArcanaCardBuff : DefaultBuff
        {
            private readonly ActorMob mob;

            public SumArcanaCardBuff(SagaDB.Skill.Skill skill, Actor actor, ActorMob mob, int lifetime)
                : base(skill, actor, "SumArcanaCard", lifetime)
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
                var map = MapManager.Instance.GetMap(actor.MapID);
                mob.ClearTaskAddition();
                map.DeleteActor(mob);
            }
        }

        //#endregion
    }
}