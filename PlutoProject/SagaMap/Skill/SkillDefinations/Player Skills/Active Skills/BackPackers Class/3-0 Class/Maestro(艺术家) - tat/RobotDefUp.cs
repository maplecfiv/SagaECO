using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._3_0_Class.Maestro_艺术家____tat
{
    internal class RobotDefUp : ISkill
    {
        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            var rank = 0.25f + 0.25f * skill.skill.Level;
            if (skill.Variable.ContainsKey("RobotDefUp"))
                skill.Variable.Remove("RobotDefUp");
            skill.Variable.Add("RobotDefUp", (int)(actor.Status.def_add * rank));
            actor.Status.def_add_skill += (short)(actor.Status.def_add * rank);

            if (skill.Variable.ContainsKey("RobotDefUp2"))
                skill.Variable.Remove("RobotDefUp2");
            skill.Variable.Add("RobotDefUp2", (int)(actor.Status.def * rank));
            actor.Status.def_skill += (short)(actor.Status.def * rank);

            actor.Buff.三转铁匠2足DEFUP = true;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.Status.def_add_skill -= (short)skill.Variable["RobotDefUp"];
            actor.Status.def_skill -= (short)skill.Variable["RobotDefUp2"];

            actor.Buff.三转铁匠2足DEFUP = false;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);

            int[] lifetime = { 0, 60000, 80000, 100000, 125000, 150000 };
            var skill2 = new DefaultBuff(skill.skill, actor, "RobotDefDown", lifetime[skill.skill.Level]);
            skill.OnAdditionStart += StartEventHandler2;
            skill.OnAdditionEnd += EndEventHandler2;
            SkillHandler.ApplyAddition(actor, skill);
        }

        private void StartEventHandler2(Actor actor, DefaultBuff skill)
        {
            actor.Buff.三转机器人UNKNOWS = true;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandler2(Actor actor, DefaultBuff skill)
        {
            actor.Buff.三转机器人UNKNOWS = false;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            var pet = SkillHandler.Instance.GetPet(pc);
            if (pet == null) return -54; //需回傳"需裝備寵物"
            if (SkillHandler.Instance.CheckMobType(pet, "MACHINE_RIDE_ROBOT"))
                //return 0;
                return -54; //封印
            return -54; //需回傳"需裝備寵物"
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 1800000;
            var skill = new DefaultBuff(args.skill, dActor, "RobotDefUp", lifetime);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(dActor, skill);
        }

        #endregion
    }
}
//if (i.Status.Additions.ContainsKey("イレイザー") 