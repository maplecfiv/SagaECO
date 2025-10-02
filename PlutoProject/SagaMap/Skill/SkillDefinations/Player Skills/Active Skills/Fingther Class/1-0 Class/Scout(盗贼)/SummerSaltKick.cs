using System.Collections.Generic;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._1_0_Class.Scout_盗贼_
{
    /// <summary>
    ///     空中迴旋腿
    /// </summary>
    public class SummerSaltKick : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            float factor = 0;
            factor = 0.75f + 0.25f * level;
            var actorPC = (ActorPC)sActor;
            if (level == 6)
            {
                factor = 3f;
                var target = new List<Actor>();
                for (var i = 0; i < 3; i++) target.Add(dActor);
                SkillHandler.Instance.PushBack(sActor, dActor, 7);
                var skill = new Stiff(args.skill, dActor, 3000);
                SkillHandler.ApplyAddition(dActor, skill);
                SkillHandler.Instance.PhysicalAttack(sActor, target, args, sActor.WeaponElement, factor);
            }
            else
            {
                SkillHandler.Instance.PushBack(sActor, dActor, 3);
                SkillHandler.Instance.PhysicalAttack(sActor, dActor, args, sActor.WeaponElement, factor);
            }
        }

        //#endregion
    }
}