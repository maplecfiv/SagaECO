using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Network.Client;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Parnter
{
    /// <summary>
    ///     私が支えます（坚固光环）
    /// </summary>
    public class ISupport : ISkill
    {
        public enum ISupportUser
        {
            Player,
            Mob,
            Boss,
            Partner
        }

        private readonly ISupportUser user = ISupportUser.Partner;

        public ISupport()
        {
        }

        public ISupport(ISupportUser user)
        {
            this.user = user;
        }

        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 7000 + 1000 * level;
            var map = MapManager.Instance.GetMap(dActor.MapID);
            if (user == ISupportUser.Mob)
            {
                lifetime = 16000;
                dActor = sActor;
            }
            else if (user == ISupportUser.Boss)
            {
                lifetime = 60000;
                dActor = sActor;
            }
            else if (user == ISupportUser.Partner)
            {
                lifetime = 60000;
                var affected = map.GetActorsArea(sActor, 500, false);
                var ActorlowHP = sActor;
                foreach (var act in affected)
                    if (!SkillHandler.Instance.CheckValidAttackTarget(sActor, act))
                    {
                        var a = SagaLib.Global.Random.Next(0, 99);
                        if (a < 40) dActor = act;
                    }
            }

            if (sActor.ActorID == dActor.ActorID)
            {
                var arg2 = new EffectArg();
                arg2.effectID = 5167;
                arg2.actorID = dActor.ActorID;
                map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SHOW_EFFECT, arg2, dActor, true);
            }

            var skill = new DefaultBuff(args.skill, dActor, "MobKyrie", lifetime);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(dActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            if (user == ISupportUser.Mob)
            {
                skill["MobKyrie"] = 7;
            }
            else if (user == ISupportUser.Boss)
            {
                skill["MobKyrie"] = 25;
            }
            else if (user == ISupportUser.Partner)
            {
                skill["MobKyrie"] = 12;
            }
            else
            {
                int[] times = { 0, 5, 5, 6, 6, 7 };
                skill["MobKyrie"] = times[skill.skill.Level];
                if (actor.type == ActorType.PC)
                {
                    var pc = (ActorPC)actor;
                    MapClient.FromActorPC(pc)
                        .SendSystemMessage(string.Format(LocalManager.Instance.Strings.SKILL_STATUS_ENTER,
                            skill.skill.Name));
                }
            }
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            if (actor.type == ActorType.PC)
            {
                var pc = (ActorPC)actor;
                MapClient.FromActorPC(pc)
                    .SendSystemMessage(
                        string.Format(LocalManager.Instance.Strings.SKILL_STATUS_LEAVE, skill.skill.Name));
            }
        }

        #endregion
    }
}