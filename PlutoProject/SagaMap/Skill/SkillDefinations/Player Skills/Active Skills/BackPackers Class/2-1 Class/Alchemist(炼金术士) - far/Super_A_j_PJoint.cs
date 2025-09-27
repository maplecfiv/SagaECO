using SagaDB.Actor;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._2_1_Class.Alchemist_炼金术士____far
{
    /// <summary>
    ///     强力援手（クリティカルアシスト）
    /// </summary>
    public class Super_A_T_PJoint : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            if (sActor.PossessionTarget != 0)
            {
                if (!dActor.Status.Additions.ContainsKey("Super_A_T_PJoint")) return 0;

                return -24;
            }

            return -23;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var life = 9999999; //应该是无限长时间
            Actor dActorReal = SkillHandler.Instance.GetPossesionedActor((ActorPC)sActor);
            var skill = new SPJointBuff(args.skill, sActor, dActorReal, life);
            SkillHandler.ApplyAddition(dActorReal, skill);
        }

        public class SPJointBuff : DefaultBuff
        {
            private Actor sActor;

            public SPJointBuff(SagaDB.Skill.Skill skill, Actor sActor, Actor actor, int lifetime)
                : base(skill, actor, "Super_A_T_PJoint", lifetime)
            {
                OnAdditionStart += StartEvent;
                OnAdditionEnd += EndEvent;
                this.sActor = sActor;
            }

            private void StartEvent(Actor actor, DefaultBuff skill)
            {
            }

            private void EndEvent(Actor actor, DefaultBuff skill)
            {
            }
        }

        #endregion
    }
}