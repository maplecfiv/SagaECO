using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Global
{
    /// <summary>
    ///     點火
    /// </summary>
    public class SpeedUpSkill : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 90000;
            dActor = SkillHandler.Instance.GetPossesionedActor((ActorPC)sActor);
            var skill = new DefaultBuff(args.skill, dActor, "SpeedUpSkill", lifetime);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(dActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            var map = MapManager.Instance.GetMap(actor.MapID);

            //Clear All MoveUp
            if (skill.Variable.ContainsKey("SpeedUpSkill_Speed"))
                skill.Variable.Remove("SpeedUpSkill_Speed");


            //移動速度
            var Speed_add = 100;
            if (skill.Variable.ContainsKey("SpeedUpSkill_Speed"))
                skill.Variable.Remove("SpeedUpSkill_Speed");
            skill.Variable.Add("SpeedUpSkill_Speed", Speed_add);


            actor.Status.speed_skill += (ushort)Speed_add;
            actor.Buff.MoveSpeedUp = true;
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            var map = MapManager.Instance.GetMap(actor.MapID);

            //移動速度
            actor.Status.speed_skill -= (ushort)skill.Variable["SpeedUpSkill_Speed"];
            actor.Buff.MoveSpeedUp = false;
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        //#endregion
    }
}