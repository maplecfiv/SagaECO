using SagaDB.Actor;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._2_1_Class.Assassin_刺客____sco
{
    /// <summary>
    ///     死神晚宴 | 背刺（バックアタック）
    /// </summary>
    public class BackAtk : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            float factor = 0;
            //背面
            if (SkillHandler.Instance.GetIsBack(sActor, dActor))
                factor = new[] { 0, 2.1f, 2.5f, 3.0f, 3.5f, 4.0f }[level];
            else
                factor = 1.1f;

            SkillHandler.Instance.PhysicalAttack(sActor, dActor, args, sActor.WeaponElement, factor);
            if (SkillHandler.Instance.CanAdditionApply(sActor, dActor, SkillHandler.DefaultAdditions.Stiff, 95))
            {
                var stiff = new Stiff(args.skill, dActor, 3000);
                SkillHandler.ApplyAddition(dActor, stiff);
            }
        }

        //#endregion
    }
}