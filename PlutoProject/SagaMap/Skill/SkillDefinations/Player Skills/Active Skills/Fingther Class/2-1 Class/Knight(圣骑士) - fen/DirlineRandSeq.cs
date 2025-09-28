namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._2_1_Class.Knight_圣骑士____fen
{
    /// <summary>
    ///     刺裂旋風（スピアサイクロン）
    /// </summary>
    public class DirlineRandSeq : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            if (SkillHandler.Instance.isEquipmentRight(sActor, ItemType.SPEAR, ItemType.RAPIER) ||
                sActor.Inventory.GetContainer(ContainerType.RIGHT_HAND2).Count > 0) return 0;
            return -5;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            uint DirlineRandSeq2_SkillID = 2382;
            args.autoCast.Add(SkillHandler.Instance.CreateAutoCastInfo(DirlineRandSeq2_SkillID, level, 0));
            args.autoCast.Add(SkillHandler.Instance.CreateAutoCastInfo(DirlineRandSeq2_SkillID, level, 560));
            args.autoCast.Add(SkillHandler.Instance.CreateAutoCastInfo(DirlineRandSeq2_SkillID, level, 1120));
        }

        #endregion
    }
}