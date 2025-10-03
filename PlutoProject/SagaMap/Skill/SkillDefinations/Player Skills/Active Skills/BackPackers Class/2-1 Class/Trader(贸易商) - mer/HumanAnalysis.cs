using System.Collections.Generic;
using SagaDB.Actor;
using SagaDB.Mob;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._2_1_Class.Trader_贸易商____mer
{
    /// <summary>
    ///     人類分析
    /// </summary>
    public class HumanAnalysis : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            if (dActor.type == ActorType.MOB)
            {
                var types = new List<MobType>();
                types.Add(MobType.HUMAN);
                types.Add(MobType.HUMAN_BOSS);
                types.Add(MobType.HUMAN_BOSS_CHAMP);
                types.Add(MobType.HUMAN_BOSS_SKILL);
                types.Add(MobType.HUMAN_CHAMP);
                types.Add(MobType.HUMAN_NOTOUCH);
                types.Add(MobType.HUMAN_RIDE);
                types.Add(MobType.HUMAN_SKILL);
                types.Add(MobType.HUMAN_SKILL_BOSS_CHAMP);
                types.Add(MobType.HUMAN_SKILL_CHAMP);
                types.Add(MobType.HUMAN_SMARK_BOSS_HETERODOXY);
                types.Add(MobType.HUMAN_SMARK_HETERODOXY);
                types.Add(MobType.HUMAN);
                types.Add(MobType.HUMAN_BOSS);
                types.Add(MobType.HUMAN_BOSS_CHAMP);
                types.Add(MobType.HUMAN_BOSS_SKILL);
                types.Add(MobType.HUMAN_CHAMP);
                types.Add(MobType.HUMAN_NOTOUCH);
                types.Add(MobType.HUMAN_RIDE);
                types.Add(MobType.HUMAN_SKILL);
                types.Add(MobType.HUMAN_SKILL_BOSS_CHAMP);
                types.Add(MobType.HUMAN_SKILL_CHAMP);
                types.Add(MobType.HUMAN_SMARK_BOSS_HETERODOXY);
                types.Add(MobType.HUMAN_SMARK_HETERODOXY);

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