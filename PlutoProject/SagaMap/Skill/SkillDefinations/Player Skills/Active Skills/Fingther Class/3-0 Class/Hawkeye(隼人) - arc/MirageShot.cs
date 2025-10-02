﻿using SagaDB.Actor;
using SagaDB.Item;
using SagaMap.ActorEventHandlers;
using SagaMap.Manager;
using SagaMap.Mob;
using SagaMap.Network.Client;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._3_0_Class.Hawkeye_隼人____arc
{
    internal class MirageShot : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            var numdown = args.skill.Level * 3;
            if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND))
            {
                if (pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.BOW)
                {
                    if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.LEFT_HAND))
                    {
                        if (pc.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].BaseData.itemType == ItemType.ARROW)
                        {
                            if (pc.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].Stack >= numdown) return 0;

                            return -55;
                        }

                        return -34;
                    }

                    return -34;
                }

                if (pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.GUN ||
                    pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.DUALGUN ||
                    pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.RIFLE)
                {
                    if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.LEFT_HAND))
                    {
                        if (pc.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].BaseData.itemType == ItemType.BULLET)
                        {
                            if (pc.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].Stack >= numdown) return 0;

                            return -56;
                        }

                        return -35;
                    }

                    return -35;
                }

                return -5;
            }

            return -5;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var numdown = level * 3;
            if (sActor.type == ActorType.PC)
            {
                var pc = (ActorPC)sActor;
                if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND))
                {
                    if (pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.BOW)
                        if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.LEFT_HAND))
                            if (pc.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].BaseData.itemType == ItemType.ARROW)
                                if (pc.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].Stack >= numdown)
                                    for (var i = 0; i < numdown; i++)
                                        MapClient.FromActorPC(pc)
                                            .DeleteItem(pc.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].Slot, 1,
                                                false);

                    if (pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.GUN ||
                        pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.DUALGUN ||
                        pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.RIFLE)
                        if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.LEFT_HAND))
                            if (pc.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].BaseData.itemType == ItemType.BULLET)
                                if (pc.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].Stack >= numdown)
                                    for (var i = 0; i < numdown; i++)
                                        MapClient.FromActorPC(pc)
                                            .DeleteItem(pc.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].Slot, 1,
                                                false);
                }
            }

            var lifetime = 5000;
            var skill = new MirageShotbuff(args.skill, sActor, lifetime, args);
            SkillHandler.ApplyAddition(sActor, skill);
            var skill2 = new MirageShotbuff(args.skill, sActor, lifetime, args);
            SkillHandler.ApplyAddition(sActor, skill2);
            args.autoCast.Add(SkillHandler.Instance.CreateAutoCastInfo(2540, level, 1000));
        }

        public class MirageShotbuff : DefaultBuff
        {
            private ActorShadow shadow;
            private SkillArg thisarg;

            public MirageShotbuff(SagaDB.Skill.Skill skill, Actor actor, int lifetime, SkillArg args)
                : base(skill, actor, "MirageShotbuff", lifetime)
            {
                thisarg = args;
                OnAdditionStart += StartEvent;
                OnAdditionEnd += EndEvent;
            }

            private void StartEvent(Actor actor, DefaultBuff skill)
            {
                shadow = MirageShotMe((ActorPC)actor, skill.skill.Level);
            }

            private void EndEvent(Actor actor, DefaultBuff skill)
            {
                var map = MapManager.Instance.GetMap(actor.MapID);
                var eh = (PetEventHandler)shadow.e;
                eh.AI.Pause();
                shadow.ClearTaskAddition();
                map.DeleteActor(shadow);
            }

            public ActorShadow MirageShotMe(ActorPC pc, byte level)
            {
                var actor = new ActorShadow(pc);
                var map = MapManager.Instance.GetMap(pc.MapID);
                actor.Name = pc.Name;
                actor.MapID = pc.MapID;
                var nx = SagaLib.Global.Random.Next(-1, 1);
                actor.X = (short)(pc.X + nx);
                var ny = SagaLib.Global.Random.Next(-1, 1);
                actor.Y = (short)(pc.Y + ny);
                actor.MaxHP = (uint)(pc.MaxHP * (0.1f + 0.2f * level));
                actor.HP = pc.MaxHP;
                //actor..Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType = ItemType.GUN;
                //actor.Owner.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType = ItemType.BOW;
                //actor.Range = 255;
                actor.Status.min_atk1 = pc.Status.min_atk1;
                actor.Status.min_atk2 = pc.Status.min_atk2;
                actor.Status.min_atk3 = pc.Status.min_atk3;
                actor.Status.max_atk1 = pc.Status.max_atk1;
                actor.Status.max_atk2 = pc.Status.max_atk2;
                actor.Status.max_atk3 = pc.Status.max_atk3;
                actor.Speed = pc.Speed;
                actor.BaseData.range = 12f;

                var eh = new PetEventHandler(actor);
                actor.e = eh;

                eh.AI.Mode = new AIMode(1);
                //switch (skill.Level)
                //{
                //    case 1:
                //        eh.AI.Mode.EventAttacking.Add(3281, 100);	//魔法衝擊波
                //        break;
                //    case 2:
                //        eh.AI.Mode.EventAttacking.Add(3281, 50);	//魔法衝擊波
                //        eh.AI.Mode.EventAttacking.Add(3127, 50);	//魔力放出
                //        break;
                //    case 3:
                //        eh.AI.Mode.EventAttacking.Add(3281, 30);	//魔法衝擊波
                //        eh.AI.Mode.EventAttacking.Add(3127, 30);	//魔力放出
                //        eh.AI.Mode.EventAttacking.Add(3291, 40);	//燃燒生命
                //        break;
                //}
                eh.AI.Mode.EventAttacking.Add(2540, 100);
                eh.AI.Mode.EventAttackingSkillRate = 100;
                eh.AI.Master = pc;
                map.RegisterActor(actor);
                actor.invisble = false;
                map.OnActorVisibilityChange(actor);
                map.SendVisibleActorsToActor(actor);
                eh.AI.Start();
                return actor;
            }
        }

        //#endregion
    }
}