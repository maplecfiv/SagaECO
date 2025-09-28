using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._3_0_Class.Hawkeye_隼人____arc
{
    public class SmokeBall : ISkill
    {
        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            if (SkillHandler.Instance.isEquipmentRight(pc, ItemType.BOW, ItemType.GUN, ItemType.RIFLE, ItemType.EXGUN,
                    ItemType.DUALGUN)) return 0;
            return -14;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            if (!dActor.Status.Additions.ContainsKey("SmokeBall"))
            {
                int[] lifetime = { 0, 45000, 60000, 75000, 90000, 120000 };
                var skill = new DefaultBuff(args.skill, sActor, "SmokeBall", lifetime[level]);
                skill.OnAdditionStart += StartEventHandler;
                skill.OnAdditionEnd += EndEventHandler;
                skill.OnCheckValid += ValidCheck;
                SkillHandler.ApplyAddition(sActor, skill);
            }
            else
            {
                sActor.Status.Additions["SmokeBall"].OnTimerEnd();
            }
        }

        private void ValidCheck(ActorPC pc, Actor dActor, out int result)
        {
            result = TryCast(pc, dActor, null);
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            int level = skill.skill.Level;
            actor.Status.combo_rate_skill += 50;
            actor.Buff.三转枪连弹 = true;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            int level = skill.skill.Level;
            actor.Status.combo_rate_skill -= 50;
            actor.Buff.三转枪连弹 = false;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }
    }
}