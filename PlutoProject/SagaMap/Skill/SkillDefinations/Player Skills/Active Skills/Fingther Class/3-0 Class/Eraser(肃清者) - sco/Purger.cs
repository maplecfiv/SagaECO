using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Eraser
{
    public class Purger : ISkill
    {
        public ushort speed_old;

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            if (!dActor.Status.Additions.ContainsKey("イレイザー"))
            {
                var lifetime = 30000 + 30000 * level;
                Actor RealActor = SkillHandler.Instance.GetPossesionedActor((ActorPC)sActor);
                var skill = new DefaultBuff(args.skill, RealActor, "イレイザー", lifetime, 1000);
                skill.OnAdditionStart += StartEventHandler;
                skill.OnAdditionEnd += EndEventHandler;
                //skill.OnUpdate += this.UpdateEventHandler;
                SkillHandler.ApplyAddition(RealActor, skill);
            }
            else
            {
                dActor.Status.Additions["イレイザー"].OnTimerEnd();
            }
        }

        //void UpdateEventHandler(Actor actor, DefaultBuff skill)
        //{
        //    if (actor.Speed != 310)
        //    {
        //        actor.Status.speed_skill = -100;
        //        actor.Status.speed_item = 0;
        //    }
        //}
        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            var speed_add = 100;
            if (skill.Variable.ContainsKey("Eraser_speed"))
                skill.Variable.Remove("Eraser_speed");
            actor.Status.Purger_Lv = skill.skill.Level;
            skill.Variable.Add("Eraser_speed", speed_add);
            actor.Status.speed_skill -= (ushort)speed_add;
            actor.Buff.MainSkillPowerUp3RD = true;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.Status.speed_skill += (ushort)skill.Variable["Eraser_speed"];
            actor.Status.Purger_Lv = 0;
            actor.Buff.MainSkillPowerUp3RD = false;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }
    }
}