using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._3_0_Class.Gladiator_剑斗士____swm
{
    /// <summary>
    ///     鬼人の構え
    /// </summary>
    public class DevilStance : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var totals = new[] { 0, 45, 60, 75, 90, 120 };
            if (!dActor.Status.Additions.ContainsKey("DevilStance"))
            {
                var lifetime = 1000 * totals[level];
                var skill = new DefaultBuff(args.skill, dActor, "DevilStance", lifetime);
                skill.OnAdditionStart += StartEventHandler;
                skill.OnAdditionEnd += EndEventHandler;
                SkillHandler.ApplyAddition(dActor, skill);
            }
            else
            {
                dActor.Status.Additions["DevilStance"].OnTimerEnd();
            }
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            var Def = 0;
            if (actor.Status.def_add > 300)
                Def = 300;
            else
                Def = actor.Status.def_add;

            if (skill.Variable.ContainsKey("DevilStance_def"))
                skill.Variable.Remove("DevilStance_def");
            skill.Variable.Add("DevilStance_def", Def);

            //最大攻擊
            actor.Status.max_atk1_skill += (short)Def;

            //最大攻擊
            actor.Status.max_atk2_skill += (short)Def;

            //最大攻擊
            actor.Status.max_atk3_skill += (short)Def;

            //最小攻擊
            actor.Status.min_atk1_skill += (short)Def;

            //最小攻擊
            actor.Status.min_atk2_skill += (short)Def;

            //最小攻擊

            actor.Status.min_atk3_skill += (short)Def;

            //右防禦
            actor.Status.def_add_skill -= (short)Def;
            actor.Buff.DevilStance = true;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            //最大攻擊
            actor.Status.max_atk1_skill -= (short)skill.Variable["DevilStance_def"];

            //最大攻擊
            actor.Status.max_atk2_skill -= (short)skill.Variable["DevilStance_def"];

            //最大攻擊
            actor.Status.max_atk3_skill -= (short)skill.Variable["DevilStance_def"];

            //最小攻擊
            actor.Status.min_atk1_skill -= (short)skill.Variable["DevilStance_def"];

            //最小攻擊
            actor.Status.min_atk2_skill -= (short)skill.Variable["DevilStance_def"];

            //最小攻擊
            actor.Status.min_atk3_skill -= (short)skill.Variable["DevilStance_def"];

            //右防禦
            actor.Status.def_add_skill += (short)skill.Variable["DevilStance_def"];
            actor.Buff.DevilStance = false;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        #endregion
    }
}