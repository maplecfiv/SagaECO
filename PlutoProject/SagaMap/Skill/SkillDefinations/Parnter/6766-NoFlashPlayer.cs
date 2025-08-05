using System.Collections.Generic;
using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Parnter
{
    /// <summary>
    ///     ホーリーフェザー
    /// </summary>
    public class NoFlashPlayer : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var affected = map.GetActorsArea(sActor, 200, true);
            var realAffected = new List<Actor>();
            ActorPC sPC;
            var pet = (ActorPartner)sActor;
            sPC = pet.Owner;
            if (sPC.Party != null)
            {
                foreach (var act in affected)
                    if (act.type == ActorType.PC)
                    {
                        var aPC = (ActorPC)act;
                        if (aPC.Party != null && sPC.Party != null)
                            if (aPC.Party.ID == sPC.Party.ID && aPC.Party.ID != 0 && !aPC.Buff.Dead &&
                                aPC.PossessionTarget == 0)
                            {
                                if (act.Buff.NoRegen) continue;

                                if (aPC.Party.ID == sPC.Party.ID) realAffected.Add(act);
                            }
                    }
            }
            else
            {
                realAffected.Add(sActor);
                realAffected.Add(sPC);
            }

            args.affectedActors = realAffected;
            args.Init();
            var lifetime = 60000;
            foreach (var rAct in realAffected)
            {
                var skill1 = new MPRecovery(args.skill, rAct, lifetime, 5000);
                SkillHandler.ApplyAddition(rAct, skill1);
                var skill2 = new SPRecovery(args.skill, rAct, lifetime, 5000);
                SkillHandler.ApplyAddition(rAct, skill2);
                var skill = new DefaultBuff(args.skill, rAct, "NoFlashPlayer", lifetime);
                skill.OnAdditionStart += StartEventHandler;
                skill.OnAdditionEnd += EndEventHandler;
                SkillHandler.ApplyAddition(rAct, skill);
            }
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.Status.mp_recover_skill += 15;
            actor.Status.hp_recover_skill += 15;
            actor.Status.sp_recover_skill += 15;
            actor.Buff.HPRegenUp = true;
            actor.Buff.SPRegenUp = true;
            actor.Buff.MPRegenUp = true;
            var map = MapManager.Instance.GetMap(actor.MapID);
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.Status.mp_recover_skill -= 15;
            actor.Status.hp_recover_skill -= 15;
            actor.Status.sp_recover_skill -= 15;
            actor.Buff.HPRegenUp = false;
            actor.Buff.SPRegenUp = false;
            actor.Buff.MPRegenUp = false;
            var map = MapManager.Instance.GetMap(actor.MapID);
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        #endregion
    }
}