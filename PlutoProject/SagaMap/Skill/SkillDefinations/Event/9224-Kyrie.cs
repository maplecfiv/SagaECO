using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Network.Client;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Event
{
    /// <summary>
    ///     スキル石?救援邀請（スキル石?ソリッドオーラ）
    /// </summary>
    public class Kyrie : ISkill
    {
        private readonly bool MobUse;

        public Kyrie()
        {
        }

        public Kyrie(bool MobUse)
        {
            this.MobUse = MobUse;
        }

        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            level = 5;
            var lifetime = 7000 + 1000 * level;
            if (MobUse) lifetime = 16000;
            var skill = new DefaultBuff(args.skill, dActor, "MobKyrie", lifetime);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(dActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            if (MobUse)
            {
                skill["MobKyrie"] = 7;
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