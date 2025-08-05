using SagaDB.Actor;
using SagaLib;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.COF_Additions.丢弃
{
    internal class Fengshenlegs : MobISkill
    {
        #region ISkill Members

        public void BeforeCast(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            if (dActor.Status.Additions.ContainsKey("Parry"))
            {
                var stun = new Stun(args.skill, dActor, 5000);
                SkillHandler.ApplyAddition(dActor, stun);
                var arg = new EffectArg();
                arg.effectID = 5133;
                arg.actorID = dActor.ActorID;
                MapManager.Instance.GetMap(sActor.MapID)
                    .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SHOW_EFFECT, arg, dActor, true);
                SkillHandler.Instance.FixAttack(sActor, dActor, args, Elements.Neutral, 888);
            }
            else
            {
                SkillHandler.Instance.FixAttack(sActor, dActor, args, Elements.Neutral, 100);
            }
        }

        #endregion
    }
}