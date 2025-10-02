using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._2_1_Class.Explorer_探险家____rag
{
    /// <summary>
    ///     隱影術（ケイブハイディング）
    /// </summary>
    public class CaveHiding : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            args.dActor = 0; //不显示效果
            var skill = new DefaultBuff(args.skill, sActor, "CaveHiding", int.MaxValue, 1000);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            skill.OnUpdate += UpdateEventHandler;
            SkillHandler.ApplyAddition(sActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            //X
            if (skill.Variable.ContainsKey("CaveHiding_X"))
                skill.Variable.Remove("CaveHiding_X");
            skill.Variable.Add("CaveHiding_X", actor.X);

            //Y
            if (skill.Variable.ContainsKey("CaveHiding_Y"))
                skill.Variable.Remove("CaveHiding_Y");
            skill.Variable.Add("CaveHiding_Y", actor.Y);

            actor.Buff.Transparent = true;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.Buff.Transparent = false;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void UpdateEventHandler(Actor actor, DefaultBuff skill)
        {
            if (actor.SP > 0 && actor.X == (short)skill.Variable["CaveHiding_X"] &&
                actor.Y == (short)skill.Variable["CaveHiding_Y"])
            {
                var map = MapManager.Instance.GetMap(actor.MapID);
                actor.SP -= 1;
                map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.HPMPSP_UPDATE, null, actor, true);
            }
            else
            {
                actor.Status.Additions["CaveHiding"].AdditionEnd();
                actor.Status.Additions.Remove("CaveHiding");
            }
        }

        //#endregion
    }
}