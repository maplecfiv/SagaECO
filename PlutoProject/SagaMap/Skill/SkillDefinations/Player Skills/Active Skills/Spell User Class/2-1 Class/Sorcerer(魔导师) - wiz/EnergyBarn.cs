using SagaDB.Actor;
using SagaLib;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._2_1_Class.Sorcerer_魔导师____wiz
{
    /// <summary>
    ///     能量先鋒（エナジーバーン）
    /// </summary>
    public class EnergyBarn : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            if (dActor.type == ActorType.MOB) return 0;

            return -14;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 2.6f + 0.9f * level;
            SkillHandler.Instance.MagicAttack(sActor, dActor, args, Elements.Neutral, factor);
            args.autoCast.Add(SkillHandler.Instance.CreateAutoCastInfo(3299, level, 2000));
        }

        //#endregion
    }
}