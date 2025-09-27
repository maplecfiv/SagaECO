using System.Collections.Generic;
using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Network.Client;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._3_0_Class.ForceMaster_原力导师____wiz
{
    /// <summary>
    ///     ディスペルフィールド
    /// </summary>
    internal class DispelField : ISkill
    {
        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 60000 * level;
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var actors = map.GetActorsArea(sActor, 200, true);
            var affected = new List<Actor>();
            if (actors.Count >= 0)
            {
                foreach (var i in actors)
                    if (i is ActorPC)
                        if (!SkillHandler.Instance.CheckValidAttackTarget(sActor, i))
                            affected.Add(i);
                foreach (var item in affected)
                {
                    var skill = new DefaultBuff(args.skill, item, "DispelField", lifetime);
                    skill.OnAdditionStart += StartEventHandler;
                    skill.OnAdditionEnd += EndEventHandler;
                    SkillHandler.ApplyAddition(item, skill);
                }
            }
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            var timeses = new[] { 0, 1, 1, 1, 2, 3 };
            var times = timeses[skill.skill.Level];
            skill["DispelField"] = times;
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
            if (actor.type == ActorType.PC)
            {
                var pc = (ActorPC)actor;
                MapClient.FromActorPC(pc)
                    .SendSystemMessage(
                        string.Format(LocalManager.Instance.Strings.SKILL_STATUS_LEAVE, skill.skill.Name));
            }
        }
    }
}