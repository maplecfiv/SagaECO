using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Gladiator
{
    /// <summary>
    ///     グラディエイター
    /// </summary>
    public class Gladiator : ISkill
    {
        #region ISkill 成員

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public ushort speed_old;

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var pc = (ActorPC)sActor;
            ApplySkill(pc, args);
        }

        public void ApplySkill(ActorPC dActor, SkillArg args)
        {
            var lifetimes = new[] { 0, 180000, 150000, 120000, 90000, 60000 };
            var lifetime = lifetimes[args.skill.Level];
            if (!dActor.Status.Additions.ContainsKey("Gladiator"))
            {
                var skill = new DefaultBuff(args.skill, dActor, "Gladiator", lifetime, 1000);
                skill.OnAdditionStart += StartEventHandler;
                skill.OnAdditionEnd += EndEventHandler;
                //skill.OnUpdate += this.UpdateEventHandler;
                SkillHandler.ApplyAddition(dActor, skill);
            }
            else
            {
                dActor.Status.Additions["Gladiator"].OnTimerEnd();
            }
        }

        //void UpdateEventHandler(Actor actor, DefaultBuff skill)
        //{
        //    SagaMap.Network.Client.MapClient.FromActorPC((ActorPC)actor).SendSystemMessage("当前速度为" + actor.Speed);
        //    if (actor.Status.speed_skill != -100)
        //    {
        //        actor.Status.speed_skill = -100;
        //    }
        //}
        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            //actor.Status.speed_skill -= 300;
            var speed_add = 100;
            if (skill.Variable.ContainsKey("Gladiator_speed"))
                skill.Variable.Remove("Gladiator_speed");
            skill.Variable.Add("Gladiator_speed", speed_add);
            actor.Status.speed_skill -= (ushort)speed_add;
            actor.Buff.MainSkillPowerUp3RD = true;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.Status.speed_skill += (ushort)skill.Variable["Gladiator_speed"];
            actor.Buff.MainSkillPowerUp3RD = false;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        #endregion
    }
}