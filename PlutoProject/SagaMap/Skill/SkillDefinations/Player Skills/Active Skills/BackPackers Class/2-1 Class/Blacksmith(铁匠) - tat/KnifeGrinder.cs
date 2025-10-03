using SagaDB.Actor;
using SagaDB.Item;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._2_1_Class.Blacksmith_铁匠____tat
{
    /// <summary>
    ///     閃電刀刃（ウェットウエポン）
    /// </summary>
    public class KnifeGrinder : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            if (dActor.type == ActorType.PC)
            {
                var pc = (ActorPC)dActor;
                if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND))
                    if (pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.SHORT_SWORD ||
                        pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.SWORD ||
                        pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.RAPIER)
                    {
                        int[] lifetimes = { 0, 30000, 40000, 60000 };
                        var lifetime = lifetimes[level];
                        var skill = new DefaultBuff(args.skill, dActor, "KnifeGrinder", lifetime);
                        skill.OnAdditionStart += StartEventHandler;
                        skill.OnAdditionEnd += EndEventHandler;
                        SkillHandler.ApplyAddition(dActor, skill);
                    }
            }
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            //最大攻擊
            var max_atk1_add = 30;
            if (skill.Variable.ContainsKey("KnifeGrinder_max_atk1"))
                skill.Variable.Remove("KnifeGrinder_max_atk1");
            skill.Variable.Add("KnifeGrinder_max_atk1", max_atk1_add);
            actor.Status.max_atk1_skill += (short)max_atk1_add;

            //最大攻擊
            var max_atk2_add = 30;
            if (skill.Variable.ContainsKey("KnifeGrinder_max_atk2"))
                skill.Variable.Remove("KnifeGrinder_max_atk2");
            skill.Variable.Add("KnifeGrinder_max_atk2", max_atk2_add);
            actor.Status.max_atk2_skill += (short)max_atk2_add;

            //最大攻擊
            var max_atk3_add = 30;
            if (skill.Variable.ContainsKey("KnifeGrinder_max_atk3"))
                skill.Variable.Remove("KnifeGrinder_max_atk3");
            skill.Variable.Add("KnifeGrinder_max_atk3", max_atk3_add);
            actor.Status.max_atk3_skill += (short)max_atk3_add;

            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            //最大攻擊
            actor.Status.max_atk1_skill -= (short)skill.Variable["KnifeGrinder_max_atk1"];

            //最大攻擊
            actor.Status.max_atk2_skill -= (short)skill.Variable["KnifeGrinder_max_atk2"];

            //最大攻擊
            actor.Status.max_atk3_skill -= (short)skill.Variable["KnifeGrinder_max_atk3"];

            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        //#endregion
    }
}