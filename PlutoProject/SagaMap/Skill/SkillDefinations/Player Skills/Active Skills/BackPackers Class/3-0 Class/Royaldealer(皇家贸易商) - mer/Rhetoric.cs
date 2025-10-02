using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._3_0_Class.
    Royaldealer_皇家贸易商____mer
{
    internal class Rhetoric : ISkill
    {
        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            var up = 80 + 70 * skill.skill.Level;
            if (skill.Variable.ContainsKey("Rhetoric"))
                skill.Variable.Remove("Rhetoric");
            skill.Variable.Add("Rhetoric", up);
            actor.Status.mp_skill += (short)up;
            actor.Status.sp_skill += (short)up;
            actor.Buff.三转レトリック = true;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.Status.mp_skill -= (short)skill.Variable["Rhetoric"];
            actor.Status.sp_skill -= (short)skill.Variable["Rhetoric"];
            actor.Buff.三转レトリック = false;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        //#region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 30000 + 30000 * level;
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var affected = map.GetActorsArea(sActor, 250, true);
            foreach (var act in affected)
                if (!SkillHandler.Instance.CheckValidAttackTarget(sActor, act))
                {
                    var skill = new DefaultBuff(args.skill, act, "Rhetoric", lifetime);
                    skill.OnAdditionStart += StartEventHandler;
                    skill.OnAdditionEnd += EndEventHandler;
                    SkillHandler.ApplyAddition(act, skill);
                }
        }

        //#endregion
    }
}