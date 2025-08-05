using SagaDB.Actor;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._2_2_Class.Sage_贤者____wiz
{
    /// <summary>
    ///     敏捷頭腦（インテレクトライズ）
    /// </summary>
    public class IntelRides : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            if (sActor.ActorID == dActor.ActorID) return -14;
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = (int)(7.5 + 7.5 * level) * 1000;
            var skill = new DefaultBuff(args.skill, dActor, "IntelRides", lifetime);
            var pc = (ActorPC)sActor;
            if (skill.Variable.ContainsKey("IntelRides_INT"))
                skill.Variable.Remove("IntelRides_INT");
            skill.Variable.Add("IntelRides_INT", pc.Int);

            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(dActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.Status.int_skill += (short)skill.Variable["IntelRides_INT"];
            //Manager.MapManager.Instance.GetMap(actor.MapID).SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.Status.int_skill -= (short)skill.Variable["IntelRides_INT"];
            // Manager.MapManager.Instance.GetMap(actor.MapID).SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        #endregion
    }
}