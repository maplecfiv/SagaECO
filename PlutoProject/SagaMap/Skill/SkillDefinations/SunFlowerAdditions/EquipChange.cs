using SagaDB.Actor;
using SagaLib;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.SunFlowerAdditions
{
    /// <summary>
    ///     巫婆长袍切换（Ragnarok）
    /// </summary>
    public class EquipChange : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 10000;
            var skill = new DefaultBuff(args.skill, sActor, "EquipChange", lifetime);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(sActor, skill);
        }


        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.Elements[Elements.Holy] -= 100;
            actor.Elements[Elements.Dark] += 100;
            actor.Buff.BodyDarkElementUp = true;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            SkillHandler.Instance.ShowEffectByActor(actor, 5241);
            actor.Elements[Elements.Holy] += 100;
            actor.Elements[Elements.Dark] -= 100;
            actor.Buff.BodyDarkElementUp = false;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        #endregion
    }
}