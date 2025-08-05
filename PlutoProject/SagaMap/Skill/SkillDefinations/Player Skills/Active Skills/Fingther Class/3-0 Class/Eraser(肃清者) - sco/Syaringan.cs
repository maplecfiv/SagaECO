using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._3_0_Class.Eraser_肃清者____sco
{
    /// <summary>
    ///     見切り
    /// </summary>
    public class Syaringan : ISkill
    {
        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var pc = (ActorPC)sActor;
            int[] lifetime = { 0, 180000, 80000, 60000, 30000, 15000 };
            var skill = new DefaultBuff(args.skill, dActor, "見切り", lifetime[level]);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(dActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            byte[] rate = { 0, 40, 60, 80, 90, 99 };
            actor.Status.Syaringan_rate = rate[skill.skill.Level];
            actor.Buff.三转見切り = true;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.Status.Syaringan_rate = 0;
            actor.Buff.三转見切り = false;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }
    }
}