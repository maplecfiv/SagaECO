using System.Collections.Generic;
using SagaDB.Actor;
using SagaLib;
using SagaMap.Manager;
using static SagaMap.Skill.SkillHandler;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._2_2_Class.Enchanter_附魔师____sha
{
    /// <summary>
    ///     精靈之怒 (エレメンタルラース)
    /// </summary>
    public class ElementalWrath : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = new[] { 0, 1.4f, 1.85f, 2.325f, 2.775f, 3.25f }[level];
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var affected = map.GetActorsArea(SagaLib.Global.PosX8to16(args.x, map.Width),
                SagaLib.Global.PosY8to16(args.y, map.Height), 300);
            var realAffected = new List<Actor>();
            foreach (var act in affected)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, act))
                    realAffected.Add(act);

            var AttackAffect = 0;
            //ClientManager.EnterCriticalArea();
            foreach (var i in realAffected)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, i))
                {
                    var FireDamage = SkillHandler.Instance.CalcDamage(false, sActor, i, args, DefType.MDef,
                        Elements.Fire, 100, factor);
                    var WaterDamage = SkillHandler.Instance.CalcDamage(false, sActor, i, args, DefType.MDef,
                        Elements.Water, 100, factor);
                    var WindDamage = SkillHandler.Instance.CalcDamage(false, sActor, i, args, DefType.MDef,
                        Elements.Wind, 100, factor);
                    var EarthDamage = SkillHandler.Instance.CalcDamage(false, sActor, i, args, DefType.MDef,
                        Elements.Earth, 100, factor);
                    AttackAffect = FireDamage + WaterDamage + WindDamage + EarthDamage;
                    SkillHandler.Instance.CauseDamage(sActor, i, AttackAffect);
                    SkillHandler.Instance.ShowVessel(i, AttackAffect);
                }
            //ClientManager.LeaveCriticalArea();
        }

        //#endregion
    }
}