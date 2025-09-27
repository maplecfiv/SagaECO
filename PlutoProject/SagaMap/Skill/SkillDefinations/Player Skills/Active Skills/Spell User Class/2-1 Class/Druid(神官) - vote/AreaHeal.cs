using System.Collections.Generic;
using SagaDB.Actor;
using SagaLib;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._2_1_Class.Druid_神官____vote
{
    /// <summary>
    ///     範圍治癒（エリアヒール）
    /// </summary>
    public class AreaHeal : ISkill
    {
        private readonly bool MobUse;

        public AreaHeal()
        {
            MobUse = false;
        }

        public AreaHeal(bool MobUse)
        {
            this.MobUse = MobUse;
        }

        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            if (MobUse) level = 5;
            var factor = -(1f + 0.4f * level);

            if (sActor.Status.Additions.ContainsKey("Cardinal")) //3转10技提升治疗量
                factor += sActor.Status.Cardinal_Rank;
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var affected = map.GetActorsArea(sActor, 100, true);
            var realAffected = new List<Actor>();
            if (sActor.type == ActorType.PC)
            {
                var pc = (ActorPC)sActor;
                foreach (var act in affected)
                    if (act.type == ActorType.PC || act.type == ActorType.PET || act.type == ActorType.SHADOW)
                        if (pc.PossessionTarget == 0)
                            realAffected.Add(act);
            }
            else
            {
                foreach (var act in affected)
                    if (!SkillHandler.Instance.CheckValidAttackTarget(sActor, act))
                        realAffected.Add(act);
            }

            SkillHandler.Instance.MagicAttack(sActor, realAffected, args, SkillHandler.DefType.IgnoreAll, Elements.Holy,
                factor);
        }

        #endregion
    }
}