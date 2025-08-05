using SagaDB.Actor;
using SagaDB.Mob;

namespace SagaMap.Skill.SkillDefinations.BladeMaster
{
    /// <summary>
    ///     隼之太刀（隼の太刀）
    /// </summary>
    public class aFalconLongSword : BeheadSkill, ISkill
    {
        #region ISkill Members

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            Proc(sActor, dActor, args, level, MobType.WATER_ANIMAL);
        }

        #endregion
    }
}