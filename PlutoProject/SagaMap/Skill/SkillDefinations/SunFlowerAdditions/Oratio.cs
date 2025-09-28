using SagaMap.Manager;
using SagaMap.Network.Client;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.SunFlowerAdditions
{
    /// <summary>
    ///     祷告（Ragnarok）
    /// </summary>
    public class Oratio : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 300000;
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var affected = map.GetActorsArea(sActor, 1500, true);

            foreach (var act in affected)
            {
                var numd = SagaLib.Global.Random.Next(1, 100);
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, act) && numd <= 60)
                {
                    var skill = new DefaultBuff(args.skill, act, "Oratio", lifetime);
                    skill.OnAdditionStart += StartEventHandler;
                    skill.OnAdditionEnd += EndEventHandler;
                    SkillHandler.ApplyAddition(act, skill);
                    SkillHandler.ApplyAddition(act, skill);
                    var arg = new EffectArg();
                    arg.effectID = 5445;
                    arg.actorID = act.ActorID;
                    arg.x = args.x;
                    arg.y = args.y;
                    map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SHOW_EFFECT, arg, sActor, true);
                }
            }
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            if (actor.type == ActorType.PC) MapClient.FromActorPC((ActorPC)actor).SendSystemMessage("光属性抗性下降");
            actor.Buff.BodyLightElementDown = true;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            if (actor.type == ActorType.PC) MapClient.FromActorPC((ActorPC)actor).SendSystemMessage("光属性抗性恢复正常");
            actor.Buff.BodyLightElementDown = false;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        public void RemoveAddition(Actor actor, string additionName)
        {
        }

        #endregion
    }
}