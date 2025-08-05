using System.Collections.Generic;
using SagaDB.Actor;
using SagaDB.Mob;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._2_1_Class.Druid_神官____vote
{
    /// <summary>
    ///     死靈分析（死霊分析）
    /// </summary>
    public class UndeadAnalysis : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            if (dActor.type == ActorType.MOB)
            {
                var types = new List<MobType>();
                types.Add(MobType.UNDEAD);
                types.Add(MobType.UNDEAD_BOSS);
                types.Add(MobType.UNDEAD_BOSS_BOMB_SKILL);
                types.Add(MobType.UNDEAD_BOSS_CHAMP_BOMB_SKILL_NOTPTDROPRANGE);
                types.Add(MobType.UNDEAD_BOSS_SKILL);
                types.Add(MobType.UNDEAD_BOSS_SKILL_CHAMP);
                types.Add(MobType.UNDEAD_BOSS_SKILL_NOTPTDROPRANGE);
                types.Add(MobType.UNDEAD_NOTOUCH);
                types.Add(MobType.UNDEAD_SKILL);

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

        #endregion
    }
}