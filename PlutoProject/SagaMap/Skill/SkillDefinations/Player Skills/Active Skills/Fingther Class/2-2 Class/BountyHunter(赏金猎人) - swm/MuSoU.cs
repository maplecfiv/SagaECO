namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._2_2_Class.BountyHunter_赏金猎人____swm
{
    /// <summary>
    ///     參擊無雙（斬撃無双）
    /// </summary>
    public class MuSoU : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            if (SkillHandler.Instance.CheckValidAttackTarget(sActor, dActor)) return 0;

            return -14;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            sActor.MuSoUCount = 0;

            for (var i = 1; i <= 10; i++)
            {
                var aci = SkillHandler.Instance.CreateAutoCastInfo(2402, level, 250);
                args.autoCast.Add(aci);
            }
            //SkillHandler.Instance.PushBack(sActor, dActor, 2);
        }

        //#endregion
    }
}