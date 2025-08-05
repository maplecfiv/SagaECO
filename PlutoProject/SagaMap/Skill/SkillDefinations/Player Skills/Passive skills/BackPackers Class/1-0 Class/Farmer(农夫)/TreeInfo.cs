using SagaDB.Actor;
using SagaDB.Mob;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.BackPackers_Class._1_0_Class.Farmer_农夫_
{
    /// <summary>
    ///     木材知識（木材知識）
    /// </summary>
    public class TreeInfo : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var skill = new Knowledge(args.skill, sActor, "TreeInfo", MobType.TREE, MobType.TREE_MATERIAL);
            SkillHandler.ApplyAddition(sActor, skill);
        }

        #endregion
    }
}