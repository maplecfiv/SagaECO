using SagaDB.Actor;

namespace SagaMap.Skill.SkillDefinations.Marionette
{
    /// <summary>
    ///     木偶時的HP自然恢復
    /// </summary>
    public class HPRecovery : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            //int lifetime = 1800000;
            var skill = new Additions.HPRecovery(args.skill, dActor, int.MaxValue, 5000, true);
            SkillHandler.ApplyAddition(dActor, skill);
            //DefaultBuff skill2 = new DefaultBuff(args.skill, dActor, "MarionetteHPRecovery", lifetime);
            //skill.OnAdditionStart += this.StartEventHandler;
            //skill.OnAdditionEnd += this.EndEventHandler;
            //SkillHandler.ApplyAddition(dActor, skill2);
        }

        //void StartEventHandler(Actor actor, DefaultBuff skill)
        //{
        //    actor.Status.hp_recover_skill += 15;
        //}
        //void EndEventHandler(Actor actor, DefaultBuff skill)
        //{
        //    actor.Status.hp_recover_skill -= 15;
        //}

        //#endregion
    }
}