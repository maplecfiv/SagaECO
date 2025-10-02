namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._2_1_Class.BladeMaster_剑圣____swm
{
    /// <summary>
    ///     巨木斷（巨木断ち）
    /// </summary>
    public class aWoodHack : BeheadSkill, ISkill
    {
        //#region ISkill Members

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            Proc(sActor, dActor, args, level, MobType.TREE);
        }

        //#endregion
    }
}