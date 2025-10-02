using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._2_1_Class.Druid_神官____vote
{
    /// <summary>
    ///     計劃者（プラーナ）
    /// </summary>
    public class RegiAllUp : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            //int lifetime = 40000 + 20000 * level;
            var lifetime = 300000 + 120000 * level;
            var skill = new DefaultBuff(args.skill, dActor, "RegiAllUp", lifetime);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(dActor, skill);
            string[] StatusNames =
                { "Sleep", "Poison", "Stun", "Silence", "Stone", "Confuse", "鈍足", "Frosen", "Stiff" };
            foreach (var StatusName in StatusNames)
            {
                var skill2 = new DefaultBuff(args.skill, dActor, StatusName + "Regi", lifetime);
                skill2.OnAdditionStart += StartBuffEventHandler;
                skill2.OnAdditionEnd += EndBuffEventHandler;
                SkillHandler.ApplyAddition(dActor, skill2);
            }
        }

        private void StartBuffEventHandler(Actor actor, DefaultBuff skill)
        {
        }

        private void EndBuffEventHandler(Actor actor, DefaultBuff skill)
        {
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.Buff.ConfuseResist = true;
            actor.Buff.FaintResist = true;
            actor.Buff.FrosenResist = true;
            actor.Buff.ParalysisResist = true;
            actor.Buff.PoisonResist = true;
            actor.Buff.SilenceResist = true;
            actor.Buff.SleepResist = true;
            actor.Buff.SpeedDownResist = true;
            actor.Buff.StoneResist = true;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.Buff.ConfuseResist = false;
            actor.Buff.FaintResist = false;
            actor.Buff.FrosenResist = false;
            actor.Buff.ParalysisResist = false;
            actor.Buff.PoisonResist = false;
            actor.Buff.SilenceResist = false;
            actor.Buff.SleepResist = false;
            actor.Buff.SpeedDownResist = false;
            actor.Buff.StoneResist = false;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        //#endregion
    }
}