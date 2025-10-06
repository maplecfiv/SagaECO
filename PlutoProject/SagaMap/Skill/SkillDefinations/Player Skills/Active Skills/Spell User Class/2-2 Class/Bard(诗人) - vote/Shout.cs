using System.Collections.Generic;
using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._2_2_Class.Bard_诗人____vote
{
    /// <summary>
    ///     獅子吼（シャウト！）
    /// </summary>
    public class Shout : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var rate = 25 + 5 * level;
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var affected = map.GetActorsArea(sActor, 250, true);
            foreach (var act in affected)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, act))
                    if (SkillHandler.Instance.CanAdditionApply(sActor, dActor, SkillHandler.DefaultAdditions.Stun,
                            rate))
                    {
                        var skill = new Stun(args.skill, act, 3400 + 600 * level);
                        SkillHandler.ApplyAddition(act, skill);
                    }
        }

        //#endregion
    }
}