using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._2_1_Class.Elementaler_元素使____sha
{
    /// <summary>
    ///     魔法極大化（ゼン）
    /// </summary>
    public class Zen : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            if (sActor.type == ActorType.PC)
            {
                dActor = SkillHandler.Instance.GetPossesionedActor((ActorPC)sActor);
                var lifetime = new[] { 0, 15000, 25000, 35000, 45000, 45000, 60000 };
                var skill = new DefaultBuff(args.skill, dActor, "Zensss", lifetime[level]);
                skill.OnAdditionStart += StartEventHandler;
                skill.OnAdditionEnd += EndEventHandler;
                SkillHandler.ApplyAddition(dActor, skill);
            }
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            var ZenMagPowerUp = 1.0f + 0.2f * skill.skill.Level;

            if (skill.Variable.ContainsKey("Zensss"))
                skill.Variable.Remove("Zensss");
            skill.Variable.Add("Zensss", (int)(ZenMagPowerUp * 10));

            actor.ZenOutLst.Add(3016);
            actor.ZenOutLst.Add(3040);
            actor.ZenOutLst.Add(3053);
            actor.ZenOutLst.Add(3028);
            actor.ZenOutLst.Add(3260);
            actor.ZenOutLst.Add(3264);
            actor.ZenOutLst.Add(3265);
            actor.ZenOutLst.Add(3301);
            actor.ZenOutLst.Add(3302);
            actor.ZenOutLst.Add(3296);
            actor.ZenOutLst.Add(3409);
            actor.ZenOutLst.Add(7746);
            actor.ZenOutLst.Add(3433);
            actor.ZenOutLst.Add(3432);
            actor.ZenOutLst.Add(3409);
            actor.ZenOutLst.Add(3398);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            if (skill.Variable.ContainsKey("Zensss"))
                skill.Variable.Remove("Zensss");
            actor.ZenOutLst.Clear();
        }

        #endregion
    }
}