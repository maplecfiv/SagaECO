using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._3_0_Class.SoulTaker_噬魂者____lock
{
    public class SoulTaker : ISkill
    {
        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            if (!dActor.Status.Additions.ContainsKey("SoulTaker"))
            {
                var lifetime = (30 + level * 30) * 1000;
                var rate = 175 + level * 25;
                var skill = new SoulTakerBuff(args.skill, sActor, lifetime, rate);
                //skill.OnUpdate += this.UpdateEventHandler;
                SkillHandler.ApplyAddition(sActor, skill);
            }
            else
            {
                sActor.Status.Additions["SoulTaker"].OnTimerEnd();
            }
        }

        //void UpdateEventHandler(Actor actor, DefaultBuff skill)
        //{
        //    //SagaMap.Network.Client.MapClient.FromActorPC((ActorPC)actor).SendSystemMessage("移动速度固定" + actor.Speed);
        //    if (actor.Speed != 310)
        //    {
        //        actor.Status.speed_skill = -100;
        //        actor.Status.speed_item = 0;
        //    }
        //}
        public class SoulTakerBuff : DefaultBuff
        {
            public SoulTakerBuff(SagaDB.Skill.Skill skill, Actor actor, int lifetime, int rate)
                : base(skill, actor, "SoulTaker", lifetime, 1000)
            {
                OnAdditionStart += StartEvent;
                OnAdditionEnd += EndEvent;
                this["rate"] = rate;
            }

            private void StartEvent(Actor actor, DefaultBuff skill)
            {
                int level = skill.skill.Level;
                var rate = 175 + level * 25;


                if (skill.Variable.ContainsKey("SoulTaker"))

                    skill.Variable.Remove("SoulTaker");

                skill.Variable.Add("SoulTaker", rate);
                //Speed Limit 
                var speed_add = 100;
                if (skill.Variable.ContainsKey("SoulTaker_speed"))
                    skill.Variable.Remove("SoulTaker_speed");
                skill.Variable.Add("SoulTaker_speed", speed_add);
                actor.Status.speed_skill -= (ushort)speed_add;
                actor.Buff.MainSkillPowerUp3RD = true;
                MapManager.Instance.GetMap(actor.MapID)
                    .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
            }

            private void EndEvent(Actor actor, DefaultBuff skill)
            {
                if (skill.Variable.ContainsKey("SoulTaker"))
                    skill.Variable.Remove("SoulTaker");
                actor.Status.speed_skill += (ushort)skill.Variable["SoulTaker_speed"];
                if (skill.Variable.ContainsKey("SoulTaker_speed"))
                    skill.Variable.Remove("SoulTaker_speed");
                actor.Buff.MainSkillPowerUp3RD = false;
                MapManager.Instance.GetMap(actor.MapID)
                    .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
            }
        }
    }
}