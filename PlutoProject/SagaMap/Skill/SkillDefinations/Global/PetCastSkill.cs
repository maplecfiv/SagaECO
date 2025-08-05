using SagaDB.Actor;

namespace SagaMap.Skill.SkillDefinations.Global
{
    /// <summary>
    ///     讓寵物使用指定技能
    /// </summary>
    public class PetCastSkill : ISkill
    {
        private readonly string MobType;
        private readonly uint NextSkillID;

        /// <summary>
        ///     初始化
        /// </summary>
        /// <param name="NextSkillID">寵物技能ID</param>
        /// <param name="MobType">寵物種類</param>
        public PetCastSkill(uint NextSkillID, string PetType)
        {
            this.NextSkillID = NextSkillID;
            MobType = PetType;
        }

        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var pet = SkillHandler.Instance.GetPet(sActor);
            if (pet == null) return;
            if (SkillHandler.Instance.CheckMobType(pet, MobType))
            {
                var ai = SkillHandler.Instance.GetMobAI(pet);
                ai.CastSkill(NextSkillID, level, dActor);
            }
        }

        #endregion
    }
}