using SagaDB.Actor;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.Fingther_Class._2_1_Class.BladeMaster_剑圣____swm
{
    /// <summary>
    ///     狂戰士（バーサーク）
    /// </summary>
    public class P_BERSERK : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lowHP = (int)(sActor.MaxHP * (0.05f + 0.05f * level));
            var active = false;
            if (sActor.HP <= lowHP) active = true;
            var skill = new DefaultPassiveSkill(args.skill, sActor, "P_BERSERK", active);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(sActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            if (skill.Activated)
            {
                var bs = new Berserk(skill.skill, actor, 30000);
                SkillHandler.ApplyAddition(actor, bs);
            }
        }

        private void EndEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
        }

        //#endregion
    }
}