using SagaDB.Actor;
using SagaLib;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Global
{
    /// <summary>
    ///     各屬性守護(各属性契约)
    /// </summary>
    public class ElementLimitUp : ISkill
    {
        public Elements element;

        public ElementLimitUp(Elements e)
        {
            element = e;
        }

        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var active = true;
            var skill = new DefaultPassiveSkill(args.skill, sActor, element + "LimitUp", active);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(sActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            //int ElementAdd = 5 * skill.skill.Level;

            ////原屬性值
            //if (skill.Variable.ContainsKey("ElementLimitUp_Element"))
            //    skill.Variable.Remove("ElementLimitUp_Element");
            //skill.Variable.Add("ElementLimitUp_Element", actor.Elements[element]);

            //actor.Elements[element] += ElementAdd;
            //if (actor.Elements[element] > 215)
            //{
            //    actor.Elements[element] = 215;
            //}
        }

        private void EndEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            //原屬性值
            //actor.Elements[element] = (short)skill.Variable["ElementLimitUp_Element"];
        }

        #endregion
    }
}