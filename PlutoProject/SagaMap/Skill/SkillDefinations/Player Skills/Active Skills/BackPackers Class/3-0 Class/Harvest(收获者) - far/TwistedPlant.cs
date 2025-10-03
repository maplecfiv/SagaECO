using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._3_0_Class.Harvest_收获者____far
{
    /// <summary>
    ///     ツイステッドプラント
    /// </summary>
    public class TwistedPlant : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            if (dActor.Status.Additions.ContainsKey("PlantShield")) return -14;
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 15000 + 15000 * level;
            var skill = new DefaultBuff(args.skill, dActor, "PlantShield", lifetime);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(dActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.Status.PlantShield = actor.MaxHP;
            actor.Buff.三转植物寄生 = true;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.Status.PlantShield = 0;
            actor.Buff.三转植物寄生 = false;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        //#endregion
    }
}