﻿using System;
using System.Linq;
using SagaDB.Actor;
using SagaLib;
using SagaMap.Manager;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill
{
    public partial class SkillHandler
    {
        //横排防御等级
        //竖列变化等级
        private readonly float[,] DEFAULTBONUS =
        {
            {
                0.00f, 1.00f, 1.00f, 1.00f, 1.00f, 1.00f, 1.00f, 1.00f, 1.00f, 1.00f, 1.00f, 1.00f, 1.00f, 1.00f, 1.00f,
                1.00f, 1.00f, 1.00f, 1.00f, 1.00f, 1.00f
            },
            {
                0.00f, 1.20f, 1.30f, 1.40f, 1.50f, 1.60f, 1.70f, 1.80f, 1.90f, 2.00f, 2.15f, 2.30f, 2.45f, 2.60f, 2.75f,
                2.90f, 3.05f, 3.20f, 3.35f, 3.50f, 3.80f
            },
            {
                0.00f, 1.20f, 1.30f, 1.40f, 1.50f, 1.60f, 1.70f, 1.80f, 1.90f, 2.00f, 2.10f, 2.20f, 2.30f, 2.40f, 2.50f,
                2.65f, 2.80f, 2.95f, 3.10f, 3.30f, 3.50f
            },
            {
                0.00f, 1.05f, 1.10f, 1.15f, 1.20f, 1.25f, 1.30f, 1.35f, 1.40f, 1.45f, 1.50f, 1.55f, 1.60f, 1.65f, 1.70f,
                1.75f, 1.80f, 1.85f, 1.90f, 1.95f, 2.00f
            },
            {
                0.00f, 1.05f, 1.10f, 1.15f, 1.20f, 1.25f, 1.30f, 1.35f, 1.40f, 1.45f, 1.50f, 1.55f, 1.60f, 1.65f, 1.70f,
                1.75f, 1.80f, 1.85f, 1.90f, 1.95f, 2.00f
            },
            {
                0.00f, 0.90f, 0.80f, 0.70f, 0.60f, 0.50f, 0.40f, 0.30f, 0.20f, 0.10f, 0.00f, 0.00f, 0.00f, 0.00f, 0.00f,
                0.00f, 0.00f, 0.00f, 0.00f, 0.00f, 0.00f
            },
            {
                0.00f, 0.95f, 0.90f, 0.85f, 0.80f, 0.75f, 0.70f, 0.65f, 0.60f, 0.55f, 0.50f, 0.45f, 0.40f, 0.35f, 0.30f,
                0.25f, 0.20f, 0.15f, 0.10f, 0.05f, 0.00f
            },
            {
                0.00f, 0.97f, 0.94f, 0.91f, 0.88f, 0.85f, 0.82f, 0.79f, 0.76f, 0.73f, 0.70f, 0.67f, 0.64f, 0.61f, 0.58f,
                0.55f, 0.52f, 0.49f, 0.46f, 0.43f, 0.40f
            },
            {
                0.00f, 0.99f, 0.96f, 0.93f, 0.90f, 0.87f, 0.84f, 0.81f, 0.79f, 0.76f, 0.73f, 0.70f, 0.67f, 0.64f, 0.61f,
                0.59f, 0.56f, 0.53f, 0.50f, 0.47f, 0.44f
            }
        };

        // 0 = 无变化
        // 1 = 增加A, 2 = 增加B, 3 = 增加C, 4 = 增加D,
        // 5 = 减少A, 6 = 减少B, 7 = 减少C, 8 = 减少D
        //     无 火 水 风 土 光 暗
        // 无   0  0  0  0  0  0  0
        // 火   0  5  2  6  0  3  7
        // 水   0  6  5  0  2  3  7
        // 风   0  2  0  5  5  3  7
        // 土   0  0  6  2  5  3  7
        // 光   0  6  6  6  6  5  1
        // 暗   0  4  4  4  4  6  5

        private readonly int[,] EFtype =
        {
            { 0, 0, 0, 0, 0, 0, 0 },
            { 0, 5, 2, 6, 0, 3, 7 },
            { 0, 6, 5, 0, 2, 3, 7 },
            { 0, 2, 0, 5, 5, 3, 7 },
            { 0, 0, 6, 2, 5, 3, 7 },
            { 0, 6, 6, 6, 6, 5, 1 },
            { 0, 4, 4, 4, 4, 6, 5 }
        };

        public int fieldelements(Map map, byte x, byte y, Elements eletype)
        {
            var fieldele = 0;
            if (eletype == Elements.Neutral) fieldele = map.Info.neutral[x, y];
            if (eletype == Elements.Fire) fieldele = map.Info.fire[x, y];
            if (eletype == Elements.Water) fieldele = map.Info.water[x, y];
            if (eletype == Elements.Wind) fieldele = map.Info.wind[x, y];
            if (eletype == Elements.Earth) fieldele = map.Info.earth[x, y];
            if (eletype == Elements.Holy) fieldele = map.Info.holy[x, y];
            if (eletype == Elements.Dark) fieldele = map.Info.dark[x, y];
            return fieldele;
        }

        private int bonustype(Elements src, Elements dst)
        {
            return EFtype[(int)src, (int)dst];
        }

        private float defaultbonus(int defincelevel, int attacktype)
        {
            return DEFAULTBONUS[attacktype, defincelevel];
        }

        /// <summary>
        ///     计算实际的属性倍率
        /// </summary>
        /// <param name="sActor">攻击者</param>
        /// <param name="dActor">防御者</param>
        /// <param name="skillelement">技能属性</param>
        /// <param name="skilltype">技能类型, 物理:0, 魔法:1</param>
        /// <param name="heal">是否为治疗技能</param>
        /// <returns>倍率</returns>
        private float efcal(Actor sActor, Actor dActor, Elements skillelement, int skilltype, bool heal)
        {
            Map map;
            byte dx, dy, sx, sy;
            var res = 1f;

            #region Calc Attacker and Defincer Coordinate

            // Attacker and Defincer Must be in the same map
            map = MapManager.Instance.GetMap(dActor.MapID);

            //Attacker Coordinate
            sx = Global.PosX16to8(sActor.X, map.Width);
            sy = Global.PosY16to8(sActor.Y, map.Height);

            //Defincer Coordinate
            dx = Global.PosX16to8(dActor.X, map.Width);
            dy = Global.PosY16to8(dActor.Y, map.Height);

            #endregion

            #region Calc Attack and Defince Element

            var attackElement = Elements.Neutral;
            var defineElement = Elements.Neutral;
            var atkValue = 0;
            var defValue = 0;

            attackElement = GetAttackElement(sActor, ref atkValue, map, sx, sy);
            defineElement = GetDefElement(dActor, ref defValue, map, dx, dy);

            if (sActor.type == ActorType.MOB)
            {
                attackElement = Elements.Neutral;
                atkValue = 100;
            }

            if (skilltype == 1)
            {
                if (skillelement != Elements.Neutral)
                {
                    attackElement = skillelement;
                    atkValue = 100 + sActor.AttackElements[skillelement] +
                               sActor.Status.attackElements_item[skillelement] +
                               sActor.Status.attackElements_skill[skillelement] +
                               sActor.Status.attackelements_iris[skillelement] +
                               fieldelements(map, sx, sy, skillelement);
                }
            }
            else
            {
                if (skillelement != Elements.Neutral)
                {
                    attackElement = skillelement;
                    atkValue = sActor.AttackElements[skillelement] + sActor.Status.attackElements_item[skillelement] +
                               sActor.Status.attackElements_skill[skillelement] +
                               sActor.Status.attackelements_iris[skillelement] +
                               fieldelements(map, sx, sy, skillelement);
                }
            }

            //if (sActor.Status.Additions.ContainsKey("Astralist"))//AS站桩不应被角色提供属性上限影响,移位
            //{
            //    atkValue += (sActor.Status.Additions["Astralist"] as DefaultBuff).Variable["Astralist"];
            //}

            if (sActor.type == ActorType.PC)
            {
                var pc = (ActorPC)sActor;
                if (pc.Skills2_1.ContainsKey(939) || pc.DualJobSkill.Exists(x => x.ID == 939)) //地
                {
                    //这里取副职的契约等级
                    var duallv = 0;
                    if (pc.DualJobSkill.Exists(x => x.ID == 939))
                        duallv = pc.DualJobSkill.FirstOrDefault(x => x.ID == 939).Level;

                    //这里取主职的契约等级
                    var mainlv = 0;
                    if (pc.Skills2_1.ContainsKey(939))
                        mainlv = pc.Skills2_1[939].Level;

                    //这里取等级最高的契约等级用来做倍率加成
                    if (attackElement == Elements.Earth)
                        atkValue = Math.Min(atkValue, 200 + Math.Max(duallv, mainlv) * 5);
                    if (defineElement == Elements.Earth)
                        defValue = Math.Min(defValue, 100 + Math.Max(duallv, mainlv) * 5);
                }

                if (pc.Skills2_1.ContainsKey(936) || pc.DualJobSkill.Exists(x => x.ID == 936)) //火
                {
                    //这里取副职的契约等级
                    var duallv = 0;
                    if (pc.DualJobSkill.Exists(x => x.ID == 936))
                        duallv = pc.DualJobSkill.FirstOrDefault(x => x.ID == 936).Level;

                    //这里取主职的契约等级
                    var mainlv = 0;
                    if (pc.Skills2_1.ContainsKey(936))
                        mainlv = pc.Skills2_1[936].Level;

                    //这里取等级最高的契约等级用来做倍率加成
                    if (attackElement == Elements.Fire)
                        atkValue = Math.Min(atkValue, 200 + Math.Max(duallv, mainlv) * 5);
                    if (defineElement == Elements.Fire)
                        defValue = Math.Min(defValue, 100 + Math.Max(duallv, mainlv) * 5);
                }

                if (pc.Skills2_1.ContainsKey(937) || pc.DualJobSkill.Exists(x => x.ID == 937)) //水
                {
                    //这里取副职的契约等级
                    var duallv = 0;
                    if (pc.DualJobSkill.Exists(x => x.ID == 937))
                        duallv = pc.DualJobSkill.FirstOrDefault(x => x.ID == 937).Level;

                    //这里取主职的契约等级
                    var mainlv = 0;
                    if (pc.Skills2_1.ContainsKey(937))
                        mainlv = pc.Skills2_1[937].Level;

                    //这里取等级最高的契约等级用来做倍率加成
                    if (attackElement == Elements.Water)
                        atkValue = Math.Min(atkValue, 200 + Math.Max(duallv, mainlv) * 5);
                    if (defineElement == Elements.Water)
                        defValue = Math.Min(defValue, 100 + Math.Max(duallv, mainlv) * 5);
                }

                if (pc.Skills2_1.ContainsKey(938) || pc.DualJobSkill.Exists(x => x.ID == 938)) //风
                {
                    //这里取副职的契约等级
                    var duallv = 0;
                    if (pc.DualJobSkill.Exists(x => x.ID == 938))
                        duallv = pc.DualJobSkill.FirstOrDefault(x => x.ID == 938).Level;

                    //这里取主职的契约等级
                    var mainlv = 0;
                    if (pc.Skills2_1.ContainsKey(938))
                        mainlv = pc.Skills2_1[938].Level;

                    //这里取等级最高的契约等级用来做倍率加成
                    if (attackElement == Elements.Wind)
                        atkValue = Math.Min(atkValue, 200 + Math.Max(duallv, mainlv) * 5);
                    if (defineElement == Elements.Wind)
                        defValue = Math.Min(defValue, 100 + Math.Max(duallv, mainlv) * 5);
                }

                if (pc.Skills2_1.ContainsKey(940) || pc.DualJobSkill.Exists(x => x.ID == 940)) //光
                {
                    //这里取副职的契约等级
                    var duallv = 0;
                    if (pc.DualJobSkill.Exists(x => x.ID == 940))
                        duallv = pc.DualJobSkill.FirstOrDefault(x => x.ID == 940).Level;

                    //这里取主职的契约等级
                    var mainlv = 0;
                    if (pc.Skills2_1.ContainsKey(940))
                        mainlv = pc.Skills2_1[940].Level;

                    //这里取等级最高的契约等级用来做倍率加成
                    if (attackElement == Elements.Holy)
                        atkValue = Math.Min(atkValue, 200 + Math.Max(duallv, mainlv) * 5);
                    if (defineElement == Elements.Holy)
                        defValue = Math.Min(defValue, 100 + Math.Max(duallv, mainlv) * 5);
                }

                if (pc.Skills2_1.ContainsKey(941) || pc.DualJobSkill.Exists(x => x.ID == 941)) //暗
                {
                    //这里取副职的契约等级
                    var duallv = 0;
                    if (pc.DualJobSkill.Exists(x => x.ID == 941))
                        duallv = pc.DualJobSkill.FirstOrDefault(x => x.ID == 941).Level;

                    //这里取主职的契约等级
                    var mainlv = 0;
                    if (pc.Skills2_1.ContainsKey(941))
                        mainlv = pc.Skills2_1[941].Level;

                    //这里取等级最高的契约等级用来做倍率加成
                    if (attackElement == Elements.Dark)
                        atkValue = Math.Min(atkValue, 200 + Math.Max(duallv, mainlv) * 5);
                    if (defineElement == Elements.Dark)
                        defValue = Math.Min(defValue, 100 + Math.Max(duallv, mainlv) * 5);
                }
            }

            //非角色提供属性值，追加
            if (sActor.Status.Additions.ContainsKey("Astralist") && skilltype == 1 &&
                skillelement != Elements.Neutral && skillelement != Elements.Holy &&
                skillelement != Elements.Dark) //AS站桩技能,JOB10
                atkValue += (sActor.Status.Additions["Astralist"] as DefaultBuff).Variable["Astralist"];

            #endregion

            #region CalcElementFactor

            if (sActor.Status.Additions.ContainsKey("DecreaseWeapon")) //FO武器属性取消
            {
                attackElement = 0;
                atkValue = 0;
            }

            if (dActor.Status.Additions.ContainsKey("DecreaseShield")) //FO无属性防护
            {
                defineElement = 0;
                defValue = 0;
            }

            var Factor = 1.000f;

            if (heal)
            {
                if (dActor.Elements[Elements.Dark] + dActor.Status.elements_item[Elements.Dark] +
                    fieldelements(map, dx, dy, Elements.Dark) <= 100)
                    res = (1f + (float)Math.Sqrt((dActor.Elements[Elements.Holy] +
                                                  dActor.Status.elements_item[Elements.Holy] +
                                                  dActor.Status.elements_iris[Elements.Holy] +
                                                  fieldelements(map, dx, dy, Elements.Holy)) / 100.0)) *
                          (float)Math.Sqrt(1.0 - (dActor.Elements[Elements.Dark] +
                                                  dActor.Status.elements_item[Elements.Dark] +
                                                  fieldelements(map, dx, dy, Elements.Dark)) / 100.0);
                else
                    res = -(float)Math.Sqrt((dActor.Elements[Elements.Dark] +
                                             dActor.Status.elements_item[Elements.Dark] +
                                             dActor.Status.elements_iris[Elements.Dark] +
                                             fieldelements(map, dx, dy, Elements.Dark)) / 100.0 - 1.0);
            }
            else
            {
                var elementbonustype = bonustype(attackElement, defineElement);


                GetElementFactor(atkValue, defValue, elementbonustype, ref Factor);

                res = res * Factor;
            }

            //if (sActor.Status.Additions.ContainsKey("EvilSoul") || sActor.Status.Additions.ContainsKey("SoulTaker"))
            //{
            //    float rate = 0f;
            //    if (sActor.Status.Additions.ContainsKey("EvilSoul"))
            //    {
            //        rate += ((float)((sActor.Status.Additions["EvilSoul"] as DefaultBuff).Variable["EvilSoul"]) / 100.0f);
            //    }
            //    if (sActor.Status.Additions.ContainsKey("SoulTaker"))
            //    {
            //        rate += ((float)((sActor.Status.Additions["SoulTaker"] as DefaultBuff).Variable["SoulTaker"]) / 100.0f);
            //    }
            //    if (attackElement == Elements.Dark)
            //        res *= rate;
            //}

            if (sActor.Status.AddElement.ContainsKey((byte)defineElement))
                res *= 1.0f + sActor.Status.AddElement[(byte)defineElement] / 100.0f;

            if (dActor.Status.SubElement.ContainsKey((byte)attackElement))
                res *= 1.0f - dActor.Status.SubElement[(byte)attackElement] / 100.0f;

            #endregion

            return res;
        }

        private void GetElementFactor(int atkValue, int defValue, int type, ref float Factor)
        {
            int deflevel = GetDefElementLevel(defValue);
            Factor = defaultbonus(deflevel, type);
            if (type > 0 && type < 5)
            {
                if (type == 4)
                    Factor += atkValue / 400.0f;
                else
                    Factor += atkValue / 100.0f;
            }
        }


        private short GetDefElementLevel(int DefinceValue)
        {
            if (DefinceValue < 10)
                return 1;
            if (DefinceValue >= 10 && DefinceValue <= 14)
                return 2;
            if (DefinceValue >= 15 && DefinceValue <= 19)
                return 3;
            if (DefinceValue >= 20 && DefinceValue <= 24)
                return 4;
            if (DefinceValue >= 25 && DefinceValue <= 29)
                return 5;
            if (DefinceValue >= 30 && DefinceValue <= 34)
                return 6;
            if (DefinceValue >= 35 && DefinceValue <= 39)
                return 7;
            if (DefinceValue >= 40 && DefinceValue <= 44)
                return 8;
            if (DefinceValue >= 45 && DefinceValue <= 49)
                return 9;
            if (DefinceValue >= 50 && DefinceValue <= 54)
                return 10;
            if (DefinceValue >= 55 && DefinceValue <= 59)
                return 11;
            if (DefinceValue >= 60 && DefinceValue <= 64)
                return 12;
            if (DefinceValue >= 65 && DefinceValue <= 69)
                return 13;
            if (DefinceValue >= 70 && DefinceValue <= 74)
                return 14;
            if (DefinceValue >= 75 && DefinceValue <= 79)
                return 15;
            if (DefinceValue >= 80 && DefinceValue <= 84)
                return 16;
            if (DefinceValue >= 85 && DefinceValue <= 89)
                return 17;
            if (DefinceValue >= 90 && DefinceValue <= 94)
                return 18;
            if (DefinceValue >= 95 && DefinceValue <= 99)
                return 19;
            return 20;
        }

        private Elements GetAttackElement(Actor sActor, ref int atkvalue, Map map, byte x, byte y)
        {
            var ele = Elements.Neutral;

            if (sActor.type == ActorType.PC)
            {
                ele = sActor.WeaponElement;
                atkvalue = sActor.Status.attackElements_item[sActor.WeaponElement]
                           + sActor.Status.attackElements_skill[sActor.WeaponElement]
                           + sActor.Status.attackelements_iris[sActor.WeaponElement];
            }
            else
            {
                foreach (var item in sActor.AttackElements)
                    if (atkvalue < item.Value + sActor.Status.attackElements_skill[item.Key])
                    {
                        ele = item.Key;
                        atkvalue = item.Value + sActor.Status.attackElements_skill[item.Key];
                    }
            }

            atkvalue += fieldelements(map, x, y, ele);
            return ele;
        }

        private Elements GetDefElement(Actor dActor, ref int defvalue, Map map, byte x, byte y)
        {
            var ele = Elements.Neutral;


            if (dActor.type == ActorType.PC)
            {
                ele = dActor.ShieldElement;
                defvalue = dActor.Status.elements_item[dActor.ShieldElement]
                           + dActor.Status.elements_skill[dActor.ShieldElement]
                           + dActor.Status.elements_iris[dActor.ShieldElement];
            }
            else
            {
                foreach (var item in dActor.Elements)
                    if (defvalue < item.Value + dActor.Status.elements_skill[item.Key])
                    {
                        defvalue = item.Value + dActor.Status.elements_skill[item.Key];
                        ele = item.Key;
                    }
            }

            defvalue += fieldelements(map, x, y, ele);

            if (dActor.Status.Additions.ContainsKey("WaterFrosenElement"))
            {
                ele = Elements.Water;
                defvalue = 100;
            }

            if (dActor.Status.Additions.ContainsKey("StoneFrosenElement"))
            {
                ele = Elements.Earth;
                defvalue = 100;
            }

            return ele;
        }

        /// <summary>
        ///     计算实际的属性倍率
        /// </summary>
        /// <param name="sActor">攻击者</param>
        /// <param name="dActor">防御者</param>
        /// <param name="skillelement">技能属性</param>
        /// <param name="skilltype">技能类型, 物理:0, 魔法:1</param>
        /// <param name="heal">是否为治疗技能</param>
        /// <returns>倍率</returns>
        private float CalcElementBonus(Actor sActor, Actor dActor, Elements element, int skilltype, bool heal)
        {
            return efcal(sActor, dActor, element, skilltype, heal);
        }
    }
}