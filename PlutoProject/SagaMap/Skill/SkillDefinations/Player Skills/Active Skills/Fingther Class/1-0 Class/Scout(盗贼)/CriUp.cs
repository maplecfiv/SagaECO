using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._1_0_Class.Scout_盗贼_
{
    /// <summary>
    ///     會心一擊
    /// </summary>
    public class CriUp : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        private bool CheckPossible(Actor sActor)
        {
            return true;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var life = 10000 - 1000 * (level - 1);
            args.dActor = 0;
            Actor realdActor = SkillHandler.Instance.GetPossesionedActor((ActorPC)sActor);
            if (CheckPossible(realdActor))
            {
                var skill = new DefaultBuff(args.skill, realdActor, "CriUp", life);
                skill.OnAdditionStart += StartEventHandler;
                skill.OnAdditionEnd += EndEventHandler;
                SkillHandler.ApplyAddition(realdActor, skill);
            }
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            var rate = (short)(skill.skill.Level * 10);
            if (skill.Variable.ContainsKey("CriUp"))
                skill.Variable.Remove("CriUp");
            skill.Variable.Add("CriUp", rate);
            actor.Status.cri_skill += rate;

            actor.Buff.CriticalRateUp = true;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            var value = skill.Variable["CriUp"];
            actor.Status.cri_skill -= (short)value;

            actor.Buff.CriticalRateUp = false;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        //#endregion
    }
}