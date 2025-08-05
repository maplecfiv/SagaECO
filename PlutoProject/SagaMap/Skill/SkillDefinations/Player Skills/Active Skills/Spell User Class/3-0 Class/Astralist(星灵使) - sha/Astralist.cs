using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._3_0_Class.Astralist_星灵使____sha
{
    /// <summary>
    ///     �����ȥ�ꥹ��
    /// </summary>
    public class Astralist : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 30000 + 30000 * level;
            var skill = new DefaultBuff(args.skill, sActor, "Astralist", lifetime, 1000);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            skill.OnUpdate += UpdateEventHandler;
            SkillHandler.ApplyAddition(sActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            //X
            if (skill.Variable.ContainsKey("Save_X"))
                skill.Variable.Remove("Save_X");
            skill.Variable.Add("Save_X", actor.X);

            //Y
            if (skill.Variable.ContainsKey("Save_Y"))
                skill.Variable.Remove("Save_Y");
            skill.Variable.Add("Save_Y", actor.Y);
            //float up = 0.5f * skill.skill.Level;
            var attackvalue = 200 + 20 * skill.skill.Level;
            if (skill.Variable.ContainsKey("Astralist"))
                skill.Variable.Remove("Astralist");
            skill.Variable.Add("Astralist", attackvalue);

            //actor.Status.ElementDamegeUp_rate += up;
            actor.Buff.MainSkillPowerUp3RD = true;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            //actor.Status.ElementDamegeUp_rate = 0;
            if (skill.Variable.ContainsKey("Astralist"))
                skill.Variable.Remove("Astralist");
            actor.Buff.MainSkillPowerUp3RD = false;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void UpdateEventHandler(Actor actor, DefaultBuff skill)
        {
            if (actor.X != (short)skill.Variable["Save_X"] || actor.Y != (short)skill.Variable["Save_Y"])
            {
                var map = MapManager.Instance.GetMap(actor.MapID);
                actor.Status.Additions["Astralist"].AdditionEnd();
                actor.Status.Additions.Remove("Astralist");
                skill.Variable.Remove("Save_X");
                skill.Variable.Remove("Save_Y");
                map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, false);
            }
        }

        #endregion
    }
}