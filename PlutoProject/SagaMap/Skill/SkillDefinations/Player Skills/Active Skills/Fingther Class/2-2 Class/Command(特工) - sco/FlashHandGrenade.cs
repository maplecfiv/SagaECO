using System.Collections.Generic;
using SagaDB.Actor;
using SagaMap.ActorEventHandlers;
using SagaMap.Manager;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Command
{
    /// <summary>
    ///     閃光手榴彈（フラッシュグレネード）
    /// </summary>
    public class FlashHandGrenade : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            int[] rate = { 0, 40, 55, 70 };
            int[] lifeTime = { 0, 10000, 9000, 8000 };
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var actor = new ActorSkill(args.skill, sActor);
            //设定技能体位置
            actor.MapID = sActor.MapID;
            actor.X = SagaLib.Global.PosX8to16(args.x, map.Width);
            actor.Y = SagaLib.Global.PosY8to16(args.y, map.Height);
            //设定技能体的事件处理器，由于技能体不需要得到消息广播，因此创建个空处理器
            actor.e = new NullEventHandler();
            var affected = map.GetActorsArea(actor, 350, false);
            var realAffected = new List<Actor>();
            foreach (var act in affected)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, act))
                    if (SkillHandler.Instance.CanAdditionApply(sActor, act, SkillHandler.DefaultAdditions.鈍足,
                            rate[level]))
                    {
                        var skill = new MoveSpeedDown(args.skill, act, lifeTime[level]);
                        SkillHandler.ApplyAddition(act, skill);
                        realAffected.Add(act);
                    }
        }

        #endregion
    }
}