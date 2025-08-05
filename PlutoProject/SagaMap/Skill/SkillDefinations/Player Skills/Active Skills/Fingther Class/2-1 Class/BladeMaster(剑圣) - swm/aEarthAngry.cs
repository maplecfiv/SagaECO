using SagaDB.Actor;
using SagaDB.Mob;

namespace SagaMap.Skill.SkillDefinations.BladeMaster
{
    /// <summary>
    ///     大地之怒（兜割り）
    /// </summary>
    public class aEarthAngry : BeheadSkill, ISkill
    {
        #region ISkill Members

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            Proc(sActor, dActor, args, level, MobType.INSECT);
        }

        #endregion
    }
}