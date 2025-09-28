using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Monster
{
    /// <summary>
    ///     艾卡納皇帝的憤怒
    /// </summary>
    public class MobConfCircle : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var rate = 30;
            var lifetime = 3000;
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var affected = map.GetActorsArea(sActor, 500, false); //既然是大范围就把范围弄大一些
            foreach (var act in affected)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, act))
                    if (SkillHandler.Instance.CanAdditionApply(sActor, act, SkillHandler.DefaultAdditions.Confuse,
                            rate))
                    {
                        var skill = new Confuse(args.skill, act, lifetime);
                        SkillHandler.ApplyAddition(act, skill);
                    }
        }

        #endregion
    }
}