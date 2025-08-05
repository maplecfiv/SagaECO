using SagaDB.Actor;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._2_1_Class.Explorer_探险家____rag
{
    /// <summary>
    ///     灑灰（目潰し）
    /// </summary>
    public class Blinding : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 2.0f + 1.5f * level;
            SkillHandler.Instance.PhysicalAttack(sActor, dActor, args, sActor.WeaponElement, factor);
            var lifetime = 6000 + 2000 * level;
            int[] rates = { 0, 69, 78, 84 };
            if (SagaLib.Global.Random.Next(0, 99) < rates[level])
            {
                var skill = new DefaultBuff(args.skill, dActor, "Blinding", lifetime);
                skill.OnAdditionStart += StartEventHandler;
                skill.OnAdditionEnd += EndEventHandler;
                SkillHandler.ApplyAddition(dActor, skill);
            }
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            int level = skill.skill.Level;
            //近命中
            var hit_melee_add = -(int)(actor.Status.hit_melee * (0.32f + 0.04f * level));
            if (skill.Variable.ContainsKey("Blinding_hit_melee"))
                skill.Variable.Remove("Blinding_hit_melee");
            skill.Variable.Add("Blinding_hit_melee", hit_melee_add);
            actor.Status.hit_melee_skill += (short)hit_melee_add;

            //遠命中
            var hit_ranged_add = -(int)(actor.Status.hit_ranged * (0.27f + 0.04f * level));
            if (skill.Variable.ContainsKey("Blinding_hit_ranged"))
                skill.Variable.Remove("Blinding_hit_ranged");
            skill.Variable.Add("Blinding_hit_ranged", hit_ranged_add);
            actor.Status.hit_ranged_skill += (short)hit_ranged_add;
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            //近命中
            actor.Status.hit_melee_skill -= (short)skill.Variable["Blinding_hit_melee"];

            //遠命中
            actor.Status.hit_ranged_skill -= (short)skill.Variable["Blinding_hit_ranged"];
        }

        #endregion
    }
}