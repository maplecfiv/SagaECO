using SagaDB.Actor;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Eraser
{
    /// <summary>
    ///     影縫い
    /// </summary>
    public class ShadowSeam : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            if (sActor.Status.Additions.ContainsKey("ShadowSeam"))
                return -30;
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            int[] lifetime = { 0, 5000, 8000, 10000, 13000, 15000 };
            var skill = new DefaultBuff(args.skill, sActor, "ShadowSeam", lifetime[level]);
            SkillHandler.ApplyAddition(sActor, skill);
        }

        #endregion
    }
}