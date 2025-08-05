using System.Collections.Generic;
using SagaDB.Actor;
using SagaLib;
using SagaMap.ActorEventHandlers;
using SagaMap.Manager;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Enchanter
{
    /// <summary>
    ///     重力刃 (グラヴィティフォール)
    /// </summary>
    public class GravityFall : ISkill
    {
        private readonly bool MobUse;

        public GravityFall()
        {
            MobUse = false;
        }

        public GravityFall(bool MobUse)
        {
            this.MobUse = MobUse;
        }

        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            if (MobUse) level = 5;
            var factor = 2.5f + 0.8f * level;

            var rate = 20 + level * 6;
            var rate2 = (4 + level * 2) * 1000;
            var pos = new short[2];
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var actor = new ActorSkill(args.skill, sActor);
            //设定技能体位置
            actor.MapID = sActor.MapID;
            actor.X = SagaLib.Global.PosX8to16(args.x, map.Width);
            actor.Y = SagaLib.Global.PosY8to16(args.y, map.Height);
            //设定技能体的事件处理器，由于技能体不需要得到消息广播，因此创建个空处理器
            actor.e = new NullEventHandler();
            var affected = map.GetActorsArea(actor, 250, false);
            var realAffected = new List<Actor>();
            var onlydamage = new List<Actor>();
            foreach (var act in affected)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, act))
                {
                    if (SkillHandler.Instance.CanAdditionApply(sActor, dActor, SkillHandler.DefaultAdditions.鈍足, rate))
                    {
                        var skill = new MoveSpeedDown(args.skill, act, rate2);
                        SkillHandler.ApplyAddition(act, skill);
                    }

                    realAffected.Add(act);
                }

            SkillHandler.Instance.MagicAttack(sActor, realAffected, args, Elements.Earth, factor);
        }

        #endregion
    }
}