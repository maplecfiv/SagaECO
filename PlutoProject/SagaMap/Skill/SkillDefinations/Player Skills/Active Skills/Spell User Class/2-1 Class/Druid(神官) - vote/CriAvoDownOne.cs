using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._2_1_Class.Druid_神官____vote
{
    /// <summary>
    ///     纏繞的光（ピュニティブライト）
    /// </summary>
    public class CriAvoDownOne : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var rate = 25 + 25 * level;
            if (SkillHandler.Instance.CanAdditionApply(sActor, dActor, "CriAvoDownOne", rate))
            {
                var skill = new DefaultBuff(args.skill, dActor, "CriAvoDownOne", 60000);
                skill.OnAdditionStart += StartEventHandler;
                skill.OnAdditionEnd += EndEventHandler;
                SkillHandler.ApplyAddition(dActor, skill);
            }
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            var cri_down = (int)(actor.Status.hit_critical * 0.5);
            var cri_avoid_down = (int)(actor.Status.avoid_critical * 0.5);
            //會心一擊
            if (skill.Variable.ContainsKey("Cri_Down"))
                skill.Variable.Remove("Cri_Down");
            skill.Variable.Add("Cri_Down", cri_down);
            actor.Status.cri_skill -= (short)cri_down;
            //actor.Status.cri_skill -= 50;
            //會心一擊迴避率?
            if (skill.Variable.ContainsKey("Cri_Avoid_Down"))
                skill.Variable.Remove("Cri_Avoid_Down");
            skill.Variable.Add("Cri_Avoid_Down", cri_avoid_down);
            actor.Status.avoid_critical -= (ushort)cri_avoid_down;
            //actor.Status.avoid_critical -= 50;
            actor.Buff.CriticalDodgeDown = true;
            actor.Buff.CriticalRateDown = true;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.Status.cri_skill += (short)skill.Variable["Cri_Down"];
            actor.Status.avoid_critical += (ushort)skill.Variable["Cri_Avoid_Down"];
            actor.Buff.CriticalRateDown = false;
            actor.Buff.CriticalDodgeDown = false;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        #endregion
    }
}