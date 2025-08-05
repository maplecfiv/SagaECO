using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.Additions;
using SagaMap.Skill.SkillDefinations;

namespace SagaMap.Skill.NewSkill.FR1
{
    /// <summary>
    ///     集中 コンセントレート
    /// </summary>
    public class HitMeleeUp : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            args.dActor = sActor.ActorID;
            var life = 180000;
            var skill = new DefaultBuff(args.skill, dActor, "HitMeleeUp", life);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(dActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            var value = 5 * skill.skill.Level;
            if (skill.Variable.ContainsKey("HitMeleeUp"))
                skill.Variable.Remove("HitMeleeUp");
            skill.Variable.Add("HitMeleeUp", value);
            actor.Status.hit_melee_skill += (short)value;

            actor.Buff.ShortHitUp = true;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            var value = skill.Variable["HitMeleeUp"];
            actor.Status.hit_melee_skill -= (short)value;

            if (skill.Variable.ContainsKey("HitMeleeUp"))
                skill.Variable.Remove("HitMeleeUp");

            actor.Buff.ShortHitUp = false;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        #endregion
    }
}