using System;
using System.Collections.Generic;
using SagaDB.Actor;
using SagaDB.Item;
using SagaDB.Mob;
using SagaLib;
using SagaMap.ActorEventHandlers;
using SagaMap.Manager;
using SagaMap.Mob;
using SagaMap.Network.Client;
using SagaMap.Packets.Client;
using SagaMap.Packets.Server;
using SagaMap.Skill.Additions.Global;
using SagaMap.Skill.SkillDefinations.Cabalist;
using SagaMap.Skill.SkillDefinations.Sage;

namespace SagaMap.Skill
{
    public partial class SkillHandler : Singleton<SkillHandler>
    {
        #region Enums

        public enum DefaultAdditions
        {
            Sleep = 3,
            Poison = 0,
            Stun = 8,
            Silence = 4,
            Stone = 1,
            Confuse = 6,
            鈍足 = 5,
            Frosen = 7,
            Stiff = 14,
            Paralyse = 2,
            CannotMove = 15
        }

        #endregion

        #region Enums

        public enum 异常状态
        {
            无 = -1,
            中毒 = 0,
            石化 = 1,
            麻痹 = 2,
            睡眠 = 3,
            沉默 = 4,
            迟钝 = 5,
            混乱 = 6,
            冻结 = 7,
            昏迷 = 8,
            灼伤 = 9
        }

        #endregion

        /// <summary>
        ///     是否在範圍內
        /// </summary>
        /// <param name="sActor">來源Actor</param>
        /// <param name="dActor">目的Actor</param>
        /// <param name="Range">範圍</param>
        /// <returns>是否在範圍內</returns>
        public bool isInRange(Actor sActor, Actor dActor, short Range)
        {
            if (Math.Abs(sActor.X - dActor.X) < Range || Math.Abs(sActor.Y - dActor.Y) < Range) return true;
            return false;
        }

        /// <summary>
        ///     移動道具
        /// </summary>
        /// <param name="item">道具</param>
        public void MoveItem(Actor dActor, Item item)
        {
            if (dActor.type != ActorType.PC) return;
            var p = new CSMG_ITEM_MOVE();
            //p.InventoryID = 
            var mc = MapClient.FromActorPC((ActorPC)dActor);
            mc.OnItemMove(p);
        }

        /// <summary>
        ///     傳送至某地圖
        /// </summary>
        /// <param name="dActor">目標玩家</param>
        /// <param name="MapID">地圖</param>
        /// <param name="x">X座標</param>
        /// <param name="y">Y座標</param>
        public void Warp(Actor dActor, uint MapID, byte x, byte y)
        {
            var map = MapManager.Instance.GetMap(dActor.MapID);
            map.SendActorToMap(dActor, MapID, Global.PosX8to16(x, map.Width), Global.PosY8to16(y, map.Height));
        }

        /// <summary>
        ///     變身成怪物
        /// </summary>
        /// <param name="sActor">目標玩家</param>
        /// <param name="MobID">怪物ID (0為變回原形)</param>
        public void TranceMob(Actor sActor, uint MobID)
        {
            if (sActor.type == ActorType.PC)
            {
                var pc = (ActorPC)sActor;
                if (MobID == 0)
                {
                    pc.TranceID = 0;
                }
                else
                {
                    var mob = MobFactory.Instance.GetMobData(MobID);
                    pc.TranceID = mob.pictid;
                }

                var client = MapClient.FromActorPC(pc);
                client.SendCharInfoUpdate();
            }
        }

        /// <summary>
        ///     判斷是否為騎寵
        /// </summary>
        /// <param name="mob"></param>
        /// <returns></returns>
        public bool IsRidePet(Actor mob)
        {
            if (mob.type != ActorType.PET) return false;

            var p = (ActorPet)mob;
            return p.BaseData.mobType.ToString().ToUpper().IndexOf("RIDE") > 1;
        }

