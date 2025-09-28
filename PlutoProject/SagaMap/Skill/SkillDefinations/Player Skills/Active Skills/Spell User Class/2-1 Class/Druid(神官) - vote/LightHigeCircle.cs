using System.Collections.Generic;
using SagaLib;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._2_1_Class.Druid_神官____vote
{
    /// <summary>
    ///     破曉之光（スパーナルレイ）
    /// </summary>
    public class LightHigeCircle : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 2.3f + 0.2f * level;
            uint TURNUNDEAD_SkillID = 3078;
            var actorPC = (ActorPC)sActor;
            if (actorPC.Skills.ContainsKey(TURNUNDEAD_SkillID))
            {
                var TURNUNDEAD = actorPC.Skills[TURNUNDEAD_SkillID];
                factor += 0.5f + 0.5f * TURNUNDEAD.Level;
            }

            var map = MapManager.Instance.GetMap(sActor.MapID);
            var affected = map.GetActorsArea(sActor, 300, false);
            var realAffected = new List<Actor>();
            foreach (var act in affected)
                if (act.type == ActorType.MOB)
                {
                    var m = (ActorMob)act;
                    if (m.BaseData.mobType.ToString().ToLower().IndexOf("undead") > -1) realAffected.Add(act);
                }

            SkillHandler.Instance.MagicAttack(sActor, realAffected, args, Elements.Holy, factor);
        }

        #endregion
    }
}