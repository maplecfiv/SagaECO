using SagaDB.Actor;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Monster
{
    /// <summary>
    ///     ソーマミラージュ (索玛幻影?)(无属性抗性100%)
    /// </summary>
    public class SomaMirage : ISkill, MobISkill
    {
        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            //sActor.Status.autoReviveRate
            //List<Actor> affected = new List<Actor>();
            //List<Actor> inrange = Manager.MapManager.Instance.GetMap(dActor.MapID).GetActorsArea(dActor, 500, false);
            //foreach (var item in inrange)
            //{
            //    if (SkillHandler.Instance.CheckValidAttackTarget(sActor, item))
            //        affected.Add(item);
            //}
            //SkillHandler.Instance.FixAttack(sActor, affected, args, SagaLib.Elements.Neutral, 5000);
            var lifetime = 30000;
            var skill = new DefaultBuff(args.skill, dActor, "SomaMirage", lifetime);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(dActor, skill);
        }

        public void BeforeCast(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.Status.NeutralDamegeDown_rate = 100;
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.Status.NeutralDamegeDown_rate = 0;
        }
    }
}