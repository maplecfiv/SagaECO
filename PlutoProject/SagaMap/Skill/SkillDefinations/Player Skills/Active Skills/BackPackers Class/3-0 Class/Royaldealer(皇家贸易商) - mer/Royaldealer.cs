using System;
using System.Linq;
using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._3_0_Class.
    Royaldealer_皇家贸易商____mer
{
    internal class RoyalDealer : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var pc = (ActorPC)sActor;
            if (pc.Skills3.ContainsKey(989) || pc.DualJobSkill.Exists(x => x.ID == 989))
            {
                var duallv = 0;
                if (pc.DualJobSkill.Exists(x => x.ID == 989))
                    duallv = pc.DualJobSkill.FirstOrDefault(x => x.ID == 989).Level;

                var mainlv = 0;
                if (pc.Skills3.ContainsKey(989))
                    mainlv = pc.Skills3[989].Level;

                var maxlv = Math.Max(duallv, mainlv);
                pc.Gold -= new[]
                {
                    0, 0, (int)(100.0f * (1.0f - 0.03f * maxlv)), (int)(250.0f * (1.0f - 0.03f * maxlv)),
                    (int)(500.0f * (1.0f - 0.03f * maxlv)), (int)(1000.0f * (1.0f - 0.03f * maxlv))
                }[level];
            }
            else
            {
                pc.Gold -= new[] { 0, 0, 100, 250, 500, 1000 }[level];
            }


            var lifetime = 30000 + 30000 * level;
            var skill = new DefaultBuff(args.skill, sActor, "RoyalDealer", lifetime, 1000);
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
            actor.Buff.MainSkillPowerUp3RD = true;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.Buff.MainSkillPowerUp3RD = false;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void UpdateEventHandler(Actor actor, DefaultBuff skill)
        {
            if (actor.X != (short)skill.Variable["Save_X"] || actor.Y != (short)skill.Variable["Save_Y"])
            {
                var map = MapManager.Instance.GetMap(actor.MapID);
                actor.Status.Additions["RoyalDealer"].AdditionEnd();
                actor.Status.Additions.Remove("RoyalDealer");
                actor.Buff.MainSkillPowerUp3RD = false;
                map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, false);
            }
        }

        #endregion
    }
}