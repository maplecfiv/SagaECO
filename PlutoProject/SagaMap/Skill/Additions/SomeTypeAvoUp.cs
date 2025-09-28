using System.Collections.Generic;

namespace SagaMap.Skill.Additions
{
    /// <summary>
    ///     當怪物為某系列時，迴避率提升
    /// </summary>
    public class SomeTypeAvoUp : DefaultPassiveSkill
    {
        /// <summary>
        ///     怪物類型
        /// </summary>
        public Dictionary<MobType, ushort> MobTypes = new Dictionary<MobType, ushort>();

        public SomeTypeAvoUp(SagaDB.Skill.Skill skill, Actor actor, string name)
            : base(skill, actor, name, true)
        {
            OnAdditionStart += StartEvent;
            OnAdditionEnd += EndEvent;
        }

        public SomeTypeAvoUp(SagaDB.Skill.Skill skill, Actor actor, string name, int peroid, int lifetime)
            : base(skill, actor, name, true, peroid, lifetime)
        {
            OnAdditionStart += StartEvent;
            OnAdditionEnd += EndEvent;
        }

        public void AddMobType(MobType type, ushort addValue)
        {
            MobTypes.Add(type, addValue);
        }

        private void StartEvent(Actor actor, DefaultPassiveSkill skill)
        {
        }

        private void EndEvent(Actor actor, DefaultPassiveSkill skill)
        {
        }
    }
}