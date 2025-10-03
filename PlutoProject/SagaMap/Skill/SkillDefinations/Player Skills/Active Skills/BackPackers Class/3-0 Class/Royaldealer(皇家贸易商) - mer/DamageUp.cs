using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._3_0_Class.
    Royaldealer_皇家贸易商____mer
{
    internal class DamageUp : ISkill
    {
        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.Status.Damage_Up_Lv = skill.skill.Level;
            actor.Buff.GetDamageUpDamageMark3RD = true;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.Status.Damage_Up_Lv = 0;
            actor.Buff.GetDamageUpDamageMark3RD = false;
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
            var lifetime = 140000 - 20000 * level;
            var skill = new DefaultBuff(args.skill, dActor, "DamageUp", lifetime);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(dActor, skill);
        }

        //#endregion
    }
}