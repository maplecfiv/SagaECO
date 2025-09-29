using SagaDB.Actor;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.Joint_Class.Breeder_驯兽师_
{
    public class LionPower : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var p = SkillHandler.Instance.GetPet(sActor);
            if (p != null)
            {
                var skill = new DefaultPassiveSkill(args.skill, p, "LionPower", true);
                skill.OnAdditionStart += StartEventHandler;
                skill.OnAdditionEnd += EndEventHandler;
                SkillHandler.ApplyAddition(sActor, skill);
            }
        }

        private void StartEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            //最大HP10%上昇 
            var hit_melee_add = (int)(actor.MaxHP * 0.1f);
            if (skill.Variable.ContainsKey("Encouragement_MaxHP"))
                skill.Variable.Remove("Encouragement_MaxHP");
            skill.Variable.Add("Encouragement_MaxHP", hit_melee_add);
            actor.MaxHP += (uint)hit_melee_add;
        }

        private void EndEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            //近命中
            actor.Status.hit_melee_skill -= (short)skill.Variable["Encouragement_MaxHP"];
        }

        #endregion
    }
}