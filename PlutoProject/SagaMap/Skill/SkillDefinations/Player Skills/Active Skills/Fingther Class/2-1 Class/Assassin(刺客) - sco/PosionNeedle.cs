using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._2_1_Class.Assassin_刺客____sco
{
    /// <summary>
    ///     毒暗器（ポイズンニードル）
    /// </summary>
    public class PosionNeedle : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            uint itemID = 10038102; //毒針
            if (SkillHandler.Instance.CountItem(sActor, itemID) >= 1)
            {
                SkillHandler.Instance.TakeItem(sActor, itemID, 1);
                return 0;
            }

            return -57;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var rate = 25 + 5 * level;
            if (SkillHandler.Instance.CanAdditionApply(sActor, dActor, "PosionNeedle", rate))
            {
                var skill = new DefaultBuff(args.skill, dActor, "PosionNeedle", 3000);
                skill.OnAdditionStart += StartEvent;
                skill.OnAdditionEnd += EndEvent;
                SkillHandler.ApplyAddition(dActor, skill);
            }

            SkillHandler.Instance.PhysicalAttack(sActor, dActor, args, sActor.WeaponElement, 1.0f);
        }

        private void StartEvent(Actor actor, DefaultBuff skill)
        {
            var map = MapManager.Instance.GetMap(actor.MapID);
            actor.Buff.Poison = true;
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);

            int value;
            if (skill.Variable.ContainsKey("PosionNeedle_ATK1"))
                skill.Variable.Remove("PosionNeedle_ATK1");
            value = actor.Status.min_atk_bs / 2;
            skill.Variable.Add("PosionNeedle_ATK1", value);
            actor.Status.min_atk1_skill -= (short)value;
            actor.Status.min_atk2_skill -= (short)value;
            actor.Status.min_atk3_skill -= (short)value;
            if (skill.Variable.ContainsKey("PosionNeedle_ATK2"))
                skill.Variable.Remove("PosionNeedle_ATK2");
            value = actor.Status.max_atk_bs / 2;
            skill.Variable.Add("PosionNeedle_ATK2", value);
            actor.Status.max_atk1_skill -= (short)value;
            actor.Status.max_atk2_skill -= (short)value;
            actor.Status.max_atk3_skill -= (short)value;
            if (skill.Variable.ContainsKey("PosionNeedle_MATK"))
                skill.Variable.Remove("PosionNeedle_MATK");
            value = actor.Status.max_matk_bs / 2;
            skill.Variable.Add("PosionNeedle_MATK", value);
            actor.Status.min_matk_skill -= (short)value;
            if (skill.Variable.ContainsKey("PosionNeedle_MATK2"))
                skill.Variable.Remove("PosionNeedle_MATK2");
            value = actor.Status.max_matk_bs / 2;
            skill.Variable.Add("PosionNeedle_MATK2", value);
            actor.Status.max_matk_skill -= (short)value;
        }

        private void EndEvent(Actor actor, DefaultBuff skill)
        {
            var map = MapManager.Instance.GetMap(actor.MapID);
            actor.Buff.Poison = false;
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);

            var value = skill.Variable["PosionNeedle_ATK1"];
            actor.Status.min_atk1_skill += (short)value;
            actor.Status.min_atk2_skill += (short)value;
            actor.Status.min_atk3_skill += (short)value;
            value = skill.Variable["PosionNeedle_ATK2"];
            actor.Status.max_atk1_skill += (short)value;
            actor.Status.max_atk2_skill += (short)value;
            actor.Status.max_atk3_skill += (short)value;
            value = skill.Variable["PosionNeedle_MATK"];
            actor.Status.min_matk_skill += (short)value;
            value = skill.Variable["PosionNeedle_MATK2"];
            actor.Status.max_matk_skill += (short)value;
        }

        #endregion
    }
}