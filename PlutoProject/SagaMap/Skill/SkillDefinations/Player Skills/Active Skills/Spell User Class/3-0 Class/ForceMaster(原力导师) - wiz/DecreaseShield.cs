using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._3_0_Class.ForceMaster_原力导师____wiz
{
    public class DecreaseShield : ISkill
    {
        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            if (args.skill.Level == 2 || args.skill.Level == 4)
            {
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, dActor)) return 0;
                return -12;
            }

            if (!SkillHandler.Instance.CheckValidAttackTarget(sActor, dActor))
            {
                if (dActor.ActorID == sActor.ActorID)
                    return 0;
                if (dActor.type == ActorType.PC && sActor.type == ActorType.PC)
                {
                    var spc = sActor;
                    var dpc = (ActorPC)dActor;
                    if (spc.Party != null && dpc.Party != null)
                    {
                        if (dpc.Buff.Dead) return -11;

                        if (dpc.PossessionTarget != 0) return -4;

                        if (spc.Party.ID == dpc.Party.ID) return 0;
                    }

                    return -12;
                }

                return -12;
            }

            return -12;
            return -12;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 40000 + 40000 * level;
            var skill = new DefaultBuff(args.skill, dActor, "DecreaseShield", lifetime);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(dActor, skill);
            if (dActor.ActorID == sActor.ActorID)
            {
                Map map = map = MapManager.Instance.GetMap(sActor.MapID);
                var arg2 = new EffectArg();
                arg2.effectID = 4294;
                arg2.actorID = sActor.ActorID;
                map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SHOW_EFFECT, arg2, sActor, true);
            }
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.Buff.BodyNatureElementUp = true;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.Buff.BodyNatureElementUp = false;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }
    }
}