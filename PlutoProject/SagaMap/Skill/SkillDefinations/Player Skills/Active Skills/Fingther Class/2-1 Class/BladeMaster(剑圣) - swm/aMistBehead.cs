using SagaDB.Actor;
using SagaDB.Mob;

namespace SagaMap.Skill.SkillDefinations.BladeMaster
{
    /// <summary>
    ///     飛霞斬（霞斬り）
    /// </summary>
    public class aMistBehead : BeheadSkill, ISkill
    {
        #region ISkill Members

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            Proc(sActor, dActor, args, level, MobType.BIRD);
        }

        #endregion
    }
}