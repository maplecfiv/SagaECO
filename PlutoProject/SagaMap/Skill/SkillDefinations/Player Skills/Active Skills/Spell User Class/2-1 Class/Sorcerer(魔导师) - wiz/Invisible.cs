using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._2_1_Class.Sorcerer_魔导师____wiz
{
    /// <summary>
    ///     透明化（インビジブル）
    /// </summary>
    public class Invisible : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var affected = map.GetActorsArea(sActor, 100, true);
            var sPC = (ActorPC)sActor;

            foreach (var act in affected)
                if (act.type == ActorType.PC)
                {
                    var aPC = (ActorPC)act;
                    if (aPC.Party != null && sPC.Party != null)
                    {
                        if (aPC.Party.ID == sPC.Party.ID && aPC.Party.ID != 0 && !aPC.Buff.Dead &&
                            aPC.PossessionTarget == 0)
                            if (aPC.Party.ID == sPC.Party.ID)
                            {
                                var skill = new DefaultBuff(args.skill, act, "Invisible", 30000);
                                skill.OnAdditionStart += StartEventHandler;
                                skill.OnAdditionEnd += EndEventHandler;
                                SkillHandler.ApplyAddition(act, skill);
                            }
                    }
                    else
                    {
                        if (act.ActorID == sActor.ActorID)
                        {
                            var skill = new DefaultBuff(args.skill, act, "Invisible", 30000);
                            skill.OnAdditionStart += StartEventHandler;
                            skill.OnAdditionEnd += EndEventHandler;
                            SkillHandler.ApplyAddition(act, skill);
                        }
                    }
                }
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
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

        #endregion
    }
}