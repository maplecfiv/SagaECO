using SagaDB.Actor;
using SagaDB.Mob;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.BackPackers_Class._1_0_Class.Ranger_冒险家_
{
    /// <summary>
    ///     寶物箱知識（宝箱知識）
    /// </summary>
    public class TreasureInfo : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var skill = new Knowledge(args.skill, sActor, "TreasureInfo", MobType.TREASURE_BOX,
                MobType.TREASURE_BOX_MATERIAL);
            SkillHandler.ApplyAddition(sActor, skill);
        }

        //#endregion
    }
}