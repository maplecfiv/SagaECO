using SagaDB.Actor;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Command
{
    /// <summary>
    ///     特攻武術修練（体術マスタリー）
    /// </summary>
    public class MartialArtDamUp : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var skill = new DefaultPassiveSkill(args.skill, sActor, "MartialArtDamUp", true);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(sActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            int level = skill.skill.Level;
            var add = (short)(10 * level);
            if (skill.Variable.ContainsKey("MartialArtDamUp"))
                skill.Variable.Remove("MartialArtDamUp");
            skill.Variable.Add("MartialArtDamUp", add);
        }

        private void EndEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            if (skill.Variable.ContainsKey("MartialArtDamUp"))
                skill.Variable.Remove("MartialArtDamUp");
        }

        #endregion
    }
}