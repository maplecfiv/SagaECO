namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._2_1_Class.Striker_猎人____arc
{
    /// <summary>
    ///     猛鳥回音（シュリルボイス）
    /// </summary>
    public class PetBirdAtkRowCircle : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var pet = SkillHandler.Instance.GetPet(sActor);
            if (pet == null) return;
            if (SkillHandler.Instance.CheckMobType(pet, "BIRD"))
            {
                var ai = SkillHandler.Instance.GetMobAI(pet);
                ai.CastSkill(6501, level, dActor);
            }
        }

        #endregion
    }
}