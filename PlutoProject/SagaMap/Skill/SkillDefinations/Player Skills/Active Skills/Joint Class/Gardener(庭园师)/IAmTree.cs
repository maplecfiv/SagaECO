using SagaDB.Actor;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Gardener
{
    /// <summary>
    ///     わたしは木
    /// </summary>
    public class IAmTree : ISkill
    {
        private readonly uint TreeMobID = 30040003; //小樹

        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = int.MaxValue;
            var skill = new DefaultBuff(args.skill, dActor, "IAmTree", lifetime);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(dActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            SkillHandler.Instance.TranceMob(actor, TreeMobID);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            SkillHandler.Instance.TranceMob(actor, 0);
        }

        #endregion
    }
}