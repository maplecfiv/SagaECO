using System.Collections.Generic;
using SagaDB.Actor;
using SagaLib;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Global.Active
{
    /// <summary>
    ///     元素风暴,各属性风暴通用技能
    ///     サンダーストーム(闪电风暴)
    ///     アクアストーム(水瓶风暴)
    ///     アースストーム(大地风暴)
    ///     ファイアストーム(烈焰风暴)
    /// </summary>
    public class ElementStorm : ISkill
    {
        private readonly Elements element;

        public ElementStorm(Elements e)
        {
            element = e;
        }

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 1.5f + 0.5f * level;
            var actorS = new ActorSkill(args.skill, sActor);
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var actors = map.GetActorsArea(SagaLib.Global.PosX8to16(args.x, map.Width),
                SagaLib.Global.PosY8to16(args.y, map.Height), 250, null);
            var affected = new List<Actor>();
            foreach (var i in actors)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, i))
                    affected.Add(i);

            SkillHandler.Instance.MagicAttack(sActor, affected, args, element, factor);
        }
    }
}