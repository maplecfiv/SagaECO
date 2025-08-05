using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._3_0_Class.Gladiator_剑斗士____swm
{
    /// <summary>
    ///     エクヴィリー
    /// </summary>
    public class Ekuviri : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 60000 * level;
            var skill = new DefaultBuff(args.skill, dActor, "Ekuviri", lifetime);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(dActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            var MinAttack = new[] { 0, 0.63f, 0.77f, 0.85f, 0.94f, 1 };

            //最小攻擊
            var min_atk1_add = (int)((actor.Status.max_atk1 - actor.Status.min_atk1) * MinAttack[skill.skill.Level]);
            if (skill.Variable.ContainsKey("Ekuviri_min_atk1"))
                skill.Variable.Remove("Ekuviri_min_atk1");
            skill.Variable.Add("Ekuviri_min_atk1", min_atk1_add);
            actor.Status.min_atk1_skill += (short)min_atk1_add;

            //最小攻擊
            var min_atk2_add = (int)((actor.Status.max_atk2 - actor.Status.min_atk2) * MinAttack[skill.skill.Level]);
            if (skill.Variable.ContainsKey("Ekuviri_min_atk2"))
                skill.Variable.Remove("Ekuviri_min_atk2");
            skill.Variable.Add("Ekuviri_min_atk2", min_atk2_add);
            actor.Status.min_atk2_skill += (short)min_atk2_add;

            //最小攻擊
            var min_atk3_add = (int)((actor.Status.max_atk3 - actor.Status.min_atk3) * MinAttack[skill.skill.Level]);
            if (skill.Variable.ContainsKey("Ekuviri_min_atk3"))
                skill.Variable.Remove("Ekuviri_min_atk3");
            skill.Variable.Add("Ekuviri_min_atk3", min_atk3_add);
            actor.Status.min_atk3_skill += (short)min_atk3_add;

            //最小魔法攻击力
            var min_matk_add = (int)((actor.Status.max_matk - actor.Status.min_matk) * MinAttack[skill.skill.Level]);
            if (skill.Variable.ContainsKey("Ekuviri_min_matk"))
                skill.Variable.Remove("Ekuviri_min_matk");
            skill.Variable.Add("Ekuviri_min_matk", min_matk_add);
            actor.Status.min_matk_skill += (short)min_matk_add;
            actor.Buff.三转波动伤害固定 = true;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            //最小攻擊
            actor.Status.min_atk1_skill -= (short)skill.Variable["Ekuviri_min_atk1"];

            //最小攻擊
            actor.Status.min_atk2_skill -= (short)skill.Variable["Ekuviri_min_atk2"];

            //最小攻擊
            actor.Status.min_atk3_skill -= (short)skill.Variable["Ekuviri_min_atk3"];

            //最小魔法攻击力
            actor.Status.min_matk_skill -= (short)skill.Variable["Ekuviri_min_matk"];

            actor.Buff.三转波动伤害固定 = false;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        #endregion
    }
}