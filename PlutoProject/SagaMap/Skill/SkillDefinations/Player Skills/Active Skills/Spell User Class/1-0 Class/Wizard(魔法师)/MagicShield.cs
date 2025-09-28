using System.Collections.Generic;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._1_0_Class.Wizard_魔法师_
{
    public class MagicShield : ISkill
    {
        private readonly bool MobUse;

        public MagicShield()
        {
            MobUse = false;
        }

        public MagicShield(bool MobUse)
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
                    if (!i.Status.Additions.ContainsKey("MagicShield"))
                    {
                        var skill1 = new DefaultBuff(args.skill, i, "MagicShield", 900000);
                        skill1.OnAdditionStart += StartEventHandler;
                        skill1.OnAdditionEnd += EndEventHandler;
                        SkillHandler.ApplyAddition(i, skill1);
                    }
            }
            else
            {
                var skill = new DefaultBuff(args.skill, dActor, "MagicShield", 9000000);
                skill.OnAdditionStart += StartEventHandler;
                skill.OnAdditionEnd += EndEventHandler;
                SkillHandler.ApplyAddition(dActor, skill);
            }
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            var mdefadd = new short[] { 0, 5, 10, 10, 15, 15 }[skill.skill.Level];
            var mdef = (short)(4 * skill.skill.Level);

            if (skill.Variable.ContainsKey("MagicShieldMDEF"))
                skill.Variable.Remove("MagicShieldMDEF");
            skill.Variable.Add("MagicShieldMDEF", mdef);

            if (skill.Variable.ContainsKey("MagicShieldMDEFADD"))
                skill.Variable.Remove("MagicShieldMDEFADD");
            skill.Variable.Add("MagicShieldMDEFADD", mdefadd);
            actor.Status.mdef_add_skill += mdefadd;
            actor.Status.mdef_skill += mdef;
            actor.Buff.MagicDefUp = true;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.Status.mdef_skill -= (short)skill.Variable["MagicShieldMDEF"];
            actor.Status.mdef_add_skill -= (short)skill.Variable["MagicShieldMDEFADD"];
            actor.Buff.MagicDefUp = false;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        #endregion
    }
}