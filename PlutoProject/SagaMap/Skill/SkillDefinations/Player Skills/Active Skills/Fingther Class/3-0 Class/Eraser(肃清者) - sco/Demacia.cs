using System.Collections.Generic;
using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._3_0_Class.Eraser_肃清者____sco
{
    /// <summary>
    ///     マーシレスシャドウ
    /// </summary>
    public class Demacia : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            if (sActor.Status.Additions.ContainsKey("Demacia"))
                return -30;
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            int[] lifetime = { 0, 10000, 15000, 15000, 15000, 20000 };
            var skill = new DefaultBuff(args.skill, sActor, "Demacia", lifetime[level]);
            SkillHandler.ApplyAddition(sActor, skill);
            var factor = 1.3f + 0.7f * level;
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var actors = map.GetActorsArea(dActor, 150, true);
            var affected = new List<Actor>();
            foreach (var i in actors)
            {
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, i))
                    affected.Add(i);
                if (SkillHandler.Instance.CanAdditionApply(sActor, i, SkillHandler.DefaultAdditions.Confuse, 10))
                {
                    var skill2 = new Stiff(args.skill, i, 2500);
                    SkillHandler.ApplyAddition(i, skill);
                }
            }

            SkillHandler.Instance.PhysicalAttack(sActor, affected, args, sActor.WeaponElement, factor);
        }

        #endregion
    }
}