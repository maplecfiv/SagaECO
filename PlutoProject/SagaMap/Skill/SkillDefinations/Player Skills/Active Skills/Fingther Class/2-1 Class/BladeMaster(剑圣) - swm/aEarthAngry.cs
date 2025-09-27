using SagaDB.Actor;
using SagaDB.Mob;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._2_1_Class.BladeMaster_剑圣____swm
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