using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._2_1_Class.Blacksmith_铁匠____tat
{
    /// <summary>
    ///     岩鎖錠（ロッククラッシャー）
    /// </summary>
    public class RockCrash : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            if (SkillHandler.Instance.CheckValidAttackTarget(sActor, dActor)) return 0;

            return -14;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 1.0f;
            if (dActor is ActorMob)
            {
                var dActorMob = (ActorMob)dActor;
                if (dActorMob.BaseData.mobType.ToString().ToLower().IndexOf("rock") > -1)
                    //加成
                    factor = factor + 1.0f * level;
            }

            SkillHandler.Instance.PhysicalAttack(sActor, dActor, args, sActor.WeaponElement, factor);
            var rate = 10 + 10 * level;
            if (SkillHandler.Instance.CanAdditionApply(sActor, dActor, SkillHandler.DefaultAdditions.Stiff, rate))
            {
                var skill = new Stiff(args.skill, dActor, 5000);
                SkillHandler.ApplyAddition(dActor, skill);
            }
        }

        //#endregion
    }
}