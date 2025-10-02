using System;
using System.Collections.Generic;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._2_1_Class.Assassin_刺客____sco
{
    /// <summary>
    ///     氣合（気合）
    /// </summary>
    public class WillPower : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var rate = new short[] { 0, 30, 50, 30, 50, 60 }[level];
            var actorPC = (ActorPC)sActor;
            try
            {
                var WillBeRemove = new List<string>();
                foreach (var s in actorPC.Status.Additions)
                    if (SagaLib.Global.Random.Next(100) < rate)
                    {
                        var addition = s.Value;
                        WillBeRemove.Add(s.Key);
                        if (addition.Activated)
                        {
                            addition.AdditionEnd();
                            addition.Activated = false;
                        }
                    }

                foreach (var AdditionName in WillBeRemove) actorPC.Status.Additions.Remove(AdditionName);
            }
            catch (Exception)
            {
            }
        }

        //#endregion
    }
}