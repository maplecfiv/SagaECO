using System.Collections.Generic;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Global
{
    /// <summary>
    ///     各種種族傷害增加
    /// </summary>
    public class SomeKindDamUp : ISkill
    {
        private readonly string name;
        private readonly List<MobType> SomeKind;

        #region Constructers

        public SomeKindDamUp(string PassiveSkillName, List<MobType> kind)
        {
            SomeKind = kind;
            name = PassiveSkillName;
        }

        public SomeKindDamUp(string PassiveSkillName, params MobType[] types)
        {
            name = PassiveSkillName;
            SomeKind = new List<MobType>();
            foreach (var t in types) SomeKind.Add(t);
        }

        #endregion

        #region ISkill Members

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

        #endregion
    }
}