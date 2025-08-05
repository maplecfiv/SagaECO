using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._2_1_Class.Knight_圣骑士____fen
{
    /// <summary>
    ///     遠距離防護（ディフレクトアロー）
    /// </summary>
    public class DifrectArrow : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            //需裝備盾牌
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 1000 * (35 - 5 * level);
            var skill = new DefaultBuff(args.skill, dActor, "DifrectArrow", lifetime);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(dActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            int level = skill.skill.Level;
            //遠距迴避
            var avoid_ranged_add = 10 + 10 * level;
            if (skill.Variable.ContainsKey("DifrectArrow_avoid_ranged"))
                skill.Variable.Remove("DifrectArrow_avoid_ranged");
            skill.Variable.Add("DifrectArrow_avoid_ranged", avoid_ranged_add);
            actor.Status.avoid_ranged_skill += (short)avoid_ranged_add;

            actor.Buff.LongDodgeUp = true;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            //遠距迴避
            actor.Status.avoid_ranged_skill -= (short)skill.Variable["DifrectArrow_avoid_ranged"];

            actor.Buff.LongDodgeUp = false;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        #endregion
    }
}