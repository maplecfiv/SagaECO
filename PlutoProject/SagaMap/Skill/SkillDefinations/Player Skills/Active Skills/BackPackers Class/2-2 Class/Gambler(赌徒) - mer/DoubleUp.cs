using SagaDB.Actor;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._2_2_Class.Gambler_赌徒____mer
{
    /// <summary>
    ///     意志昂揚（ダブルアップ）
    /// </summary>
    public class DoubleUp : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var active = true;
            var skill = new DefaultPassiveSkill(args.skill, sActor, "DoubleUp", active);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(sActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            actor.Status.doubleUpList.Add(2127); //連續投擲
            actor.Status.doubleUpList.Add(3286); //擲骰子
            actor.Status.doubleUpList.Add(3287); //幸福輪盤
            actor.Status.doubleUpList.Add(2374); //卡飛旋鏢
            actor.Status.doubleUpList.Add(2375); //一擲千金
        }

        private void EndEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            actor.Status.doubleUpList.Clear();
        }

        //#endregion
    }
}