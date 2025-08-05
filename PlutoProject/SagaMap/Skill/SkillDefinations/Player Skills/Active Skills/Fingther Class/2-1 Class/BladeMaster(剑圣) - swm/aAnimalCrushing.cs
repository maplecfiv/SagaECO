using SagaDB.Actor;
using SagaDB.Mob;

namespace SagaMap.Skill.SkillDefinations.BladeMaster
{
    /// <summary>
    ///     アギト砕き
    /// </summary>
    public class aAnimalCrushing : BeheadSkill, ISkill
    {
        #region ISkill Members

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            Proc(sActor, dActor, args, level, MobType.WATER_ANIMAL);
        }

        #endregion
    }
}