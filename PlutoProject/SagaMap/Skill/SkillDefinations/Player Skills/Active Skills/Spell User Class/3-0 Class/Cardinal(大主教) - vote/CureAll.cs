using SagaDB.Actor;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._3_0_Class.Cardinal_大主教____vote
{
    public class CureAll : ISkill
    {
        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            if (sActor.Party != null) return 0;
            return -12;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var sPC = (ActorPC)sActor;
            var cureRate = new[] { 0, 40, 60, 60, 60, 60, 100 };
            foreach (var act in sPC.Party.Members.Values)
                if (act.Online)
                    if (act.Party.ID != 0 && !act.Buff.Dead && act.MapID == sActor.MapID)
                        if (SagaLib.Global.Random.Next(0, 100) <= cureRate[level])
                        {
                            RemoveAddition(dActor, "Poison");
                            RemoveAddition(dActor, "鈍足");
                            RemoveAddition(dActor, "Stone");
                            RemoveAddition(dActor, "Silence");
                            RemoveAddition(dActor, "Stun");
                            RemoveAddition(dActor, "Sleep");
                            RemoveAddition(dActor, "Frosen");
                            RemoveAddition(dActor, "Confuse");
                        }
        }

        public void RemoveAddition(Actor actor, string additionName)
        {
            if (actor.Status.Additions.ContainsKey(additionName))
            {
                var addition = actor.Status.Additions[additionName];
                actor.Status.Additions.Remove(additionName);
                if (addition.Activated) addition.AdditionEnd();
                addition.Activated = false;
            }
        }
    }
}