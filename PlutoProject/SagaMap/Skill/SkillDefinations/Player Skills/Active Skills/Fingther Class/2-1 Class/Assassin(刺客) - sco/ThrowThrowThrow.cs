using System.Collections.Generic;
using SagaDB.Actor;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.FR2_1
{
    public class ThrowThrowThrow : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 1.5f + 0.5f * level;
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var affected = new List<Actor>();
            var list = map.GetRoundAreaActors(SagaLib.Global.PosX8to16(args.x, map.Width),
                SagaLib.Global.PosY8to16(args.y, map.Height), 400, true);
            foreach (var i in list)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, i))
                    affected.Add(i);
            //args.affectedActors = affected;
            //args.Init();
            SkillHandler.Instance.PhysicalAttack(sActor, affected, args, sActor.WeaponElement, factor);
        }

        #endregion
    }
}