        /// <summary>
        ///     Actor動作
        /// </summary>
        /// <param name="dActor"></param>
        /// <param name="motion"></param>
        public void NPCMotion(Actor dActor, MotionType motion)
        {
            var p = new SSMG_CHAT_MOTION();
            p.ActorID = dActor.ActorID;
            p.Motion = motion;
            var map = MapManager.Instance.GetMap(dActor.MapID);
            var actors = map.GetActorsArea(dActor, 10000, true);
            foreach (var act in actors)
                if (act.type == ActorType.PC)
                {
                    var pc = (ActorPC)act;
                    MapClient.FromActorPC(pc).netIO.SendPacket(p);
                }
        }

        /// <summary>
        ///     檢查DEM的右手裝備
        /// </summary>
        /// <param name="sActor"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool CheckDEMRightEquip(Actor sActor, ItemType type)
        {
            if (sActor.type == ActorType.PC)
            {
                var pc = (ActorPC)sActor;
                if (pc.Race == PC_RACE.DEM)
                {
                    var EQItems = pc.Inventory.GetContainer(ContainerType.RIGHT_HAND2);
                    if (EQItems.Count > 0)
                        if (EQItems[0].BaseData.itemType == type)
                            return true;
                }
            }

            return false;
        }
        //放置Skill定義中所需的共通Function
        //Place the common functions which will used by SkillDefinations

        #region Utility

        /// <summary>
        ///     取得被憑依的角色
        /// </summary>
        /// <param name="sActor">角色</param>
        /// <returns>被憑依者</returns>
        public ActorPC GetPossesionedActor(ActorPC sActor)
        {
            if (sActor.PossessionTarget == 0) return sActor;

            var map = MapManager.Instance.GetMap(sActor.MapID);
            var act = map.GetActor(sActor.PossessionTarget);
            if (act != null)
                if (act.type == ActorType.PC)
                    return (ActorPC)act;

            return sActor;
        }

        /// <summary>
        ///     吸引怪物
        /// </summary>
        /// <param name="sActor">施放技能者</param>
        /// <param name="dActor">目標</param>
        public void AttractMob(Actor sActor, Actor dActor)
        {
            AttractMob(sActor, dActor, 1000);
        }

        /// <summary>
        ///     吸引怪物
        /// </summary>
        /// <param name="sActor">施放技能者</param>
        /// <param name="dActor">目標</param>
        /// <param name="damage">給予的傷害</param>
        public void AttractMob(Actor sActor, Actor dActor, int damage)
        {
            if (dActor.type == ActorType.MOB)
            {
                var mob = (MobEventHandler)dActor.e;
                mob.AI.OnAttacked(sActor, damage);
                //mob.AI.DamageTable[sActor.ActorID] += damage;
            }
        }

        /// <summary>
        ///     判斷怪物是否為Boss
        /// </summary>
        /// <param name="mob"></param>
        /// <returns></returns>
        public bool isBossMob(Actor mob)
        {
            if (mob.type != ActorType.MOB) return false;

            return isBossMob((ActorMob)mob);
        }

        /// <summary>
        ///     判斷怪物是否為Boss
        /// </summary>
        /// <param name="mob"></param>
        /// <returns></returns>
        public bool isBossMob(ActorMob mob)
        {
            return CheckMobType(mob, "boss");
        }

        /// <summary>
        ///     檢查是否可以被賦予狀態
        /// </summary>
        /// <param name="sActor">賦予者</param>
        /// <param name="dActor">目標</param>
        /// <param name="AdditionName">狀態名稱</param>
        /// <param name="rate">原始成功率</param>
        /// <returns>是否可被賦予</returns>
        public bool CanAdditionApply(Actor sActor, Actor dActor, string AdditionName, int rate)
        {
            //王不受狀態影響
            if (Instance.isBossMob(dActor)) return false;
            if (rate <= 0) return false;
            var newRate = (float)rate;
            //成功率下降(抗性)
            if (dActor.Status.Additions.ContainsKey(AdditionName + "Regi")) newRate = newRate * 0.1f; //減少90%
            if (sActor != dActor)
                if (AdditionName == "Poison")
                    // 提升放毒成功率（毒成功率上昇）
                    if (sActor.Status.Additions.ContainsKey("PoisonRateUp"))
                        newRate = newRate * 1.5f; //增加50%

            //成功率上升(魔法革命、提升異常狀態成功率)
            if (sActor.Status.Additions.ContainsKey("MagHitUpCircle"))
            {
                var mhucb = (MagHitUpCircle.MagHitUpCircleBuff)sActor.Status.Additions["MagHitUpCircle"];
                newRate = newRate * mhucb.Rate;
            }

            if (sActor.Status.Additions.ContainsKey("AllRateUp"))
            {
                var arub = (AllRateUp.AllRateUpBuff)sActor.Status.Additions["AllRateUp"];
                newRate = newRate * arub.Rate;
            }

            //是否可被賦予
            return Global.Random.Next(0, 99) < newRate;
        }

