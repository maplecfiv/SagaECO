using SagaDB.Actor;
using SagaDB.Mob;

namespace SagaMap.Skill.SkillDefinations.BladeMaster
{
    /// <summary>
    ///     巨木斷（巨木断ち）
    /// </summary>
    public class aWoodHack : BeheadSkill, ISkill
    {
        #region ISkill Members

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            Proc(sActor, dActor, args, level, MobType.TREE);
        }

        #endregion
    }
}