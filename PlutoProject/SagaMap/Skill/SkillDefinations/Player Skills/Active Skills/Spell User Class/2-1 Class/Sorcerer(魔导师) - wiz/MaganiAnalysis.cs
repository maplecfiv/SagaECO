using System.Collections.Generic;
using SagaDB.Actor;
using SagaDB.Mob;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._2_1_Class.Sorcerer_魔导师____wiz
{
    /// <summary>
    ///     魔法生物分析（魔法生物分析）
    /// </summary>
    public class MaganiAnalysis : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            if (dActor.type == ActorType.MOB)
            {
                var types = new List<MobType>();
                types.Add(MobType.MAGIC_CREATURE);
                types.Add(MobType.MAGIC_CREATURE_BOSS);
                types.Add(MobType.MAGIC_CREATURE_BOSS_SKILL);
                types.Add(MobType.MAGIC_CREATURE_BOSS_SKILL_NOTPTDROPRANGE);
                types.Add(MobType.MAGIC_CREATURE_LVDIFF);
                types.Add(MobType.MAGIC_CREATURE_MATERIAL);
                types.Add(MobType.MAGIC_CREATURE_NOTOUCH);
                types.Add(MobType.MAGIC_CREATURE_NOTPTDROPRANGE);
                types.Add(MobType.MAGIC_CREATURE_RIDE);
                types.Add(MobType.MAGIC_CREATURE_SKILL);

                var mob = (ActorMob)dActor;
                if (types.Contains(mob.BaseData.mobType)) return 0;
            }

            return -4;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var skill = new Analysis(args.skill, dActor);
            SkillHandler.ApplyAddition(dActor, skill);
        }

        //#endregion
    }
}