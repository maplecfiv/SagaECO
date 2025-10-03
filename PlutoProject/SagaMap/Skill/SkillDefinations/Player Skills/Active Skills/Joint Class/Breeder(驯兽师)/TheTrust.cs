using SagaDB.Actor;
using SagaLib;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Joint_Class.Breeder_驯兽师_
{
    /// <summary>
    ///     信頼の証（信頼の証）
    /// </summary>
    public class TheTrust : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var pet = SkillHandler.Instance.GetPet(sActor);
            if (pet != null)
            {
                MotionType[] actions =
                {
                    MotionType.BREAK, MotionType.JOY, MotionType.RELAX, MotionType.STAND, MotionType.NONE,
                    MotionType.DOGEZA
                };
                SkillHandler.Instance.NPCMotion(pet, actions[SagaLib.Global.Random.Next(0, actions.Length - 1)]);
            }
        }

        //#endregion
    }
}