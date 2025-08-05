using System.Collections.Generic;
using SagaDB.Actor;
using SagaLib;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Monster
{
    /// <summary>
    ///     アブソリュートテリトリー
    /// </summary>
    public class Abusoryutoteritori : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 10.0f;
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var affected = map.GetActorsArea(SagaLib.Global.PosX8to16(args.x, map.Width),
                SagaLib.Global.PosY8to16(args.y, map.Height), 500, false);
            var realAffected = new List<Actor>();
            foreach (var act in affected)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, act))
                    realAffected.Add(act);

            SkillHandler.Instance.MagicAttack(sActor, realAffected, args, Elements.Wind, factor);
            var hp_recovery = 0;
            foreach (var hp in args.hp) hp_recovery += hp;
            SkillHandler.Instance.FixAttack(sActor, sActor, args, Elements.Holy, -hp_recovery);
        }

        #endregion
    }
}