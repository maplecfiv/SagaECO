using SagaDB.Actor;
using SagaLib;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._1_0_Class.Votes_祭司_
{
    public class HolyShield : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            SkillHandler.RemoveAddition(dActor, "EarthShield");
            SkillHandler.RemoveAddition(dActor, "FireShield");
            SkillHandler.RemoveAddition(dActor, "WaterShield");
            SkillHandler.RemoveAddition(dActor, "WindShield");
            SkillHandler.RemoveAddition(dActor, "DarkShield");
            var life = new[] { 0, 15000, 35000, 60000, 100000, 150000 }[level];
            var skill = new DefaultBuff(args.skill, dActor, "HolyShield", life);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(dActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            var atk1 = skill.skill.Level * 5;
            if (skill.Variable.ContainsKey("HolyShield"))
                skill.Variable.Remove("HolyShield");
            skill.Variable.Add("HolyShield", atk1);
            actor.Status.elements_skill[Elements.Holy] += atk1;

            actor.Buff.BodyHolyElementUp = true;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            var value = skill.Variable["HolyShield"];
            actor.Status.elements_skill[Elements.Holy] -= (short)value;

            actor.Buff.BodyHolyElementUp = false;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        #endregion
    }
}