using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Network.Client;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._1_0_Class.Swordman_剑士_
{
    /// <summary>
    ///     反擊
    /// </summary>
    public class Counter : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            args.dActor = 0; //不显示效果
            var lifetime = 30000 + 30000 * level;
            var skill = new DefaultBuff(args.skill, dActor, "Counter", lifetime);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(dActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            if (skill.Variable.ContainsKey("Counter"))
                skill.Variable.Remove("Counter");
            skill.Variable.Add("Counter", 50 + 50 * skill.skill.Level);
            if (actor.type == ActorType.PC)
            {
                var pc = (ActorPC)actor;
                MapClient.FromActorPC(pc)
                    .SendSystemMessage(
                        string.Format(LocalManager.Instance.Strings.SKILL_STATUS_ENTER, skill.skill.Name));
            }
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            if (skill.Variable.ContainsKey("Counter"))
                skill.Variable.Remove("Counter");
            if (actor.type == ActorType.PC)
            {
                var pc = (ActorPC)actor;
                MapClient.FromActorPC(pc)
                    .SendSystemMessage(
                        string.Format(LocalManager.Instance.Strings.SKILL_STATUS_LEAVE, skill.skill.Name));
            }
        }

        //#endregion
    }
}