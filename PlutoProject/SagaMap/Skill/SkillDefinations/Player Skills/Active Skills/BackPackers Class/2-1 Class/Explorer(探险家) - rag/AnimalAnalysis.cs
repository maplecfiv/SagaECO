using System.Collections.Generic;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._2_1_Class.Explorer_探险家____rag
{
    /// <summary>
    ///     動物分析
    /// </summary>
    public class AnimalAnalysis : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            if (dActor.type == ActorType.MOB)
            {
                var types = new List<MobType>();
                types.Add(MobType.ANIMAL);
                types.Add(MobType.ANIMAL_BOMB_SKILL);
                types.Add(MobType.ANIMAL_BOSS);
                types.Add(MobType.ANIMAL_BOSS_SKILL);
                types.Add(MobType.ANIMAL_BOSS_SKILL_NOTPTDROPRANGE);
                types.Add(MobType.ANIMAL_NOTOUCH);
                types.Add(MobType.ANIMAL_NOTPTDROPRANGE);
                types.Add(MobType.ANIMAL_RIDE);
                types.Add(MobType.ANIMAL_RIDE_BREEDER);
                types.Add(MobType.ANIMAL_SKILL);
                types.Add(MobType.ANIMAL_SPBOSS_SKILL);
                types.Add(MobType.ANIMAL_SPBOSS_SKILL_NOTPTDROPRANGE);
                types.Add(MobType.ANIMAL);
                types.Add(MobType.ANIMAL_BOMB_SKILL);
                types.Add(MobType.ANIMAL_BOSS);
                types.Add(MobType.ANIMAL_BOSS_SKILL);
                types.Add(MobType.ANIMAL_BOSS_SKILL_NOTPTDROPRANGE);
                types.Add(MobType.ANIMAL_NOTOUCH);
                types.Add(MobType.ANIMAL_NOTPTDROPRANGE);
                types.Add(MobType.ANIMAL_RIDE);
                types.Add(MobType.ANIMAL_RIDE_BREEDER);
                types.Add(MobType.ANIMAL_SKILL);
                types.Add(MobType.ANIMAL_SPBOSS_SKILL);
                types.Add(MobType.ANIMAL_SPBOSS_SKILL_NOTPTDROPRANGE);

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