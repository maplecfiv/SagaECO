using SagaDB.Actor;
using SagaLib;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Global.Passive
{
    /// <summary>
    ///     各種屬性魂
    /// </summary>
    public class ElementAddUp : ISkill
    {
        private readonly string name;
        private Elements theElement = Elements.Neutral;

        public ElementAddUp(Elements e, string Name)
        {
            name = Name;
            theElement = e;
        }

        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var active = true;
            var skill = new DefaultPassiveSkill(args.skill, sActor, name, active);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(sActor, skill);
        }

        public void StartEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
        }

        public void EndEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
        }

        //#endregion
    }
}