        /// <summary>
        ///     檢查是否可以被賦予狀態
        /// </summary>
        /// <param name="sActor">賦予者</param>
        /// <param name="dActor">目標</param>
        /// <param name="theAddition">狀態類型</param>
        /// <param name="rate">原始成功率</param>
        /// <returns>是否可被賦予</returns>
        public bool CanAdditionApply(Actor sActor, Actor dActor, DefaultAdditions theAddition, int rate)
        {
            //怪物抗性
            if (sActor.Status.control_rate_bonus != 0)
                rate += sActor.Status.control_rate_bonus;
            if (dActor.type == ActorType.MOB)
            {
                var mob = (ActorMob)dActor;
                var value = (int)theAddition;
                if (value < 9)
                {
                    var dStatus = (AbnormalStatus)Enum.ToObject(typeof(AbnormalStatus), value);
                    var regiValue = mob.AbnormalStatus[dStatus];
                    if (regiValue == 100) return false; //完全抵抗

                    rate = (int)(rate * (1f - regiValue / 100));
                }
            }

            return CanAdditionApply(sActor, dActor, theAddition.ToString(), rate);
        }

        /// <summary>
        ///     返回异常状态持续时间，若返回0，则本次异常判定失败
        /// </summary>
        /// <param name="sActor">攻</param>
        /// <param name="dActor">受</param>
        /// <param name="baserate">基础几率</param>
        /// <param name="time">基础持续时间</param>
        /// <param name="resistance">异常状态的种类，影响持续时间，buff类请无视</param>
        /// <param name="fixedrate">固定几率,0-100，大于0代表无视命中率和回避率</param>
        /// <returns></returns>
        public int AdditionApply(Actor sActor, Actor dActor, int baserate, int time, 异常状态 resistance = 异常状态.无,
            int fixedrate = 0)
        {
            short res = 0;
            if (resistance != 异常状态.无)
            {
                var value = (int)resistance;
                var dStatus = (AbnormalStatus)Enum.ToObject(typeof(AbnormalStatus), value);
                res = ((ActorMob)dActor).AbnormalStatus[dStatus];
            }

            return AdditionApply(sActor.Status.hit_magic, dActor.Status.hit_magic, baserate, time, res, fixedrate);
        }

        /// <summary>
        ///     返回异常状态持续时间，若返回0，则本次异常判定失败
        /// </summary>
        /// <param name="hit">命中率</param>
        /// <param name="avoid">回避率</param>
        /// <param name="baserate">基础几率</param>
        /// <param name="time">基础持续时间</param>
        /// <param name="resistance">异常状态数值，影响持续时间，buff类请无视</param>
        /// <param name="fixedrate">固定几率,0-100，大于0代表无视命中率和回避率</param>
        /// <returns></returns>
        public int AdditionApply(int hit, int avoid, int baserate, int time, int resistance, int fixedrate = 0)
        {
            var t = 0;
            var rand = Global.Random.Next(0, 99);
            var rate = baserate * (hit / (hit + 0.5 * avoid + 10) * 2);
            if (rand < rate || (fixedrate > 0 && rand < fixedrate)) t = time * (255 - resistance) / 255;
            return t;
        }

