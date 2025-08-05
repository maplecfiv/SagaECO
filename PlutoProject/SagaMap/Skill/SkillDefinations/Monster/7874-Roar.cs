using System.Collections.Generic;
using SagaDB.Actor;
using SagaLib;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Monster
{
    internal class Roar : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 1.8f;
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var actors = map.GetActorsArea(SagaLib.Global.PosX8to16(args.x, map.Width),
                SagaLib.Global.PosY8to16(args.y, map.Height), 500, null);
            var affected = new List<Actor>();
            foreach (var i in actors)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, i))
                {
                    affected.Add(i);
                    if (SkillHandler.Instance.CanAdditionApply(sActor, i, SkillHandler.DefaultAdditions.Stun, 50))
                    {
                        var skill = new Stun(args.skill, i, 5000);
                        SkillHandler.ApplyAddition(i, skill);
                    }
                }

            SkillHandler.Instance.MagicAttack(sActor, affected, args, Elements.Neutral, factor);
        }

        #endregion
    }
}