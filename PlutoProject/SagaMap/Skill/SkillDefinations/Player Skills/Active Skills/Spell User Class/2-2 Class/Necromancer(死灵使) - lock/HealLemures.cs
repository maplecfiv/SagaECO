using SagaDB.Actor;
using SagaLib;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._2_2_Class.Necromancer_死灵使____lock
{
    /// <summary>
    ///     治癒死靈（ネクロヒーリング）
    /// </summary>
    public class HealLemures : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            uint[] HP_add = { 0, 550, 880, 1100, 1320, 1540 };
            if (sActor.Status.Additions.ContainsKey("SummobLemures"))
            {
                var skill = (SummobLemures.SummobLemuresBuff)sActor.Status.Additions["SummobLemures"];
                if (skill.mob != null)
                {
                    args.affectedActors.Add(skill.mob);
                    skill.mob.HP += HP_add[level];
                    if (skill.mob.HP > skill.mob.MaxHP) skill.mob.HP = skill.mob.MaxHP;
                    args.Init();
                    args.flag[0] |= AttackFlag.HP_HEAL | AttackFlag.NO_DAMAGE;

                    var map = MapManager.Instance.GetMap(sActor.MapID);
                    map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.HPMPSP_UPDATE, null, skill.mob, true);
                }
            }
        }

        #endregion
    }
}