        /// <summary>
        ///     檢查裝備是否正確
        /// </summary>
        /// <param name="sActor">人物</param>
        /// <param name="ItemType">裝備種類</param>
        /// <returns></returns>
        public bool isEquipmentRight(Actor sActor, params ItemType[] ItemType)
        {
            if (sActor.type == ActorType.PC)
            {
                var pc = (ActorPC)sActor;
                if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND))
                {
                    foreach (var t in ItemType)
                        if (pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == t)
                            return true;

                    return false;
                }

                return false;
            }

            return true;
        }

        /// <summary>
        ///     計算道具數量
        /// </summary>
        /// <param name="pc">人物</param>
        /// <param name="itemID">道具ID</param>
        /// <returns>數量</returns>
        public int CountItem(ActorPC pc, uint itemID)
        {
            var item = pc.Inventory.GetItem(itemID, Inventory.SearchType.ITEM_ID);
            if (item != null) return item.Stack;

            return 0;
        }

        /// <summary>
        ///     取走道具
        /// </summary>
        /// <param name="pc">人物</param>
        /// <param name="itemID">道具ID</param>
        /// <param name="count">數量</param>
        public void TakeItem(ActorPC pc, uint itemID, ushort count)
        {
            var client = MapClient.FromActorPC(pc);
            Logger.LogItemLost(Logger.EventType.ItemNPCLost, pc.Name + "(" + pc.CharID + ")", "(" + itemID + ")",
                string.Format("SkillTake Count:{0}", count), true);
            client.DeleteItemID(itemID, count, true);
        }

        /// <summary>
        ///     给予玩家指定个数的道具
        /// </summary>
        /// <param name="pc">玩家</param>
        /// <param name="itemID">道具ID</param>
        /// <param name="count">个数</param>
        /// <param name="identified">是否鉴定</param>
        public List<Item> GiveItem(ActorPC pc, uint itemID, ushort count, bool identified)
        {
            var client = MapClient.FromActorPC(pc);
            var ret = new List<Item>();
            for (var i = 0; i < count; i++)
            {
                var item = ItemFactory.Instance.GetItem(itemID);
                item.Stack = 1;
                item.Identified = true; //免鉴定
                Logger.LogItemGet(Logger.EventType.ItemNPCGet, pc.Name + "(" + pc.CharID + ")",
                    item.BaseData.name + "(" + item.ItemID + ")",
                    string.Format("SkillGive Count:{0}", item.Stack), true);
                client.AddItem(item, true);
                ret.Add(item);
            }

            return ret;
        }

        /// <summary>
        ///     產生自動使用技能的資訊
        /// </summary>
        /// <param name="skillID">技能ID</param>
        /// <param name="level">等級</param>
        /// <param name="delay">延遲</param>
        /// <returns>自動使用技能的資訊</returns>
        public AutoCastInfo CreateAutoCastInfo(uint skillID, byte level, int delay)
        {
            var info = new AutoCastInfo();
            info.delay = delay;
            info.level = level;
            info.skillID = skillID;
            return info;
        }

        /// <summary>
        ///     產生自動使用技能的資訊
        /// </summary>
        /// <param name="skillID">技能ID</param>
        /// <param name="level">等級</param>
        /// <param name="delay">延遲</param>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        /// <returns>自動使用技能的資訊</returns>
        public AutoCastInfo CreateAutoCastInfo(uint skillID, byte level, int delay, byte x, byte y)
        {
            var info = new AutoCastInfo();
            info.delay = delay;
            info.level = level;
            info.skillID = skillID;
            info.x = x;
            info.y = y;
            return info;
        }

        /// <summary>
        ///     給予固定傷害
        /// </summary>
        /// <param name="sActor">攻擊者</param>
        /// <param name="dActor">被攻擊者</param>
        /// <param name="arg">技能參數</param>
        /// <param name="element">屬性</param>
        /// <param name="Damage">傷害</param>
        public void FixAttack(Actor sActor, Actor dActor, SkillArg arg, Elements element, float Damage)
        {
            var actors = new List<Actor>();
            actors.Add(dActor);
            FixAttack(sActor, actors, arg, element, Damage);
        }

        /// <summary>
        ///     給予固定傷害
        /// </summary>
        /// <param name="sActor">攻擊者</param>
        /// <param name="dActor">被攻擊者</param>
        /// <param name="arg">技能參數</param>
        /// <param name="element">屬性</param>
        /// <param name="Damage">傷害</param>
        public void FixAttack(Actor sActor, List<Actor> dActor, SkillArg arg, Elements element, float Damage)
        {
            MagicAttack(sActor, dActor, arg, DefType.IgnoreAll, element, 50, Damage, 0, true);
        }


        public Dictionary<Actor, int> CalcMagicAttackWithoutDamage(Actor sActor, List<Actor> dActor, SkillArg arg,
            Elements element, float MATKBonus)
        {
            var dmgtable = new Dictionary<Actor, int>();
            var index = 0;
            if (sActor.Status.PlusElement_rate > 0)
                MATKBonus += sActor.Status.PlusElement_rate;

            int damage;

            //calculate the MATK
            int matk;
            var mindamage = 0;
            var maxdamage = 0;
            var counter = 0;
            var map = MapManager.Instance.GetMap(sActor.MapID);

            mindamage = sActor.Status.min_matk;
            maxdamage = sActor.Status.max_matk;


            if (mindamage > maxdamage) maxdamage = mindamage;

            foreach (var i in dActor)
            {
                damage = 0;
                var target = i;
                if (i.Status == null)
                    continue;

                //NOTOUCH類MOB 跳過判定
                if (i.type == ActorType.MOB)
                {
                    var checkmob = (ActorMob)i;
                    switch (checkmob.BaseData.mobType)
                    {
                        case MobType.ANIMAL_NOTOUCH:
                        case MobType.BIRD_NOTOUCH:
                        case MobType.ELEMENT_BOSS_NOTOUCH:
                        case MobType.HUMAN_NOTOUCH:
                        case MobType.ELEMENT_NOTOUCH:
                        case MobType.PLANT_NOTOUCH:
                        case MobType.MACHINE_NOTOUCH:
                        case MobType.NONE_NOTOUCH:
                        case MobType.UNDEAD_NOTOUCH:
                        case MobType.WATER_ANIMAL_NOTOUCH:
                        case MobType.PLANT_BOSS_NOTOUCH:
                            continue;
                    }
                }

                var isPossession = false;
                var isHost = false;

                if (i.type == ActorType.PC)
                {
                    var pc = (ActorPC)i;
                    if (pc.PossesionedActors.Count > 0 && pc.PossessionTarget == 0)
                    {
                        isPossession = true;
                        isHost = true;
                    }

                    if (pc.PossessionTarget != 0)
                    {
                        isPossession = true;
                        isHost = false;
                    }
                }

                matk = Global.Random.Next(mindamage, maxdamage);
                if (element != Elements.Neutral)
                {
                    var eleBonus = CalcElementBonus(sActor, i, element, 1,
                        MATKBonus < 0 && !(i.Status.undead && element == Elements.Holy));
                    matk = (int)(matk * eleBonus * MATKBonus);
                }
                else
                {
                    matk = (int)(matk * 1f * MATKBonus);
                }


                if (MATKBonus > 0)
                    damage = CalcMagDamage(sActor, i, DefType.MDef, matk, 0);
                else
                    damage = matk;

                //AttackResult res = AttackResult.Hit;
                var res = CalcAttackResult(sActor, target, true);
                if (res == AttackResult.Critical)
                    res = AttackResult.Hit;
                if (i.Buff.Frosen && element == Elements.Fire)
                    RemoveAddition(i, i.Status.Additions["WaterFrosenElement"]);
                if (i.Buff.Stone && element == Elements.Water)
                    RemoveAddition(i, i.Status.Additions["StoneFrosenElement"]);

                if (sActor.type == ActorType.PC && target.type == ActorType.PC)
                    if (damage > 0)
                        damage = (int)(damage * Configuration.Instance.PVPDamageRateMagic);

                if (target.Status.Additions.ContainsKey("DamageNullify")) //boss状态
                    damage = (int)(damage * 1f);

                if (damage <= 0 && MATKBonus >= 0)
                    damage = 1;

                if (isPossession && isHost && target.Status.Additions.ContainsKey("DJoint"))
                {
                    var buf = (DefaultBuff)target.Status.Additions["DJoint"];
                    if (Global.Random.Next(0, 99) < buf["Rate"])
                    {
                        var dst = map.GetActor((uint)buf["Target"]);
                        if (dst != null)
                        {
                            target = dst;
                            arg.affectedActors[index + counter] = target;
                        }
                    }
                }

                var ride = false;
                if (target.type == ActorType.PC)
                {
                    var pc = (ActorPC)target;
                    if (pc.Pet != null)
                        ride = pc.Pet.Ride;
                }

                if (sActor.type == ActorType.PC && target.type == ActorType.MOB)
                {
                    var mob = (ActorMob)target;
                    if (mob.BaseData.mobType.ToString().Contains("CHAMP") && !sActor.Buff.StateOfMonsterKillerChamp)
                        damage = damage / 10;
                }

                if (!dmgtable.ContainsKey(target))
                    dmgtable.Add(target, damage);
                else
                    dmgtable[target] += damage;

                counter++;
            }

            return dmgtable;
        }

        /// <summary>
        ///     取得搭档
        /// </summary>
        /// <param name="sActor">玩家</param>
        /// <returns>寵物</returns>
        public ActorPartner GetPartner(Actor sActor)
        {
            if (sActor.type != ActorType.PC) return null;
            var pc = (ActorPC)sActor;
            if (pc.Partner == null) return null;
            return pc.Partner;
        }

        /// <summary>
        ///     取得寵物
        /// </summary>
        /// <param name="sActor">玩家</param>
        /// <returns>寵物</returns>
        public ActorPet GetPet(Actor sActor)
        {
            if (sActor.type != ActorType.PC) return null;
            var pc = (ActorPC)sActor;
            if (pc.Pet == null) return null;
            return pc.Pet;
        }

        /// <summary>
        ///     取得寵物AI
        /// </summary>
        /// <param name="sActor">玩家</param>
        /// <returns>寵物AI</returns>
        public MobAI GetMobAI(Actor sActor)
        {
            return GetMobAI(GetPet(sActor));
        }

        /// <summary>
        ///     取得寵物AI
        /// </summary>
        /// <param name="pet">寵物</param>
        /// <returns>寵物AI</returns>
        public MobAI GetMobAI(ActorPet pet)
        {
            if (pet == null) return null;
            if (pet.Ride) return null;
            var peh = (PetEventHandler)pet.e;
            return peh.AI;
        }

        /// <summary>
        ///     檢查怪物類型
        /// </summary>
        /// <param name="mob">怪物</param>
        /// <param name="LowerType">怪物類型</param>
        /// <returns>類型是否正確</returns>
        public bool CheckMobType(ActorMob mob, string MobType)
        {
            return mob.BaseData.mobType.ToString().ToLower().IndexOf(MobType.ToLower()) > -1;
        }

        /// <summary>
        ///     檢查怪物類型
        /// </summary>
        /// <param name="pet">怪物</param>
        /// <param name="type">怪物類型</param>
        /// <returns>類型是否正確</returns>
        public bool CheckMobType(ActorMob pet, params MobType[] type)
        {
            if (pet == null) return false;
            foreach (var t in type)
                if (pet.BaseData.mobType == t)
                    return true;

            return false;
        }

        /// <summary>
        ///     解除憑依
        /// </summary>
        /// <param name="pc">玩家</param>
        /// <param name="pos">部位(None=全部)</param>
        public void PossessionCancel(ActorPC pc, PossessionPosition pos)
        {
            if (pc.Status.Additions.ContainsKey("SoulProtect")) return;
            if (pos == PossessionPosition.NONE)
            {
                PossessionCancel(pc, PossessionPosition.CHEST);
                PossessionCancel(pc, PossessionPosition.LEFT_HAND);
                PossessionCancel(pc, PossessionPosition.NECK);
                PossessionCancel(pc, PossessionPosition.RIGHT_HAND);
            }
            else
            {
                var p = new CSMG_POSSESSION_CANCEL();
                p.PossessionPosition = pos;
                MapClient.FromActorPC(pc).OnPossessionCancel(p);
            }
        }

        /// <summary>
        ///     改變玩家大小
        /// </summary>
        /// <param name="dActor">人物</param>
        /// <param name="playersize">大小</param>
        public void ChangePlayerSize(ActorPC dActor, uint playersize)
        {
            var client = MapClient.FromActorPC(dActor);
            client.Character.Size = playersize;
            client.SendPlayerSizeUpdate();
        }

        /// <summary>
        ///     在对象位置处显示特效
        /// </summary>
        /// <param name="actor">对象</param>
        /// <param name="effectID">特效ID</param>
        public void ShowEffectByActor(Actor actor, uint effectID)
        {
            var map = MapManager.Instance.GetMap(actor.MapID);
            ShowEffect(map, actor, Global.PosX16to8(actor.X, map.Width), Global.PosY16to8(actor.Y, map.Height),
                effectID);
        }

        /// <summary>
        ///     在对象位置处显示特效
        /// </summary>
        /// <param name="actor">对象</param>
        /// <param name="effectID">特效ID</param>
        public void ShowEffectOnActor(Actor actor, uint effectID)
        {
            var map = MapManager.Instance.GetMap(actor.MapID);
            ShowEffect(map, actor, effectID);
        }

        /// <summary>
        ///     在指定对象处显示特效
        /// </summary>
        /// <param name="map">map</param>
        /// <param name="target">对象</param>
        /// <param name="effectID">特效ID</param>
        public void ShowEffect(Map map, Actor target, uint effectID)
        {
            var arg = new EffectArg();
            arg.effectID = effectID;
            arg.actorID = target.ActorID;
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SHOW_EFFECT, arg, target, true);
        }

        /// <summary>
        ///     在指定坐标显示特效
        /// </summary>
        /// <param name="map">map</param>
        /// <param name="x">X坐标</param>
        /// <param name="y">Y坐标</param>
        /// <param name="effectID">特效ID</param>
        public void ShowEffect(Map map, Actor actor, byte x, byte y, uint effectID)
        {
            var arg = new EffectArg();
            arg.effectID = effectID;
            arg.actorID = 0xFFFFFFFF;
            arg.x = x;
            arg.y = y;
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SHOW_EFFECT, arg, actor, true);
        }

        /// <summary>
        ///     在指定对象处显示特效
        /// </summary>
        /// <param name="pc">玩家</param>
        /// <param name="target">对象</param>
        /// <param name="effectID">特效ID</param>
        public void ShowEffect(ActorPC pc, Actor target, uint effectID)
        {
            var arg = new EffectArg();
            arg.effectID = effectID;
            arg.actorID = target.ActorID;
            var client = GetMapClient(pc);
            client.map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SHOW_EFFECT, arg, pc, true);
        }

        /// <summary>
        ///     在指定坐标显示特效
        /// </summary>
        /// <param name="pc">玩家</param>
        /// <param name="x">X坐标</param>
        /// <param name="y">Y坐标</param>
        /// <param name="effectID">特效ID</param>
        public void ShowEffect(ActorPC pc, byte x, byte y, uint effectID)
        {
            var arg = new EffectArg();
            arg.effectID = effectID;
            arg.actorID = 0xFFFFFFFF;
            arg.x = x;
            arg.y = y;
            var client = GetMapClient(pc);
            client.map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SHOW_EFFECT, arg, pc, true);
        }

        private MapClient GetMapClient(ActorPC pc)
        {
            var eh = (PCEventHandler)pc.e;
            return eh.Client;
        }

        #endregion
    }
}