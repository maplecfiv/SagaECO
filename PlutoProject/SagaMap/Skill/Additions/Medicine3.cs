using System;
using SagaDB.Actor;
using SagaLib;
using SagaMap.Manager;

namespace SagaMap.Skill.Additions
{
    public class Medicine3 : DefaultBuff
    {
        public Medicine3(SagaDB.Skill.Skill skill, Actor actor, int lifetime)
            : base(skill, actor, "Medicine3", lifetime,
                (int)(2000.0f * (1.0f - Math.Max(actor.Status.cspd, actor.Status.aspd) / 1000f)))
        {
            OnAdditionStart += StartEvent;
            OnAdditionEnd += EndEvent;
            OnUpdate += TimerUpdate;
        }

        private void StartEvent(Actor actor, DefaultBuff skill)
        {
            var map = MapManager.Instance.GetMap(actor.MapID);
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);

            if (skill.Variable.ContainsKey("MedicineHealing3"))
                skill.Variable.Remove("MedicineHealing3");

            int spadd = actor.Status.sp_medicine;
            skill.Variable.Add("MedicineHealing3", spadd);
        }

        private void EndEvent(Actor actor, DefaultBuff skill)
        {
            var map = MapManager.Instance.GetMap(actor.MapID);
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);

            var recover = (uint)skill.Variable["MedicineHealing3"];
            if (actor.Status.Additions.ContainsKey("FoodFighter"))
            {
                var dps = actor.Status.Additions["FoodFighter"] as DefaultPassiveSkill;
                var rate = dps.Variable["FoodFighter"] / 100.0f + 1.0f;
                recover = (uint)(recover * rate);
            }

            if (actor.HP > 0 && !actor.Buff.Dead)
            {
                if (actor.SP < actor.MaxSP - recover)
                    actor.SP += recover;
                else
                    actor.SP = actor.MaxSP;
            }

            SkillHandler.Instance.ShowVessel(actor, 0, 0, -(int)recover);
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.HPMPSP_UPDATE, null, actor, true);
            actor.Status.sp_medicine -= (short)skill.Variable["MedicineHealing3"];
        }

        private void TimerUpdate(Actor actor, DefaultBuff skill)
        {
            //测试去除技能同步锁ClientManager.EnterCriticalArea();
            try
            {
                if (actor.HP > 0 && !actor.Buff.Dead)
                {
                    var map = MapManager.Instance.GetMap(actor.MapID);
                    var recover = (uint)skill.Variable["MedicineHealing3"];
                    if (actor.Status.Additions.ContainsKey("FoodFighter"))
                    {
                        var dps = actor.Status.Additions["FoodFighter"] as DefaultPassiveSkill;
                        var rate = dps.Variable["FoodFighter"] / 100.0f + 1.0f;
                        recover = (uint)(recover * rate);
                    }

                    if (actor.HP > 0 && !actor.Buff.Dead)
                    {
                        if (actor.SP < actor.MaxSP - recover)
                            actor.SP += recover;
                        else
                            actor.SP = actor.MaxSP;
                    }

                    SkillHandler.Instance.ShowVessel(actor, 0, 0, -(int)recover);
                    map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.HPMPSP_UPDATE, null, actor, true);
                }
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);
            }
            //测试去除技能同步锁ClientManager.LeaveCriticalArea();
        }
    }
}