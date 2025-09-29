using SagaDB.Actor;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.Joint_Class.Gardener_庭园师_
{
    /// <summary>
    ///     トピアリー
    /// </summary>
    public class Topiary : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var active = true;
            var skill = new DefaultPassiveSkill(args.skill, sActor, "Topiary", active);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(sActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            /*
             * トピアリー †
                Passive
                習得JOBLV：23
                効果：「トピアリーツリー」を自在に刈り取ることができる。
                トピアリーツリーはタイニー、インスマウス、マンドラニンジンの形に刈り取れる。
                タイニー、インスマウス、マンドラニンジンの形には何度でも刈り取り戻すことが可能。
             */
        }

        private void EndEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
        }

        #endregion
    }
}