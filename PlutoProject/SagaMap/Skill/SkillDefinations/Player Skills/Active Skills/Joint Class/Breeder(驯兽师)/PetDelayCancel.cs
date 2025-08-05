using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Breeder
{
    /// <summary>
    ///     ペットディレイキャンセル（ペットディレイキャンセル）
    /// </summary>
    public class PetDelayCancel : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 20000;
            var p = SkillHandler.Instance.GetPet(sActor);
            if (p != null)
            {
                var skill = new DefaultBuff(args.skill, p, "PetDelayCancel", lifetime);
                skill.OnAdditionStart += StartEventHandler;
                skill.OnAdditionEnd += EndEventHandler;
                SkillHandler.ApplyAddition(dActor, skill);
            }
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.Status.aspd_skill_perc += 1.5f;

            actor.Buff.DelayCancel = true;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            var raspd_skill_perc_restore = 2.5f;
            if (actor.Status.aspd_skill_perc > raspd_skill_perc_restore)
                actor.Status.aspd_skill_perc -= raspd_skill_perc_restore;
            else
                actor.Status.aspd_skill_perc = 0;

            actor.Buff.DelayCancel = false;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        #endregion
    }
}