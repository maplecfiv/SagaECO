using System.Collections.Generic;
using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Wizard
{
    public class EnergyShield : ISkill
    {
        private readonly bool MobUse;

        public EnergyShield()
        {
            MobUse = false;
        }

        public EnergyShield(bool MobUse)
        {
            this.MobUse = MobUse;
        }

        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            if (dActor.Status.Additions.ContainsKey("DevineBarrier")) return -12;
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            if (MobUse) level = 5;


            if (MobUse)
            {
                var map = MapManager.Instance.GetMap(sActor.MapID);
                var affected = map.GetActorsArea(sActor, 500, false);
                var realAffected = new List<Actor>();
                foreach (var act in affected)
                    if (act.type == ActorType.MOB)
                        realAffected.Add(act);

                realAffected.Add(sActor);
                foreach (var i in realAffected)
                    if (!i.Status.Additions.ContainsKey("EnergyShield"))
                    {
                        var skill1 = new DefaultBuff(args.skill, i, "EnergyShield", 900000);
                        skill1.OnAdditionStart += StartEventHandler;
                        skill1.OnAdditionEnd += EndEventHandler;
                        SkillHandler.ApplyAddition(i, skill1);
                    }
            }
            else
            {
                var skill = new DefaultBuff(args.skill, dActor, "EnergyShield", 900000);
                skill.OnAdditionStart += StartEventHandler;
                skill.OnAdditionEnd += EndEventHandler;
                SkillHandler.ApplyAddition(dActor, skill);
            }
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            var defadd = new short[] { 0, 5, 10, 10, 15, 15 }[skill.skill.Level];
            var def = new short[] { 0, 3, 3, 6, 6, 9 }[skill.skill.Level];

            if (skill.Variable.ContainsKey("EnergyShieldDEF"))
                skill.Variable.Remove("EnergyShieldDEF");
            skill.Variable.Add("EnergyShieldDEF", def);

            if (skill.Variable.ContainsKey("EnergyShieldDEFADD"))
                skill.Variable.Remove("EnergyShieldDEFADD");
            skill.Variable.Add("EnergyShieldDEFADD", defadd);
            actor.Status.def_add_skill += defadd;
            actor.Status.def_skill += def;
            actor.Buff.DefUp = true;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.Status.def_skill -= (short)skill.Variable["EnergyShieldDEF"];
            actor.Status.def_add_skill -= (short)skill.Variable["EnergyShieldDEFADD"];
            actor.Buff.DefUp = false;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        #endregion
    }
}