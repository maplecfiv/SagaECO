using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._1_0_Class.Scout_盗贼_
{
    /// <summary>
    ///     敏捷的動作
    /// </summary>
    public class AvoidMeleeUp : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 60000 * level;
            Actor realactor = SkillHandler.Instance.GetPossesionedActor((ActorPC)sActor);
            var skill = new DefaultBuff(args.skill, realactor, "AvoidBurst", lifetime);
            skill.OnAdditionStart += skill_OnAdditionStart;
            skill.OnAdditionEnd += skill_OnAdditionEnd;
            SkillHandler.ApplyAddition(realactor, skill);
        }

        private void skill_OnAdditionStart(Actor actor, DefaultBuff skill)
        {
            int level = skill.skill.Level;
            var savo = new short[] { 0, 6, 12, 18, 24, 30 }[level];
            if (skill.Variable.ContainsKey("AvoidBurst"))
                skill.Variable.Remove("AvoidBurst");
            skill.Variable.Add("AvoidBurst", savo);
            actor.Status.avoid_melee_skill += savo;
            actor.Buff.ShortDodgeUp = true;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void skill_OnAdditionEnd(Actor actor, DefaultBuff skill)
        {
            actor.Status.avoid_melee_skill -= (short)skill.Variable["AvoidBurst"];
            actor.Buff.ShortDodgeUp = false;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        //#endregion
    }
}