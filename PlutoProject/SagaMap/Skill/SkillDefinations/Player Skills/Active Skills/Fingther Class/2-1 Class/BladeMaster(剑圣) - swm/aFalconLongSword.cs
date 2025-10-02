namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._2_1_Class.BladeMaster_剑圣____swm
{
    /// <summary>
    ///     隼之太刀（隼の太刀）
    /// </summary>
    public class aFalconLongSword : BeheadSkill, ISkill
    {
        //#region ISkill Members

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            Proc(sActor, dActor, args, level, MobType.WATER_ANIMAL);
        }

        //#endregion
    }
}