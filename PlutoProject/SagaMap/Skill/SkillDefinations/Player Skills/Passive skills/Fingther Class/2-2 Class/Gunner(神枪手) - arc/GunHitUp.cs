using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.Fingther_Class._2_2_Class.Gunner_神枪手____arc
{
    /// <summary>
    ///     雙手槍命中率提升（２丁拳銃命中率上昇）
    /// </summary>
    public class GunHitUp : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var active = false;
            if (sActor.type == ActorType.PC)
            {
                var pc = (ActorPC)sActor;
                if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND))
                    if (pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.DUALGUN)
                        active = true;

                var skill = new DefaultPassiveSkill(args.skill, sActor, "GunHitUp", active);
                skill.OnAdditionStart += StartEventHandler;
                skill.OnAdditionEnd += EndEventHandler;
                SkillHandler.ApplyAddition(sActor, skill);
            }
        }

        private void StartEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            int level = skill.skill.Level;
            int[] Hit_Ranged_Add = { 0, 3, 6, 10, 15, 20 };

            //遠命中
            var hit_ranged_add = Hit_Ranged_Add[level];
            if (skill.Variable.ContainsKey("GunHitUp_hit_ranged"))
                skill.Variable.Remove("GunHitUp_hit_ranged");
            skill.Variable.Add("GunHitUp_hit_ranged", hit_ranged_add);
            actor.Status.hit_ranged_skill += (short)hit_ranged_add;
        }

        private void EndEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            //遠命中
            actor.Status.hit_ranged_skill -= (short)skill.Variable["GunHitUp_hit_ranged"];
        }

        #endregion
    }
}