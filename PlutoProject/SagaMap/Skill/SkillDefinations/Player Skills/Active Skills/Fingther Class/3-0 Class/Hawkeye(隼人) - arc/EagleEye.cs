using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._3_0_Class.Hawkeye_隼人____arc
{
    /// <summary>
    ///     ホークアイ
    /// </summary>
    public class EagleEye : ISkill
    {
        //#region ISkill 成員

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            if (sActor.Status.Additions.ContainsKey("ホークアイ")) sActor.Status.Additions["ホークアイ"].AdditionEnd();
            var lifetimes = new[] { 0, 60000, 90000, 120000, 150000, 180000 };
            var lifetime = lifetimes[args.skill.Level];
            var skill = new DefaultBuff(args.skill, sActor, "ホークアイ", lifetime, 1000);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            skill.OnUpdate += UpdateEventHandler;
            SkillHandler.ApplyAddition(sActor, skill);
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

            var factor = new[] { 0, 150, 175, 200, 225, 250 }[skill.skill.Level];
            if (skill.Variable.ContainsKey("ホークアイ"))
                skill.Variable.Remove("ホークアイ");
            skill.Variable.Add("ホークアイ", factor);

            actor.Buff.MainSkillPowerUp3RD = true;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.Buff.MainSkillPowerUp3RD = false;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void UpdateEventHandler(Actor actor, DefaultBuff skill)
        {
            if (actor.X != (short)skill.Variable["Save_X"] || actor.Y != (short)skill.Variable["Save_Y"])
            {
                var map = MapManager.Instance.GetMap(actor.MapID);
                actor.Status.Additions["ホークアイ"].AdditionEnd();
                actor.Status.Additions.Remove("ホークアイ");
                map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, false);
            }
        }

        //#endregion
    }
}