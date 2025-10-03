using SagaDB.Actor;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._2_2_Class.DarkStalker_黑暗骑士____fen
{
    /// <summary>
    ///     黑暗騎士（ダークネスオブナイト）
    /// </summary>
    public class DarknessOfNight : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            for (var i = 0; i < 5; i++)
                args.autoCast.Add(SkillHandler.Instance.CreateAutoCastInfo(2407, level, 560 * i));
        }

        //#endregion
    }
}