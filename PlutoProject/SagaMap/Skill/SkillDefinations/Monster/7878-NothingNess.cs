using System.Collections.Generic;
using SagaLib;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Monster
{
    public class NothingNess : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 4.0f;
            var actors = MapManager.Instance.GetMap(dActor.MapID).GetActorsArea(dActor, 100, true);
            var affected = new List<Actor>();
            //取得有效Actor（即玩家）
            foreach (var i in actors)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, i))
                    affected.Add(i);

            SkillHandler.Instance.MagicAttack(sActor, affected, args, Elements.Dark, factor / affected.Count);
        }

        #endregion
    }
}