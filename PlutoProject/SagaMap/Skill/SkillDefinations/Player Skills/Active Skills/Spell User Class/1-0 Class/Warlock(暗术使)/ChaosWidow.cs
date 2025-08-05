using System.Collections.Generic;
using SagaDB.Actor;
using SagaLib;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._1_0_Class.Warlock_暗术使_
{
    public class ChaosWidow : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            if (SkillHandler.Instance.CheckValidAttackTarget(pc, dActor)) return 0;

            return -14;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            float factor = 0;
            float SuckBlood = 0;
            factor = 1.10f + 0.25f * level;
            if (dActor.Darks == 1)
            {
                var factor2 = 1.5f + 0.5f * level;
                if (level == 6)
                {
                    factor2 = 3f;
                    SuckBlood = 0.3f;
                }
                else
                {
                    SuckBlood = 0.05f;
                }

                MapManager.Instance.GetMap(sActor.MapID).SendEffect(dActor, 5202);
                dActor.Darks = 0;
                var add = new SkillArg();
                add = args.Clone();
                add.skill.BaseData.id = 100;
                SkillHandler.Instance.MagicAttack(sActor, dActor, add, Elements.Dark, factor2);
                MapManager.Instance.GetMap(sActor.MapID)
                    .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SKILL, add, sActor, true);
                add.skill.BaseData.id = 3134;
            }

            var list = new List<Actor>();
            list.Add(dActor);
            if (level == 6)
                factor = 5f;
            SkillHandler.Instance.MagicAttack(sActor, list, args, SkillHandler.DefType.MDef, Elements.Dark, 200, factor,
                0, false, false, SuckBlood);
        }

        #endregion
    }
}