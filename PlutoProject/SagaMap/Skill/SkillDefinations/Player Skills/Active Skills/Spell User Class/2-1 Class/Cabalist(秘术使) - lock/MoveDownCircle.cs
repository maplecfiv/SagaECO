using System.Collections.Generic;
using SagaDB.Actor;
using SagaMap.ActorEventHandlers;
using SagaMap.Manager;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Cabalist
{
    /// <summary>
    ///     束手就擒（ヒンダーハンド）
    /// </summary>
    public class MoveDownCircle : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 3000 * level;
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var actor = new ActorSkill(args.skill, sActor);
            //设定技能体位置
            actor.MapID = sActor.MapID;
            actor.X = SagaLib.Global.PosX8to16(args.x, map.Width);
            actor.Y = SagaLib.Global.PosY8to16(args.y, map.Height);
            //设定技能体的事件处理器，由于技能体不需要得到消息广播，因此创建个空处理器
            actor.e = new NullEventHandler();
            var affected = map.GetActorsArea(actor, 150, false);
            var realAffected = new List<Actor>();
            foreach (var act in affected)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, act))
                {
                    var skill = new MoveSpeedDown(args.skill, act, lifetime);
                    SkillHandler.ApplyAddition(act, skill);
                }
        }

        #endregion
    }
}