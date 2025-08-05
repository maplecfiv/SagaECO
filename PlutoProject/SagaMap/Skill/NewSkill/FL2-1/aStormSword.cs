using System.Collections.Generic;
using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.SkillDefinations;

namespace SagaMap.Skill.NewSkill.FL2_1
{
    /// <summary>
    ///     旋風劍（旋風剣）
    /// </summary>
    public class StormSword : ISkill
    {
        //bool MobUse;
        //public aStormSword()
        //{
        //    this.MobUse = false;
        //}
        //public aStormSword(bool MobUse)
        //{
        //    this.MobUse = MobUse;
        //}

        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            float factor = 0;
            factor = 2.5f + 0.5f * level;
            if (sActor.type != ActorType.PC) factor = 3.0f;


            var map = MapManager.Instance.GetMap(sActor.MapID);
            var affected = map.GetActorsArea(sActor, 150, false);
            var realAffected = new List<Actor>();
            foreach (var act in affected)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, act))
                    realAffected.Add(act);

            SkillHandler.Instance.PhysicalAttack(sActor, realAffected, args, sActor.WeaponElement, factor);
        }

        #endregion
    }
}