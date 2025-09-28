using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._2_1_Class.Striker_猎人____arc
{
    /// <summary>
    ///     猛鳥回音（シュリルボイス）[接續技能]
    /// </summary>
    public class PetBirdAtkRowCircle2 : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 30000;
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var affected = map.GetActorsArea(sActor, 200, false);
            foreach (var act in affected)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, act))
                {
                    var skill = new DefaultBuff(args.skill, act, "PetBirdAtkRowCircle", lifetime);
                    skill.OnAdditionStart += StartEventHandler;
                    skill.OnAdditionEnd += EndEventHandler;
                    SkillHandler.ApplyAddition(act, skill);
                    var skill2 = new Confuse(args.skill, act, lifetime);
                    SkillHandler.ApplyAddition(act, skill2);
                }
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            int level = skill.skill.Level;
            //近命中
            var hit_melee_add = -(6 * level);
            if (skill.Variable.ContainsKey("PetBirdAtkRowCircle_hit_melee"))
                skill.Variable.Remove("PetBirdAtkRowCircle_hit_melee");
            skill.Variable.Add("PetBirdAtkRowCircle_hit_melee", hit_melee_add);
            actor.Status.hit_melee_skill += (short)hit_melee_add;
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            //近命中
            actor.Status.hit_melee_skill -= (short)skill.Variable["PetBirdAtkRowCircle_hit_melee"];
        }

        #endregion
    }
}