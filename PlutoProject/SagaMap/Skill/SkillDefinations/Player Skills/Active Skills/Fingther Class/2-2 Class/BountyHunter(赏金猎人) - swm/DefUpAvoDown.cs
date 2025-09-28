using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._2_2_Class.BountyHunter_赏金猎人____swm
{
    /// <summary>
    ///     散漫身姿（ブラフ）
    /// </summary>
    public class DefUpAvoDown : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = (45 - 5 * level) * 1000;
            var skill = new DefaultBuff(args.skill, dActor, "DefUpAvoDown", lifetime);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(dActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            int level = skill.skill.Level;
            var avo_range_down = -(int)(actor.Status.avoid_ranged * 0.1f * level);
            var avo_melee_down = -(int)(actor.Status.avoid_melee * 0.1f * level);
            var def_add_add = (int)(actor.Status.def_add * (0.1f + 0.1f * level));
            var def_add = (int)(actor.Status.def * (0.1f + 0.1f * level));
            //avo_range_down
            if (skill.Variable.ContainsKey("DefUpAvoDown_avo_range_down"))
                skill.Variable.Remove("DefUpAvoDown_avo_range_down");
            skill.Variable.Add("DefUpAvoDown_avo_range_down", avo_range_down);
            actor.Status.avoid_ranged_skill += (short)avo_range_down;
            //avo_melee_down
            if (skill.Variable.ContainsKey("DefUpAvoDown_avo_melee_down"))
                skill.Variable.Remove("DefUpAvoDown_avo_melee_down");
            skill.Variable.Add("DefUpAvoDown_avo_melee_down", avo_melee_down);
            actor.Status.avoid_melee_skill += (short)avo_melee_down;
            //def_add_add
            if (skill.Variable.ContainsKey("DefUpAvoDown_def_add_add"))
                skill.Variable.Remove("DefUpAvoDown_def_add_add");
            skill.Variable.Add("DefUpAvoDown_def_add_add", def_add_add);
            actor.Status.def_add_skill += (short)def_add_add;
            //def_add
            if (skill.Variable.ContainsKey("DefUpAvoDown_def_add"))
                skill.Variable.Remove("DefUpAvoDown_def_add");
            skill.Variable.Add("DefUpAvoDown_def_add", def_add);
            actor.Status.def_skill += (short)def_add;

            actor.Buff.DefUp = true;
            actor.Buff.DefRateUp = true;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.Status.avoid_ranged_skill -= (short)skill.Variable["DefUpAvoDown_avo_range_down"];
            actor.Status.avoid_melee_skill -= (short)skill.Variable["DefUpAvoDown_avo_melee_down"];
            actor.Status.def_add_skill -= (short)skill.Variable["DefUpAvoDown_def_add_add"];
            actor.Status.def_skill -= (short)skill.Variable["DefUpAvoDown_def_add"];

            actor.Buff.DefUp = false;
            actor.Buff.DefRateUp = false;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        #endregion
    }
}