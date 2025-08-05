using System;
using SagaDB.Marionette;
using SagaDB.Skill;
using SagaLib;
using SagaMap.PC;
using SagaMap.Skill;
using Marionette = SagaMap.Tasks.PC.Marionette;

namespace SagaMap.Network.Client
{
    public partial class MapClient
    {
        public void MarionetteActivate(uint marionetteID)
        {
            MarionetteActivate(marionetteID, true, true);
        }

        public void MarionetteActivate(uint marionetteID, bool delay, bool duration)
        {
            var marionette = MarionetteFactory.Instance[marionetteID];
            if (marionette != null)
            {
                var task = new Marionette(this, marionette.Duration);
                if (Character.Tasks.ContainsKey("Marionette") && duration)
                {
                    MarionetteDeactivate();
                    Character.Tasks["Marionette"].Deactivate();
                    Character.Tasks.Remove("Marionette");
                }

                if (!duration && Character.Marionette != null)
                {
                    foreach (uint i in Character.Marionette.skills)
                    {
                        var skill = SkillFactory.Instance.GetSkill(i, 1);
                        if (skill != null)
                            if (Character.Skills.ContainsKey(i))
                                Character.Skills.Remove(i);
                    }

                    SkillHandler.Instance.CastPassiveSkills(Character);
                }

                if (!Character.Tasks.ContainsKey("Marionette"))
                {
                    Character.Tasks.Add("Marionette", task);
                    task.Activate();
                }

                if (delay)
                {
                    if (!Character.Status.Additions.ContainsKey("MarioTimeUp"))
                        Character.NextMarionetteTime = DateTime.Now + new TimeSpan(0, 0, marionette.Delay);
                    else
                        Character.NextMarionetteTime =
                            DateTime.Now + new TimeSpan(0, 0, (int)(marionette.Delay * 0.6f));
                }

                Character.Marionette = marionette;
                SendCharInfoUpdate();
                foreach (uint i in marionette.skills)
                {
                    var skill = SkillFactory.Instance.GetSkill(i, 1);
                    if (skill != null)
                        if (!Character.Skills.ContainsKey(i))
                        {
                            skill.NoSave = true;
                            Character.Skills.Add(i, skill);
                            if (!skill.BaseData.active)
                            {
                                var arg = new SkillArg();
                                arg.skill = skill;
                                SkillHandler.Instance.SkillCast(Character, Character, arg);
                            }
                        }
                }

                StatusFactory.Instance.CalcStatus(Character);
                SendPlayerInfo();
            }
        }

        public void MarionetteDeactivate()
        {
            MarionetteDeactivate(false);
        }

        public void MarionetteDeactivate(bool disconnecting)
        {
            if (Character.Marionette == null)
                return;
            var marionette = Character.Marionette;
            Character.Marionette = null;
            if (!disconnecting) SendCharInfoUpdate();
            foreach (uint i in marionette.skills)
            {
                var skill = SkillFactory.Instance.GetSkill(i, 1);
                if (skill != null)
                    if (Character.Skills.ContainsKey(i))
                        Character.Skills.Remove(i);
            }

            SkillHandler.Instance.CastPassiveSkills(Character);
            StatusFactory.Instance.CalcStatus(Character);
            if (!disconnecting)
            {
                SendPlayerInfo();
                SendMotion(MotionType.JOY, 0);
            }
        }
    }
}