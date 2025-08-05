using System;
using System.Collections.Generic;
using System.Linq;
using SagaDB.Actor;
using SagaDB.Item;
using SagaLib;
using SagaMap.Manager;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill
{
    public partial class SkillHandler
    {
        public void ItemUse(Actor sActor, Actor dActor, SkillArg arg)
        {
            var list = new List<Actor>();
            list.Add(dActor);
            ItemUse(sActor, list, arg);
        }

        public void ItemUse(Actor sActor, List<Actor> dActor, SkillArg arg)
        {
            var counter = 0;
            arg.affectedActors = dActor;
            arg.Init();

            if (arg.item.BaseData.duration == 0)
                foreach (var i in dActor)
                {
                    if (i.Buff.NoRegen)
                        continue;
                    uint itemhp, itemsp, itemmp, itemep;
                    if (arg.item.BaseData.isRate)
                    {
                        var recover = 1.0f;
                        if (arg.item.BaseData.itemType == ItemType.FOOD)
                        {
                            float rate = 1, rate_iris = 1;
                            if (i.Status.Additions.ContainsKey("FoodFighter")) //食物技能加成
                            {
                                var dps = i.Status.Additions["FoodFighter"] as DefaultPassiveSkill;
                                rate = dps.Variable["FoodFighter"] / 100.0f + 1.0f;
                            }

                            if (i.Status.foot_iris > 100) //追加iris卡逻辑
                                rate_iris = i.Status.potion_iris / 100.0f;
                            recover = recover * (rate + rate_iris - 1);
                        }

                        if (arg.item.BaseData.itemType == ItemType.POTION)
                        {
                            float rate = 1, rate_iris = 1;
                            if (i.Status.Additions.ContainsKey("PotionFighter")) //药品技能加成
                            {
                                var dps = i.Status.Additions["PotionFighter"] as DefaultPassiveSkill;
                                rate = dps.Variable["PotionFighter"] / 100.0f + 1.0f;
                            }

                            if (i.Status.potion_iris > 100) //追加iris卡逻辑
                                rate_iris = i.Status.potion_iris / 100.0f;
                            recover = recover * (rate + rate_iris - 1);
                        }


                        itemhp = (uint)(i.MaxHP * arg.item.BaseData.hp * recover / 100);
                        itemsp = (uint)(i.MaxSP * arg.item.BaseData.sp * recover / 100);
                        itemmp = (uint)(i.MaxMP * arg.item.BaseData.mp * recover / 100);
                        itemep = arg.item.BaseData.delay / 100;
                    }
                    else
                    {
                        var recover = 1.0f;
                        if (arg.item.BaseData.itemType == ItemType.FOOD)
                        {
                            float rate = 1, rate_iris = 1;
                            if (i.Status.Additions.ContainsKey("FoodFighter")) //食物技能加成
                            {
                                var dps = i.Status.Additions["FoodFighter"] as DefaultPassiveSkill;
                                rate = dps.Variable["FoodFighter"] / 100.0f + 1.0f;
                            }

                            if (i.Status.foot_iris > 100) //追加iris卡逻辑
                                rate_iris = i.Status.potion_iris / 100.0f;
                            recover = recover * (rate + rate_iris - 1);
                        }

                        if (arg.item.BaseData.itemType == ItemType.POTION)
                        {
                            float rate = 1, rate_iris = 1;
                            if (i.Status.Additions.ContainsKey("PotionFighter")) //药品技能加成
                            {
                                var dps = i.Status.Additions["PotionFighter"] as DefaultPassiveSkill;
                                rate = dps.Variable["PotionFighter"] / 100.0f + 1.0f;
                            }

                            if (i.Status.potion_iris > 100) //追加iris卡逻辑
                                rate_iris = i.Status.potion_iris / 100.0f;
                            recover = recover * (rate + rate_iris - 1);
                        }

                        itemhp = (uint)(arg.item.BaseData.hp * recover);
                        itemsp = (uint)(arg.item.BaseData.sp * recover);
                        itemmp = (uint)(arg.item.BaseData.mp * recover);
                        itemep = arg.item.BaseData.delay;
                    }

                    if (sActor is ActorPC)
                    {
                        var pc = (ActorPC)sActor;

                        if ((pc.Skills.ContainsKey(103) || pc.DualJobSkill.Exists(x => x.ID == 103)) &&
                            arg.item.BaseData.hp > 0)
                        {
                            var duallv = 0;
                            if (pc.DualJobSkill.Exists(x => x.ID == 103))
                                duallv = pc.DualJobSkill.FirstOrDefault(x => x.ID == 103).Level;

                            var mainlv = 0;
                            if (pc.Skills.ContainsKey(103))
                                mainlv = pc.Skills[103].Level;

                            itemhp += (uint)(15 + arg.item.BaseData.hp * Math.Max(duallv, mainlv) * 0.03f);
                        }

                        if ((pc.Skills.ContainsKey(104) || pc.DualJobSkill.Exists(x => x.ID == 104)) &&
                            arg.item.BaseData.mp > 0)
                        {
                            var duallv = 0;
                            if (pc.DualJobSkill.Exists(x => x.ID == 104))
                                duallv = pc.DualJobSkill.FirstOrDefault(x => x.ID == 104).Level;

                            var mainlv = 0;
                            if (pc.Skills.ContainsKey(104))
                                mainlv = pc.Skills[104].Level;

                            itemmp += (uint)(15 + arg.item.BaseData.mp * Math.Max(duallv, mainlv) * 0.03f);
                        }

                        if ((pc.Skills.ContainsKey(105) || pc.DualJobSkill.Exists(x => x.ID == 105)) &&
                            arg.item.BaseData.sp > 0)
                        {
                            var duallv = 0;
                            if (pc.DualJobSkill.Exists(x => x.ID == 105))
                                duallv = pc.DualJobSkill.FirstOrDefault(x => x.ID == 105).Level;

                            var mainlv = 0;
                            if (pc.Skills.ContainsKey(105))
                                mainlv = pc.Skills[105].Level;

                            itemsp += (uint)(15 + arg.item.BaseData.sp * Math.Max(duallv, mainlv) * 0.03f);
                        }
                    }

                    i.HP = i.HP + itemhp;
                    i.SP = i.SP + itemsp;
                    i.MP = i.MP + itemmp;
                    i.EP = i.EP + itemep;

                    if (i.HP > i.MaxHP)
                        i.HP = i.MaxHP;
                    if (i.SP > i.MaxSP)
                        i.SP = i.MaxSP;
                    if (i.MP > i.MaxMP)
                        i.MP = i.MaxMP;
                    if (i.EP > i.MaxEP)
                        i.EP = i.MaxEP;

                    if (arg.item.BaseData.hp > 0)
                    {
                        arg.flag[counter] |= AttackFlag.HP_HEAL;
                        arg.hp[counter] = (int)-itemhp;
                    }
                    else if (arg.item.BaseData.hp < 0)
                    {
                        arg.flag[counter] |= AttackFlag.HP_DAMAGE;
                        arg.hp[counter] = (int)-itemhp;
                    }

                    if (arg.item.BaseData.sp > 0)
                    {
                        arg.flag[counter] |= AttackFlag.SP_HEAL;
                        arg.sp[counter] = (int)-itemsp;
                    }
                    else if (arg.item.BaseData.sp < 0)
                    {
                        arg.flag[counter] |= AttackFlag.SP_DAMAGE;
                        arg.sp[counter] = (int)-itemsp;
                    }

                    if (arg.item.BaseData.mp > 0)
                    {
                        arg.flag[counter] |= AttackFlag.MP_HEAL;
                        arg.mp[counter] = (int)-itemmp;
                    }
                    else if (arg.item.BaseData.mp < 0)
                    {
                        arg.flag[counter] |= AttackFlag.MP_DAMAGE;
                        arg.mp[counter] = (int)-itemmp;
                    }

                    counter++;
                    MapManager.Instance.GetMap(sActor.MapID)
                        .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.HPMPSP_UPDATE, null, i, true);
                }
            else if (arg.item.BaseData.duration > 0)
                foreach (var i in dActor)
                {
                    if (arg.item.BaseData.hp > 0)
                    {
                        i.Status.hp_medicine = arg.item.BaseData.hp;
                        var skill1 = new Medicine1(null, i, (int)arg.item.BaseData.duration);
                        ApplyAddition(i, skill1);
                    }

                    if (arg.item.BaseData.mp > 0)
                    {
                        i.Status.mp_medicine = arg.item.BaseData.mp;
                        var skill2 = new Medicine2(null, i, (int)arg.item.BaseData.duration);
                        ApplyAddition(i, skill2);
                    }

                    if (arg.item.BaseData.sp > 0)
                    {
                        i.Status.sp_medicine = arg.item.BaseData.sp;
                        var skill3 = new Medicine3(null, i, (int)arg.item.BaseData.duration);
                        ApplyAddition(i, skill3);
                    }
                }
            //arg.delay = 5000;
        }
    }
}