using SagaDB.Actor;
using SagaMap.Manager;

namespace SagaMap.Skill.Additions.Global
{
    /// <summary>
    ///     狂戰士
    /// </summary>
    public class Berserk : DefaultBuff
    {
        public Berserk(SagaDB.Skill.Skill skill, Actor actor, int lifetime)
            : base(skill, actor, "Berserk", lifetime)
        {
            OnAdditionStart += StartEvent;
            OnAdditionEnd += EndEvent;
        }

        private void StartEvent(Actor actor, DefaultBuff skill)
        {
            //最大攻擊
            var max_atk1_add = (int)actor.Status.max_atk_bs;
            if (skill.Variable.ContainsKey("Berserk_max_atk1"))
                skill.Variable.Remove("Berserk_max_atk1");
            skill.Variable.Add("Berserk_max_atk1", max_atk1_add);
            actor.Status.max_atk1_skill += (short)max_atk1_add;

            //最大攻擊
            var max_atk2_add = (int)actor.Status.max_atk_bs;
            if (skill.Variable.ContainsKey("Berserk_max_atk2"))
                skill.Variable.Remove("Berserk_max_atk2");
            skill.Variable.Add("Berserk_max_atk2", max_atk2_add);
            actor.Status.max_atk2_skill += (short)max_atk2_add;

            //最大攻擊
            var max_atk3_add = (int)actor.Status.max_atk_bs;
            if (skill.Variable.ContainsKey("Berserk_max_atk3"))
                skill.Variable.Remove("Berserk_max_atk3");
            skill.Variable.Add("Berserk_max_atk3", max_atk3_add);
            actor.Status.max_atk3_skill += (short)max_atk3_add;

            //最小攻擊
            var min_atk1_add = (int)actor.Status.min_atk_bs;
            if (skill.Variable.ContainsKey("Berserk_min_atk1"))
                skill.Variable.Remove("Berserk_min_atk1");
            skill.Variable.Add("Berserk_min_atk1", min_atk1_add);
            actor.Status.min_atk1_skill += (short)min_atk1_add;

            //最小攻擊
            var min_atk2_add = (int)actor.Status.min_atk_bs;
            if (skill.Variable.ContainsKey("Berserk_min_atk2"))
                skill.Variable.Remove("Berserk_min_atk2");
            skill.Variable.Add("Berserk_min_atk2", min_atk2_add);
            actor.Status.min_atk2_skill += (short)min_atk2_add;

            //最小攻擊
            var min_atk3_add = (int)actor.Status.min_atk_bs;
            if (skill.Variable.ContainsKey("Berserk_min_atk3"))
                skill.Variable.Remove("Berserk_min_atk3");
            skill.Variable.Add("Berserk_min_atk3", min_atk3_add);
            actor.Status.min_atk3_skill += (short)min_atk3_add;


            //右防禦
            var def_add_add = -actor.Status.def_add;
            if (skill.Variable.ContainsKey("Berserk_def_add"))
                skill.Variable.Remove("Berserk_def_add");
            skill.Variable.Add("Berserk_def_add", def_add_add);
            actor.Status.def_add_skill += (short)def_add_add;


            var map = MapManager.Instance.GetMap(actor.MapID);
            actor.Buff.Berserker = true;
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEvent(Actor actor, DefaultBuff skill)
        {
            //最大攻擊
            actor.Status.max_atk1_skill -= (short)skill.Variable["Berserk_max_atk1"];

            //最大攻擊
            actor.Status.max_atk2_skill -= (short)skill.Variable["Berserk_max_atk2"];

            //最大攻擊
            actor.Status.max_atk3_skill -= (short)skill.Variable["Berserk_max_atk3"];

            //最小攻擊
            actor.Status.min_atk1_skill -= (short)skill.Variable["Berserk_min_atk1"];

            //最小攻擊
            actor.Status.min_atk2_skill -= (short)skill.Variable["Berserk_min_atk2"];

            //最小攻擊
            actor.Status.min_atk3_skill -= (short)skill.Variable["Berserk_min_atk3"];

            //右防禦
            actor.Status.def_add_skill -= (short)skill.Variable["Berserk_def_add"];

            var map = MapManager.Instance.GetMap(actor.MapID);
            actor.Buff.Berserker = false;
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }
    }
}