using System.Collections.Generic;
using SagaDB.Actor;
using SagaLib;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._2_1_Class.Sorcerer_魔导师____wiz
{
    /// <summary>
    ///     能量先鋒（エナジーバーン）[接續技能]
    /// </summary>
    public class EnergyBarnSEQ : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var map = MapManager.Instance.GetMap(dActor.MapID);
            var affected = map.GetActorsArea(dActor, 100, true);
            var realAffected = new List<Actor>();
            foreach (var act in affected)
                if (act.type == ActorType.MOB)
                    realAffected.Add(act);

            SkillHandler.Instance.MagicAttack(sActor, realAffected, args, Elements.Neutral, 3.0f);
        }

        #endregion
    }
}