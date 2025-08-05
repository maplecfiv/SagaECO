using SagaDB.Actor;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._2_1_Class.Knight_圣骑士____fen
{
    /// <summary>
    ///     防禦援手（ディフェンスアシスト）
    /// </summary>
    public class DJoint : ISkill
    {
        #region DJoint_Addition

        public class DJointBuff : DefaultBuff
        {
            private float rate;
            private Actor sActor;

            public DJointBuff(SagaDB.Skill.Skill skill, Actor sActor, Actor actor, int lifetime, float rate)
                : base(skill, actor, "DJoint", lifetime)
            {
                this.rate = rate;
                this.sActor = sActor;
                this["Target"] = (int)sActor.ActorID;
                OnAdditionStart += StartEvent;
                OnAdditionEnd += EndEvent;
            }

            private void StartEvent(Actor actor, DefaultBuff skill)
            {
                skill["Rate"] = 10 + 10 * skill.skill.Level;
                //Map map = Manager.MapManager.Instance.GetMap(actor.MapID);
                //actor.Buff.Sleep = true;
                //map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
            }

            private void EndEvent(Actor actor, DefaultBuff skill)
            {
                skill["Rate"] = 0;
                //Map map = Manager.MapManager.Instance.GetMap(actor.MapID);
                //actor.Buff.Sleep = false;
                //map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
            }
        }

        #endregion

        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            //使用條件：憑依時
            if (sActor.PossessionTarget != 0)
            {
                if (!dActor.Status.Additions.ContainsKey("A_T_DJoint")) return 0;

                return -24;
            }

            return -23;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 60000 - 10000 * level;
            var f = 0.1f + 0.1f * level;
            var skill = new DJointBuff(args.skill, sActor, dActor, lifetime, f);
            SkillHandler.ApplyAddition(dActor, skill);
        }

        #endregion
    }
}