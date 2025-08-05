using SagaDB.Actor;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._2_2_Class.DarkStalker_黑暗骑士____fen
{
    public class DarkMist : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            if (SkillHandler.Instance.CheckValidAttackTarget(pc, dActor)) return 0;

            return -14;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            float[] factors = { 0f, 1.2f, 1.4f, 1.6f, 1.8f, 2.1f };
            var factor = factors[level];
            SkillHandler.Instance.MagicAttack(sActor, dActor, args, sActor.WeaponElement, factor);
            var rate = 15 + 5 * level;
            if (SagaLib.Global.Random.Next(0, 99) < rate)
            {
                var skill = new DefaultBuff(args.skill, dActor, "DarkMist", rate * 1000);
                skill.OnAdditionStart += StartEventHandler;
                skill.OnAdditionEnd += EndEventHandler;
                SkillHandler.ApplyAddition(dActor, skill);
            }
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            int level = skill.skill.Level;
            var avo_range_down = -(int)(actor.Status.avoid_ranged * (0.2f + 0.05f * level));
            var avo_melee_down = -(int)(actor.Status.avoid_melee * (0.2f + 0.05f * level));
            //avo_range_down
            if (skill.Variable.ContainsKey("DarkMist_avo_range_down"))
                skill.Variable.Remove("DarkMist_avo_range_down");
            skill.Variable.Add("DarkMist_avo_range_down", avo_range_down);
            actor.Status.avoid_ranged_skill += (short)avo_range_down;
            //avo_melee_down
            if (skill.Variable.ContainsKey("DarkMist_avo_melee_down"))
                skill.Variable.Remove("DarkMist_avo_melee_down");
            skill.Variable.Add("DarkMist_avo_melee_down", avo_melee_down);
            actor.Status.avoid_melee_skill += (short)avo_melee_down;
            //Manager.MapManager.Instance.GetMap(actor.MapID).SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.Status.avoid_ranged_skill -= (short)skill.Variable["DarkMist_avo_range_down"];
            actor.Status.avoid_melee_skill -= (short)skill.Variable["DarkMist_avo_melee_down"];
        }

        #endregion
    }
}