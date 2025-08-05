using SagaDB.Actor;
using SagaDB.Item;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._2_1_Class.Knight_圣骑士____fen
{
    /// <summary>
    ///     胳膊卸力（アームスラスト）
    /// </summary>
    public class HitRow : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            if (SkillHandler.Instance.isEquipmentRight(sActor, ItemType.RAPIER) ||
                sActor.Inventory.GetContainer(ContainerType.RIGHT_HAND2).Count > 0)
            {
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, dActor)) return 0;

                return -14;
            }

            return -5;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 1000;
            var factor = 1.8f;
            SkillHandler.Instance.PhysicalAttack(sActor, dActor, args, sActor.WeaponElement, factor);
            var skill = new DefaultBuff(args.skill, dActor, "HitRow", lifetime);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(dActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            float[] rate = { 0, 0.03f, 0.06f, 0.09f, 0.12f, 0.15f, 0.9f };
            var atk1 = (int)(actor.Status.min_atk1 * rate[skill.skill.Level]);
            var atk2 = (int)(actor.Status.min_atk2 * rate[skill.skill.Level]);
            var atk3 = (int)(actor.Status.min_atk3 * rate[skill.skill.Level]);

            if (skill.Variable.ContainsKey("HitRow_atk1_down"))
                skill.Variable.Remove("HitRow_atk1_down");
            skill.Variable.Add("HitRow_atk1_down", atk1);
            actor.Status.min_atk1_skill -= (short)atk1;

            if (skill.Variable.ContainsKey("HitRow_atk2_down"))
                skill.Variable.Remove("HitRow_atk2_down");
            skill.Variable.Add("HitRow_atk2_down", atk2);
            actor.Status.min_atk2_skill -= (short)atk2;

            if (skill.Variable.ContainsKey("HitRow_atk3_down"))
                skill.Variable.Remove("HitRow_atk3_down");
            skill.Variable.Add("HitRow_atk3_down", atk3);
            actor.Status.min_atk3_skill -= (short)atk3;
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.Status.min_atk1_skill += (short)skill.Variable["HitRow_atk1_down"];
            actor.Status.min_atk2_skill += (short)skill.Variable["HitRow_atk2_down"];
            actor.Status.min_atk3_skill += (short)skill.Variable["HitRow_atk3_down"];
        }

        #endregion
    }
}