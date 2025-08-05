using SagaDB.Actor;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.TreasureHunter
{
    /// <summary>
    ///     警戒（警戒）
    /// </summary>
    public class Caution : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 1.0f;
            SkillHandler.Instance.PhysicalAttack(sActor, dActor, args, sActor.WeaponElement, factor);
            var lifetime = 1000 * level;
            var skill = new DefaultBuff(args.skill, dActor, "Caution", lifetime);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(dActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            int level = skill.skill.Level;
            //左防禦
            var def_add = 10 + 2 * level;
            if (skill.Variable.ContainsKey("Caution_def"))
                skill.Variable.Remove("Caution_def");
            skill.Variable.Add("Caution_def", def_add);
            actor.Status.def_skill += (short)def_add;
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            //左防禦
            actor.Status.def_skill -= (short)skill.Variable["Caution_def"];
        }

        #endregion
    }
}