using SagaDB.Actor;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Joint_Class.Breeder_驯兽师_
{
    /// <summary>
    ///     アクロバットイベイジョン（アクロバットイベイジョン）
    /// </summary>
    public class Akurobattoibeijon : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var damage = 2500;
            var p = SkillHandler.Instance.GetPet(sActor);
            if (p != null)
            {
                var map = MapManager.Instance.GetMap(sActor.MapID);
                var affected = map.GetActorsArea(p, 250, false);
                foreach (var act in affected)
                    if (SkillHandler.Instance.CheckValidAttackTarget(p, act))
                        SkillHandler.Instance.AttractMob(p, act, damage);
            }
        }

        #endregion
    }
}