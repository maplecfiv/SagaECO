using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Network.Client;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._2_1_Class.Sorcerer_魔导师____wiz
{
    /// <summary>
    ///     固体光环（ソリッドオーラ）
    /// </summary>
    public class SolidAura : ISkill
    {
        public enum KyrieUser
        {
            Player,
            Mob,
            Boss
        }

        private readonly KyrieUser user = KyrieUser.Player;

        public SolidAura()
        {
        }

        public SolidAura(KyrieUser user)
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
            if (user == KyrieUser.Mob)
            {
                lifetime = 16000;
                dActor = sActor;
            }
            else if (user == KyrieUser.Boss)
            {
                lifetime = 60000;
                dActor = sActor;
            }

            var map = MapManager.Instance.GetMap(dActor.MapID);
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
            if (user == KyrieUser.Mob)
            {
                skill["MobKyrie"] = 7;
            }
            else if (user == KyrieUser.Boss)
            {
                skill["MobKyrie"] = 25;
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