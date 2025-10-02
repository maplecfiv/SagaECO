namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._2_2_Class.Command_特工____sco
{
    /// <summary>
    ///     定時炸彈（セットボム）
    /// </summary>
    public class SetBomb : ISkill
    {
        //#region ISkill Members

        private readonly uint itemID = 10022307;

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            if (SkillHandler.Instance.CheckValidAttackTarget(sActor, dActor))
            {
                if (SkillHandler.Instance.CountItem(sActor, itemID) > 0)
                {
                    SkillHandler.Instance.TakeItem(sActor, itemID, 1);
                    return 0;
                }

                return -2;
            }

            return -14;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            SkillHandler.Instance.PhysicalAttack(sActor, dActor, args, sActor.WeaponElement, 1.0f);
            args.autoCast.Add(SkillHandler.Instance.CreateAutoCastInfo(2378, level, 3000));
        }

        //#endregion
    }
}