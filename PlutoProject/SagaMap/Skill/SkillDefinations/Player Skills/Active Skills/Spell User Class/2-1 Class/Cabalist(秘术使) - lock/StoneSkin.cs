using SagaDB.Actor;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Cabalist
{
    /// <summary>
    ///     石化皮膚（メデューサスキン）
    /// </summary>
    public class StoneSkin : ISkill
    {
        private readonly bool MobUse;

        public StoneSkin()
        {
            MobUse = false;
        }

        public StoneSkin(bool MobUse)
        {
            this.MobUse = MobUse;
        }

        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            if (MobUse) level = 5;
            int[] lifetime = { 0, 30000, 30000, 45000, 45000, 60000 };
            var skill = new DefaultBuff(args.skill, dActor, "StoneSkin", lifetime[level]);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(dActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
        }

        #endregion
    }
}