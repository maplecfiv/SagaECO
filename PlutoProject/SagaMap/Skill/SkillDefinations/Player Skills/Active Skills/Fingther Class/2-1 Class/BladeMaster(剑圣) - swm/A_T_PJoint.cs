using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._2_1_Class.BladeMaster_剑圣____swm
{
    /// <summary>
    ///     攻擊援手（アタックアシスト）
    /// </summary>
    public class A_T_PJoint : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            if (sActor.PossessionTarget != 0)
            {
                dActor = SkillHandler.Instance.GetPossesionedActor(sActor);
                if (!dActor.Status.Additions.ContainsKey("A_T_PJoint")) return 0;

                return -24;
            }

            return -23;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var life = 15000 + level * 5000;
            Actor dActorReal = SkillHandler.Instance.GetPossesionedActor((ActorPC)sActor);
            var skill = new PJointBuff(args.skill, sActor, dActorReal, life);
            SkillHandler.ApplyAddition(dActorReal, skill);
        }

        public class PJointBuff : DefaultBuff
        {
            private readonly Actor sActor;

            public PJointBuff(SagaDB.Skill.Skill skill, Actor sActor, Actor actor, int lifetime)
                : base(skill, actor, "A_T_PJoint", lifetime)
            {
                OnAdditionStart += StartEvent;
                OnAdditionEnd += EndEvent;
                this.sActor = sActor;
            }

            private void StartEvent(Actor actor, DefaultBuff skill)
            {
                var factor = 0.15f + skill.skill.Level * 0.05f;

                //最大攻擊
                var max_atk1_add = (int)(sActor.Status.max_atk_bs * factor);
                if (skill.Variable.ContainsKey("A_T_PJoint_max_atk1"))
                    skill.Variable.Remove("A_T_PJoint_max_atk1");
                skill.Variable.Add("A_T_PJoint_max_atk1", max_atk1_add);
                actor.Status.max_atk1_skill += (short)max_atk1_add;

                //最大攻擊
                var max_atk2_add = (int)(sActor.Status.max_atk_bs * factor);
                if (skill.Variable.ContainsKey("A_T_PJoint_max_atk2"))
                    skill.Variable.Remove("A_T_PJoint_max_atk2");
                skill.Variable.Add("A_T_PJoint_max_atk2", max_atk2_add);
                actor.Status.max_atk2_skill += (short)max_atk2_add;

                //最大攻擊
                var max_atk3_add = (int)(sActor.Status.max_atk_bs * factor);
                if (skill.Variable.ContainsKey("A_T_PJoint_max_atk3"))
                    skill.Variable.Remove("A_T_PJoint_max_atk3");
                skill.Variable.Add("A_T_PJoint_max_atk3", max_atk3_add);
                actor.Status.max_atk3_skill += (short)max_atk3_add;

                //最小攻擊
                var min_atk1_add = (int)(sActor.Status.min_atk_bs * factor);
                if (skill.Variable.ContainsKey("A_T_PJoint_min_atk1"))
                    skill.Variable.Remove("A_T_PJoint_min_atk1");
                skill.Variable.Add("A_T_PJoint_min_atk1", min_atk1_add);
                actor.Status.min_atk1_skill += (short)min_atk1_add;

                //最小攻擊
                var min_atk2_add = (int)(sActor.Status.min_atk_bs * factor);
                if (skill.Variable.ContainsKey("A_T_PJoint_min_atk2"))
                    skill.Variable.Remove("A_T_PJoint_min_atk2");
                skill.Variable.Add("A_T_PJoint_min_atk2", min_atk2_add);
                actor.Status.min_atk2_skill += (short)min_atk2_add;

                //最小攻擊
                var min_atk3_add = (int)(sActor.Status.min_atk_bs * factor);
                if (skill.Variable.ContainsKey("A_T_PJoint_min_atk3"))
                    skill.Variable.Remove("A_T_PJoint_min_atk3");
                skill.Variable.Add("A_T_PJoint_min_atk3", min_atk3_add);
                actor.Status.min_atk3_skill += (short)min_atk3_add;

                //最小魔攻
                var min_matk_add = (int)(sActor.Status.min_matk * factor);
                if (skill.Variable.ContainsKey("A_T_PJoint_min_matk"))
                    skill.Variable.Remove("A_T_PJoint_min_matk");
                skill.Variable.Add("A_T_PJoint_min_matk", min_matk_add);
                actor.Status.min_matk_skill += (short)min_matk_add;

                //最大魔攻
                var max_matk_add = (int)(sActor.Status.max_matk * factor);
                if (skill.Variable.ContainsKey("A_T_PJoint_max_matk"))
                    skill.Variable.Remove("A_T_PJoint_max_matk");
                skill.Variable.Add("A_T_PJoint_max_matk", max_matk_add);
                actor.Status.max_matk_skill += (short)max_matk_add;
                MapManager.Instance.GetMap(actor.MapID)
                    .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
            }

            private void EndEvent(Actor actor, DefaultBuff skill)
            {
                //最大攻擊
                actor.Status.max_atk1_skill -= (short)skill.Variable["A_T_PJoint_max_atk1"];

                //最大攻擊
                actor.Status.max_atk2_skill -= (short)skill.Variable["A_T_PJoint_max_atk2"];

                //最大攻擊
                actor.Status.max_atk3_skill -= (short)skill.Variable["A_T_PJoint_max_atk3"];

                //最小攻擊
                actor.Status.min_atk1_skill -= (short)skill.Variable["A_T_PJoint_min_atk1"];

                //最小攻擊
                actor.Status.min_atk2_skill -= (short)skill.Variable["A_T_PJoint_min_atk2"];

                //最小攻擊
                actor.Status.min_atk3_skill -= (short)skill.Variable["A_T_PJoint_min_atk3"];

                //最小魔攻
                actor.Status.min_matk_skill -= (short)skill.Variable["A_T_PJoint_min_matk"];

                //最小魔攻
                actor.Status.max_matk_skill -= (short)skill.Variable["A_T_PJoint_max_matk"];
                MapManager.Instance.GetMap(actor.MapID)
                    .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
            }
        }

        //#endregion
    }
}