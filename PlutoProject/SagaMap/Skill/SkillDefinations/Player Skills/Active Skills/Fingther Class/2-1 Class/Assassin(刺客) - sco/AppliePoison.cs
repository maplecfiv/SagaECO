using SagaDB.Actor;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._2_1_Class.Assassin_刺客____sco
{
    /// <summary>
    ///     滲毒（アプライポイズン）
    /// </summary>
    public class AppliePoison : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            uint itemID = 10000302; //毒藥
            if (SkillHandler.Instance.CountItem(sActor, itemID) > 0)
            {
                SkillHandler.Instance.TakeItem(sActor, itemID, 1);
                return 0;
            }

            return -2;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 50000 * level;
            var skill = new DefaultBuff(args.skill, dActor, "AppliePoison", lifetime);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(dActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
        }

        //#endregion
    }
}