using SagaDB.Actor;
using SagaDB.Item;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Advance_Novice.Joker_小丑_
{
    /// <summary>
    ///     小丑
    /// </summary>
    public class Joker : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }


        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            if (sActor.type == ActorType.PC)
            {
                var pc = (ActorPC)sActor;
                if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND))
                {
                    if ((pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.SWORD ||
                         pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.AXE ||
                         pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.HAMMER ||
                         pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.SPEAR) &&
                        pc.Inventory.GetContainer(ContainerType.RIGHT_HAND2).Count > 0)
                        args.autoCast.Add(SkillHandler.Instance.CreateAutoCastInfo(2562, level, 0));
                }
                else
                {
                    args.autoCast.Add(SkillHandler.Instance.CreateAutoCastInfo(2560, level, 0));
                }
            }
        }


        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            int level = skill.skill.Level;
            var hp = (int)(actor.MaxHP * 0.2f * level);
            var mp = (int)(actor.MaxMP * 0.2f * level);
            var sp = (int)(actor.MaxSP * 0.2f * level);
            var max_atk1_add = (int)(actor.Status.max_atk_bs * (0.5f * level));
            var max_atk2_add = (int)(actor.Status.max_atk_bs * (0.5f * level));
            var max_atk3_add = (int)(actor.Status.max_atk_bs * (0.5f * level));
            var min_atk1_add = (int)(actor.Status.min_atk_bs * (0.5f * level));
            var min_atk2_add = (int)(actor.Status.min_atk_bs * (0.5f * level));
            var min_atk3_add = (int)(actor.Status.min_atk_bs * (0.5f * level));
            var max_matk_add = (int)(actor.Status.max_matk_bs * (0.5f * level));
            var min_matk_add = (int)(actor.Status.min_matk_bs * (0.5f * level));

            if (skill.Variable.ContainsKey("Joker1_HP"))
                skill.Variable.Remove("Joker1_HP");
            skill.Variable.Add("Joker1_HP", hp);
            actor.Status.hp_skill += (short)hp;

            if (skill.Variable.ContainsKey("Joker1_MP"))
                skill.Variable.Remove("Joker1_MP");
            skill.Variable.Add("Joker1_MP", mp);
            actor.Status.mp_skill += (short)mp;

            if (skill.Variable.ContainsKey("Joker1_SP"))
                skill.Variable.Remove("Joker1_SP");
            skill.Variable.Add("Joker1_SP", sp);
            actor.Status.sp_skill += (short)sp;

            //大傷
            if (skill.Variable.ContainsKey("Joker1_MAX_ATK1"))
                skill.Variable.Remove("Joker1_MAX_ATK1");
            skill.Variable.Add("Joker1_MAX_ATK1", max_atk1_add);
            actor.Status.max_atk1_skill += (short)max_atk1_add;

            if (skill.Variable.ContainsKey("Joker1_MAX_ATK2"))
                skill.Variable.Remove("Joker1_MAX_ATK2");
            skill.Variable.Add("Joker1_MAX_ATK2", max_atk2_add);
            actor.Status.max_atk2_skill += (short)max_atk2_add;

            if (skill.Variable.ContainsKey("Joker1_MAX_ATK3"))
                skill.Variable.Remove("Joker1_MAX_ATK3");
            skill.Variable.Add("Joker1_MAX_ATK3", max_atk3_add);
            actor.Status.max_atk3_skill += (short)max_atk3_add;

            //小伤
            if (skill.Variable.ContainsKey("Joker1_MIN_ATK1"))
                skill.Variable.Remove("Joker1_MIN_ATK1");
            skill.Variable.Add("Joker1_MIN_ATK1", min_atk1_add);
            actor.Status.min_atk1_skill += (short)min_atk1_add;

            if (skill.Variable.ContainsKey("Joker1_MIN_ATK2"))
                skill.Variable.Remove("Joker1_MIN_ATK2");
            skill.Variable.Add("Joker1_MIN_ATK2", min_atk2_add);
            actor.Status.min_atk2_skill += (short)min_atk2_add;

            if (skill.Variable.ContainsKey("Joker1_MIN_ATK3"))
                skill.Variable.Remove("Joker1_MIN_ATK3");
            skill.Variable.Add("Joker1_MIN_ATK3", min_atk3_add);
            actor.Status.min_atk3_skill += (short)min_atk3_add;

            //魔伤
            if (skill.Variable.ContainsKey("Joker1_MAX_MATK"))
                skill.Variable.Remove("Joker1_MAX_MATK");
            skill.Variable.Add("Joker1_MAX_MATK", max_matk_add);
            actor.Status.max_matk_skill += (short)max_matk_add;

            if (skill.Variable.ContainsKey("Joker1_MIN_MATK"))
                skill.Variable.Remove("Joker1_MIN_MATK");
            skill.Variable.Add("Joker1_MIN_MATK", min_matk_add);
            actor.Status.min_matk_skill += (short)min_matk_add;

            actor.Buff.MaxHPUp = true;
            actor.Buff.MaxSPUp = true;
            actor.Buff.MaxMPUp = true;
            actor.Buff.MaxAtkUp = true;
            actor.Buff.MinAtkUp = true;
            actor.Buff.MinMagicAtkUp = true;
            actor.Buff.MaxMagicAtkUp = true;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            //大傷
            actor.Status.max_atk1_skill -= (short)skill.Variable["Joker_MAX_ATK1"];
            actor.Status.max_atk2_skill -= (short)skill.Variable["Joker_MAX_ATK2"];
            actor.Status.max_atk3_skill -= (short)skill.Variable["Joker_MAX_ATK3"];
            //小傷
            actor.Status.min_atk1_skill -= (short)skill.Variable["Joker_MIN_ATK1"];
            actor.Status.min_atk2_skill -= (short)skill.Variable["Joker_MIN_ATK2"];
            actor.Status.min_atk3_skill -= (short)skill.Variable["Joker_MIN_ATK3"];

            //魔伤
            actor.Status.max_matk_skill -= (short)skill.Variable["Joker_MAX_MATK"];
            actor.Status.min_matk_skill -= (short)skill.Variable["Joker_MIN_MATK"];

            actor.Buff.MaxHPUp = false;
            actor.Buff.MaxSPUp = false;
            actor.Buff.MaxMPUp = false;
            actor.Buff.MaxAtkUp = false;
            actor.Buff.MinAtkUp = false;
            actor.Buff.MinMagicAtkUp = false;
            actor.Buff.MaxMagicAtkUp = false;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        //#endregion
    }
}