using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._1_0_Class.Swordman_剑士_
{
    /// <summary>
    ///     神隼之眼
    /// </summary>
    public class Feint : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            if (pc.Status.Additions.ContainsKey("嘲讽CD"))
                return -30;
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var skill2 = new SkillCD(args.skill, sActor, "嘲讽CD", 5000);
            SkillHandler.ApplyAddition(sActor, skill2);
            int[] array = { sActor.Status.max_atk1, sActor.Status.max_atk2, sActor.Status.max_atk3 };
            var damage = (int)(Max(array) * 15f);
            SkillHandler.Instance.AttractMob(sActor, dActor, damage);
            MapManager.Instance.GetMap(sActor.MapID).SendEffect(dActor, 4539);
        }

        private int Max(int[] array)
        {
            var max = int.MinValue;
            foreach (var i in array)
                if (i > max)
                    max = i;
            return max;
        }

        #endregion
    }
}