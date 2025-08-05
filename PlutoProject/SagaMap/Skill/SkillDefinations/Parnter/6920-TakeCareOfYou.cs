using System.Collections.Generic;
using SagaDB.Actor;
using SagaLib;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Parnter
{
    /// <summary>
    ///     手当ていたします。
    /// </summary>
    public class TakeCareOfYou : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            float factor = 0;
            factor = -5.3f;
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var affected = map.GetActorsArea(sActor, 500, false);
            var realAffected = new List<Actor>();
            var ActorlowHP = sActor;
            foreach (var act in affected)
                if (act.type == ActorType.PARTNER || act.type == ActorType.PC || act.type == ActorType.PET)
                    if (act.HP / act.MaxHP < (float)(ActorlowHP.HP / ActorlowHP.MaxHP) && act.type == ActorType.MOB)
                        ActorlowHP = act;

            SkillHandler.Instance.MagicAttack(sActor, ActorlowHP, args, SkillHandler.DefType.IgnoreAll, Elements.Holy,
                factor);
            //SkillHandler.Instance.(sActor, ActorlowHP, args, SagaLib.Elements.Holy, factor);
        }

        #endregion
    }
}