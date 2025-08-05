using SagaDB.Actor;
using SagaLib;
using SagaMap.Manager;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Astralist
{
    /// <summary>
    ///     ����֮��Լ����
    /// </summary>
    public class YugenKeiyaku : ISkill
    {
        public SkillArg arg = new SkillArg();

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            if (sActor.WeaponElement != Elements.Neutral) return 0;
            return -12;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 150000 + 30000 * level;
            var skill = new DefaultBuff(args.skill, dActor, "YugenKeiyaku", lifetime);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(dActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            //��ʱ�Ҳ�����ȷ��ͼ��
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }
    }
}