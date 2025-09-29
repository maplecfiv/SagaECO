using System.Collections.Generic;
using SagaDB.Actor;
using SagaDB.Mob;

namespace SagaMap.Skill.Additions
{
    /// <summary>
    ///     知識用的Buff
    /// </summary>
    public class Knowledge : DefaultPassiveSkill
    {
        /// <summary>
        ///     怪物類型
        /// </summary>
        public List<MobType> MobTypes = new List<MobType>();

        public Knowledge(SagaDB.Skill.Skill skill, Actor actor, string name)
            : base(skill, actor, name, true)
        {
            OnAdditionStart += StartEvent;
            OnAdditionEnd += EndEvent;
        }

        public Knowledge(SagaDB.Skill.Skill skill, Actor actor, string name, int peroid, int lifetime)
            : base(skill, actor, name, true, peroid, lifetime)
        {
            OnAdditionStart += StartEvent;
            OnAdditionEnd += EndEvent;
        }


        public Knowledge(SagaDB.Skill.Skill skill, Actor actor, string name, params MobType[] mobTypes)
            : base(skill, actor, name, true)
        {
            OnAdditionStart += StartEvent;
            OnAdditionEnd += EndEvent;

            MobTypes.AddRange(mobTypes);
        }

        private void StartEvent(Actor actor, DefaultPassiveSkill skill)
        {
        }

        private void EndEvent(Actor actor, DefaultPassiveSkill skill)
        {
        }
    }
}