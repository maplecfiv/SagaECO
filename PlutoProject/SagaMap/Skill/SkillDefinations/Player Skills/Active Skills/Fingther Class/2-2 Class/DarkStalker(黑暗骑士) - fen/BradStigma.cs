using SagaDB.Actor;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._2_2_Class.DarkStalker_黑暗骑士____fen
{
    /// <summary>
    /// </summary>
    /// 血的烙印（血の烙印）
    public class BradStigma : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 25000;
            SkillHandler.Instance.AttractMob(sActor, dActor);
            var skill = new DefaultBuff(args.skill, dActor, "BradStigma", lifetime);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(dActor, skill);


            //闇屬性傷害提升 16% 20% 24% 28% 32% 
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            var factor = new[] { 0, 16, 20, 24, 28, 32 }[skill.skill.Level];
            if (skill.Variable.ContainsKey("BradStigma"))
                skill.Variable.Remove("BradStigma");
            skill.Variable.Add("BradStigma", factor);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            if (skill.Variable.ContainsKey("BradStigma")) skill.Variable.Remove("BradStigma");
        }

        #endregion
    }
}