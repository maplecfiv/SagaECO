using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Tatarabe
{
    /// <summary>
    ///     銅牆鐵壁（亀の構え）
    /// </summary>
    public class PosturetorToise : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 100000 * level;
            var skill = new DefaultBuff(args.skill, dActor, "PosturetorToise", lifetime, 1000);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            skill.OnUpdate += UpdateEventHandler;
            SkillHandler.ApplyAddition(dActor, skill);
        }

        private void UpdateEventHandler(Actor actor, DefaultBuff skill)
        {
            if (actor.X != (short)skill.Variable["Save_X"] || actor.Y != (short)skill.Variable["Save_Y"])
            {
                var map = MapManager.Instance.GetMap(actor.MapID);
                actor.Status.Additions["PosturetorToise"].AdditionEnd();
                actor.Status.Additions.Remove("PosturetorToise");
                skill.Variable.Remove("Save_X");
                skill.Variable.Remove("Save_Y");
                map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, false);
            }
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            //X
            if (skill.Variable.ContainsKey("Save_X"))
                skill.Variable.Remove("Save_X");
            skill.Variable.Add("Save_X", actor.X);

            //Y
            if (skill.Variable.ContainsKey("Save_Y"))
                skill.Variable.Remove("Save_Y");
            skill.Variable.Add("Save_Y", actor.Y);

            if (skill.Variable.ContainsKey("PosturetorToiseDefUp"))
                skill.Variable.Remove("PosturetorToiseDefUp");
            skill.Variable.Add("PosturetorToiseDefUp", 10 * skill.skill.Level);
            actor.Status.def_skill += (short)(10 * skill.skill.Level);
            actor.Buff.DefUp = true;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.Status.def_skill -= (short)skill.Variable["PosturetorToiseDefUp"];
            actor.Buff.DefUp = false;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        #endregion
    }
}