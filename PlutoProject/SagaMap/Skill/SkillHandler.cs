using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.CSharp;
using SagaDB.Actor;
using SagaDB.Item;
using SagaDB.Map;
using SagaDB.Mob;
using SagaDB.Skill;
using SagaLib;
using SagaLib.Tasks;
using SagaMap.ActorEventHandlers;
using SagaMap.Manager;
using SagaMap.Mob;
using SagaMap.Network.Client;
using SagaMap.Packets.Client.Item;
using SagaMap.Packets.Client.Possession;
using SagaMap.Packets.Server.Actor;
using SagaMap.Packets.Server.Chat;
using SagaMap.Packets.Server.Item;
using SagaMap.PC;
using SagaMap.Skill.Additions;
using SagaMap.Skill.NewSkill.FL2_1;
using SagaMap.Skill.NewSkill.FL2_2;
using SagaMap.Skill.NewSkill.FR1;
using SagaMap.Skill.NewSkill.FR2_2;
using SagaMap.Skill.NewSkill.Item;
using SagaMap.Skill.NewSkill.NewBoss;
using SagaMap.Skill.NewSkill.Traveler;
using SagaMap.Skill.SkillDefinations;
using SagaMap.Skill.SkillDefinations.COF_Additions.BOSS朋朋;
using SagaMap.Skill.SkillDefinations.COF_Additions.丢弃;
using SagaMap.Skill.SkillDefinations.COF_Additions.天骸鸢;
using SagaMap.Skill.SkillDefinations.COF_Additions.巨大咕咕鸡;
using SagaMap.Skill.SkillDefinations.COF_Additions.武器技能;
using SagaMap.Skill.SkillDefinations.COF_Additions.熊爹;
using SagaMap.Skill.SkillDefinations.COF_Additions.领主骑士;
using SagaMap.Skill.SkillDefinations.Event;
using SagaMap.Skill.SkillDefinations.FGarden;
using SagaMap.Skill.SkillDefinations.Global;
using SagaMap.Skill.SkillDefinations.Global.Active;
using SagaMap.Skill.SkillDefinations.Global.Passive;
using SagaMap.Skill.SkillDefinations.Marionette;
using SagaMap.Skill.SkillDefinations.Monster;
using SagaMap.Skill.SkillDefinations.Parnter;
using SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Advance_Novice.Joker_小丑_;
using SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._1_0_Class.Farmer_农夫_;
using SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._1_0_Class.Merchant_商人_;
using SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._1_0_Class.Ranger_冒险家_;
using SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._1_0_Class.Tatarabe_矿工_;
using SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._2_1_Class.Alchemist_炼金术士____far;
using SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._2_1_Class.Blacksmith_铁匠____tat;
using SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._2_1_Class.Explorer_探险家____rag;
using SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._2_1_Class.Trader_贸易商____mer;
using SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._2_2_Class.Gambler_赌徒____mer;
using SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._2_2_Class.Machinery_机械师____tat;
using SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._2_2_Class.Marionest_木偶师____far;
using
    SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._2_2_Class.TreasureHunter_考古学家____rag;
using SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._3_0_Class.Harvest_收获者____far;
using SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._3_0_Class.Maestro_艺术家____tat;
using SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._3_0_Class.Royaldealer_皇家贸易商____mer;
using SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._3_0_Class.Stryder_风行者____rag;
using SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._1_0_Class.Archer_弓箭手_;
using SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._1_0_Class.Fencer_骑士_;
using SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._1_0_Class.Scout_盗贼_;
using SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._1_0_Class.Swordman_剑士_;
using SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._2_1_Class.Assassin_刺客____sco;
using SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._2_1_Class.BladeMaster_剑圣____swm;
using SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._2_1_Class.Knight_圣骑士____fen;
using SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._2_1_Class.Striker_猎人____arc;
using SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._2_2_Class.BountyHunter_赏金猎人____swm;
using SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._2_2_Class.Command_特工____sco;
using SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._2_2_Class.DarkStalker_黑暗骑士____fen;
using SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._2_2_Class.Gunner_神枪手____arc;
using SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._3_0_Class.Eraser_肃清者____sco;
using SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._3_0_Class.Gladiator_剑斗士____swm;
using SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._3_0_Class.Guardian_守护者____fen;
using SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._3_0_Class.Hawkeye_隼人____arc;
using SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Joint_Class.Breeder_驯兽师_;
using SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Joint_Class.Gardener_庭园师_;
using SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._1_0_Class.Shaman_精灵使_;
using SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._1_0_Class.Votes_祭司_;
using SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._1_0_Class.Warlock_暗术使_;
using SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._1_0_Class.Wizard_魔法师_;
using SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._2_1_Class.Cabalist_秘术使____lock;
using SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._2_1_Class.Druid_神官____vote;
using SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._2_1_Class.Elementaler_元素使____sha;
using SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._2_1_Class.Sorcerer_魔导师____wiz;
using SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._2_2_Class.Bard_诗人____vote;
using SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._2_2_Class.Enchanter_附魔师____sha;
using SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._2_2_Class.Necromancer_死灵使____lock;
using SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._2_2_Class.Sage_贤者____wiz;
using SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._3_0_Class.Astralist_星灵使____sha;
using SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._3_0_Class.Cardinal_大主教____vote;
using SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._3_0_Class.ForceMaster_原力导师____wiz;
using SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._3_0_Class.SoulTaker_噬魂者____lock;
using SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.Advance_Novice.Joker_小丑_;
using SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.BackPackers_Class._1_0_Class.Farmer_农夫_;
using SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.BackPackers_Class._1_0_Class.Merchant_商人_;
using SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.BackPackers_Class._1_0_Class.Ranger_冒险家_;
using SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.BackPackers_Class._1_0_Class.Tatarabe_矿工_;
using SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.BackPackers_Class._2_1_Class.Alchemist_炼金术士____far;
using SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.BackPackers_Class._2_1_Class.Blacksmith_铁匠____tat;
using SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.BackPackers_Class._2_1_Class.Explorer_探险家____rag;
using SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.BackPackers_Class._2_1_Class.Trader_贸易商____mer;
using SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.BackPackers_Class._2_2_Class.Gambler_赌徒____mer;
using SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.BackPackers_Class._2_2_Class.Machinery_机械师____tat;
using SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.BackPackers_Class._3_0_Class.Harvest_收获者____far;
using SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.BackPackers_Class._3_0_Class.Royaldealer_皇家贸易商____mer;
using SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.BackPackers_Class._3_0_Class.Stryder_风行者____rag;
using SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.Fingther_Class._1_0_Class.Fencer_骑士_;
using SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.Fingther_Class._1_0_Class.Scout_盗贼_;
using SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.Fingther_Class._2_1_Class.Assassin_刺客____sco;
using SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.Fingther_Class._2_1_Class.BladeMaster_剑圣____swm;
using SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.Fingther_Class._2_1_Class.Knight_圣骑士____fen;
using SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.Fingther_Class._2_1_Class.Striker_猎人____arc;
using SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.Fingther_Class._2_2_Class.BountyHunter_赏金猎人____swm;
using SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.Fingther_Class._2_2_Class.Command_特工____sco;
using SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.Fingther_Class._2_2_Class.DarkStalker_黑暗骑士____fen;
using SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.Fingther_Class._2_2_Class.Gunner_神枪手____arc;
using SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.Fingther_Class._3_0_Class.Eraser_肃清者____sco;
using SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.Fingther_Class._3_0_Class.Gladiator_剑斗士____swm;
using SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.Fingther_Class._3_0_Class.Guardian_守护者____fen;
using SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.Fingther_Class._3_0_Class.Hawkeye_隼人____arc;
using SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.Joint_Class.Breeder_驯兽师_;
using SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.Joint_Class.Gardener_庭园师_;
using SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.Spell_User_Class._1_0_Class.Shaman_精灵使_;
using SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.Spell_User_Class._1_0_Class.Warlock_暗术使_;
using SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.Spell_User_Class._1_0_Class.Wizard_魔法师_;
using SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.Spell_User_Class._2_1_Class.Cabalist_秘术使____lock;
using SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.Spell_User_Class._2_1_Class.Druid_神官____vote;
using SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.Spell_User_Class._2_2_Class.Bard_诗人____vote;
using SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.Spell_User_Class._2_2_Class.Necromancer_死灵使____lock;
using SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.Spell_User_Class._2_2_Class.Sage_贤者____wiz;
using SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.Spell_User_Class._3_0_Class.Cardinal_大主教____vote;
using SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.Spell_User_Class._3_0_Class.ForceMaster_原力导师____wiz;
using SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.Spell_User_Class._3_0_Class.SoulTaker_噬魂者____lock;
using SagaMap.Skill.SkillDefinations.Repair;
using SagaMap.Skill.SkillDefinations.SunFlowerAdditions;
using SagaMap.Skill.SkillDefinations.Weapon.Passive;
using AreaHeal =
    SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._2_1_Class.Druid_神官____vote.AreaHeal;
using AReflection =
    SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._2_2_Class.Sage_贤者____wiz.AReflection;
using Blow = SagaMap.Skill.SkillDefinations.Monster.Blow;
using Brandish = SagaMap.Skill.SkillDefinations.Monster.Brandish;
using CAPACommunion =
    SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._3_0_Class.Royaldealer_皇家贸易商____mer.
    CAPACommunion;
using ChainLightning =
    SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._2_1_Class.Elementaler_元素使____sha.
    ChainLightning;
using ChgTrance = SagaMap.Skill.SkillDefinations.Event.ChgTrance;
using ConfuseBlow = SagaMap.Skill.SkillDefinations.Monster.ConfuseBlow;
using CriUp =
    SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._3_0_Class.Royaldealer_皇家贸易商____mer.
    CriUp;
using CureAll =
    SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._3_0_Class.Cardinal_大主教____vote.CureAll;
using DarkMist =
    SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._2_2_Class.DarkStalker_黑暗骑士____fen.
    DarkMist;
using DarkStorm = SagaMap.Skill.SkillDefinations.Monster.DarkStorm;
using EarthArrow = SagaMap.Skill.SkillDefinations.Monster.EarthArrow;
using EarthQuake =
    SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._3_0_Class.Astralist_星灵使____sha.
    EarthQuake;
using ExpUp = SagaMap.Skill.SkillDefinations.Event.ExpUp;
using FireArrow = SagaMap.Skill.SkillDefinations.Monster.FireArrow;
using Fish = SagaMap.Skill.SkillDefinations.Global.Active.Fish;
using Gravity = SagaMap.Skill.SkillDefinations.Event.Gravity;
using Healing =
    SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._1_0_Class.Votes_祭司_.Healing;
using HPRecovery = SagaMap.Skill.SkillDefinations.Marionette.HPRecovery;
using Iai = SagaMap.Skill.SkillDefinations.Monster.Iai;
using IceArrow = SagaMap.Skill.SkillDefinations.Monster.IceArrow;
using Invisible = SagaMap.Skill.Additions.Invisible;
using LightOne = SagaMap.Skill.SkillDefinations.Monster.LightOne;
using MAG_INT_DEX_UP = SagaMap.Skill.SkillDefinations.Event.MAG_INT_DEX_UP;
using MagPoison = SagaMap.Skill.SkillDefinations.Monster.MagPoison;
using MagSlow = SagaMap.Skill.SkillDefinations.Monster.MagSlow;
using MPRecovery = SagaMap.Skill.SkillDefinations.Marionette.MPRecovery;
using Phalanx = SagaMap.Skill.SkillDefinations.Monster.Phalanx;
using RobotAtkUp =
    SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._3_0_Class.Maestro_艺术家____tat.
    RobotAtkUp;
using RobotDefUp =
    SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._3_0_Class.Maestro_艺术家____tat.
    RobotDefUp;
using Rush = SagaMap.Skill.SkillDefinations.Monster.Rush;
using ShadowBlast = SagaMap.Skill.SkillDefinations.c1skill.ShadowBlast;
using ShockWave =
    SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._3_0_Class.ForceMaster_原力导师____wiz.
    ShockWave;
using SPRecovery = SagaMap.Skill.SkillDefinations.Marionette.SPRecovery;
using STR_VIT_AGI_UP = SagaMap.Skill.SkillDefinations.Event.STR_VIT_AGI_UP;
using StunBlow = SagaMap.Skill.SkillDefinations.Monster.StunBlow;
using SumMob = SagaMap.Skill.SkillDefinations.Global.SumMob;
using SumMobCastSkill = SagaMap.Skill.SkillDefinations.Global.SumMobCastSkill;
using TrDrop2 = SagaMap.Skill.SkillDefinations.Monster.TrDrop2;
using TurnUndead =
    SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._1_0_Class.Votes_祭司_.TurnUndead;
using WaterArrow = SagaMap.Skill.SkillDefinations.Monster.WaterArrow;
using WaterGroove = SagaMap.Skill.SkillDefinations.Monster.WaterGroove;
using WindArrow = SagaMap.Skill.SkillDefinations.Monster.WindArrow;

namespace SagaMap.Skill {
    public class GetElementResult {
        public Elements Elements { get; set; }
        public int Value { get; set; }

        public GetElementResult(Elements elements, int value) {
            Elements = elements;
            Value = value;
        }
    }

    public class SkillHandler : Singleton<SkillHandler> {
        private float
            CalcIrisDmgFactor(Actor sActor, Actor dActor, Elements skillelement, int skilltype,
                bool heal) /*skilltype=0,物理；1，魔法*/ {
            var res = 1f;
            if (sActor.type == ActorType.PC) {
                var spc = (ActorPC)sActor;
                foreach (var i in spc.IrisAbilityLevels.Keys) {
                    var a = false;
                    switch (i.ID) {
                        //bool depend on sActor
                        case 2001: //初心者光环
                            if (spc.Level <= 60) a = true;
                            break;
                        case 2021: //融会贯通
                            if (spc.Level <= 60) a = true;
                            break;
                        case 2041: //巅峰武艺
                            if (spc.Level <= 60) a = true;
                            break;
                        case 2101: //全力以赴
                            if (spc.Level <= 60) a = true;
                            break;
                        case 2151: //底力
                            if (spc.Level <= 60) a = true;
                            break;
                        case 2300: //无之信仰
                            if (spc.Level <= 60) a = true;
                            break;
                        case 2310: //火之信仰
                            if (spc.Level <= 60) a = true;
                            break;
                        case 2320: //水之信仰
                            if (spc.Level <= 60) a = true;
                            break;
                        case 2330: //风之信仰
                            if (spc.Level <= 60) a = true;
                            break;
                        case 2340: //地之信仰
                            if (spc.Level <= 60) a = true;
                            break;
                        case 2350: //光之信仰
                            if (spc.Level <= 60) a = true;
                            break;
                        case 2360: //暗之信仰
                            if (spc.Level <= 60) a = true;
                            break;
                        case 2400: //短剑大师
                            if (spc.Level <= 60) a = true;
                            break;
                        case 2405: //爪类大师
                            if (spc.Level <= 60) a = true;
                            break;
                        case 2410: //锤类大师
                            if (spc.Level <= 60) a = true;
                            break;
                        case 2415: //斧类大师
                            if (spc.Level <= 60) a = true;
                            break;
                        case 2420: //鞭类大师
                            if (spc.Level <= 60) a = true;
                            break;
                        case 2425: //剑类大师
                            if (spc.Level <= 60) a = true;
                            break;
                        case 2430: //细剑大师
                            if (spc.Level <= 60) a = true;
                            break;
                        case 2435: //矛类大师
                            if (spc.Level <= 60) a = true;
                            break;
                        case 2440: //弓类大师
                            if (spc.Level <= 60) a = true;
                            break;
                        case 2445: //手枪大师
                            if (spc.Level <= 60) a = true;
                            break;
                        case 2450: //步枪大师
                            if (spc.Level <= 60) a = true;
                            break;
                        case 2455: //双枪大师
                            if (spc.Level <= 60) a = true;
                            break;
                        case 2460: //书类大师
                            if (spc.Level <= 60) a = true;
                            break;
                        case 2465: //杖类大师
                            if (spc.Level <= 60) a = true;
                            break;
                        case 2470: //琴类大师
                            if (spc.Level <= 60) a = true;
                            break;
                        case 2475: //包包大师
                            if (spc.Level <= 60) a = true;
                            break;
                        case 2480: //神奇大师
                            if (spc.Level <= 60) a = true;
                            break;
                        //bool depend on dActor
                    }
                }
            }

            if (dActor.type == ActorType.PC) {
            }

            return res;
        }

        private float CalcIrisHitFactor(Actor sActor, Actor dActor) {
            var res = 1f;
            if (sActor.type == ActorType.PC) {
            }

            if (dActor.type == ActorType.PC) {
            }

            return res;
        }

        private float CalcIrisSkillCastSpeedFactor(Actor sActor) {
            var res = 1f;
            if (sActor.type == ActorType.PC) {
            }

            return res;
        }

        private float CalcIrisSkillCostFactor(Actor sActor) {
            var res = 1f;
            if (sActor.type == ActorType.PC) {
            }

            return res;
        }

        private float CalcIrisExpFactor(Actor sActor, Actor dActor) {
            var res = 1f;
            if (sActor.type == ActorType.PC) {
            }

            return res;
        }

        /// <summary>
        ///     检查BUFF对伤害的影响
        /// </summary>
        /// <param name="dActor">目标</param>
        /// <param name="damage">输入伤害</param>
        /// <returns></returns>
        public int CheckBuffForDamage(Actor sActor, Actor dActor, int damage) {
            if (dActor.Status.Additions.ContainsKey("Invincible")) //绝对壁垒
                damage = 0;
            //技能以及状态判定
            if (sActor.type == ActorType.PC) {
                var pcsActor = (ActorPC)sActor;
                if (sActor.Status.Additions
                    .ContainsKey(
                        "BurnRate")) // && SkillHandler.Instance.isEquipmentRight(pcsActor, SagaDB.Item.ItemType.CARD))//皇家贸易商
                    //副职不存在3371于是不进行判定
                    if (pcsActor.Skills3.ContainsKey(3371))
                        if (pcsActor.Skills3[3371].Level > 1) {
                            int[] gold = { 0, 0, 100, 250, 500, 1000 };
                            if (pcsActor.Gold > gold[pcsActor.Skills3[3371].Level]) {
                                pcsActor.Gold -= gold[pcsActor.Skills3[3371].Level];
                                damage += gold[pcsActor.Skills3[3371].Level];
                            }
                        }
            }

            if (sActor.type == ActorType.PC && dActor.type == ActorType.MOB) {
                var mob = (ActorMob)dActor;
                if (mob.BaseData.mobType.ToString().Contains("CHAMP") && !sActor.Buff.StateOfMonsterKillerChamp)
                    damage = damage / 10;
            }

            //if (sActor.type == ActorType.PC)
            //{
            //    int score = damage / 100;
            //    if (score == 0)
            //        score = 1;
            //    ODWarManager.Instance.UpdateScore(sActor.MapID, sActor.ActorID, score);
            //}
            if (dActor.Status.Additions.ContainsKey("DamageUp")) //伤害标记
            {
                var DamageUpRank = dActor.Status.Damage_Up_Lv * 0.1f + 1.1f;
                damage = (int)(damage * DamageUpRank);
            }

            if (dActor.Status.PhysiceReduceRate > 0) //物理抗性
            {
                if (dActor.Status.PhysiceReduceRate > 1)
                    damage = (int)(damage / dActor.Status.PhysiceReduceRate);
                else
                    damage = (int)(damage / (1.0f + dActor.Status.PhysiceReduceRate));
            }

            //加伤处理下
            if (dActor.Seals > 0)
                damage = (int)(damage * (1f + 0.05f * dActor.Seals)); //圣印
            if (sActor.Status.Additions.ContainsKey("ruthless") &&
                (dActor.Buff.Stun || dActor.Buff.Stone || dActor.Buff.Frosen || dActor.Buff.Poison ||
                 dActor.Buff.Sleep || dActor.Buff.SpeedDown || dActor.Buff.Confused || dActor.Buff.Paralysis))
                if (sActor.type == ActorType.PC) {
                    var rate = 1f + ((ActorPC)sActor).TInt["ruthless"] * 0.1f;
                    damage = (int)(damage * rate); //无情打击
                }
            //加伤处理上

            //减伤处理下
            if (dActor.Status.Additions.ContainsKey("DamageNullify")) //boss状态
                damage = (int)(damage * 0f);
            if (dActor.Status.Additions.ContainsKey("EnergyShield")) //能量加护
            {
                if (dActor.type == ActorType.PC)
                    damage = (int)(damage * (1f - 0.02f * ((ActorPC)dActor).TInt["EnergyShieldlv"]));
                else
                    damage = (int)(damage * 0.9f);
            }

            if (dActor.Status.Additions.ContainsKey("Counter")) damage /= 2;

            if (dActor.Status.Additions.ContainsKey("Blocking") && dActor.Status.Blocking_LV != 0 &&
                dActor.type == ActorType.PC) //3转骑士格挡
            {
                var pc = (ActorPC)dActor;
                if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND) &&
                    pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.LEFT_HAND))
                    if (pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.SHIELD ||
                        pc.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].BaseData.itemType == ItemType.SHIELD) {
                        var SutanOdds = dActor.Status.Blocking_LV * 5;
                        var ParryOdds = new[] { 0, 15, 25, 35, 65, 75 }[dActor.Status.Blocking_LV];
                        float ParryResult = 4 + 6 * dActor.Status.Blocking_LV;
                        var args = new SagaDB.Skill.Skill();
                        //不管是主职还是副职,检查盾牌专精是否存在
                        if (pc.Skills.ContainsKey(116) || pc.DualJobSkills.Exists(x => x.ID == 116)) {
                            //这里取副职的盾牌专精等级
                            var duallv = 0;
                            if (pc.DualJobSkills.Exists(x => x.ID == 116))
                                duallv = pc.DualJobSkills.FirstOrDefault(x => x.ID == 116).Level;

                            //这里取主职的盾牌专精等级
                            var mainlv = 0;
                            if (pc.Skills.ContainsKey(116))
                                mainlv = pc.Skills[116].Level;

                            //这里取等级最高的盾牌专精等级用来做运算
                            var level = Math.Max(duallv, mainlv);

                            ParryResult += level * 3;
                            //ParryResult += pc.Skills[116].Level * 3;
                        }

                        if (Global.Random.Next(1, 100) <= ParryOdds) {
                            damage = damage - (int)(damage * ParryResult / 100.0f);
                            if (Instance.CanAdditionApply(dActor, sActor, DefaultAdditions.Stun, SutanOdds)) {
                                var skill = new Stun(args, sActor, 1000 + 500 * dActor.Status.Blocking_LV);
                                ApplyAddition(sActor, skill);
                            }
                        }
                    }
            }
            //减伤处理上

            //开始处理最终伤害放大


            //杀戮放大
            if (sActor.Status.Additions.ContainsKey("Efuikasu"))
                damage = (int)(damage * (1.0f + sActor.KillingMarkCounter * 0.05f));


            //火心仅对物理伤害放大，取消

            //竜眼放大
            if (sActor.Status.Additions.ContainsKey("DragonEyeOpen")) {
                var rate = (sActor.Status.Additions["DragonEyeOpen"] as DefaultBuff).Variable["DragonEyeOpen"];
                damage = (int)(damage * ((double)rate / 100));
            }

            if (sActor.type == ActorType.PC) {
                var pc = (ActorPC)sActor;
                if (pc.Party != null && sActor.Status.pt_dmg_up_iris > 100)
                    damage = (int)(damage * (sActor.Status.pt_dmg_up_iris / 100.0f));
                //iris卡种族增伤部分
                if (dActor.Race == Race.HUMAN && pc.Status.human_dmg_up_iris > 100)
                    damage = (int)(damage * (pc.Status.human_dmg_up_iris / 100.0f));
                else if (dActor.Race == Race.BIRD && pc.Status.bird_dmg_up_iris > 100)
                    damage = (int)(damage * (pc.Status.bird_dmg_up_iris / 100.0f));
                else if (dActor.Race == Race.ANIMAL && pc.Status.animal_dmg_up_iris > 100)
                    damage = (int)(damage * (pc.Status.animal_dmg_up_iris / 100.0f));
                else if (dActor.Race == Race.MAGIC_CREATURE && pc.Status.magic_c_dmg_up_iris > 100)
                    damage = (int)(damage * (pc.Status.magic_c_dmg_up_iris / 100.0f));
                else if (dActor.Race == Race.PLANT && pc.Status.plant_dmg_up_iris > 100)
                    damage = (int)(damage * (pc.Status.plant_dmg_up_iris / 100.0f));
                else if (dActor.Race == Race.WATER_ANIMAL && pc.Status.water_a_dmg_up_iris > 100)
                    damage = (int)(damage * (pc.Status.water_a_dmg_up_iris / 100.0f));
                else if (dActor.Race == Race.MACHINE && pc.Status.machine_dmg_up_iris > 100)
                    damage = (int)(damage * (pc.Status.machine_dmg_up_iris / 100.0f));
                else if (dActor.Race == Race.ROCK && pc.Status.rock_dmg_up_iris > 100)
                    damage = (int)(damage * (pc.Status.rock_dmg_up_iris / 100.0f));
                else if (dActor.Race == Race.ELEMENT && pc.Status.element_dmg_up_iris > 100)
                    damage = (int)(damage * (pc.Status.element_dmg_up_iris / 100.0f));
                else if (dActor.Race == Race.UNDEAD && pc.Status.undead_dmg_up_iris > 100)
                    damage = (int)(damage * (pc.Status.undead_dmg_up_iris / 100.0f));
            }

            if (sActor.WeaponElement == Elements.Holy)
                if (dActor.Status.Additions.ContainsKey("Oratio"))
                    damage = (int)(damage * 1.25f);

            return damage;
        }

        /// <summary>
        ///     只计算面板左右防御的影响 不考虑任何面板外的状态和判定
        /// </summary>
        /// <param name="dActor"></param>
        /// <param name="defType">可以对物理技能进行魔法类防御判定</param>
        /// <param name="atk"></param>
        /// <param name="ignore">根据deftype无视相应防御类型的百分比值，不可超过100%，即左防50的目标ignore0.5后只计算25左防，若是右放200则计算100</param>
        /// <returns></returns>
        public int CalcPhyDamage(Actor sActor, Actor dActor, DefType defType, int atk, float ignore,
            AttackResult res = AttackResult.Hit) {
            int damage, def1 = 0, def2 = 0;
            switch (defType) {
                case DefType.Def:
                    def1 = dActor.Status.def;
                    if (dActor is ActorMob)
                        def1 += dActor.Status.def_skill;
                    def2 = dActor.Status.def_add;
                    if (dActor is ActorMob)
                        def2 += dActor.Status.def_add_skill;
                    break;
                case DefType.MDef:
                    def1 = dActor.Status.mdef;
                    if (dActor is ActorMob)
                        def1 += dActor.Status.mdef_skill;
                    def2 = dActor.Status.mdef_add;
                    if (dActor is ActorMob)
                        def2 += dActor.Status.mdef_add_skill;
                    break;
                case DefType.IgnoreLeft:
                    def1 = 0;
                    def2 = dActor.Status.def_add;
                    if (dActor is ActorMob)
                        def2 += dActor.Status.def_add_skill;
                    break;
                case DefType.IgnoreRight:
                    def1 = dActor.Status.def;
                    if (dActor is ActorMob)
                        def1 += dActor.Status.def_skill;
                    def2 = 0;
                    break;
                case DefType.IgnoreAll:
                    def1 = 0;
                    def2 = 0;
                    break;
            }

            if (res == AttackResult.Critical)
                def2 = 0;

            //damage = (int)(atk * (1.0 - (def2 * (1.0 + def1 / 100.0) * a) / (def2 * (1.0 + def1 / 100.0) * a + 1.0)));
            if (sActor.Status.Purger_Lv > 0) {
                def1 = Math.Max(0, def1 - 10 * sActor.Status.Purger_Lv);
                //def1 -= (10 * sActor.Status.Purger_Lv);
                def2 = (int)(def2 * (1.0f - sActor.Status.Purger_Lv / 10.0f));
            }

            if (def1 < 0) def1 = 0;
            if (def2 < 0) def2 = 0;
            if (dActor.type == ActorType.PC) {
                var pc = dActor as ActorPC;
                damage = (int)(atk * (1.0f - def1 / 100.0f) - def2 - (pc.Vit + pc.Status.vit_rev + pc.Status.vit_item +
                                                                      pc.Status.vit_chip + pc.Status.vit_mario +
                                                                      pc.Status.vit_skill) / 3.0f);
            }
            else {
                //这个算法经过考察是错误的.那就是说玩家攻击怪物和怪物攻击玩家应用的公式是不一样的
                //damage = (int)((float)atk * (1.0f - (float)((float)def1 / 100.0f)) - def2);
                var divright = atk > def2 ? atk - def2 : 1.0f;
                var dmgreduce = 1.0f;
                if (sActor.type == ActorType.PC && sActor.Status.DefRatioAtk)
                    dmgreduce = 1.0f + (def1 + dActor.Status.vit_skill) / 100.0f;
                else
                    dmgreduce = 1.0f - def1 / 100.0f;
                damage = (int)(divright * dmgreduce);
            }

            return damage;
        }

        /// <summary>
        ///     只计算面板左右防御的影响 不考虑任何面板外的状态和判定
        /// </summary>
        /// <param name="dActor"></param>
        /// <param name="defType">可以对魔法攻击进行物理防御判定</param>
        /// <param name="atk"></param>
        /// <param name="ignore">根据deftype无视相应防御类型的百分比值，不可超过100%，即左防50的目标ignore0.5后只计算25左防，若是右放200则计算100</param>
        /// <returns></returns>
        public int CalcMagDamage(Actor sActor, Actor dActor, DefType defType, int atk, float ignore) {
            int damage = 0, def1 = 0, def2 = 0;
            //double a = 0.008;
            switch (defType) {
                case DefType.Def:
                    def1 = dActor.Status.def;
                    if (dActor is ActorMob)
                        def1 += dActor.Status.def_skill;
                    def2 = dActor.Status.def_add;
                    if (dActor is ActorMob)
                        def2 += dActor.Status.def_add_skill;
                    break;
                case DefType.MDef:
                    def1 = dActor.Status.mdef;
                    if (dActor is ActorMob)
                        def1 += dActor.Status.mdef_skill;
                    def2 = dActor.Status.mdef_add;
                    if (dActor is ActorMob)
                        def2 += dActor.Status.mdef_add_skill;
                    break;
                case DefType.IgnoreLeft:
                    def1 = 0;
                    def2 = dActor.Status.mdef_add;
                    if (dActor is ActorMob)
                        def2 += dActor.Status.mdef_add_skill;
                    break;
                case DefType.IgnoreRight:
                    def1 = dActor.Status.mdef;
                    if (dActor is ActorMob)
                        def1 += dActor.Status.mdef_skill;
                    def2 = 0;
                    break;
                case DefType.IgnoreAll:
                    def1 = 0;
                    def2 = 0;
                    break;
                case DefType.DefIgnoreRight:
                    def1 = dActor.Status.def;
                    if (dActor is ActorMob)
                        def1 += dActor.Status.def_skill;
                    def2 = 0;
                    break;
            }

            if (sActor.Status.ForceMaster_Lv > 0 && defType == DefType.MDef) {
                var defdown = new[] { 24, 30, 36, 42, 50 };
                def1 -= defdown[sActor.Status.ForceMaster_Lv - 1];
                //def2 = (int)((float)def2 * (float)(1.0f - defdown[sActor.Status.ForceMaster_Lv - 1]));
            }

            if (def1 < 0) def1 = 0;
            if (def2 < 0) def2 = 0;
            if (dActor.type == ActorType.PC) {
                //damage = (int)(atk * (1.0 - (def2 * (1.0 + def1 / 100.0) * a) / (def2 * (1.0 + def1 / 100.0) * a + 1.0)));
                damage = (int)(atk * (1.0f - def1 / 100.0f) - def2);
            }

            else {
                var divright = atk > def2 ? atk - def2 : 1.0f;
                var dmgreduce = 1.0f - def1 / 100.0f;
                damage = (int)(divright * dmgreduce);
            }

            return damage;
        }

        private int checkPositive(float num) {
            if (num > 0)
                return (int)num;
            return 0;
        }

        //#region Enums

        public enum DefaultAdditions {
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

        //#endregion

        //#region Enums

        public enum 异常状态 {
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

        //#endregion

        /// <summary>
        ///     是否在範圍內
        /// </summary>
        /// <param name="sActor">來源Actor</param>
        /// <param name="dActor">目的Actor</param>
        /// <param name="Range">範圍</param>
        /// <returns>是否在範圍內</returns>
        public bool isInRange(Actor sActor, Actor dActor, short Range) {
            if (Math.Abs(sActor.X - dActor.X) < Range || Math.Abs(sActor.Y - dActor.Y) < Range) return true;
            return false;
        }

        /// <summary>
        ///     移動道具
        /// </summary>
        /// <param name="item">道具</param>
        public void MoveItem(Actor dActor, Item item) {
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
        public void Warp(Actor dActor, uint MapID, byte x, byte y) {
            var map = MapManager.Instance.GetMap(dActor.MapID);
            map.SendActorToMap(dActor, MapID, Global.PosX8to16(x, map.Width), Global.PosY8to16(y, map.Height));
        }

        /// <summary>
        ///     變身成怪物
        /// </summary>
        /// <param name="sActor">目標玩家</param>
        /// <param name="MobID">怪物ID (0為變回原形)</param>
        public void TranceMob(Actor sActor, uint MobID) {
            if (sActor.type == ActorType.PC) {
                var pc = (ActorPC)sActor;
                if (MobID == 0) {
                    pc.TranceID = 0;
                }
                else {
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
        public bool IsRidePet(Actor mob) {
            if (mob.type != ActorType.PET) return false;

            var p = (ActorPet)mob;
            return p.BaseData.mobType.ToString().ToUpper().IndexOf("RIDE") > 1;
        }

        /// <summary>
        ///     Actor動作
        /// </summary>
        /// <param name="dActor"></param>
        /// <param name="motion"></param>
        public void NPCMotion(Actor dActor, MotionType motion) {
            var p = new SSMG_CHAT_MOTION();
            p.ActorID = dActor.ActorID;
            p.Motion = motion;
            var map = MapManager.Instance.GetMap(dActor.MapID);
            var actors = map.GetActorsArea(dActor, 10000, true);
            foreach (var act in actors)
                if (act.type == ActorType.PC) {
                    var pc = (ActorPC)act;
                    MapClient.FromActorPC(pc).NetIo.SendPacket(p);
                }
        }

        /// <summary>
        ///     檢查DEM的右手裝備
        /// </summary>
        /// <param name="sActor"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool CheckDEMRightEquip(Actor sActor, ItemType type) {
            if (sActor.type == ActorType.PC) {
                var pc = (ActorPC)sActor;
                if (pc.Race == PC_RACE.DEM) {
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

        //#region Utility

        /// <summary>
        ///     取得被憑依的角色
        /// </summary>
        /// <param name="sActor">角色</param>
        /// <returns>被憑依者</returns>
        public ActorPC GetPossesionedActor(ActorPC sActor) {
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
        public void AttractMob(Actor sActor, Actor dActor) {
            AttractMob(sActor, dActor, 1000);
        }

        /// <summary>
        ///     吸引怪物
        /// </summary>
        /// <param name="sActor">施放技能者</param>
        /// <param name="dActor">目標</param>
        /// <param name="damage">給予的傷害</param>
        public void AttractMob(Actor sActor, Actor dActor, int damage) {
            if (dActor.type == ActorType.MOB) {
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
        public bool isBossMob(Actor mob) {
            if (mob.type != ActorType.MOB) return false;

            return isBossMob((ActorMob)mob);
        }

        /// <summary>
        ///     判斷怪物是否為Boss
        /// </summary>
        /// <param name="mob"></param>
        /// <returns></returns>
        public bool isBossMob(ActorMob mob) {
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
        public bool CanAdditionApply(Actor sActor, Actor dActor, string AdditionName, int rate) {
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
            if (sActor.Status.Additions.ContainsKey("MagHitUpCircle")) {
                var mhucb = (MagHitUpCircle.MagHitUpCircleBuff)sActor.Status.Additions["MagHitUpCircle"];
                newRate = newRate * mhucb.Rate;
            }

            if (sActor.Status.Additions.ContainsKey("AllRateUp")) {
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
        public bool CanAdditionApply(Actor sActor, Actor dActor, DefaultAdditions theAddition, int rate) {
            //怪物抗性
            if (sActor.Status.control_rate_bonus != 0)
                rate += sActor.Status.control_rate_bonus;
            if (dActor.type == ActorType.MOB) {
                var mob = (ActorMob)dActor;
                var value = (int)theAddition;
                if (value < 9) {
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
            int fixedrate = 0) {
            short res = 0;
            if (resistance != 异常状态.无) {
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
        public int AdditionApply(int hit, int avoid, int baserate, int time, int resistance, int fixedrate = 0) {
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
        public bool isEquipmentRight(Actor sActor, params ItemType[] ItemType) {
            if (sActor.type == ActorType.PC) {
                var pc = (ActorPC)sActor;
                if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND)) {
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
        public int CountItem(ActorPC pc, uint itemID) {
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
        public void TakeItem(ActorPC pc, uint itemID, ushort count) {
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
        public List<Item> GiveItem(ActorPC pc, uint itemID, ushort count, bool identified) {
            var client = MapClient.FromActorPC(pc);
            var ret = new List<Item>();
            for (var i = 0; i < count; i++) {
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
        public AutoCastInfo CreateAutoCastInfo(uint skillID, byte level, int delay) {
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
        public AutoCastInfo CreateAutoCastInfo(uint skillID, byte level, int delay, byte x, byte y) {
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
        public void FixAttack(Actor sActor, Actor dActor, SkillArg arg, Elements element, float Damage) {
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
        public void FixAttack(Actor sActor, List<Actor> dActor, SkillArg arg, Elements element, float Damage) {
            MagicAttack(sActor, dActor, arg, DefType.IgnoreAll, element, 50, Damage, 0, true);
        }


        public Dictionary<Actor, int> CalcMagicAttackWithoutDamage(Actor sActor, List<Actor> dActor, SkillArg arg,
            Elements element, float MATKBonus) {
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

            foreach (var i in dActor) {
                damage = 0;
                var target = i;
                if (i.Status == null)
                    continue;

                //NOTOUCH類MOB 跳過判定
                if (i.type == ActorType.MOB) {
                    var checkmob = (ActorMob)i;
                    switch (checkmob.BaseData.mobType) {
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

                if (i.type == ActorType.PC) {
                    var pc = (ActorPC)i;
                    if (pc.PossesionedActors.Count > 0 && pc.PossessionTarget == 0) {
                        isPossession = true;
                        isHost = true;
                    }

                    if (pc.PossessionTarget != 0) {
                        isPossession = true;
                        isHost = false;
                    }
                }

                matk = Global.Random.Next(mindamage, maxdamage);
                if (element != Elements.Neutral) {
                    var eleBonus = CalcElementBonus(sActor, i, element, 1,
                        MATKBonus < 0 && !(i.Status.undead && element == Elements.Holy));
                    matk = (int)(matk * eleBonus * MATKBonus);
                }
                else {
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
                        damage = (int)(damage * Configuration.Configuration.Instance.PVPDamageRateMagic);

                if (target.Status.Additions.ContainsKey("DamageNullify")) //boss状态
                    damage = (int)(damage * 1f);

                if (damage <= 0 && MATKBonus >= 0)
                    damage = 1;

                if (isPossession && isHost && target.Status.Additions.ContainsKey("DJoint")) {
                    var buf = (DefaultBuff)target.Status.Additions["DJoint"];
                    if (Global.Random.Next(0, 99) < buf["Rate"]) {
                        var dst = map.GetActor((uint)buf["Target"]);
                        if (dst != null) {
                            target = dst;
                            arg.affectedActors[index + counter] = target;
                        }
                    }
                }

                var ride = false;
                if (target.type == ActorType.PC) {
                    var pc = (ActorPC)target;
                    if (pc.Pet != null)
                        ride = pc.Pet.Ride;
                }

                if (sActor.type == ActorType.PC && target.type == ActorType.MOB) {
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
        public ActorPartner GetPartner(Actor sActor) {
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
        public ActorPet GetPet(Actor sActor) {
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
        public MobAI GetMobAI(Actor sActor) {
            return GetMobAI(GetPet(sActor));
        }

        /// <summary>
        ///     取得寵物AI
        /// </summary>
        /// <param name="pet">寵物</param>
        /// <returns>寵物AI</returns>
        public MobAI GetMobAI(ActorPet pet) {
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
        public bool CheckMobType(ActorMob mob, string MobType) {
            return mob.BaseData.mobType.ToString().ToLower().IndexOf(MobType.ToLower()) > -1;
        }

        /// <summary>
        ///     檢查怪物類型
        /// </summary>
        /// <param name="pet">怪物</param>
        /// <param name="type">怪物類型</param>
        /// <returns>類型是否正確</returns>
        public bool CheckMobType(ActorMob pet, params MobType[] type) {
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
        public void PossessionCancel(ActorPC pc, PossessionPosition pos) {
            if (pc.Status.Additions.ContainsKey("SoulProtect")) return;
            if (pos == PossessionPosition.NONE) {
                PossessionCancel(pc, PossessionPosition.CHEST);
                PossessionCancel(pc, PossessionPosition.LEFT_HAND);
                PossessionCancel(pc, PossessionPosition.NECK);
                PossessionCancel(pc, PossessionPosition.RIGHT_HAND);
            }
            else {
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
        public void ChangePlayerSize(ActorPC dActor, uint playersize) {
            var client = MapClient.FromActorPC(dActor);
            client.Character.Size = playersize;
            client.SendPlayerSizeUpdate();
        }

        /// <summary>
        ///     在对象位置处显示特效
        /// </summary>
        /// <param name="actor">对象</param>
        /// <param name="effectID">特效ID</param>
        public void ShowEffectByActor(Actor actor, uint effectID) {
            var map = MapManager.Instance.GetMap(actor.MapID);
            ShowEffect(map, actor, Global.PosX16to8(actor.X, map.Width), Global.PosY16to8(actor.Y, map.Height),
                effectID);
        }

        /// <summary>
        ///     在对象位置处显示特效
        /// </summary>
        /// <param name="actor">对象</param>
        /// <param name="effectID">特效ID</param>
        public void ShowEffectOnActor(Actor actor, uint effectID) {
            var map = MapManager.Instance.GetMap(actor.MapID);
            ShowEffect(map, actor, effectID);
        }

        /// <summary>
        ///     在指定对象处显示特效
        /// </summary>
        /// <param name="map">map</param>
        /// <param name="target">对象</param>
        /// <param name="effectID">特效ID</param>
        public void ShowEffect(Map map, Actor target, uint effectID) {
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
        public void ShowEffect(Map map, Actor actor, byte x, byte y, uint effectID) {
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
        public void ShowEffect(ActorPC pc, Actor target, uint effectID) {
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
        public void ShowEffect(ActorPC pc, byte x, byte y, uint effectID) {
            var arg = new EffectArg();
            arg.effectID = effectID;
            arg.actorID = 0xFFFFFFFF;
            arg.x = x;
            arg.y = y;
            var client = GetMapClient(pc);
            client.map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SHOW_EFFECT, arg, pc, true);
        }

        private MapClient GetMapClient(ActorPC pc) {
            var eh = (PCEventHandler)pc.e;
            return eh.Client;
        }

        //#endregion

        //横排防御等级
        //竖列变化等级
        private readonly float[,] DEFAULTBONUS = {
            {
                0.00f, 1.00f, 1.00f, 1.00f, 1.00f, 1.00f, 1.00f, 1.00f, 1.00f, 1.00f, 1.00f, 1.00f, 1.00f, 1.00f, 1.00f,
                1.00f, 1.00f, 1.00f, 1.00f, 1.00f, 1.00f
            }, {
                0.00f, 1.20f, 1.30f, 1.40f, 1.50f, 1.60f, 1.70f, 1.80f, 1.90f, 2.00f, 2.15f, 2.30f, 2.45f, 2.60f, 2.75f,
                2.90f, 3.05f, 3.20f, 3.35f, 3.50f, 3.80f
            }, {
                0.00f, 1.20f, 1.30f, 1.40f, 1.50f, 1.60f, 1.70f, 1.80f, 1.90f, 2.00f, 2.10f, 2.20f, 2.30f, 2.40f, 2.50f,
                2.65f, 2.80f, 2.95f, 3.10f, 3.30f, 3.50f
            }, {
                0.00f, 1.05f, 1.10f, 1.15f, 1.20f, 1.25f, 1.30f, 1.35f, 1.40f, 1.45f, 1.50f, 1.55f, 1.60f, 1.65f, 1.70f,
                1.75f, 1.80f, 1.85f, 1.90f, 1.95f, 2.00f
            }, {
                0.00f, 1.05f, 1.10f, 1.15f, 1.20f, 1.25f, 1.30f, 1.35f, 1.40f, 1.45f, 1.50f, 1.55f, 1.60f, 1.65f, 1.70f,
                1.75f, 1.80f, 1.85f, 1.90f, 1.95f, 2.00f
            }, {
                0.00f, 0.90f, 0.80f, 0.70f, 0.60f, 0.50f, 0.40f, 0.30f, 0.20f, 0.10f, 0.00f, 0.00f, 0.00f, 0.00f, 0.00f,
                0.00f, 0.00f, 0.00f, 0.00f, 0.00f, 0.00f
            }, {
                0.00f, 0.95f, 0.90f, 0.85f, 0.80f, 0.75f, 0.70f, 0.65f, 0.60f, 0.55f, 0.50f, 0.45f, 0.40f, 0.35f, 0.30f,
                0.25f, 0.20f, 0.15f, 0.10f, 0.05f, 0.00f
            }, {
                0.00f, 0.97f, 0.94f, 0.91f, 0.88f, 0.85f, 0.82f, 0.79f, 0.76f, 0.73f, 0.70f, 0.67f, 0.64f, 0.61f, 0.58f,
                0.55f, 0.52f, 0.49f, 0.46f, 0.43f, 0.40f
            }, {
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

        private readonly int[,] EFtype = {
            { 0, 0, 0, 0, 0, 0, 0 },
            { 0, 5, 2, 6, 0, 3, 7 },
            { 0, 6, 5, 0, 2, 3, 7 },
            { 0, 2, 0, 5, 5, 3, 7 },
            { 0, 0, 6, 2, 5, 3, 7 },
            { 0, 6, 6, 6, 6, 5, 1 },
            { 0, 4, 4, 4, 4, 6, 5 }
        };

        public int fieldelements(Map map, byte x, byte y, Elements eletype) {
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

        private int bonustype(Elements src, Elements dst) {
            return EFtype[(int)src, (int)dst];
        }

        private float defaultbonus(int defincelevel, int attacktype) {
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
        private float efcal(Actor sActor, Actor dActor, Elements skillelement, int skilltype, bool heal) {
            Map map;
            byte dx, dy, sx, sy;
            var res = 1f;

            //#region Calc Attacker and Defincer Coordinate

            // Attacker and Defincer Must be in the same map
            map = MapManager.Instance.GetMap(dActor.MapID);

            //Attacker Coordinate
            sx = Global.PosX16to8(sActor.X, map.Width);
            sy = Global.PosY16to8(sActor.Y, map.Height);

            //Defincer Coordinate
            dx = Global.PosX16to8(dActor.X, map.Width);
            dy = Global.PosY16to8(dActor.Y, map.Height);

            //#endregion

            //#region Calc Attack and Defince Element

            var attackElement = Elements.Neutral;
            var defineElement = Elements.Neutral;
            var atkValue = 0;
            var defValue = 0;

            GetElementResult getAtkElementResult = GetAttackElement(sActor, atkValue, map, sx, sy);
            attackElement = getAtkElementResult.Elements;
            atkValue = getAtkElementResult.Value;
            GetElementResult getDefElementResult = GetDefElement(dActor, defValue, map, dx, dy);
            defineElement = getDefElementResult.Elements;
            defValue = getDefElementResult.Value;

            if (sActor.type == ActorType.MOB) {
                attackElement = Elements.Neutral;
                atkValue = 100;
            }

            if (skilltype == 1) {
                if (skillelement != Elements.Neutral) {
                    attackElement = skillelement;
                    atkValue = 100 + sActor.AttackElements[skillelement] +
                               sActor.Status.attackElements_item[skillelement] +
                               sActor.Status.attackElements_skill[skillelement] +
                               sActor.Status.attackelements_iris[skillelement] +
                               fieldelements(map, sx, sy, skillelement);
                }
            }
            else {
                if (skillelement != Elements.Neutral) {
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

            if (sActor.type == ActorType.PC) {
                var pc = (ActorPC)sActor;
                if (pc.Skills2_1.ContainsKey(939) || pc.DualJobSkills.Exists(x => x.ID == 939)) //地
                {
                    //这里取副职的契约等级
                    var duallv = 0;
                    if (pc.DualJobSkills.Exists(x => x.ID == 939))
                        duallv = pc.DualJobSkills.FirstOrDefault(x => x.ID == 939).Level;

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

                if (pc.Skills2_1.ContainsKey(936) || pc.DualJobSkills.Exists(x => x.ID == 936)) //火
                {
                    //这里取副职的契约等级
                    var duallv = 0;
                    if (pc.DualJobSkills.Exists(x => x.ID == 936))
                        duallv = pc.DualJobSkills.FirstOrDefault(x => x.ID == 936).Level;

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

                if (pc.Skills2_1.ContainsKey(937) || pc.DualJobSkills.Exists(x => x.ID == 937)) //水
                {
                    //这里取副职的契约等级
                    var duallv = 0;
                    if (pc.DualJobSkills.Exists(x => x.ID == 937))
                        duallv = pc.DualJobSkills.FirstOrDefault(x => x.ID == 937).Level;

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

                if (pc.Skills2_1.ContainsKey(938) || pc.DualJobSkills.Exists(x => x.ID == 938)) //风
                {
                    //这里取副职的契约等级
                    var duallv = 0;
                    if (pc.DualJobSkills.Exists(x => x.ID == 938))
                        duallv = pc.DualJobSkills.FirstOrDefault(x => x.ID == 938).Level;

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

                if (pc.Skills2_1.ContainsKey(940) || pc.DualJobSkills.Exists(x => x.ID == 940)) //光
                {
                    //这里取副职的契约等级
                    var duallv = 0;
                    if (pc.DualJobSkills.Exists(x => x.ID == 940))
                        duallv = pc.DualJobSkills.FirstOrDefault(x => x.ID == 940).Level;

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

                if (pc.Skills2_1.ContainsKey(941) || pc.DualJobSkills.Exists(x => x.ID == 941)) //暗
                {
                    //这里取副职的契约等级
                    var duallv = 0;
                    if (pc.DualJobSkills.Exists(x => x.ID == 941))
                        duallv = pc.DualJobSkills.FirstOrDefault(x => x.ID == 941).Level;

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

            //#endregion

            //#region CalcElementFactor

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

            if (heal) {
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
            else {
                var elementbonustype = bonustype(attackElement, defineElement);


                Factor = GetElementFactor(atkValue, defValue, elementbonustype, Factor);

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

            //#endregion

            return res;
        }

        private float GetElementFactor(int atkValue, int defValue, int type, float Factor) {
            int deflevel = GetDefElementLevel(defValue);
            Factor = defaultbonus(deflevel, type);
            if (type > 0 && type < 5) {
                if (type == 4)
                    Factor += atkValue / 400.0f;
                else
                    Factor += atkValue / 100.0f;
            }

            return Factor;
        }


        private short GetDefElementLevel(int DefinceValue) {
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

        private GetElementResult GetAttackElement(Actor sActor, int atkvalue, Map map, byte x, byte y) {
            var ele = Elements.Neutral;

            if (sActor.type == ActorType.PC) {
                ele = sActor.WeaponElement;
                atkvalue = sActor.Status.attackElements_item[sActor.WeaponElement]
                           + sActor.Status.attackElements_skill[sActor.WeaponElement]
                           + sActor.Status.attackelements_iris[sActor.WeaponElement];
            }
            else {
                foreach (var item in sActor.AttackElements)
                    if (atkvalue < item.Value + sActor.Status.attackElements_skill[item.Key]) {
                        ele = item.Key;
                        atkvalue = item.Value + sActor.Status.attackElements_skill[item.Key];
                    }
            }

            atkvalue += fieldelements(map, x, y, ele);
            return new GetElementResult(ele, atkvalue);
        }

        private GetElementResult GetDefElement(Actor dActor, int defvalue, Map map, byte x, byte y) {
            var ele = Elements.Neutral;


            if (dActor.type == ActorType.PC) {
                ele = dActor.ShieldElement;
                defvalue = dActor.Status.elements_item[dActor.ShieldElement]
                           + dActor.Status.elements_skill[dActor.ShieldElement]
                           + dActor.Status.elements_iris[dActor.ShieldElement];
            }
            else {
                foreach (var item in dActor.Elements)
                    if (defvalue < item.Value + dActor.Status.elements_skill[item.Key]) {
                        defvalue = item.Value + dActor.Status.elements_skill[item.Key];
                        ele = item.Key;
                    }
            }

            defvalue += fieldelements(map, x, y, ele);

            if (dActor.Status.Additions.ContainsKey("WaterFrosenElement")) {
                ele = Elements.Water;
                defvalue = 100;
            }

            if (dActor.Status.Additions.ContainsKey("StoneFrosenElement")) {
                ele = Elements.Earth;
                defvalue = 100;
            }

            return new GetElementResult(ele, defvalue);
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
        private float CalcElementBonus(Actor sActor, Actor dActor, Elements element, int skilltype, bool heal) {
            return efcal(sActor, dActor, element, skilltype, heal);
        }

        private List<Actor> ProcessAttackPossession(Actor dActor) {
            var list = new List<Actor>();
            if (dActor.type != ActorType.PC)
                return list;
            var pc = (ActorPC)dActor;
            foreach (var i in pc.PossesionedActors) {
                if (!i.Online)
                    continue;
                if (Global.Random.Next(0, 99) < i.Status.possessionCancel)
                    continue;
                switch (i.PossessionPosition) {
                    case PossessionPosition.NECK:
                        if (Global.Random.Next(0, 99) < 8)
                            list.Add(i);
                        break;
                    case PossessionPosition.CHEST:
                        if (Global.Random.Next(0, 99) < 12)
                            list.Add(i);
                        break;
                    case PossessionPosition.RIGHT_HAND:
                        if (Global.Random.Next(0, 99) < 15)
                            list.Add(i);
                        break;
                    case PossessionPosition.LEFT_HAND:
                        if (Global.Random.Next(0, 99) < 18)
                            list.Add(i);
                        break;
                }
            }

            return list;
        }

        public enum DefType {
            Def,
            MDef,
            IgnoreAll,
            IgnoreLeft,
            IgnoreRight,
            MDefIgnoreLeft,
            MdefIgnoreRight,
            DefIgnoreLeft,
            DefIgnoreRight
        }

        internal int magicalCounter = 0;
        internal int physicalCounter = 0;
        private ActorPC thispc = new ActorPC();
        private Elements Toelements = Elements.Neutral;

        /// <summary>
        ///     对单一目标进行物理攻击
        /// </summary>
        /// <param name="sActor">原目标</param>
        /// <param name="dActor">对象目标</param>
        /// <param name="arg">技能参数</param>
        /// <param name="element">元素</param>
        /// <param name="ATKBonus">攻击加成</param>
        public int PhysicalAttack(Actor sActor, Actor dActor, SkillArg arg, Elements element, float ATKBonus) {
            var list = new List<Actor>();
            list.Add(dActor);
            return PhysicalAttack(sActor, list, arg, element, ATKBonus);
        }

        /// <summary>
        ///     对多个目标进行物理攻击
        /// </summary>
        /// <param name="sActor">原目标</param>
        /// <param name="dActor">对象目标集合</param>
        /// <param name="arg">技能参数</param>
        /// <param name="element">元素</param>
        /// <param name="ATKBonus">攻击加成</param>
        public int PhysicalAttack(Actor sActor, List<Actor> dActor, SkillArg arg, Elements element, float ATKBonus) {
            return PhysicalAttack(sActor, dActor, arg, element, 0, ATKBonus);
        }

        /// <summary>
        ///     对多个目标进行物理攻击
        /// </summary>
        /// <param name="sActor">原目标</param>
        /// <param name="dActor">对象目标集合</param>
        /// <param name="arg">技能参数</param>
        /// <param name="element">元素</param>
        /// <param name="index">arg中参数偏移</param>
        /// <param name="ATKBonus">攻击加成</param>
        public int PhysicalAttack(Actor sActor, List<Actor> dActor, SkillArg arg, Elements element, int index,
            float ATKBonus) {
            //if(index >0)
            /*if (dActor.Count > 12)
            {
                int ACount = dActor.Count;
                for (int i = 12; i < ACount; i++)
                {
                    dActor.RemoveAt(12);
                }
            }*/
            return PhysicalAttack(sActor, dActor, arg, DefType.Def, element, index, ATKBonus, false);
        }

        /// <summary>
        ///     对多个目标进行物理攻击
        /// </summary>
        /// <param name="sActor">原目标</param>
        /// <param name="dActor">对象目标集合</param>
        /// <param name="arg">技能参数</param>
        /// <param name="element">元素</param>
        /// <param name="index">arg中参数偏移</param>
        /// <param name="defType">使用的防御类型</param>
        /// <param name="ATKBonus">攻击加成</param>
        public int PhysicalAttack(Actor sActor, List<Actor> dActor, SkillArg arg, DefType defType, Elements element,
            int index, float ATKBonus, bool setAtk) {
            return PhysicalAttack(sActor, dActor, arg, defType, element, index, ATKBonus, setAtk, 0, false);
        }

        public int PhysicalAttack(Actor sActor, List<Actor> dActor, SkillArg arg, DefType defType, Elements element,
            int index, float ATKBonus, bool setAtk, float SuckBlood, bool doublehate) {
            return PhysicalAttack(sActor, dActor, arg, defType, element, index, ATKBonus, setAtk, SuckBlood, doublehate,
                0, 0);
        }

        public int PhysicalAttack(Actor sActor, List<Actor> dActor, SkillArg arg, DefType defType, Elements element,
            int index, float ATKBonus, bool setAtk, float SuckBlood, bool doublehate, int shitbonus, int scribonus) {
            return PhysicalAttack(sActor, dActor, arg, defType, element, index, ATKBonus, setAtk, SuckBlood, doublehate,
                shitbonus, scribonus, "noeffect", 0);
        }

        public int PhysicalAttack(Actor sActor, List<Actor> dActor, SkillArg arg, DefType defType, Elements element,
            int index, float ATKBonus, bool setAtk, float SuckBlood, bool doublehate, int shitbonus, int scribonus,
            string effectname, int lifetime, float ignore = 0) {
            return PhysicalAttack(sActor, dActor, arg, defType, element, index, ATKBonus, setAtk, SuckBlood, doublehate,
                shitbonus, scribonus, 0, "noeffect", 0);
        }

        /// <summary>
        ///     对多个目标进行物理攻击
        /// </summary>
        /// <param name="sActor">原目标</param>
        /// <param name="dActor">对象目标集合</param>
        /// <param name="arg">技能参数</param>
        /// <param name="element">元素</param>
        /// <param name="index">arg中参数偏移</param>
        /// <param name="defType">使用的防御类型</param>
        /// <param name="ATKBonus">攻击加成</param>
        /// <param name="ignore">无视防御比</param>
        public int PhysicalAttack(Actor sActor, List<Actor> dActor, SkillArg arg, DefType defType, Elements element,
            int index, float ATKBonus, bool setAtk, float SuckBlood, bool doublehate, int shitbonus, int scribonus,
            int cridamagebonus, string effectname, int lifetime, float ignore = 0) {
            if (dActor.Count == 0) return 0;
            if (sActor.Status == null)
                return 0;
            if (sActor.Status.Additions.ContainsKey("ArmBreak") && arg.skill != null) //断腕击状态无法使用物理技能
            {
                if (sActor.type == ActorType.PC) {
                    var pc = (ActorPC)sActor;
                    MapClient.FromActorPC(pc).SendSystemMessage("目前处于物理技能封印状态");
                }

                return 0;
            }

            if (sActor.type == ActorType.PC && arg.skill == null) //要求不是技能
            {
                var pc = (ActorPC)sActor;
                if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND)) {
                    if (pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.BOW)
                        if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.LEFT_HAND))
                            if (pc.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].BaseData.itemType == ItemType.ARROW)
                                if (pc.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].Stack > 0)
                                    GetlongARROW(pc, arg);
                    //MapClient.FromActorPC(pc).DeleteItem(pc.Inventory.Equipments[SagaDB.Item.EnumEquipSlot.LEFT_HAND].Slot, 1, false);
                    if (pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.GUN ||
                        pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.DUALGUN ||
                        pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.RIFLE)
                        if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.LEFT_HAND))
                            if (pc.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].BaseData.itemType == ItemType.BULLET)
                                if (pc.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].Stack > 0)
                                    GetlongARROW(pc, arg);
                    //MapClient.FromActorPC(pc).DeleteItem(pc.Inventory.Equipments[SagaDB.Item.EnumEquipSlot.LEFT_HAND].Slot, 1, false);
                }
            }

            if (dActor.Count > 10) {
                foreach (var item in dActor)
                    DoDamage(true, sActor, item, arg, defType, element, index, ATKBonus, scribonus, cridamagebonus);
                return 0;
            }

            var damage = 0;

            int atk;
            var mindamage = 0;
            var maxdamage = 0;
            var counter = 0;
            var map = MapManager.Instance.GetMap(dActor[0].MapID);

            if (index == 0) {
                arg.affectedActors = new List<Actor>();
                foreach (var i in dActor)
                    arg.affectedActors.Add(i);
                arg.Init();
            }

            switch (arg.type) {
                case ATTACK_TYPE.BLOW:
                    mindamage = sActor.Status.min_atk1;
                    maxdamage = sActor.Status.max_atk1;
                    break;
                case ATTACK_TYPE.SLASH:
                    mindamage = sActor.Status.min_atk2;
                    maxdamage = sActor.Status.max_atk2;
                    break;
                case ATTACK_TYPE.STAB:
                    mindamage = sActor.Status.min_atk3;
                    maxdamage = sActor.Status.max_atk3;
                    break;
            }


            //if (sActor.Status.Additions.ContainsKey("破戒"))
            //{
            //    mindamage = sActor.Status.min_matk;
            //    maxdamage = sActor.Status.max_matk;
            //}
            if (mindamage > maxdamage) maxdamage = mindamage;
            foreach (var i in dActor) {
                if (i.Status == null)
                    continue;
                if (i.type == ActorType.ITEM)
                    continue;
                //NOTOUCH類MOB 跳過判定
                if (i.type == ActorType.MOB) {
                    var checkmob = (ActorMob)i;
                    switch (checkmob.BaseData.mobType) {
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

                //投掷武器
                if (sActor.type == ActorType.PC) {
                    var pc = (ActorPC)sActor;

                    if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND))
                        if (pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.THROW ||
                            pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.CARD) {
                            if (pc.Skills3.ContainsKey(989) || pc.DualJobSkills.Exists(x => x.ID == 989)) {
                                if (pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType ==
                                    ItemType.CARD &&
                                    pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].Stack > 0) {
                                    var duallv = 0;
                                    if (pc.DualJobSkills.Exists(x => x.ID == 989))
                                        duallv = pc.DualJobSkills.FirstOrDefault(x => x.ID == 989).Level;

                                    var mainlv = 0;
                                    if (pc.Skills3.ContainsKey(989))
                                        mainlv = pc.Skills3[989].Level;

                                    var maxlv = Math.Max(duallv, mainlv);
                                    if (arg.skill == null) //普通攻击
                                    {
                                        if (Global.Random.Next(0, 99) > maxlv * 5) GetlongCARD(pc, arg);
                                        //MapClient.FromActorPC(pc).DeleteItem(pc.Inventory.Equipments[SagaDB.Item.EnumEquipSlot.RIGHT_HAND].Slot, 1, false);
                                    }
                                    else if (arg.skill.ID == 2517 || arg.skill.ID == 2518) //ストレートフラッシュ
                                    {
                                        if (Global.Random.Next(0, 99) > maxlv * 3) GetlongCARD(pc, arg);
                                        //MapClient.FromActorPC(pc).DeleteItem(pc.Inventory.Equipments[SagaDB.Item.EnumEquipSlot.RIGHT_HAND].Slot, 1, false);
                                    }
                                }
                            }
                            else {
                                if (pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].Stack > 0)
                                    MapClient.FromActorPC(pc)
                                        .DeleteItem(pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].Slot, 1, false);
                            }
                        }
                }

                ////弓箭，枪
                //if (sActor.type == ActorType.PC)
                //{
                //    ActorPC pc = (ActorPC)sActor;
                //    if (pc.Inventory.Equipments.ContainsKey(SagaDB.Item.EnumEquipSlot.RIGHT_HAND))
                //    {
                //        if (pc.Inventory.Equipments[SagaDB.Item.EnumEquipSlot.RIGHT_HAND].BaseData.itemType == SagaDB.Item.ItemType.BOW)
                //        {
                //            if (pc.Inventory.Equipments.ContainsKey(SagaDB.Item.EnumEquipSlot.LEFT_HAND))
                //            {
                //                if (pc.Inventory.Equipments[SagaDB.Item.EnumEquipSlot.LEFT_HAND].BaseData.itemType == SagaDB.Item.ItemType.ARROW)
                //                {
                //                    if (pc.Inventory.Equipments[SagaDB.Item.EnumEquipSlot.LEFT_HAND].Stack > 0)
                //                    {
                //                        GetlongARROW(pc, arg);
                //                        //MapClient.FromActorPC(pc).DeleteItem(pc.Inventory.Equipments[SagaDB.Item.EnumEquipSlot.LEFT_HAND].Slot, 1, false);
                //                    }

                //                }
                //                else
                //                {
                //                    if (counter == 0)
                //                        arg.result = -1;
                //                    continue;
                //                }
                //            }
                //            else
                //            {
                //                if (counter == 0)
                //                    arg.result = -1;
                //                continue;
                //            }
                //        }
                //        if (pc.Inventory.Equipments[SagaDB.Item.EnumEquipSlot.RIGHT_HAND].BaseData.itemType == SagaDB.Item.ItemType.GUN ||
                //            pc.Inventory.Equipments[SagaDB.Item.EnumEquipSlot.RIGHT_HAND].BaseData.itemType == SagaDB.Item.ItemType.DUALGUN ||
                //            pc.Inventory.Equipments[SagaDB.Item.EnumEquipSlot.RIGHT_HAND].BaseData.itemType == SagaDB.Item.ItemType.RIFLE)
                //        {
                //            if (pc.Inventory.Equipments.ContainsKey(SagaDB.Item.EnumEquipSlot.LEFT_HAND))
                //            {
                //                if (pc.Inventory.Equipments[SagaDB.Item.EnumEquipSlot.LEFT_HAND].BaseData.itemType == SagaDB.Item.ItemType.BULLET)
                //                {
                //                    if (pc.Inventory.Equipments[SagaDB.Item.EnumEquipSlot.LEFT_HAND].Stack > 0)
                //                        GetlongARROW(pc, arg);
                //                    //MapClient.FromActorPC(pc).DeleteItem(pc.Inventory.Equipments[SagaDB.Item.EnumEquipSlot.LEFT_HAND].Slot, 1, false);
                //                }
                //                else
                //                {
                //                    if (counter == 0)
                //                        arg.result = -1;
                //                    continue;
                //                }
                //            }
                //            else
                //            {
                //                if (counter == 0)
                //                    arg.result = -1;
                //                continue;
                //            }
                //        }
                //    }
                //}
                //判断命中结果
                //short dis = Map.Distance(sActor, i);
                //if (arg.argType == SkillArg.ArgType.Active)
                //    shitbonus = 50;

                var res = CalcAttackResult(sActor, i, sActor.Range > 3, shitbonus, scribonus);
                if (i.Status.Additions.ContainsKey("Warn")) //警戒
                    if (res == AttackResult.Critical)
                        res = AttackResult.Hit;

                if (sActor.Status.Additions.ContainsKey("PerfectRiotStamp"))
                    if (arg.skill != null && arg.skill.ID != 0)
                        if (arg.skill.ID == 2180)
                            res = AttackResult.Hit;

                if (sActor.Status.Additions.ContainsKey("Super_A_T_PJoint"))
                    if (arg.skill != null && arg.skill.ID != 0) {
                        RemoveAddition(sActor, "Super_A_T_PJoint");
                        res = AttackResult.Critical;
                    }

                var target = i;

                if (res == AttackResult.Miss || res == AttackResult.Avoid || res == AttackResult.Guard ||
                    res == AttackResult.Parry) {
                    if (res == AttackResult.Miss)
                        arg.flag[index + counter] = AttackFlag.MISS;
                    else if (res == AttackResult.Avoid)
                        arg.flag[index + counter] = AttackFlag.AVOID;
                    else if (res == AttackResult.Parry)
                        arg.flag[index + counter] = AttackFlag.AVOID2;
                    else
                        arg.flag[index + counter] = AttackFlag.GUARD;

                    try {
                        var y = "普通攻击";
                        if (arg != null)
                            if (arg.skill != null)
                                y = arg.skill.Name;
                        //string s = "物理伤害";
                        SendAttackMessage(2, target, "从 " + sActor.Name + " 处的 " + y + "", "被你 " + res);
                        SendAttackMessage(3, sActor, "你的 " + y + " 对 " + target.Name + "", "被 " + res);
                    }
                    catch (Exception ex) {
                        Logger.GetLogger().Error(ex, ex.Message);
                    }
                }
                else {
                    var restKryrie = 0;
                    if (i.type == ActorType.PC) {
                        var me = (ActorPC)i;
                        if (me.Skills2_2.ContainsKey(956) || me.DualJobSkills.Exists(x => x.ID == 956)) //二段斩击，已激活副职判定
                        {
                            var duallv = 0;
                            if (me.DualJobSkills.Exists(x => x.ID == 956))
                                duallv = me.DualJobSkills.FirstOrDefault(x => x.ID == 956).Level;

                            var mainlv = 0;
                            if (me.Skills2_2.ContainsKey(956))
                                mainlv = me.Skills2_2[956].Level;


                            var TotalLv = Math.Max(duallv, mainlv);
                            var nr = Global.Random.Next(0, 1000);
                            if (TotalLv * 20 > nr)
                                if (i.Status.Additions.ContainsKey("ConSword")) {
                                    arg.flag[index + counter] = AttackFlag.HP_DAMAGE | AttackFlag.NO_DAMAGE;
                                    var arg2 = new EffectArg();
                                    arg2.effectID = 4173;
                                    arg2.actorID = i.ActorID;
                                    map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SHOW_EFFECT, arg2, i, true);
                                    restKryrie = 1;
                                }
                        }
                    }

                    if (i.Status.Additions.ContainsKey("MobKyrie")) {
                        var buf = (DefaultBuff)i.Status.Additions["MobKyrie"];
                        restKryrie = buf["MobKyrie"];
                        arg.flag[index + counter] = AttackFlag.HP_DAMAGE | AttackFlag.NO_DAMAGE;
                        if (restKryrie > 0) {
                            buf["MobKyrie"]--;
                            var arg2 = new EffectArg();
                            arg2.effectID = 4173;
                            arg2.actorID = i.ActorID;
                            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SHOW_EFFECT, arg2, i, true);
                            if (restKryrie == 1) {
                                RemoveAddition(i, "MobKyrie");
                                RemoveAddition(i, "KyrieEleison");
                            }
                        }
                    }

                    if (i.Status.Additions.ContainsKey("ArtFullTrap1")) {
                        var healnum = i.Status.max_matk_bs + i.Status.max_atk_bs;
                        i.HP += (uint)healnum;
                        if (i.HP > i.MaxHP)
                            i.HP = i.MaxHP;
                        Instance.ShowVessel(i, -healnum);
                        //SkillHandler.RemoveAddition(i, "ArtFullTrap1");
                    }

                    if (i.Status.Additions.ContainsKey("ArtFullTrap2")) {
                        var healnum = (int)((i.Status.max_matk_bs + i.Status.max_atk_bs) / 5.0f);
                        i.SP += (uint)healnum;
                        if (i.SP > i.MaxSP)
                            i.SP = i.MaxSP;
                        Instance.ShowVessel(i, 0, 0, -healnum);
                        //SkillHandler.RemoveAddition(i, "ArtFullTrap2");
                    }

                    if (i.Status.Additions.ContainsKey("ArtFullTrap3")) {
                        var healnum = (int)((i.Status.max_matk_bs + i.Status.max_atk_bs) / 5.0f);
                        i.MP += (uint)healnum;
                        if (i.MP > i.MaxMP)
                            i.MP = i.MaxMP;
                        Instance.ShowVessel(i, 0, -healnum);
                        //SkillHandler.RemoveAddition(i, "ArtFullTrap3");
                    }

                    //if (i.Status.Additions.ContainsKey("ArtFullTrap4"))
                    //{
                    //    arg.argType = SkillArg.ArgType.Attack;
                    //    PhysicalAttack(i, sActor, arg, i.WeaponElement, 1.0f);
                    //    SkillArg args = new SkillArg();
                    //    args.skill = SagaDB.Skill.SkillFactory.Instance.GetSkill(2552, 4);
                    //    Poison skill = new Poison(args.skill, sActor, 7000);
                    //    //SkillHandler.RemoveAddition(i, "ArtFullTrap4");
                    //}
                    //if (i.Status.Additions.ContainsKey("ArtFullTrap5"))
                    //{
                    //    SkillArg args = new SkillArg();
                    //    args.skill = SagaDB.Skill.SkillFactory.Instance.GetSkill(2478, 5);
                    //    //SagaDB.Skill.SkillFactory.Instance.GetSkill(2478, 5);
                    //    //args.type = ATTACK_TYPE.SLASH;
                    //    //AutoCastInfo info = new AutoCastInfo();
                    //    //info.skillID = 2478;
                    //    //args.autoCast.Add(info);
                    //    args.autoCast.Add(SkillHandler.Instance.CreateAutoCastInfo(2478, 5, 0));
                    //    //SkillHandler.RemoveAddition(i, "ArtFullTrap5");
                    //}
                    if (restKryrie == 0) {
                        var isPossession = false;
                        var isHost = false;
                        if (i.type == ActorType.PC) {
                            var pc = (ActorPC)i;
                            if (pc.PossesionedActors.Count > 0 && pc.PossessionTarget == 0) {
                                isPossession = true;
                                isHost = true;
                            }

                            if (pc.PossessionTarget != 0) {
                                isPossession = true;
                                isHost = false;
                            }
                        }

                        //处理凭依伤害
                        if (isHost && isPossession && ATKBonus > 0) {
                            var possessionDamage = ProcessAttackPossession(i);
                            if (possessionDamage.Count > 0) {
                                arg.Remove(i);
                                var oldcount = arg.flag.Count;
                                arg.Extend(possessionDamage.Count);
                                foreach (var j in possessionDamage)
                                    if (Global.Random.Next(0, 99) < i.Status.possessionTakeOver)
                                        arg.affectedActors.Add(i);
                                    else
                                        arg.affectedActors.Add(j);
                                PhysicalAttack(sActor, possessionDamage, arg, element, oldcount, ATKBonus);
                                continue;
                            }
                        }

                        if (!setAtk) {
                            atk = Global.Random.Next(mindamage, maxdamage);
                            //TODO: element bonus, range bonus
                            var eleBonus = CalcElementBonus(sActor, i, element, 0, false);

                            if (i.Status.Contract_Lv != 0) {
                                var tmpele = Elements.Neutral;
                                switch (i.Status.Contract_Lv) {
                                    case 1:
                                        tmpele = Elements.Fire;
                                        break;
                                    case 2:
                                        tmpele = Elements.Water;
                                        break;
                                    case 3:
                                        tmpele = Elements.Earth;
                                        break;
                                    case 4:
                                        tmpele = Elements.Wind;
                                        break;
                                }

                                if (tmpele == element)
                                    eleBonus -= 0.15f;
                                else
                                    eleBonus += 1.0f;
                            }

                            if ((sActor.Status.Additions.ContainsKey("EvilSoul") ||
                                 sActor.Status.Additions.ContainsKey("SoulTaker")) && element == Elements.Dark &&
                                eleBonus > 0 &&
                                (arg.skill.ID == 2537 || //黑暗之光
                                 arg.skill.ID == 2230 || //吸收灵魂
                                 arg.skill.ID == 2229 //死神之刃
                                )) //邪恶灵魂仅对物理攻击本身为暗属性的技能有效
                            {
                                if (sActor.Status.Additions.ContainsKey("EvilSoul"))
                                    //atkValue += (sActor.Status.Additions["EvilSoul"] as DefaultBuff).Variable["EvilSoul"];
                                    eleBonus +=
                                        (sActor.Status.Additions["EvilSoul"] as DefaultBuff).Variable["EvilSoul"] /
                                        100.0f;
                                if (sActor.Status.Additions.ContainsKey("SoulTaker") && arg.skill != null &&
                                    arg.skill.ID != 0)
                                    //atkValue += (sActor.Status.Additions["SoulTaker"] as DefaultBuff).Variable["SoulTaker"];
                                    eleBonus +=
                                        (sActor.Status.Additions["SoulTaker"] as DefaultBuff).Variable["SoulTaker"] /
                                        100.0f;
                            }

                            atk = (int)Math.Ceiling(atk * eleBonus * ATKBonus);
                            /*
                            if (i.Buff.Frosen == true && element == Elements.Fire)
                            {
                                RemoveAddition(i, i.Status.Additions["WaterFrosenElement"]);
                            }
                            if (i.Buff.Stone == true && element == Elements.Water)
                            {
                                RemoveAddition(i, i.Status.Additions["StoneFrosenElement"]);
                            }
                            */
                            if (arg.skill != null)
                                if (sActor.Status.doubleUpList.Contains((ushort)arg.skill.ID))
                                    atk *= 2;
                        }
                        else {
                            atk = (int)ATKBonus;
                        }

                        damage = CalcPhyDamage(sActor, i, defType, atk, ignore, res);

                        //if (damage > atk)
                        //    damage = atk;

                        //IStats stats = (IStats)i;
                        switch (arg.type) {
                            case ATTACK_TYPE.BLOW:
                                damage = (int)(damage * (1f - i.Status.damage_atk1_discount));
                                break;
                            case ATTACK_TYPE.SLASH:
                                damage = (int)(damage * (1f - i.Status.damage_atk2_discount));
                                break;
                            case ATTACK_TYPE.STAB:
                                damage = (int)(damage * (1f - i.Status.damage_atk3_discount));
                                break;
                        }


                        if (sActor.type == ActorType.PC && target.type == ActorType.PC)
                            damage = (int)(damage * Configuration.Configuration.Instance.PVPDamageRatePhysic);

                        if (damage <= 0) damage = 1;


                        if (isPossession && isHost && target.Status.Additions.ContainsKey("DJoint")) {
                            var buf = (DefaultBuff)target.Status.Additions["DJoint"];
                            if (Global.Random.Next(0, 99) < buf["Rate"]) {
                                var dst = map.GetActor((uint)buf["Target"]);
                                if (dst != null) {
                                    target = dst;
                                    arg.affectedActors[index + counter] = target;
                                }
                            }
                        }


                        //计算暴击增益
                        if (res == AttackResult.Critical) {
                            damage = (int)(damage * (CalcCritBonus(sActor, target, scribonus) + cridamagebonus));
                            if (sActor.Status.Additions.ContainsKey("CriDamUp")) {
                                var rate =
                                    (sActor.Status.Additions["CriDamUp"] as DefaultPassiveSkill).Variable["CriDamUp"] /
                                    100.0f + 1.0f;
                                damage = (int)(damage * rate);
                            }
                        }

                        //宠判定？
                        var ride = false;
                        if (target.type == ActorType.PC) {
                            var pc = (ActorPC)target;
                            if (pc.Pet != null)
                                ride = pc.Pet.Ride;
                        }

                        //宠物成长
                        if (res == AttackResult.Critical) {
                            if (sActor.type == ActorType.PET)
                                ProcessPetGrowth(sActor, PetGrowthReason.CriticalHit);
                            if (i.type == ActorType.PET && damage > 0)
                                ProcessPetGrowth(i, PetGrowthReason.PhysicalBeenHit);
                            if (i.type == ActorType.PC && damage > 0) {
                                var pc = (ActorPC)target;

                                if (ride) ProcessPetGrowth(pc.Pet, PetGrowthReason.PhysicalBeenHit);
                            }
                        }
                        else {
                            if (sActor.type == ActorType.PET)
                                ProcessPetGrowth(sActor, PetGrowthReason.PhysicalHit);
                            if (i.type == ActorType.PET && damage > 0)
                                ProcessPetGrowth(i, PetGrowthReason.PhysicalBeenHit);
                            if (i.type == ActorType.PC && damage > 0) {
                                var pc = (ActorPC)target;

                                if (ride) ProcessPetGrowth(pc.Pet, PetGrowthReason.PhysicalBeenHit);
                            }
                        }

                        //技能以及状态判定
                        if (sActor.type == ActorType.PC) {
                            var pcsActor = (ActorPC)sActor;
                            if (sActor.Status.Additions
                                .ContainsKey(
                                    "BurnRate")) // && SkillHandler.Instance.isEquipmentRight(pcsActor, SagaDB.Item.ItemType.CARD))//皇家贸易商
                                //副职不存在3371技能,不进行副职逻辑判定
                                if (pcsActor.Skills3.ContainsKey(3371))
                                    if (pcsActor.Skills3[3371].Level > 1) {
                                        int[] gold = { 0, 0, 100, 250, 500, 1000 };
                                        if (pcsActor.Gold > gold[pcsActor.Skills3[3371].Level]) {
                                            pcsActor.Gold -= gold[pcsActor.Skills3[3371].Level];
                                            damage += gold[pcsActor.Skills3[3371].Level];
                                        }
                                    }
                        }
                        //if (sActor.type == ActorType.PC && target.type == ActorType.MOB)
                        //{
                        //    ActorMob mob = (ActorMob)target;
                        //    if (mob.BaseData.mobType.ToString().Contains("CHAMP") && !sActor.Buff.StateOfMonsterKillerChamp)
                        //        damage = damage / 10;
                        //}

                        //if (sActor.type == ActorType.PC)
                        //{
                        //    int score = damage / 100;
                        //    if (score == 0)
                        //        score = 1;
                        //    ODWarManager.Instance.UpdateScore(sActor.MapID, sActor.ActorID, score);
                        //}
                        if (target.Status.Additions.ContainsKey("DamageUp")) //伤害标记
                        {
                            var DamageUpRank = target.Status.Damage_Up_Lv * 0.1f + 1.1f;
                            damage = (int)(damage * DamageUpRank);
                        }

                        if (target.Status.PhysiceReduceRate > 0) //物理抗性
                            damage -= (int)(damage * (target.Status.PhysiceReduceRate / 100.0f));
                        //if (target.Status.PhysiceReduceRate > 1)
                        //    damage = (int)((float)damage / target.Status.PhysiceReduceRate);
                        //else
                        //    damage = (int)((float)damage / (1.0f + target.Status.PhysiceReduceRate));
                        if (target.Status.physice_rate_iris < 100)
                            damage = (int)(damage * (target.Status.physice_rate_iris / 100.0f));

                        //加伤处理下
                        //if (target.Seals > 0)
                        //    damage = (int)(damage * (float)(1f + 0.05f * target.Seals));//圣印
                        //if (sActor.Status.Additions.ContainsKey("ruthless") &&
                        //    (target.Buff.Stun || target.Buff.Stone || target.Buff.Frosen || target.Buff.Poison ||
                        //    target.Buff.Sleep || target.Buff.SpeedDown || target.Buff.Confused || target.Buff.Paralysis))
                        //{
                        //    if (sActor.type == ActorType.PC)
                        //    {
                        //        float rate = 1f + (((ActorPC)sActor).TInt["ruthless"] * 0.1f);
                        //        damage = (int)(damage * rate);//无情打击
                        //    }
                        //}
                        //加伤处理上

                        //减伤处理下(已完成副职逻辑)
                        if (target.Status.Additions.ContainsKey("DamageNullify")) //boss状态
                            damage = (int)(damage * 0f);
                        if (target.Status.Additions.ContainsKey("EnergyShield")) //能量加护
                        {
                            if (target.type == ActorType.PC)
                                damage = (int)(damage * (1f - 0.02f * ((ActorPC)target).TInt["EnergyShieldlv"]));
                            else
                                damage = (int)(damage * 0.9f);
                        }

                        if (target.Status.Additions.ContainsKey("Counter")) damage /= 2;

                        if (target.Status.Additions.ContainsKey("Assumptio")) damage = (int)(damage / 3.0f * 2.0f);

                        if (target.type == ActorType.PC) {
                            var pc = (ActorPC)target;
                            if (pc.Party != null && pc.Status.pt_dmg_down_iris < 100)
                                damage = (int)(damage * (pc.Status.pt_dmg_up_iris / 100.0f));
                            if (pc.Status.Element_down_iris < 100 && element != Elements.Neutral)
                                damage = (int)(damage * (pc.Status.Element_down_iris / 100.0f));

                            //iris卡种族减伤部分
                            if (sActor.Race == Race.HUMAN && pc.Status.human_dmg_down_iris < 100)
                                damage = (int)(damage * (pc.Status.human_dmg_down_iris / 100.0f));

                            else if (sActor.Race == Race.BIRD && pc.Status.bird_dmg_down_iris < 100)
                                damage = (int)(damage * (pc.Status.bird_dmg_down_iris / 100.0f));
                            else if (sActor.Race == Race.ANIMAL && pc.Status.animal_dmg_down_iris < 100)
                                damage = (int)(damage * (pc.Status.animal_dmg_down_iris / 100.0f));
                            else if (sActor.Race == Race.MAGIC_CREATURE && pc.Status.magic_c_dmg_down_iris < 100)
                                damage = (int)(damage * (pc.Status.magic_c_dmg_down_iris / 100.0f));
                            else if (sActor.Race == Race.PLANT && pc.Status.plant_dmg_down_iris < 100)
                                damage = (int)(damage * (pc.Status.plant_dmg_down_iris / 100.0f));
                            else if (sActor.Race == Race.WATER_ANIMAL && pc.Status.water_a_dmg_down_iris < 100)
                                damage = (int)(damage * (pc.Status.water_a_dmg_down_iris / 100.0f));
                            else if (sActor.Race == Race.MACHINE && pc.Status.machine_dmg_down_iris < 100)
                                damage = (int)(damage * (pc.Status.machine_dmg_down_iris / 100.0f));
                            else if (sActor.Race == Race.ROCK && pc.Status.rock_dmg_down_iris < 100)
                                damage = (int)(damage * (pc.Status.rock_dmg_down_iris / 100.0f));
                            else if (sActor.Race == Race.ELEMENT && pc.Status.element_dmg_down_iris < 100)
                                damage = (int)(damage * (pc.Status.element_dmg_down_iris / 100.0f));
                            else if (sActor.Race == Race.UNDEAD && pc.Status.undead_dmg_down_iris < 100)
                                damage = (int)(damage * (pc.Status.undead_dmg_down_iris / 100.0f));
                        }

                        if (target.Status.Additions.ContainsKey("Blocking") && target.Status.Blocking_LV != 0 &&
                            target.type == ActorType.PC) //3转骑士格挡
                        {
                            var pc = (ActorPC)target;
                            if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND) &&
                                pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.LEFT_HAND))
                                if (pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType ==
                                    ItemType.SHIELD ||
                                    pc.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].BaseData.itemType ==
                                    ItemType.SHIELD) {
                                    var SutanOdds = target.Status.Blocking_LV * 5;
                                    var ParryOdds = new[] { 0, 15, 25, 35, 65, 75 }[target.Status.Blocking_LV];
                                    float ParryResult = 4 + 6 * target.Status.Blocking_LV;
                                    var args = new SagaDB.Skill.Skill();
                                    //不管是主职还是副职,判定盾牌专精知识是否存在
                                    if (pc.Skills.ContainsKey(116) || pc.DualJobSkills.Exists(x => x.ID == 116)) {
                                        //这里取副职的盾牌专精等级
                                        var duallv = 0;
                                        if (pc.DualJobSkills.Exists(x => x.ID == 116))
                                            duallv = pc.DualJobSkills.FirstOrDefault(x => x.ID == 116).Level;

                                        //这里取主职的剑圣等级
                                        var mainlv = 0;
                                        if (pc.Skills.ContainsKey(116))
                                            mainlv = pc.Skills[116].Level;

                                        //这里取等级最高的剑圣等级用来做居合的倍率加成
                                        var level = Math.Max(duallv, mainlv);

                                        ParryResult += level * 3;
                                        //ParryResult += pc.Skills[116].Level * 3;
                                    }

                                    if (Global.Random.Next(1, 100) <= ParryOdds) {
                                        damage = damage - (int)(damage * ParryResult / 100.0f);
                                        if (Instance.CanAdditionApply(target, sActor, DefaultAdditions.Stun,
                                                SutanOdds)) {
                                            var skill = new Stun(args, sActor, 1000 + 500 * target.Status.Blocking_LV);
                                            ApplyAddition(sActor, skill);
                                        }
                                    }
                                }
                        }
                        //减伤处理上

                        //开始处理最终伤害放大

                        if (sActor.Status.Additions
                            .ContainsKey("HpLostDamUp")) //暂时判定血色战刃在该位置,因为wiki明确提出龙眼有效,所以推测最终伤害百分比增益都有效
                        {
                            var buf = (DefaultBuff)sActor.Status.Additions["HpLostDamUp"];
                            if (sActor.HP > buf["HPLost"]) {
                                sActor.HP -= (uint)buf["HPLost"];
                                damage += buf["DamUp"];
                                var tmp = new SkillArg();
                                tmp.sActor = sActor.ActorID;
                                tmp.dActor = 0xffffffff;
                                tmp.x = arg.x;
                                tmp.y = arg.y;
                                tmp.argType = SkillArg.ArgType.Active;
                                tmp.autoCast = arg.autoCast;
                                tmp.skill = SkillFactory.Instance.GetSkill(2200, 1);
                                tmp.affectedActors.Add(sActor);
                                tmp.Init();
                                tmp.hp[0] = buf["HPLost"];
                                tmp.flag[0] = AttackFlag.HP_DAMAGE | AttackFlag.ATTACK_EFFECT;
                                map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.HPMPSP_UPDATE, tmp, sActor, true);
                            }
                        }

                        //杀戮
                        if (sActor.Status.Additions.ContainsKey("Efuikasu"))
                            damage = (int)(damage * (1.0f + sActor.KillingMarkCounter * 0.05f));

                        //血印
                        if (target.Status.Additions.ContainsKey("BradStigma") && element == Elements.Dark) {
                            var rate = (target.Status.Additions["BradStigma"] as DefaultBuff).Variable["BradStigma"];
                            //MapClient.FromActorPC((ActorPC)sActor).SendSystemMessage("你的血印技能，使你的暗屬攻擊加成(" + rate + "%)。");
                            damage += (int)(damage * ((double)rate / 100.0f));
                        }

                        //火心放大
                        if (sActor.Status.Additions.ContainsKey("FrameHart")) {
                            var rate = (sActor.Status.Additions["FrameHart"] as DefaultBuff).Variable["FrameHart"];
                            damage = (int)(damage * ((double)rate / 100.0f));
                        }

                        //友情的一击
                        if (sActor.Status.Additions.ContainsKey("BlowOfFriendship")) damage = (int)(damage * 1.15f);

                        //竜眼放大
                        if (sActor.Status.Additions.ContainsKey("DragonEyeOpen")) {
                            var rate =
                                (sActor.Status.Additions["DragonEyeOpen"] as DefaultBuff).Variable["DragonEyeOpen"];
                            damage = (int)(damage * ((double)rate / 100.0f));
                        }

                        if (sActor.type == ActorType.PC) {
                            var pc = (ActorPC)sActor;
                            if (pc.Party != null && sActor.Status.pt_dmg_up_iris > 100)
                                damage = (int)(damage * (sActor.Status.pt_dmg_up_iris / 100.0f));
                            //iris卡种族增伤部分
                            if (target.Race == Race.HUMAN && pc.Status.human_dmg_up_iris > 100)
                                damage = (int)(damage * (pc.Status.human_dmg_up_iris / 100.0f));
                            else if (target.Race == Race.BIRD && pc.Status.bird_dmg_up_iris > 100)
                                damage = (int)(damage * (pc.Status.bird_dmg_up_iris / 100.0f));
                            else if (target.Race == Race.ANIMAL && pc.Status.animal_dmg_up_iris > 100)
                                damage = (int)(damage * (pc.Status.animal_dmg_up_iris / 100.0f));
                            else if (target.Race == Race.MAGIC_CREATURE && pc.Status.magic_c_dmg_up_iris > 100)
                                damage = (int)(damage * (pc.Status.magic_c_dmg_up_iris / 100.0f));
                            else if (target.Race == Race.PLANT && pc.Status.plant_dmg_up_iris > 100)
                                damage = (int)(damage * (pc.Status.plant_dmg_up_iris / 100.0f));
                            else if (target.Race == Race.WATER_ANIMAL && pc.Status.water_a_dmg_up_iris > 100)
                                damage = (int)(damage * (pc.Status.water_a_dmg_up_iris / 100.0f));
                            else if (target.Race == Race.MACHINE && pc.Status.machine_dmg_up_iris > 100)
                                damage = (int)(damage * (pc.Status.machine_dmg_up_iris / 100.0f));
                            else if (target.Race == Race.ROCK && pc.Status.rock_dmg_up_iris > 100)
                                damage = (int)(damage * (pc.Status.rock_dmg_up_iris / 100.0f));
                            else if (target.Race == Race.ELEMENT && pc.Status.element_dmg_up_iris > 100)
                                damage = (int)(damage * (pc.Status.element_dmg_up_iris / 100.0f));
                            else if (target.Race == Race.UNDEAD && pc.Status.undead_dmg_up_iris > 100)
                                damage = (int)(damage * (pc.Status.undead_dmg_up_iris / 100.0f));

                            if (pc.Skills2_1.ContainsKey(310) || pc.DualJobSkills.Exists(x => x.ID == 310)) //HAW2-1追魂箭
                            {
                                var duallv = 0;
                                if (pc.DualJobSkills.Exists(x => x.ID == 310))
                                    duallv = pc.DualJobSkills.FirstOrDefault(x => x.ID == 310).Level;

                                var mainlv = 0;
                                if (pc.Skills2_1.ContainsKey(310))
                                    mainlv = pc.Skills2_1[310].Level;

                                var level = Math.Max(duallv, mainlv);
                                if (target.Buff.Stun ||
                                    target.Buff.Stone ||
                                    target.Buff.Frosen ||
                                    target.Buff.Poison ||
                                    target.Buff.Sleep ||
                                    target.Buff.SpeedDown ||
                                    target.Buff.Confused ||
                                    target.Buff.Paralysis ||
                                    target.Buff.STRDown ||
                                    target.Buff.VITDown ||
                                    target.Buff.INTDown ||
                                    target.Buff.DEXDown ||
                                    target.Buff.AGIDown ||
                                    target.Buff.MAGDown ||
                                    target.Buff.MaxHPDown ||
                                    target.Buff.MaxMPDown ||
                                    target.Buff.MaxSPDown ||
                                    target.Buff.MinAtkDown ||
                                    target.Buff.MaxAtkDown ||
                                    target.Buff.MinMagicAtkDown ||
                                    target.Buff.MaxMagicAtkDown ||
                                    target.Buff.DefDown ||
                                    target.Buff.DefRateDown ||
                                    target.Buff.MagicDefDown ||
                                    target.Buff.MagicDefRateDown ||
                                    target.Buff.ShortHitDown ||
                                    target.Buff.LongHitDown ||
                                    target.Buff.MagicHitDown ||
                                    target.Buff.ShortDodgeDown ||
                                    target.Buff.LongDodgeDown ||
                                    target.Buff.MagicAvoidDown ||
                                    target.Buff.CriticalRateDown ||
                                    target.Buff.CriticalDodgeDown ||
                                    target.Buff.HPRegenDown ||
                                    target.Buff.MPRegenDown ||
                                    target.Buff.SPRegenDown ||
                                    target.Buff.AttackSpeedDown ||
                                    target.Buff.CastSpeedDown ||
                                    target.Buff.SpeedDown ||
                                    target.Buff.Berserker)
                                    if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND))
                                        if (pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType ==
                                            ItemType.BOW)
                                            damage = (int)(damage * (1.1f + 0.02f * level));
                            }

                            if (pc.Skills2_2.ContainsKey(314) || pc.DualJobSkills.Exists(x => x.ID == 314)) //GU2-1追魂刃
                            {
                                var duallv = 0;
                                if (pc.DualJobSkills.Exists(x => x.ID == 314))
                                    duallv = pc.DualJobSkills.FirstOrDefault(x => x.ID == 314).Level;

                                var mainlv = 0;
                                if (pc.Skills2_2.ContainsKey(314))
                                    mainlv = pc.Skills2_2[314].Level;

                                var level = Math.Max(duallv, mainlv);
                                if (target.Buff.Stun ||
                                    target.Buff.Stone ||
                                    target.Buff.Frosen ||
                                    target.Buff.Poison ||
                                    target.Buff.Sleep ||
                                    target.Buff.SpeedDown ||
                                    target.Buff.Confused ||
                                    target.Buff.Paralysis ||
                                    target.Buff.STRDown ||
                                    target.Buff.VITDown ||
                                    target.Buff.INTDown ||
                                    target.Buff.DEXDown ||
                                    target.Buff.AGIDown ||
                                    target.Buff.MAGDown ||
                                    target.Buff.MaxHPDown ||
                                    target.Buff.MaxMPDown ||
                                    target.Buff.MaxSPDown ||
                                    target.Buff.MinAtkDown ||
                                    target.Buff.MaxAtkDown ||
                                    target.Buff.MinMagicAtkDown ||
                                    target.Buff.MaxMagicAtkDown ||
                                    target.Buff.DefDown ||
                                    target.Buff.DefRateDown ||
                                    target.Buff.MagicDefDown ||
                                    target.Buff.MagicDefRateDown ||
                                    target.Buff.ShortHitDown ||
                                    target.Buff.LongHitDown ||
                                    target.Buff.MagicHitDown ||
                                    target.Buff.ShortDodgeDown ||
                                    target.Buff.LongDodgeDown ||
                                    target.Buff.MagicAvoidDown ||
                                    target.Buff.CriticalRateDown ||
                                    target.Buff.CriticalDodgeDown ||
                                    target.Buff.HPRegenDown ||
                                    target.Buff.MPRegenDown ||
                                    target.Buff.SPRegenDown ||
                                    target.Buff.AttackSpeedDown ||
                                    target.Buff.CastSpeedDown ||
                                    target.Buff.SpeedDown ||
                                    target.Buff.Berserker)
                                    if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND))
                                        if (pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType ==
                                            ItemType.SHORT_SWORD ||
                                            pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType ==
                                            ItemType.SWORD ||
                                            pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType ==
                                            ItemType.RAPIER)
                                            damage = (int)(damage * (1.1f + 0.02f * level));
                            }
                        }

                        if (sActor.WeaponElement == Elements.Holy)
                            if (target.Status.Additions.ContainsKey("Oratio"))
                                damage = (int)(damage * 1.25f);

                        //"ChgstSwoDamUp"
                        if (sActor.Status.Additions.ContainsKey("ホークアイ")) //HAW站桩
                            damage = (int)(damage *
                                           ((sActor.Status.Additions["ホークアイ"] as DefaultBuff).Variable["ホークアイ"] /
                                            100.0f));
                        //最终伤害放大处理结束

                        if (sActor.Status.Additions.ContainsKey("RoyalDealer")) //皇家贸易商站桩追加不受任何因素影响的1000伤害
                        {
                            var pc = (ActorPC)sActor;
                            var maxlv = 0;
                            if (pc.Skills3.ContainsKey(3371))
                                maxlv = pc.Skills3[3371].Level;
                            damage += new[] { 0, 0, 100, 250, 500, 1000 }[maxlv];
                        }

                        if (i.Status.NeutralDamegeDown_rate > 0 && element == Elements.Neutral)
                            damage = (int)(damage * (1.0f - i.Status.NeutralDamegeDown_rate / 100.0f));
                        if (i.Status.NeutralDamegeDown_rate > 0 && element != Elements.Neutral)
                            damage = (int)(damage * (1.0f - i.Status.ElementDamegeDown_rate / 100.0f));
                        //金刚不坏处理
                        if (i.Status.Additions.ContainsKey("MentalStrength")) {
                            var rate = (i.Status.Additions["MentalStrength"] as DefaultBuff).Variable["MentalStrength"];
                            damage = (int)(damage * (1.0f - (double)rate / 100.0f));
                        }

                        //处理bonus的技能伤害控制
                        uint skid = 0;
                        if (arg != null)
                            if (arg.skill != null)
                                skid = arg.skill.ID;
                        if (skid != 0) {
                            if (sActor.Status.SkillRate.ContainsKey(skid))
                                damage = (int)(damage * (1.0f + sActor.Status.SkillRate[skid] / 100.0f));
                            if (sActor.Status.SkillDamage.ContainsKey(skid))
                                damage += (int)sActor.Status.SkillDamage[skid];
                        }


                        //处理无法恢复
                        if (target.Buff.NoRegen && damage < 0)
                            damage = 0;

                        //吸血效果下
                        if (SuckBlood != 0 && damage != 0)
                            if (sActor.type == ActorType.PC) {
                                var hp = (int)(damage * SuckBlood);
                                if (((ActorPC)sActor).TInt["SuckBlood"] > 0)
                                    hp = (int)(hp * (1f + ((ActorPC)sActor).TInt["SuckBlood"] * 0.1f));
                                sActor.HP += (uint)hp;
                                if (sActor.HP > sActor.MaxHP)
                                    sActor.HP = sActor.MaxHP;
                                Instance.ShowVessel(sActor, -hp);

                                try {
                                    var y1 = "普通攻击";
                                    if (arg != null)
                                        if (arg.skill != null)
                                            y1 = arg.skill.Name;
                                    SendAttackMessage(1, target, "从 " + sActor.Name + " 处的 " + y1 + "",
                                        "受到了 " + -damage + " 点" + "恢复效果");
                                }
                                catch (Exception ex) {
                                    Logger.GetLogger().Error(ex, ex.Message);
                                }
                            }

                        //吸血效果上(已完成副职逻辑)
                        if (i.type == ActorType.PC) {
                            var pcs = (ActorPC)i;


                            //不管是主职还是副职,判定技能黑玫瑰的刺是否存在(前部逻辑不明暂且无视)
                            if (i.Status.Additions.ContainsKey("Bounce") && Global.Random.Next(0, 100) < 35 &&
                                (pcs.Skills3.ContainsKey(2497) || pcs.DualJobSkills.Exists(x => x.ID == 2497))) //黒薔薇の棘
                            {
                                //这里取副职的黑玫瑰的刺等级
                                var duallv = 0;
                                if (pcs.DualJobSkills.Exists(x => x.ID == 2497))
                                    duallv = pcs.DualJobSkills.FirstOrDefault(x => x.ID == 2497).Level;

                                //这里取主职的黑玫瑰的刺等级
                                var mainlv = 0;
                                if (pcs.Skills3.ContainsKey(2497))
                                    mainlv = pcs.Skills3[2497].Level;

                                //这里取等级最高的黑玫瑰的刺等级
                                var skilllv = Math.Max(duallv, mainlv);
                                //byte skilllv = pcs.Skills3[2497].Level;
                                float rank = 0;
                                var damage1 = 0;
                                if (sActor.type == ActorType.PC)
                                    rank = 0.4f + 0.2f * skilllv;
                                else if (sActor.type == ActorType.MOB) rank = 2.0f + 0.2f * skilllv;
                                damage1 = (int)(damage * rank);
                                arg.affectedActors.Add(sActor);
                                arg.hp.Add(damage1);
                                arg.sp.Add(0);
                                arg.mp.Add(0);
                                arg.flag.Add(AttackFlag.HP_DAMAGE);
                                if (sActor.HP < damage1 + 1)
                                    sActor.HP -= sActor.HP + 1;
                                else
                                    sActor.HP -= (uint)damage1;
                            }
                        }

                        try {
                            var y = "普通攻击";
                            if (arg != null)
                                if (arg.skill != null)
                                    y = arg.skill.Name;
                            var s = "物理伤害";
                            if (res == AttackResult.Critical)
                                s = "物理暴击伤害";
                            if (damage < 0)
                                s = "治疗效果";
                            if (damage > 0)
                                SendAttackMessage(2, target, "从 " + sActor.Name + " 处的 " + y + "",
                                    "受到了 " + damage + " 点" + s);
                            SendAttackMessage(3, sActor, "你的 " + y + " 对 " + target.Name + "",
                                "造成了 " + damage + " 点" + s);
                        }
                        catch (Exception ex) {
                            Logger.GetLogger().Error(ex, ex.Message);
                        }

                        //伤害结算之前附加中毒效果,如果有涂毒而且目标没中毒的话
                        if (sActor.Status.Additions.ContainsKey("AppliePoison") &&
                            !i.Status.Additions.ContainsKey("Poison"))
                            if (Instance.CanAdditionApply(sActor, i, DefaultAdditions.Poison, 95)) {
                                var poi = new Poison(arg.skill, i, 15000);
                                ApplyAddition(i, poi);
                            }

                        if (target.HP != 0) {
                            if (damage > 0) {
                                if (target.Status.PlantShield > 0) {
                                    var dmgleft = target.Status.PlantShield - damage;
                                    if (dmgleft <= 0) {
                                        target.Status.PlantShield = 0;
                                        target.Status.Additions["PlantShield"].AdditionEnd();

                                        if (target.HP > Math.Abs(dmgleft))
                                            target.HP = (uint)(target.HP + dmgleft);
                                        else
                                            target.HP = 0;
                                    }
                                    else {
                                        target.Status.PlantShield -= (uint)damage;
                                    }
                                }
                                else {
                                    if (damage > target.HP)
                                        target.HP = 0;
                                    else
                                        target.HP = (uint)(target.HP - damage);
                                }
                            }
                            else {
                                target.HP = (uint)(target.HP - damage);
                            }
                        }

                        if (target.HP > target.MaxHP)
                            target.HP = target.MaxHP;

                        //结算HP结果
                        if (target.HP != 0) {
                            arg.hp[index + counter] = damage;
                            if (target.HP > 0) {
                                //damage = (short)sActor.Status.min_atk1;
                                arg.flag[index + counter] = AttackFlag.HP_DAMAGE | AttackFlag.ATTACK_EFFECT;
                                if (res == AttackResult.Critical)
                                    arg.flag[index + counter] |= AttackFlag.CRITICAL;

                                //处理反击
                                if (target.Status.Additions.ContainsKey("Counter")) {
                                    var newArg = new SkillArg();
                                    var rate = (target.Status.Additions["Counter"] as DefaultBuff).Variable["Counter"] /
                                               100.0f;
                                    Instance.Attack(target, sActor, newArg, rate);
                                    target.Status.Additions["Counter"].AdditionEnd();
                                    MapManager.Instance.GetMap(target.MapID)
                                        .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.ATTACK, newArg, target,
                                            true);
                                }

                                if (target.Status.Additions.ContainsKey("Gladiator")) {
                                    var pcs = (ActorPC)target;
                                    //if (Global.Random.Next(0, 100) >= 50 && i.HP > damage)
                                    if (Global.Random.Next(0, 100) <= 30 + 10 * pcs.Skills3[3362].Level &&
                                        i.HP > 0) //不认可需要进行HP与伤害的互动判定,修改为生命值大于0即可,目前运作正常
                                    {
                                        var newArg = new SkillArg();
                                        Instance.Attack(target, sActor, newArg, 1.5f);
                                        MapManager.Instance.GetMap(target.MapID)
                                            .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.ATTACK, newArg, target,
                                                true);
                                    }
                                }

                                if (target.Status.Additions.ContainsKey("ArtFullTrap4")) {
                                    var newArg = new SkillArg();
                                    Instance.Attack(target, sActor, newArg, 1.0f);
                                    MapManager.Instance.GetMap(target.MapID)
                                        .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.ATTACK, newArg, target,
                                            true);
                                }
                            }
                            else {
                                //damage = (int)target.HP;
                                target.HP = 0;
                                if (!ride && !target.Buff.Reborn)
                                    arg.flag[index + counter] =
                                        AttackFlag.DIE | AttackFlag.HP_DAMAGE | AttackFlag.ATTACK_EFFECT;
                                else
                                    arg.flag[index + counter] = AttackFlag.HP_DAMAGE | AttackFlag.ATTACK_EFFECT;
                                if (res == AttackResult.Critical)
                                    arg.flag[index + counter] |= AttackFlag.CRITICAL;
                            }
                            //arg.flag[i] |=  AttackFlag.ATTACK_EFFECT;

                            arg.hp[index + counter] = damage;
                        }
                        else {
                            if (!ride && !target.Buff.Reborn)
                                arg.flag[index + counter] =
                                    AttackFlag.DIE | AttackFlag.HP_DAMAGE | AttackFlag.ATTACK_EFFECT;
                            else
                                arg.flag[index + counter] = AttackFlag.HP_DAMAGE | AttackFlag.ATTACK_EFFECT;
                            if (res == AttackResult.Critical)
                                arg.flag[index + counter] |= AttackFlag.CRITICAL;
                            arg.hp[index + counter] = damage;
                        }

                        //吸血？
                        if (sActor.Status.Additions.ContainsKey("BloodLeech") && !sActor.Buff.NoRegen) {
                            var add = (BloodLeech)sActor.Status.Additions["BloodLeech"];
                            if (sActor.type == ActorType.PC) {
                                var b = 0;
                                b = Global.Random.Next(0, 10000);
                                if (b <= 10000 - (2000 + 1000 * add.rate / 0.1)) //wiki没写具体,所以根据内容,推算为1级吸收概率50%,5级10%
                                {
                                    var heal = (uint)(damage * add.rate); //吸收量
                                    var healmax = (uint)(sActor.MaxHP * (0.08 + 0.02 * add.rate / 0.1));
                                    if (heal > healmax) heal = healmax;

                                    sActor.HP += heal;
                                    if (sActor.HP > sActor.MaxHP)
                                        sActor.HP = sActor.MaxHP;
                                    Instance.ShowVessel(sActor, (int)-heal);
                                    MapManager.Instance.GetMap(sActor.MapID)
                                        .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.HPMPSP_UPDATE, null, sActor,
                                            true);
                                }
                            }
                            else {
                                var heal = (int)(damage * add.rate);
                                arg.affectedActors.Add(sActor);
                                arg.hp.Add(heal);
                                arg.sp.Add(0);
                                arg.mp.Add(0);
                                arg.flag.Add(AttackFlag.HP_HEAL | AttackFlag.NO_DAMAGE);

                                sActor.HP += (uint)heal;
                                if (sActor.HP > sActor.MaxHP)
                                    sActor.HP = sActor.MaxHP;
                            }


                            MapManager.Instance.GetMap(sActor.MapID)
                                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.HPMPSP_UPDATE, null, sActor, true);
                        }

                        //SP吸收
                        if (sActor.Status.Additions.ContainsKey("SpLeech") && !sActor.Buff.NoRegen) {
                            var add = (SpLeech)sActor.Status.Additions["SpLeech"];
                            if (sActor.type == ActorType.PC) {
                                var SpLevel = (int)(add.rate / 0.05f);
                                if (Global.Random.Next(0, 99) < SpLevel * 10) {
                                    //uint Spheal = (uint)(damage*);//吸收量,SP吸收没有吸收概率,都是100%
                                    var Sphealmax = (uint)Math.Min(damage, (uint)(sActor.MaxSP * 0.05 * SpLevel));
                                    //if (Spheal > Sphealmax)
                                    //{
                                    //    Spheal = Sphealmax;
                                    //}
                                    //arg.affectedActors.Add(sActor);
                                    //arg.hp.Add(0);
                                    //arg.sp.Add((int)Spheal);
                                    //arg.mp.Add(0);
                                    sActor.SP += Sphealmax;
                                    Instance.ShowVessel(sActor, 0, 0, (int)-Sphealmax);
                                    //arg.flag.Add(AttackFlag.SP_HEAL | AttackFlag.NO_DAMAGE);

                                    //sActor.SP += (uint)Spheal;
                                    if (sActor.SP > sActor.MaxSP)
                                        sActor.SP = sActor.MaxSP;
                                    MapManager.Instance.GetMap(sActor.MapID)
                                        .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.HPMPSP_UPDATE, null, sActor,
                                            true);
                                }
                            }
                            else {
                                var Spheal = (uint)(damage * add.rate); //吸收量,SP吸收没有吸收概率,都是100%
                                if (Spheal > sActor.MaxSP * 0.5 * 5) Spheal = (uint)(sActor.MaxSP * 0.5 * 5);
                                sActor.SP += Spheal;
                                Instance.ShowVessel(sActor, 0, 0, (int)-Spheal);
                                sActor.SP += Spheal;
                                if (sActor.SP > sActor.MaxSP)
                                    sActor.SP = sActor.MaxSP;
                                MapManager.Instance.GetMap(sActor.MapID)
                                    .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.HPMPSP_UPDATE, null, sActor,
                                        true);
                            }
                        }
                    }
                }

                ApplyDamage(sActor, target, damage, doublehate, arg);
                if ((res == AttackResult.Critical || res == AttackResult.Hit) &&
                    sActor.Status.Additions.ContainsKey("WithinWeeks") && sActor.type == ActorType.PC) {
                    var thispc = (ActorPC)sActor;
                    var level = thispc.CInt["WithinWeeksLevel"];
                    switch (thispc.CInt["WithinWeeksLevel"]) {
                        case 1:
                            if (Instance.CanAdditionApply(sActor, target, DefaultAdditions.Silence, 5)) {
                                var skill = new Silence(arg.skill, target, 750 + 250 * level);
                                ApplyAddition(target, skill);
                            }

                            break;
                        case 2:
                            if (Instance.CanAdditionApply(sActor, target, DefaultAdditions.CannotMove, 5)) {
                                var skill = new CannotMove(arg.skill, target, 1000);
                                ApplyAddition(target, skill);
                            }

                            break;
                        case 3:
                            if (Instance.CanAdditionApply(sActor, target, DefaultAdditions.Stiff, 5)) {
                                var skill = new Stiff(arg.skill, target, 1000);
                                ApplyAddition(target, skill);
                            }

                            break;
                        case 4:
                            if (Instance.CanAdditionApply(sActor, target, DefaultAdditions.Confuse, 5)) {
                                var skill = new Confuse(arg.skill, target, 3000);
                                ApplyAddition(target, skill);
                            }

                            break;
                        case 5:
                            if (Instance.CanAdditionApply(sActor, target, DefaultAdditions.Stun, 10 * level)) {
                                var skill = new Stun(arg.skill, target, 2000);
                                ApplyAddition(target, skill);
                            }

                            break;
                    }
                }

                if ((res == AttackResult.Critical || res == AttackResult.Hit) &&
                    sActor.Status.Additions.ContainsKey("EnchantWeapon") && sActor.type == ActorType.PC &&
                    dActor.Count == 1) {
                    var thispc = (ActorPC)sActor;
                    switch (thispc.CInt["EnchantWeaponLevel"]) {
                        case 1:
                            if (Instance.CanAdditionApply(sActor, target, DefaultAdditions.CannotMove, 25)) {
                                var skill = new CannotMove(arg.skill, target, 6000);
                                ApplyAddition(target, skill);
                            }

                            break;
                        case 2:
                            if (Instance.CanAdditionApply(sActor, target, DefaultAdditions.Frosen, 20)) {
                                var skill = new Freeze(arg.skill, target, 4000);
                                ApplyAddition(target, skill);
                            }

                            break;
                        case 3:
                            if (Instance.CanAdditionApply(sActor, target, DefaultAdditions.Stiff, 15)) {
                                var skill = new Stiff(arg.skill, target, 2000);
                                ApplyAddition(target, skill);
                            }

                            break;
                    }
                }


                if ((res == AttackResult.Miss || res == AttackResult.Avoid || res == AttackResult.Guard) &&
                    dActor.Count == 1) //弓3转23级技能
                    if (sActor.Status.MissRevenge_rate > 0 &&
                        Global.Random.Next(0, 100) < sActor.Status.MissRevenge_rate) {
                        sActor.Status.MissRevenge_hit = true;
                        arg.sActor = sActor.ActorID;
                        arg.dActor = i.ActorID;
                        arg.type = sActor.Status.attackType;
                        arg.delayRate = 1f;
                        PhysicalAttack(sActor, target, arg, sActor.WeaponElement, 1f);
                    }

                if (i.Status.Additions.ContainsKey("TranceBody") && element != Elements.Neutral &&
                    element != Elements.Holy && element != Elements.Dark) //ASJOB13技能
                {
                    thispc = (ActorPC)i;
                    if (i.Status.Additions.ContainsKey("HolyShield"))
                        RemoveAddition(i, "HolyShield");
                    if (i.Status.Additions.ContainsKey("DarkShield"))
                        RemoveAddition(i, "DarkShield");
                    if (i.Status.Additions.ContainsKey("FireShield"))
                        RemoveAddition(i, "FireShield");
                    if (i.Status.Additions.ContainsKey("WaterShield"))
                        RemoveAddition(i, "WaterShield");
                    if (i.Status.Additions.ContainsKey("WindShield"))
                        RemoveAddition(i, "WindShield");
                    if (i.Status.Additions.ContainsKey("EarthShield"))
                        RemoveAddition(i, "EarthShield");
                    RemoveAddition(i, "TranceBody");
                    i.Buff.BodyDarkElementUp = false;
                    i.Buff.BodyEarthElementUp = false;
                    i.Buff.BodyFireElementUp = false;
                    i.Buff.BodyWaterElementUp = false;
                    i.Buff.BodyWindElementUp = false;
                    i.Buff.BodyHolyElementUp = false;
                    var life = 150000 + 30000 * arg.skill.Level;
                    if (element == Elements.Earth) //魔法属性
                        Toelements = Elements.Earth;
                    else if (element == Elements.Wind)
                        Toelements = Elements.Wind;
                    else if (element == Elements.Fire)
                        Toelements = Elements.Fire;
                    else if (element == Elements.Water) Toelements = Elements.Water;
                    var skill = new DefaultBuff(arg.skill, i, Toelements + "Shield", life);
                    skill.OnAdditionStart += StartEventHandlerMele;
                    skill.OnAdditionEnd += EndEventHandlerMele;
                    ApplyAddition(i, skill);
                }

                counter++;
                MapManager.Instance.GetMap(sActor.MapID)
                    .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.HPMPSP_UPDATE, null, target, true);
            }

            var aspd = (short)(sActor.Status.aspd + sActor.Status.aspd_skill);
            if (aspd > 800)
                aspd = 800;
            //可能的攻速位置?备注
            arg.delay = 2000 - (uint)(2000 * aspd * 0.001f);

            arg.delay = (uint)(arg.delay * arg.delayRate);

            if (sActor.Status.aspd_skill_perc >= 1f)
                arg.delay = (uint)(arg.delay / sActor.Status.aspd_skill_perc);

            return damage;
        }


        ///// <summary>
        ///// 对多个目标进行物理攻击
        ///// </summary>
        ///// <param name="sActor">原目标</param>
        ///// <param name="dActor">对象目标集合</param>
        ///// <param name="arg">技能参数</param>
        ///// <param name="element">元素</param>
        ///// <param name="index">arg中参数偏移</param>
        /////<param name="defType">使用的防御类型</param>
        ///// <param name="ATKBonus">攻击加成</param>
        /////  <param name="ignore">无视防御比</param>
        //public int PhysicalAttack(Actor sActor, List<Actor> dActor, SkillArg arg, DefType defType, Elements element, int index, float ATKBonus, bool setAtk, float SuckBlood, bool doublehate, int shitbonus, int scribonus, int cridamagebonus, string effectname, int lifetime, float ignore = 0, int igoreAddDef = 0)
        //{
        //    if (dActor.Count == 0) return 0;
        //    if (sActor.Status == null) return 0;

        //    if (!CheckStatusCanBeAttact(sActor, 1))
        //        return 0;

        //    if (dActor.Count > 10)
        //    {
        //        foreach (var item in dActor)
        //            DoDamage(true, sActor, item, arg, defType, element, index, ATKBonus);
        //        return 0;
        //    }

        //    if (index == 0)
        //    {
        //        arg.affectedActors = new List<Actor>();
        //        foreach (Actor i in dActor)
        //            arg.affectedActors.Add(i);
        //        arg.Init();
        //    }

        //    # region 基础攻击力计算

        //    int damage = 0;
        //    int atk;
        //    int mindamage = 0;
        //    int maxdamage = 0;
        //    int mindamageM = 0;
        //    int maxdamageM = 0;
        //    int counter = 0;
        //    Map map = Manager.MapManager.Instance.GetMap(dActor[0].MapID);

        //    switch (arg.type)
        //    {
        //        case ATTACK_TYPE.BLOW:
        //            mindamage = sActor.Status.min_atk1;
        //            maxdamage = sActor.Status.max_atk1;
        //            break;
        //        case ATTACK_TYPE.SLASH:
        //            mindamage = sActor.Status.min_atk2;
        //            maxdamage = sActor.Status.max_atk2;
        //            break;
        //        case ATTACK_TYPE.STAB:
        //            mindamage = sActor.Status.min_atk3;
        //            maxdamage = sActor.Status.max_atk3;
        //            break;
        //    }
        //    //check
        //    if (mindamage > maxdamage) maxdamage = mindamage;

        //    mindamageM = sActor.Status.min_matk;
        //    maxdamageM = sActor.Status.max_matk;
        //    if (mindamageM > maxdamageM) maxdamageM = mindamageM;

        //    # endregion

        //    foreach (Actor i in dActor)
        //    {
        //        if (i.Status == null)
        //            continue;

        //        //NOTOUCH類MOB 跳過判定
        //        if (i.type == ActorType.MOB)
        //        {
        //            ActorMob checkmob = (ActorMob)i;
        //            switch (checkmob.BaseData.mobType)
        //            {
        //                case SagaDB.Mob.MobType.ANIMAL_NOTOUCH:
        //                case SagaDB.Mob.MobType.BIRD_NOTOUCH:
        //                case SagaDB.Mob.MobType.ELEMENT_BOSS_NOTOUCH:
        //                case SagaDB.Mob.MobType.HUMAN_NOTOUCH:
        //                case SagaDB.Mob.MobType.ELEMENT_NOTOUCH:
        //                case SagaDB.Mob.MobType.PLANT_NOTOUCH:
        //                case SagaDB.Mob.MobType.MACHINE_NOTOUCH:
        //                case SagaDB.Mob.MobType.NONE_NOTOUCH:
        //                case SagaDB.Mob.MobType.UNDEAD_NOTOUCH:
        //                case SagaDB.Mob.MobType.WATER_ANIMAL_NOTOUCH:
        //                case SagaDB.Mob.MobType.PLANT_BOSS_NOTOUCH:
        //                    continue;

        //            }

        //        }

        //        //#region 注释内容
        //        //投掷武器
        //        /*if (sActor.type == ActorType.PC)
        //        {
        //            ActorPC pc = (ActorPC)sActor;
        //            if (pc.Inventory.Equipments.ContainsKey(SagaDB.Item.EnumEquipSlot.RIGHT_HAND))
        //            {
        //                if (pc.Inventory.Equipments[SagaDB.Item.EnumEquipSlot.RIGHT_HAND].BaseData.itemType == SagaDB.Item.ItemType.THROW)
        //                {
        //                    MapClient.FromActorPC(pc).DeleteItem(pc.Inventory.Equipments[SagaDB.Item.EnumEquipSlot.RIGHT_HAND].Slot, 1, false);
        //                }

        //                if (pc.Inventory.Equipments[SagaDB.Item.EnumEquipSlot.RIGHT_HAND].BaseData.itemType == SagaDB.Item.ItemType.CARD)
        //                {
        //                    MapClient.FromActorPC(pc).DeleteItem(pc.Inventory.Equipments[SagaDB.Item.EnumEquipSlot.RIGHT_HAND].Slot, 1, false);
        //                }
        //            }
        //        }

        //        //弓箭，枪
        //        if (sActor.type == ActorType.PC)
        //        {
        //            ActorPC pc = (ActorPC)sActor;
        //            if (pc.Inventory.Equipments.ContainsKey(SagaDB.Item.EnumEquipSlot.RIGHT_HAND))
        //            {
        //                if (pc.Inventory.Equipments[SagaDB.Item.EnumEquipSlot.RIGHT_HAND].BaseData.itemType == SagaDB.Item.ItemType.BOW)
        //                {
        //                    if (pc.Inventory.Equipments.ContainsKey(SagaDB.Item.EnumEquipSlot.LEFT_HAND))
        //                    {
        //                        if (pc.Inventory.Equipments[SagaDB.Item.EnumEquipSlot.LEFT_HAND].BaseData.itemType == SagaDB.Item.ItemType.ARROW)
        //                        {
        //                            if (pc.Inventory.Equipments[SagaDB.Item.EnumEquipSlot.LEFT_HAND].Stack > 0)
        //                                MapClient.FromActorPC(pc).DeleteItem(pc.Inventory.Equipments[SagaDB.Item.EnumEquipSlot.LEFT_HAND].Slot, 1, false);
        //                        }
        //                        else
        //                        {
        //                            if (counter == 0)
        //                                arg.result = -1;
        //                            continue;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        if (counter == 0)
        //                            arg.result = -1;
        //                        continue;
        //                    }
        //                }
        //                if (pc.Inventory.Equipments[SagaDB.Item.EnumEquipSlot.RIGHT_HAND].BaseData.itemType == SagaDB.Item.ItemType.GUN ||
        //                    pc.Inventory.Equipments[SagaDB.Item.EnumEquipSlot.RIGHT_HAND].BaseData.itemType == SagaDB.Item.ItemType.DUALGUN ||
        //                    pc.Inventory.Equipments[SagaDB.Item.EnumEquipSlot.RIGHT_HAND].BaseData.itemType == SagaDB.Item.ItemType.RIFLE)
        //                {
        //                    if (pc.Inventory.Equipments.ContainsKey(SagaDB.Item.EnumEquipSlot.LEFT_HAND))
        //                    {
        //                        if (pc.Inventory.Equipments[SagaDB.Item.EnumEquipSlot.LEFT_HAND].BaseData.itemType == SagaDB.Item.ItemType.BULLET)
        //                        {
        //                            if (pc.Inventory.Equipments[SagaDB.Item.EnumEquipSlot.LEFT_HAND].Stack > 0)
        //                                MapClient.FromActorPC(pc).DeleteItem(pc.Inventory.Equipments[SagaDB.Item.EnumEquipSlot.LEFT_HAND].Slot, 1, false);
        //                        }
        //                        else
        //                        {
        //                            if (counter == 0)
        //                                arg.result = -1;
        //                            continue;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        if (counter == 0)
        //                            arg.result = -1;
        //                        continue;
        //                    }
        //                }
        //            }
        //        }*/
        //        //判断命中结果
        //        //short dis = Map.Distance(sActor, i);
        //        //这个补全技能补正后去掉
        //        //#endregion
        //        if (arg.argType == SkillArg.ArgType.Active)
        //            shitbonus = 50;
        //        AttackResult res = CalcAttackResult(sActor, i, sActor.Range > 3);
        //        bool ismiss = false;
        //        if (res == AttackResult.Miss)
        //        {
        //            res = AttackResult.Hit;
        //            ismiss = true;
        //        }
        //        //#region 注释卡片判定
        //        /*
        //    if (sActor.type == ActorType.MOB && dActor.type == ActorType.PC)
        //    {
        //        ActorMob mob = (ActorMob)sActor;
        //        Addition[] list = dActor.Status.Additions.Values.ToArray();
        //        foreach (Addition i in list)
        //        {
        //            if (i.GetType() == typeof(Skill.Additions.Global.SomeTypeAvoUp))
        //            {
        //                Additions.Global.SomeTypeAvoUp up = (Additions.Global.SomeTypeAvoUp)i;
        //                if (up.MobTypes.ContainsKey(mob.BaseData.mobType))
        //                {
        //                    dAvoid += (int)(dAvoid * ((float)up.MobTypes[mob.BaseData.mobType] / 100));
        //                }
        //            }
        //        }
        //    }
        //    if (dActor.type == ActorType.MOB && sActor.type == ActorType.PC)
        //    {
        //        ActorMob mob = (ActorMob)dActor;
        //        Addition[] list = sActor.Status.Additions.Values.ToArray();
        //        foreach (Addition i in list)
        //        {
        //            if (i.GetType() == typeof(Skill.Additions.Global.SomeTypeHitUp))
        //            {
        //                Additions.Global.SomeTypeHitUp up = (Additions.Global.SomeTypeHitUp)i;
        //                if (up.MobTypes.ContainsKey(mob.BaseData.mobType))
        //                {
        //                    sHit += (int)(sHit * ((float)up.MobTypes[mob.BaseData.mobType] / 100));
        //                }
        //            }
        //        }
        //    }*/
        //        //#endregion
        //        Actor target = i;
        //        //if (i.type == ActorType.PC)
        //        //{
        //        //    ActorPC me = (ActorPC)i;
        //        //    if (me.Status.Additions.ContainsKey("圣骑士的牺牲") && me.Party != null)
        //        //    {
        //        //        ActorPC victim = (ActorPC)map.Actors[(uint)me.TInt["牺牲者ActorID"]];
        //        //        if (victim == null) break;
        //        //        if (victim.Party != me.Party && (5 > Math.Max(Math.Abs(me.X - victim.X) / 100, Math.Abs(me.Y - victim.Y) / 100))) break;
        //        //        target = victim;
        //        //        ShowEffectByActor(i, 4345);
        //        //    }
        //        //}

        //        if (res == AttackResult.Miss || res == AttackResult.Avoid || res == AttackResult.Guard)
        //        {
        //            if (res == AttackResult.Miss)
        //            {
        //                arg.flag[index + counter] = AttackFlag.MISS;
        //            }
        //            else if (res == AttackResult.Avoid)
        //                arg.flag[index + counter] = AttackFlag.AVOID;
        //            else
        //                arg.flag[index + counter] = AttackFlag.GUARD;
        //            if (i.Status.Additions.ContainsKey("Parry"))//格挡
        //            {
        //                if (sActor == null)
        //                    return 0;
        //                ActorPC pc = (ActorPC)i;
        //                if (pc.Inventory.Equipments.ContainsKey(SagaDB.Item.EnumEquipSlot.RIGHT_HAND) || pc.Inventory.Equipments.ContainsKey(SagaDB.Item.EnumEquipSlot.LEFT_HAND))
        //                {
        //                    Network.Client.MapClient.FromActorPC(pc).SendSkillDummy(100, 1);
        //                    if (i.Status.Additions.ContainsKey("Parry"))
        //                        i.Status.Additions["Parry"].AdditionEnd();
        //                    ShowEffect(pc, pc, 4135);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            int restKryrie = 0;
        //            if (i.Status.Additions.ContainsKey("MobKyrie"))//救援邀请，留着有参考价值
        //            {
        //                Additions.Global.DefaultBuff buf = (Additions.Global.DefaultBuff)i.Status.Additions["MobKyrie"];
        //                restKryrie = buf["MobKyrie"];
        //                arg.flag[index + counter] = AttackFlag.HP_DAMAGE | AttackFlag.NO_DAMAGE;
        //                if (restKryrie > 0)
        //                {
        //                    buf["MobKyrie"]--;
        //                    EffectArg arg2 = new EffectArg();
        //                    arg2.effectID = 4173;
        //                    arg2.actorID = i.ActorID;
        //                    map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SHOW_EFFECT, arg2, i, true);
        //                    if (restKryrie == 1)
        //                        SkillHandler.RemoveAddition(i, "MobKyrie");
        //                }
        //            }
        //            if (restKryrie == 0)
        //            {
        //                bool isPossession = false;
        //                bool isHost = false;
        //                if (i.type == ActorType.PC)
        //                {
        //                    ActorPC pc = (ActorPC)i;
        //                    if (pc.PossesionedActors.Count > 0 && pc.PossessionTarget == 0)
        //                    {
        //                        isPossession = true;
        //                        isHost = true;
        //                    }
        //                    if (pc.PossessionTarget != 0)
        //                    {
        //                        isPossession = true;
        //                        isHost = false;
        //                    }
        //                }
        //                //处理凭依伤害
        //                if (isHost && isPossession && ATKBonus > 0)
        //                {
        //                    List<Actor> possessionDamage = ProcessAttackPossession(i);
        //                    if (possessionDamage.Count > 0)
        //                    {
        //                        arg.Remove(i);
        //                        int oldcount = arg.flag.Count;
        //                        arg.Extend(possessionDamage.Count);
        //                        foreach (Actor j in possessionDamage)
        //                        {
        //                            if (Global.Random.Next(0, 99) < i.Status.possessionTakeOver)
        //                                arg.affectedActors.Add(i);
        //                            else
        //                                arg.affectedActors.Add(j);
        //                        }
        //                        PhysicalAttack(sActor, possessionDamage, arg, element, oldcount, ATKBonus);
        //                        continue;
        //                    }
        //                }

        //                if (!setAtk)
        //                {
        //                    atk = (int)Global.Random.Next(mindamage, maxdamage);
        //                    //TODO: element bonus, range bonus
        //                    atk = (int)(atk * CalcElementBonus(sActor, i, element, 0, false) * ATKBonus);
        //                }
        //                else
        //                {
        //                    atk = (int)Global.Random.Next(mindamage, maxdamage);
        //                    atk += (int)Global.Random.Next(mindamageM, maxdamageM);

        //                    atk = (int)(atk * CalcElementBonus(sActor, i, element, 0, false) * ATKBonus);
        //                    //atk = (int)ATKBonus;
        //                }
        //                //int igoreAddDef = 0;


        //                damage = CalcPhyDamage(sActor, i, defType, atk, ignore);

        //                if (damage > atk)
        //                    damage = atk;

        //                if (i.type == ActorType.PARTNER) return 20;
        //                IStats stats = (IStats)i;
        //                switch (arg.type)
        //                {
        //                    case ATTACK_TYPE.BLOW:
        //                        damage = (int)(damage * (1f - i.Status.damage_atk1_discount));
        //                        break;
        //                    case ATTACK_TYPE.SLASH:
        //                        damage = (int)(damage * (1f - i.Status.damage_atk2_discount));
        //                        break;
        //                    case ATTACK_TYPE.STAB:
        //                        damage = (int)(damage * (1f - i.Status.damage_atk3_discount));
        //                        break;
        //                }

        //                if (sActor.type == ActorType.PC && target.type == ActorType.PC)
        //                {
        //                    damage = (int)(damage * Configuration.Instance.PVPDamageRatePhysic);
        //                }
        //                damage = checkbuff(sActor, target, arg, 0, damage);
        //                damage = checkirisbuff(sActor, target, arg, 0, damage);
        //                if (damage <= 0) damage = 1;

        //                if (res == AttackResult.Critical)
        //                {
        //                    damage = (int)(((float)damage) * (float)(CalcCritBonus(sActor, target, scribonus) + (float)cridamagebonus));
        //                    if (sActor.Status.Additions.ContainsKey("CriDamUp"))
        //                    {
        //                        float rate = (float)((float)(sActor.Status.Additions["CriDamUp"] as DefaultPassiveSkill).Variable["CriDamUp"] / 100.0f + 1.0f);
        //                        damage = (int)((float)damage * rate);
        //                    }
        //                }

        //                checkdebuff(sActor, target, arg, 0);
        //                //宠判定？
        //                bool ride = false;
        //                if (target.type == ActorType.PC)
        //                {
        //                    ActorPC pc = (ActorPC)target;
        //                    if (pc.Pet != null)
        //                        ride = pc.Pet.Ride;
        //                }
        //                //宠物成长
        //                if (res == AttackResult.Critical)
        //                {
        //                    if (sActor.type == ActorType.PET)
        //                        ProcessPetGrowth(sActor, PetGrowthReason.CriticalHit);
        //                    if (i.type == ActorType.PET && damage > 0)
        //                        ProcessPetGrowth(i, PetGrowthReason.PhysicalBeenHit);
        //                    if (i.type == ActorType.PC && damage > 0)
        //                    {
        //                        ActorPC pc = (ActorPC)target;

        //                        if (ride)
        //                        {
        //                            ProcessPetGrowth(pc.Pet, PetGrowthReason.PhysicalBeenHit);
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    if (sActor.type == ActorType.PET)
        //                        ProcessPetGrowth(sActor, PetGrowthReason.PhysicalHit);
        //                    if (i.type == ActorType.PET && damage > 0)
        //                    {
        //                        ProcessPetGrowth(i, PetGrowthReason.PhysicalBeenHit);
        //                    }
        //                    if (i.type == ActorType.PC && damage > 0)
        //                    {
        //                        ActorPC pc = (ActorPC)target;

        //                        if (ride)
        //                        {
        //                            ProcessPetGrowth(pc.Pet, PetGrowthReason.PhysicalBeenHit);
        //                        }
        //                    }
        //                }

        //                //技能以及状态判定
        //                if (sActor.type == ActorType.PC && target.type == ActorType.MOB)
        //                {
        //                    ActorMob mob = (ActorMob)target;
        //                    if (mob.BaseData.mobType.ToString().Contains("CHAMP") && !sActor.Buff.StateOfMonsterKillerChamp)
        //                        damage = damage / 10;
        //                }

        //                if (sActor.type == ActorType.PC)
        //                {
        //                    int score = damage / 100;
        //                    if (score == 0)
        //                        score = 1;
        //                    ODWarManager.Instance.UpdateScore(sActor.MapID, sActor.ActorID, score);
        //                }
        //                //加伤处理下
        //                if (i.Status.Additions.ContainsKey("Invincible"))//绝对壁垒
        //                    damage = 0;
        //                //技能以及状态判定
        //                if (sActor.type == ActorType.PC)
        //                {
        //                    ActorPC pcsActor = (ActorPC)sActor;
        //                    if (sActor.Status.Additions.ContainsKey("BurnRate"))// && SkillHandler.Instance.isEquipmentRight(pcsActor, SagaDB.Item.ItemType.CARD))//皇家贸易商
        //                    {
        //                        if (pcsActor.Skills3.ContainsKey(3371))
        //                        {
        //                            if (pcsActor.Skills3[3371].Level > 1)
        //                            {
        //                                int[] gold = { 0, 0, 100, 250, 500, 1000 };
        //                                if (pcsActor.Gold > gold[pcsActor.Skills3[3371].Level])
        //                                {
        //                                    pcsActor.Gold -= gold[pcsActor.Skills3[3371].Level];
        //                                    damage += gold[pcsActor.Skills3[3371].Level];
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //                if (sActor.type == ActorType.PC && i.type == ActorType.MOB)
        //                {
        //                    ActorMob mob = (ActorMob)i;
        //                    if (mob.BaseData.mobType.ToString().Contains("CHAMP") && !sActor.Buff.StateOfMonsterKillerChamp)
        //                        damage = damage / 10;
        //                }

        //                //if (sActor.type == ActorType.PC)
        //                //{
        //                //    int score = damage / 100;
        //                //    if (score == 0)
        //                //        score = 1;
        //                //    ODWarManager.Instance.UpdateScore(sActor.MapID, sActor.ActorID, score);
        //                //}
        //                if (i.Status.Additions.ContainsKey("DamageUp"))//伤害标记
        //                {
        //                    float DamageUpRank = i.Status.Damage_Up_Lv * 0.1f + 1.1f;
        //                    damage = (int)(damage * DamageUpRank);
        //                }

        //                if (i.Status.PhysiceReduceRate > 0)//物理抗性
        //                {
        //                    if (i.Status.PhysiceReduceRate > 1)
        //                        damage = (int)((float)damage / i.Status.PhysiceReduceRate);
        //                    else
        //                        damage = (int)((float)damage / (1.0f + i.Status.PhysiceReduceRate));
        //                }

        //                //加伤处理下
        //                if (i.Seals > 0)
        //                    damage = (int)(damage * (float)(1f + 0.05f * i.Seals));//圣印
        //                if (sActor.Status.Additions.ContainsKey("ruthless") &&
        //                    (i.Buff.Stun || i.Buff.Stone || i.Buff.Frosen || i.Buff.Poison ||
        //                    i.Buff.Sleep || i.Buff.SpeedDown || i.Buff.Confused || i.Buff.Paralysis))
        //                {
        //                    if (sActor.type == ActorType.PC)
        //                    {
        //                        float rate = 1f + (((ActorPC)sActor).TInt["ruthless"] * 0.1f);
        //                        damage = (int)(damage * rate);//无情打击
        //                    }
        //                }
        //                //加伤处理上

        //                //减伤处理下
        //                if (i.Status.Additions.ContainsKey("DamageNullify"))//boss状态
        //                    damage = (int)(damage * (float)0f);
        //                if (i.Status.Additions.ContainsKey("EnergyShield"))//能量加护
        //                {
        //                    if (i.type == ActorType.PC)
        //                        damage = (int)(damage * (float)(1f - 0.02f * ((ActorPC)i).TInt["EnergyShieldlv"]));
        //                    else
        //                        damage = (int)(damage * (float)0.9f);
        //                }
        //                if (i.Status.Additions.ContainsKey("Counter"))
        //                {
        //                    damage /= 2;
        //                }

        //                if (i.Status.Additions.ContainsKey("Blocking") && i.Status.Blocking_LV != 0 && i.type == ActorType.PC)//3转骑士格挡
        //                {
        //                    ActorPC pc = (ActorPC)i;
        //                    if (pc.Inventory.Equipments.ContainsKey(SagaDB.Item.EnumEquipSlot.RIGHT_HAND) &&
        //                        pc.Inventory.Equipments.ContainsKey(SagaDB.Item.EnumEquipSlot.LEFT_HAND))
        //                    {
        //                        if (pc.Inventory.Equipments[SagaDB.Item.EnumEquipSlot.RIGHT_HAND].BaseData.itemType == SagaDB.Item.ItemType.SHIELD ||
        //                            pc.Inventory.Equipments[SagaDB.Item.EnumEquipSlot.LEFT_HAND].BaseData.itemType == SagaDB.Item.ItemType.SHIELD)
        //                        {
        //                            int SutanOdds = i.Status.Blocking_LV * 5;
        //                            int SutanTime = 1000 + i.Status.Blocking_LV * 500;
        //                            int ParryOdds = new int[] { 0, 15, 25, 35, 65, 75 }[i.Status.Blocking_LV];
        //                            float ParryResult = 4 + 6 * i.Status.Blocking_LV;
        //                            SagaDB.Skill.Skill args = new SagaDB.Skill.Skill();
        //                            if (pc.Skills.ContainsKey(116))
        //                            {
        //                                ParryResult += pc.Skills[116].Level * 3;
        //                            }
        //                            if (Global.Random.Next(1, 100) <= ParryOdds)
        //                            {
        //                                damage = damage - (int)(damage * ParryResult / 100.0f);
        //                                if (SkillHandler.Instance.CanAdditionApply(i, sActor, SkillHandler.DefaultAdditions.Stun, SutanOdds))
        //                                {
        //                                    Additions.Global.Stun skill = new SagaMap.Skill.Additions.Global.Stun(args, sActor, 1000 + 500 * i.Status.Blocking_LV);
        //                                    SkillHandler.ApplyAddition(sActor, skill);
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //                //减伤处理上

        //                //开始处理最终伤害放大

        //                //杀戮放大
        //                if (sActor.Status.Additions.ContainsKey("Efuikasu"))
        //                    damage = (int)((float)damage * (1.0f + (float)sActor.KillingMarkCounter * 0.05f));

        //                //火心放大
        //                if (sActor.Status.Additions.ContainsKey("FrameHart"))
        //                {
        //                    int rate = (sActor.Status.Additions["FrameHart"] as DefaultBuff).Variable["FrameHart"];
        //                    damage = (int)((double)damage * (double)((double)rate / 100));
        //                }

        //                //竜眼放大
        //                if (sActor.Status.Additions.ContainsKey("DragonEyeOpen"))
        //                {
        //                    int rate = (sActor.Status.Additions["DragonEyeOpen"] as DefaultBuff).Variable["DragonEyeOpen"];
        //                    damage = (int)((double)damage * (double)((double)rate / 100));
        //                }
        //                //吸血效果下
        //                if (SuckBlood != 0)
        //                {
        //                    if (sActor.type == ActorType.PC)
        //                    {
        //                        int hp = (int)(damage * SuckBlood);
        //                        if (((ActorPC)sActor).TInt["SuckBlood"] > 0)
        //                            hp = (int)(hp * (float)(1f + ((ActorPC)sActor).TInt["SuckBlood"] * 0.1f));
        //                        sActor.HP += (uint)hp;
        //                        if (sActor.HP > sActor.MaxHP)
        //                            sActor.HP = sActor.MaxHP;
        //                        Instance.ShowVessel(sActor, -hp);
        //                    }
        //                }
        //                //吸血效果上


        //                if (i.type == ActorType.PC)
        //                {
        //                    ActorPC pcs = (ActorPC)i;

        //                    if (i.Status.Additions.ContainsKey("剑斗士"))
        //                    {
        //                        if (Global.Random.Next(0, 100) >= 50 && i.HP > damage)
        //                            PhysicalAttack(i, sActor, arg, Elements.Neutral, 1.5f);
        //                    }

        //                    if (i.Status.Additions.ContainsKey("Bounce") && Global.Random.Next(0, 100) < 35 && pcs.Skills3.ContainsKey(2497))//黒薔薇の棘
        //                    {
        //                        byte skilllv = pcs.Skills3[2497].Level;
        //                        float rank = 0;
        //                        int damage1 = 0;
        //                        if (sActor.type == ActorType.PC)
        //                        {
        //                            rank = 0.4f + 0.2f * skilllv;
        //                        }
        //                        else if (sActor.type == ActorType.MOB)
        //                        {
        //                            rank = 2.0f + 0.2f * skilllv;
        //                        }
        //                        damage1 = (int)(damage * rank);
        //                        arg.affectedActors.Add(sActor);
        //                        arg.hp.Add(damage1);
        //                        arg.sp.Add(0);
        //                        arg.mp.Add(0);
        //                        arg.flag.Add(AttackFlag.HP_DAMAGE);
        //                        if (sActor.HP < damage1 + 1)
        //                        {
        //                            sActor.HP -= sActor.HP + 1;
        //                        }
        //                        else
        //                            sActor.HP -= (uint)damage1;
        //                    }

        //                }

        //                //结算HP结果
        //                arg.hp[index + counter] = damage;
        //                if (damage >= 0)
        //                    arg.flag[index + counter] = AttackFlag.HP_DAMAGE | AttackFlag.ATTACK_EFFECT;
        //                else
        //                    arg.flag[index + counter] = AttackFlag.HP_HEAL | AttackFlag.NO_DAMAGE;
        //                if (res == AttackResult.Critical)
        //                    arg.flag[index + counter] |= AttackFlag.CRITICAL;

        //                //伤害结算之前附加中毒效果,如果有涂毒而且目标没中毒的话
        //                if (sActor.Status.Additions.ContainsKey("AppliePoison") && !i.Status.Additions.ContainsKey("Poison"))
        //                {
        //                    if (SkillHandler.Instance.CanAdditionApply(sActor, i, DefaultAdditions.Poison, 95))
        //                    {
        //                        Poison poi = new Poison(arg.skill, i, 15000);
        //                        SkillHandler.ApplyAddition(i, poi);
        //                    }
        //                }

        //                //结算HP结果
        //                if (target.HP != 0)
        //                {
        //                    arg.hp[index + counter] = damage;
        //                    if (target.HP > damage)
        //                    {
        //                        //damage = (short)sActor.Status.min_atk1;
        //                        arg.flag[index + counter] = AttackFlag.HP_DAMAGE | AttackFlag.ATTACK_EFFECT;
        //                        if (res == AttackResult.Critical)
        //                            arg.flag[index + counter] |= AttackFlag.CRITICAL;

        //                        //处理反击
        //                        if (target.Status.Additions.ContainsKey("Counter"))
        //                        {
        //                            SkillArg newArg = new SkillArg();
        //                            float rate = (target.Status.Additions["Counter"] as DefaultBuff).Variable["Counter"] / 100.0f;
        //                            SkillHandler.Instance.Attack(target, sActor, newArg, rate);
        //                            target.Status.Additions["Counter"].AdditionEnd();
        //                            MapManager.Instance.GetMap(target.MapID).SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.ATTACK, newArg, target, true);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        damage = (int)target.HP;
        //                        if (!ride && !target.Buff.Reborn)
        //                            arg.flag[index + counter] = AttackFlag.DIE | AttackFlag.HP_DAMAGE | AttackFlag.ATTACK_EFFECT;
        //                        else
        //                            arg.flag[index + counter] = AttackFlag.HP_DAMAGE | AttackFlag.ATTACK_EFFECT;
        //                        if (res == AttackResult.Critical)
        //                            arg.flag[index + counter] |= AttackFlag.CRITICAL;
        //                    }
        //                    //arg.flag[i] |=  AttackFlag.ATTACK_EFFECT;
        //                    if (target.HP != 0)
        //                        target.HP = (uint)(target.HP - damage);
        //                }
        //                else
        //                {
        //                    if (!ride && !target.Buff.Reborn)
        //                        arg.flag[index + counter] = AttackFlag.DIE | AttackFlag.HP_DAMAGE | AttackFlag.ATTACK_EFFECT;
        //                    else
        //                        arg.flag[index + counter] = AttackFlag.HP_DAMAGE | AttackFlag.ATTACK_EFFECT;
        //                    if (res == AttackResult.Critical)
        //                        arg.flag[index + counter] |= AttackFlag.CRITICAL;
        //                    arg.hp[index + counter] = damage;
        //                }

        //                //吸血？
        //                if (sActor.Status.Additions.ContainsKey("BloodLeech") && !sActor.Buff.NoRegen)
        //                {
        //                    Additions.Global.BloodLeech add = (Additions.Global.BloodLeech)sActor.Status.Additions["BloodLeech"];
        //                    int heal = (int)(damage * add.rate);
        //                    arg.affectedActors.Add(sActor);
        //                    arg.hp.Add(heal);
        //                    arg.sp.Add(0);
        //                    arg.mp.Add(0);
        //                    arg.flag.Add(AttackFlag.HP_HEAL | AttackFlag.NO_DAMAGE);

        //                    sActor.HP += (uint)heal;
        //                    if (sActor.HP > sActor.MaxHP)
        //                        sActor.HP = sActor.MaxHP;
        //                    Manager.MapManager.Instance.GetMap(sActor.MapID).SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.HPMPSP_UPDATE, null, sActor, true);
        //                }
        //            }
        //        }
        //        ApplyDamage(sActor, target, damage, doublehate, arg);
        //        counter++;
        //        Manager.MapManager.Instance.GetMap(sActor.MapID).SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.HPMPSP_UPDATE, null, target, true);
        //        //MapClient.FromActorPC((ActorPC)sActor).SendPartyMemberHPMPSP((ActorPC)sActor);
        //    }


        //    short aspd = (short)(sActor.Status.aspd + sActor.Status.aspd_skill);
        //    if (aspd > 800)
        //        aspd = 800;

        //    arg.delay = 2000 - (uint)(2000 * aspd * 0.001f);

        //    arg.delay = (uint)(arg.delay * arg.delayRate);

        //    if (sActor.Status.aspd_skill_perc >= 1f)
        //        arg.delay = (uint)(arg.delay / sActor.Status.aspd_skill_perc);

        //    return damage;
        //}

        public int MagicAttack(Actor sActor, Actor dActor, SkillArg arg, Elements element, float MATKBonus) {
            return MagicAttack(sActor, dActor, arg, element, 50, MATKBonus);
        }

        public int MagicAttack(Actor sActor, Actor dActor, SkillArg arg, Elements element, float MATKBonus,
            float ignore) {
            var list = new List<Actor>();
            list.Add(dActor);
            return MagicAttack(sActor, list, arg, DefType.MDef, element, 50, MATKBonus, 0, false, false, 0, false,
                ignore);
        }

        public int MagicAttack(Actor sActor, Actor dActor, SkillArg arg, Elements element, int elementValue,
            float MATKBonus) {
            var list = new List<Actor>();
            list.Add(dActor);
            return MagicAttack(sActor, list, arg, element, elementValue, MATKBonus);
        }

        public int MagicAttack(Actor sActor, Actor dActor, SkillArg arg, DefType defType, Elements element,
            float MATKBonus) {
            return MagicAttack(sActor, dActor, arg, defType, element, 50, MATKBonus);
        }

        public int MagicAttack(Actor sActor, Actor dActor, SkillArg arg, DefType defType, Elements element,
            int elementValue, float MATKBonus) {
            var list = new List<Actor>();
            list.Add(dActor);
            return MagicAttack(sActor, list, arg, defType, element, elementValue, MATKBonus);
        }

        public int MagicAttack(Actor sActor, List<Actor> dActor, SkillArg arg, Elements element, float MATKBonus) {
            return MagicAttack(sActor, dActor, arg, element, 50, MATKBonus);
        }

        public int MagicAttack(Actor sActor, List<Actor> dActor, SkillArg arg, Elements element, int elementValue,
            float MATKBonus) {
            return MagicAttack(sActor, dActor, arg, element, elementValue, MATKBonus, 0);
        }

        public int MagicAttack(Actor sActor, List<Actor> dActor, SkillArg arg, DefType defType, Elements element,
            float MATKBonus) {
            return MagicAttack(sActor, dActor, arg, defType, element, 50, MATKBonus);
        }

        public int MagicAttack(Actor sActor, List<Actor> dActor, SkillArg arg, DefType defType, Elements element,
            int elementValue, float MATKBonus) {
            return MagicAttack(sActor, dActor, arg, defType, element, elementValue, MATKBonus,
                0); //Use element holy to represent not using this param.
        }

        public int MagicAttack(Actor sActor, List<Actor> dActor, SkillArg arg, Elements element, int elementValue,
            float MATKBonus, int index) {
            return MagicAttack(sActor, dActor, arg, DefType.MDef, element, elementValue, MATKBonus, index);
        }

        public int MagicAttack(Actor sActor, List<Actor> dActor, SkillArg arg, DefType defType, Elements element,
            int elementValue, float MATKBonus, int index) {
            return MagicAttack(sActor, dActor, arg, defType, element, elementValue, MATKBonus, index, false);
        }

        public int MagicAttack(Actor sActor, List<Actor> dActor, SkillArg arg, DefType defType, Elements element,
            float MATKBonus, int index, bool setAtk) {
            return MagicAttack(sActor, dActor, arg, defType, element, 50, MATKBonus, index, setAtk);
        }

        public int MagicAttack(Actor sActor, List<Actor> dActor, SkillArg arg, DefType defType, Elements element,
            int elementValue, float MATKBonus, int index, bool setAtk) {
            return MagicAttack(sActor, dActor, arg, defType, element, elementValue, MATKBonus, index, setAtk, false);
        }

        public int MagicAttack(Actor sActor, List<Actor> dActor, SkillArg arg, DefType defType, Elements element,
            int elementValue, float MATKBonus, int index, bool setAtk, bool noReflect) {
            return MagicAttack(sActor, dActor, arg, defType, element, elementValue, MATKBonus, index, setAtk, false, 0);
        }

        public int MagicAttack(Actor sActor, List<Actor> dActor, SkillArg arg, DefType defType, Elements element,
            int elementValue, float MATKBonus, int index, bool setAtk, bool noReflect, float SuckBlood,
            bool WeaponAttack = false, float ignore = 0) {
            return MagicAttack(sActor, dActor, arg, defType, element, elementValue, MATKBonus, 0, index, setAtk, false,
                SuckBlood, WeaponAttack, ignore);
        }

        public int MagicAttack(Actor sActor, List<Actor> dActor, SkillArg arg, DefType defType, Elements element,
            int elementValue, float MATKBonus, int mcridamagebonus, int index, bool setAtk, bool noReflect,
            float SuckBlood, bool WeaponAttack = false, float ignore = 0) {
            if (dActor.Count == 0)
                return 0;
            if (sActor.Status == null)
                return 0;

            //wiz3转元素增伤
            //if (sActor.Status.PlusElement_rate > 0)
            //   MATKBonus += sActor.Status.PlusElement_rate;

            if (dActor.Count > 10) {
                foreach (var item in dActor)
                    DoDamage(false, sActor, item, arg, defType, element, index, MATKBonus);
                return 0;
            }

            var damage = 0;

            //calculate the MATK
            int matk;
            var mindamage = 0;
            var maxdamage = 0;
            var counter = 0;
            var map = MapManager.Instance.GetMap(dActor[0].MapID);
            if (index == 0) {
                arg.affectedActors = new List<Actor>();
                foreach (var i in dActor)
                    arg.affectedActors.Add(i);
                arg.Init();
            }

            if (WeaponAttack) {
                mindamage = ((ActorPC)sActor).Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].MAtk +
                            ((ActorPC)sActor).Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.matk;

                maxdamage = mindamage;
            }
            else {
                mindamage = sActor.Status.min_matk;
                maxdamage = sActor.Status.max_matk;
            }

            if (mindamage > maxdamage) maxdamage = mindamage;

            foreach (var i in dActor) {
                damage = 0;
                var target = i;
                if (i.Status == null)
                    continue;
                if (i.type == ActorType.ITEM)
                    continue;
                //NOTOUCH類MOB 跳過判定
                if (i.type == ActorType.MOB) {
                    var checkmob = (ActorMob)i;
                    switch (checkmob.BaseData.mobType) {
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

                if (i.type == ActorType.PC && i.Status.Additions.ContainsKey("GoodLucky")) {
                    var pc = (ActorPC)i;
                    if (pc.Skills2_2.ContainsKey(960) || pc.DualJobSkills.Exists(x => x.ID == 960)) {
                        var duallv = 0;
                        if (pc.DualJobSkills.Exists(x => x.ID == 960))
                            duallv = pc.DualJobSkills.FirstOrDefault(x => x.ID == 960).Level;

                        //这里取主职的剑圣等级
                        var mainlv = 0;
                        if (pc.Skills2_2.ContainsKey(960))
                            mainlv = pc.Skills2_2[960].Level;

                        var maxlv = Math.Max(duallv, mainlv) * 4;
                        if (Global.Random.Next(0, 99) < maxlv) {
                            MapClient.FromActorPC(pc).SendSystemMessage("魔法被回避了");
                            continue;
                        }
                    }
                }

                var restKryrie = 0;
                if (i.Status.Additions.ContainsKey("DispelField")) {
                    var buf = (DefaultBuff)i.Status.Additions["DispelField"];
                    restKryrie = buf["DispelField"];
                    arg.flag[index + counter] = AttackFlag.HP_DAMAGE | AttackFlag.NO_DAMAGE;
                    if (restKryrie > 0) {
                        buf["DispelField"]--;
                        var arg2 = new EffectArg();
                        arg2.effectID = 4173;
                        arg2.actorID = i.ActorID;
                        map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SHOW_EFFECT, arg2, i, true);
                        if (restKryrie == 1)
                            RemoveAddition(i, "DispelField");
                    }
                }

                if (restKryrie == 0) {
                    //判断命中结果
                    //short dis = Map.Distance(sActor, i);
                    //if (arg.argType == SkillArg.ArgType.Active)
                    //    shitbonus = 50;
                    var res = CalcMagicAttackResult(sActor, i);
                    //res = AttackResult.Miss;

                    if (res == AttackResult.Miss || res == AttackResult.Avoid || res == AttackResult.Guard ||
                        res == AttackResult.Parry) {
                        if (res == AttackResult.Miss)
                            arg.flag[index + counter] = AttackFlag.MISS;
                        else if (res == AttackResult.Avoid)
                            arg.flag[index + counter] = AttackFlag.AVOID;
                        else if (res == AttackResult.Parry)
                            arg.flag[index + counter] = AttackFlag.AVOID2;
                        else
                            arg.flag[index + counter] = AttackFlag.GUARD;

                        try {
                            var y = "普通攻击";
                            if (arg != null)
                                if (arg.skill != null)
                                    y = arg.skill.Name;
                            //string s = "物理伤害";
                            SendAttackMessage(2, target, "从 " + sActor.Name + " 处的 " + y + "", "被你 " + res);
                            SendAttackMessage(3, sActor, "你的 " + y + " 对 " + target.Name + "", "被 " + res);
                        }
                        catch (Exception ex) {
                            Logger.GetLogger().Error(ex, ex.Message);
                        }
                    }
                    else {
                        if (i.Status.Additions.ContainsKey("MagicReflect") && i != sActor && !noReflect) {
                            arg.Remove(i);
                            var oldcount = arg.flag.Count;
                            arg.Extend(1);
                            arg.affectedActors.Add(sActor);
                            var dst = new List<Actor>();
                            dst.Add(sActor);
                            RemoveAddition(i, "MagicReflect");
                            MagicAttack(sActor, dst, arg, DefType.MDef, element, elementValue, MATKBonus, oldcount,
                                setAtk, true);
                            continue;
                        }

                        if (i.Status.reflex_odds > 0 && i.type == ActorType.PC) //3转骑士反射盾
                        {
                            var pc = (ActorPC)i;
                            if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.LEFT_HAND)) //右手不可能持盾
                                if (pc.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].BaseData.itemType ==
                                    ItemType.SHIELD)
                                    if (Global.Random.Next(0, 100) < 100 * i.Status.reflex_odds) {
                                        arg.Remove(i);
                                        var oldcount = arg.flag.Count;
                                        arg.Extend(1);
                                        arg.affectedActors.Add(sActor);
                                        var dst = new List<Actor>();
                                        dst.Add(sActor);
                                        MagicAttack(sActor, dst, arg, DefType.MDef, element, elementValue, MATKBonus,
                                            oldcount, false, true);
                                        continue;
                                    }
                        }

                        var isPossession = false;
                        var isHost = false;

                        if (i.type == ActorType.PC) {
                            var pc = (ActorPC)i;
                            if (pc.PossesionedActors.Count > 0 && pc.PossessionTarget == 0) {
                                isPossession = true;
                                isHost = true;
                            }

                            if (pc.PossessionTarget != 0) {
                                isPossession = true;
                                isHost = false;
                            }
                        }

                        //处理凭依伤害
                        if (isHost && isPossession && MATKBonus > 0) {
                            var possessionDamage = ProcessAttackPossession(i);
                            if (possessionDamage.Count > 0) {
                                arg.Remove(i);
                                var oldcount = arg.flag.Count;
                                arg.Extend(possessionDamage.Count);
                                foreach (var j in possessionDamage)
                                    if (Global.Random.Next(0, 99) < i.Status.possessionTakeOver)
                                        arg.affectedActors.Add(i);
                                    else
                                        arg.affectedActors.Add(j);
                                MagicAttack(sActor, possessionDamage, arg, element, elementValue, MATKBonus, oldcount);
                                continue;
                            }
                        }

                        //先校验施法者是不是玩家
                        if (sActor.type == ActorType.PC) {
                            //wiz3转JOB3,属性对无属性魔法增伤
                            var pci = sActor as ActorPC;
                            float rates = 0;
                            //不管是主职还是副职, 只要习得技能,则进入增伤判定
                            if ((pci.Skills3.ContainsKey(986) || pci.DualJobSkills.Exists(x => x.ID == 986)) &&
                                element == Elements.Neutral) {
                                //这里取副职的等级
                                var duallv = 0;
                                if (pci.DualJobSkills.Exists(x => x.ID == 986))
                                    duallv = pci.DualJobSkills.FirstOrDefault(x => x.ID == 986).Level;

                                //这里取主职的等级
                                var mainlv = 0;
                                if (pci.Skills3.ContainsKey(986))
                                    mainlv = pci.Skills3[986].Level;
                                rates = 0.02f + 0.002f * mainlv;
                                //int elements = (int)pci.WeaponElement[pci.WeaponElement];
                                var elements = pci.Status.attackElements_item[pci.WeaponElement]
                                               + pci.Status.attackElements_skill[pci.WeaponElement]
                                               + pci.Status.attackelements_iris[pci.WeaponElement];

                                //int elements = pci.Inventory.Equipments[SagaDB.Item.EnumEquipSlot.RIGHT_HAND].BaseData.element[SagaLib.Elements.Dark] +
                                //pci.Inventory.Equipments[SagaDB.Item.EnumEquipSlot.RIGHT_HAND].BaseData.element[SagaLib.Elements.Earth] +
                                //pci.Inventory.Equipments[SagaDB.Item.EnumEquipSlot.RIGHT_HAND].BaseData.element[SagaLib.Elements.Fire] +
                                //pci.Inventory.Equipments[SagaDB.Item.EnumEquipSlot.RIGHT_HAND].BaseData.element[SagaLib.Elements.Holy] +
                                //pci.Inventory.Equipments[SagaDB.Item.EnumEquipSlot.RIGHT_HAND].BaseData.element[SagaLib.Elements.Neutral] +
                                //pci.Inventory.Equipments[SagaDB.Item.EnumEquipSlot.RIGHT_HAND].BaseData.element[SagaLib.Elements.Water] +
                                //pci.Inventory.Equipments[SagaDB.Item.EnumEquipSlot.RIGHT_HAND].BaseData.element[SagaLib.Elements.Wind];

                                if (elements > 0) MATKBonus += rates * elements;
                            }
                        }

                        if (!setAtk) {
                            matk = Global.Random.Next(mindamage, maxdamage);
                            if (element != Elements.Neutral) {
                                var eleBonus = CalcElementBonus(sActor, i, element, 1,
                                    MATKBonus < 0 && !(i.Status.undead && element == Elements.Holy));
                                if ((sActor.Status.Additions.ContainsKey("EvilSoul") ||
                                     sActor.Status.Additions.ContainsKey("SoulTaker")) && element == Elements.Dark &&
                                    eleBonus > 0) {
                                    if (sActor.Status.Additions.ContainsKey("EvilSoul"))
                                        //atkValue += (sActor.Status.Additions["EvilSoul"] as DefaultBuff).Variable["EvilSoul"];
                                        eleBonus +=
                                            (sActor.Status.Additions["EvilSoul"] as DefaultBuff).Variable["EvilSoul"] /
                                            100.0f;
                                    if (sActor.Status.Additions.ContainsKey("SoulTaker") && arg.skill != null &&
                                        arg.skill.ID != 0)
                                        //atkValue += (sActor.Status.Additions["SoulTaker"] as DefaultBuff).Variable["SoulTaker"];
                                        eleBonus +=
                                            (sActor.Status.Additions["SoulTaker"] as DefaultBuff).Variable
                                                ["SoulTaker"] / 100.0f;
                                }

                                if (sActor.Status.Contract_Lv != 0) //CAJOB40
                                {
                                    var tmpele = Elements.Neutral;
                                    switch (sActor.Status.Contract_Lv) {
                                        case 1:
                                            tmpele = Elements.Fire;
                                            break;
                                        case 2:
                                            tmpele = Elements.Water;
                                            break;
                                        case 3:
                                            tmpele = Elements.Earth;
                                            break;
                                        case 4:
                                            tmpele = Elements.Wind;
                                            break;
                                    }

                                    if (tmpele == element)
                                        eleBonus += 0.5f;
                                    else
                                        eleBonus -= 0.65f;
                                }

                                if (i.Status.Contract_Lv != 0) {
                                    var tmpele = Elements.Neutral;
                                    switch (i.Status.Contract_Lv) {
                                        case 1:
                                            tmpele = Elements.Fire;
                                            break;
                                        case 2:
                                            tmpele = Elements.Water;
                                            break;
                                        case 3:
                                            tmpele = Elements.Earth;
                                            break;
                                        case 4:
                                            tmpele = Elements.Wind;
                                            break;
                                    }

                                    if (tmpele == element)
                                        eleBonus -= 0.15f;
                                    else
                                        eleBonus += 1.0f;
                                }

                                matk = (int)(matk * eleBonus * MATKBonus);
                            }
                            else {
                                matk = (int)(matk * 1f * MATKBonus);
                            }
                        }
                        else {
                            matk = (int)MATKBonus;
                        }

                        if (MATKBonus > 0)
                            damage = CalcMagDamage(sActor, i, defType, matk, ignore);
                        else
                            damage = matk;

                        //AttackResult res = AttackResult.Hit;
                        //AttackResult res = CalcAttackResult(sActor, target, true);
                        //if (res == AttackResult.Critical)
                        //    res = AttackResult.Hit;
                        /*
                        if (i.Buff.Frosen == true && element == Elements.Fire)
                        {
                            RemoveAddition(i, i.Status.Additions["WaterFrosenElement"]);
                        }
                        if (i.Buff.Stone == true && element == Elements.Water)
                        {
                            RemoveAddition(i, i.Status.Additions["StoneFrosenElement"]);
                        }
                        */

                        if (sActor.type == ActorType.PC && target.type == ActorType.PC)
                            if (damage > 0)
                                damage = (int)(damage * Configuration.Configuration.Instance.PVPDamageRateMagic);

                        if (target.Status.Additions.ContainsKey("DamageUp")) //伤害标记
                        {
                            var DamageUpRank = target.Status.Damage_Up_Lv * 0.1f + 1.1f;
                            damage = (int)(damage * DamageUpRank);
                        }

                        if (target.Status.Additions.ContainsKey("DamageNullify")) //boss状态
                            damage = (int)(damage * 0f);

                        if (target.Status.MagicRuduceRate > 0) //魔法抵抗力
                        {
                            if (target.Status.MagicRuduceRate > 1)
                                damage = (int)(damage / target.Status.MagicRuduceRate);
                            else
                                damage = (int)(damage / (1.0f + target.Status.MagicRuduceRate));
                        }

                        if (target.Status.magic_rate_iris < 100 && MATKBonus >= 0) //iris卡片提供的魔法伤害减少
                            damage = (int)(damage * (target.Status.magic_rate_iris / 100.0f));


                        if (damage <= 0 && MATKBonus >= 0)
                            damage = 1;

                        if (isPossession && isHost && target.Status.Additions.ContainsKey("DJoint")) {
                            var buf = (DefaultBuff)target.Status.Additions["DJoint"];
                            if (Global.Random.Next(0, 99) < buf["Rate"]) {
                                var dst = map.GetActor((uint)buf["Target"]);
                                if (dst != null) {
                                    target = dst;
                                    arg.affectedActors[index + counter] = target;
                                }
                            }
                        }

                        if (sActor.type == ActorType.PET)
                            ProcessPetGrowth(sActor, PetGrowthReason.SkillHit);
                        if (i.type == ActorType.PET && damage > 0)
                            ProcessPetGrowth(i, PetGrowthReason.MagicalBeenHit);

                        var ride = false;
                        if (target.type == ActorType.PC) {
                            var pc = (ActorPC)target;
                            if (pc.Pet != null)
                                ride = pc.Pet.Ride;
                        }

                        //if (sActor.type == ActorType.PC && target.type == ActorType.MOB)
                        //{
                        //    ActorMob mob = (ActorMob)target;
                        //    if (mob.BaseData.mobType.ToString().Contains("CHAMP") && !sActor.Buff.StateOfMonsterKillerChamp)
                        //        damage = damage / 10;
                        //}

                        //if (sActor.type == ActorType.PC)
                        //{
                        //    int score = damage / 100;
                        //    if (score == 0 && damage != 0)
                        //        score = 1;
                        //    ODWarManager.Instance.UpdateScore(sActor.MapID, sActor.ActorID, Math.Abs(score));
                        //}

                        //减伤处理下
                        if (target.Status.Additions.ContainsKey("无敌") && MATKBonus > 0)
                            damage = 0;


                        if (target.Status.Additions.ContainsKey("MagicShield")) //魔力加护
                        {
                            if (target.type == ActorType.PC)
                                damage = (int)(damage * (1f - 0.02f * ((ActorPC)target).TInt["MagicShieldlv"]));
                            else
                                damage = (int)(damage * 0.9f);
                        }

                        if (target.Status.MagicRuduceRate != 0)
                            damage = (int)(damage * 1f - target.Status.MagicRuduceRate);
                        if (target.Status.Additions.ContainsKey("Assumptio")) damage = (int)(damage / 3.0f * 2.0f);

                        if (target.type == ActorType.PC) {
                            var pc = (ActorPC)target;
                            if (pc.Party != null && pc.Status.pt_dmg_down_iris < 100)
                                damage = (int)(damage * (pc.Status.pt_dmg_up_iris / 100.0f));
                            if (pc.Status.Element_down_iris < 100 && element != Elements.Neutral)
                                damage = (int)(damage * (pc.Status.Element_down_iris / 100.0f));

                            //iris卡种族减伤部分
                            if (sActor.Race == Race.HUMAN && pc.Status.human_dmg_down_iris < 100)
                                damage = (int)(damage * (pc.Status.human_dmg_down_iris / 100.0f));

                            else if (sActor.Race == Race.BIRD && pc.Status.bird_dmg_down_iris < 100)
                                damage = (int)(damage * (pc.Status.bird_dmg_down_iris / 100.0f));
                            else if (sActor.Race == Race.ANIMAL && pc.Status.animal_dmg_down_iris < 100)
                                damage = (int)(damage * (pc.Status.animal_dmg_down_iris / 100.0f));
                            else if (sActor.Race == Race.MAGIC_CREATURE && pc.Status.magic_c_dmg_down_iris < 100)
                                damage = (int)(damage * (pc.Status.magic_c_dmg_down_iris / 100.0f));
                            else if (sActor.Race == Race.PLANT && pc.Status.plant_dmg_down_iris < 100)
                                damage = (int)(damage * (pc.Status.plant_dmg_down_iris / 100.0f));
                            else if (sActor.Race == Race.WATER_ANIMAL && pc.Status.water_a_dmg_down_iris < 100)
                                damage = (int)(damage * (pc.Status.water_a_dmg_down_iris / 100.0f));
                            else if (sActor.Race == Race.MACHINE && pc.Status.machine_dmg_down_iris < 100)
                                damage = (int)(damage * (pc.Status.machine_dmg_down_iris / 100.0f));
                            else if (sActor.Race == Race.ROCK && pc.Status.rock_dmg_down_iris < 100)
                                damage = (int)(damage * (pc.Status.rock_dmg_down_iris / 100.0f));
                            else if (sActor.Race == Race.ELEMENT && pc.Status.element_dmg_down_iris < 100)
                                damage = (int)(damage * (pc.Status.element_dmg_down_iris / 100.0f));
                            else if (sActor.Race == Race.UNDEAD && pc.Status.undead_dmg_down_iris < 100)
                                damage = (int)(damage * (pc.Status.undead_dmg_down_iris / 100.0f));
                        }
                        //减伤处理上
                        //开始处理最终伤害放大

                        if (!setAtk) {
                            //杀戮放大
                            if (sActor.Status.Additions.ContainsKey("Efuikasu"))
                                damage = (int)(damage * (1.0f + sActor.KillingMarkCounter * 0.05f));

                            //魔法攻擊不再有火心加成
                            //By KK 2018-04-09
                            //火心放大
                            /*
                            if (sActor.Status.Additions.ContainsKey("FrameHart"))
                            {
                                int rate = (sActor.Status.Additions["FrameHart"] as DefaultBuff).Variable["FrameHart"];
                                damage = (int)((double)damage * (double)((double)rate / 100));
                            }
                            */
                            //血印
                            if (target.Status.Additions.ContainsKey("BradStigma") && element == Elements.Dark) {
                                var rate = (target.Status.Additions["BradStigma"] as DefaultBuff)
                                    .Variable["BradStigma"];
                                //MapClient.FromActorPC((ActorPC)sActor).SendSystemMessage("你的血印技能，使你的暗屬攻擊加成(" + rate + "%)。");
                                damage += (int)(damage * ((double)rate / 100.0f));
                            }

                            //友情的一击
                            if (sActor.Status.Additions.ContainsKey("BlowOfFriendship")) damage = (int)(damage * 1.15f);

                            //竜眼放大
                            if (sActor.Status.Additions.ContainsKey("DragonEyeOpen")) {
                                var rate =
                                    (sActor.Status.Additions["DragonEyeOpen"] as DefaultBuff).Variable["DragonEyeOpen"];
                                damage = (int)(damage * ((double)rate / 100));
                            }

                            //极大放大
                            if (sActor.Status.Additions.ContainsKey("Zensss") &&
                                !sActor.ZenOutLst.Contains(arg.skill.ID)) {
                                var zenbonus = (sActor.Status.Additions["Zensss"] as DefaultBuff).Variable["Zensss"] /
                                               10.0f;
                                //MATKBonus *= zenbonus;
                                damage = (int)(damage * zenbonus);
                            }

                            if (sActor.type == ActorType.PC) {
                                var pc = (ActorPC)sActor;
                                if (pc.Party != null && pc.Status.pt_dmg_up_iris > 100)
                                    damage = (int)(damage * (pc.Status.pt_dmg_up_iris / 100.0f));

                                //iris卡种族增伤部分
                                if (target.Race == Race.BIRD && pc.Status.human_dmg_up_iris > 100)
                                    damage = (int)(damage * (pc.Status.human_dmg_up_iris / 100.0f));
                                else if (target.Race == Race.HUMAN && pc.Status.bird_dmg_up_iris > 100)
                                    damage = (int)(damage * (pc.Status.bird_dmg_up_iris / 100.0f));
                                else if (target.Race == Race.ANIMAL && pc.Status.animal_dmg_up_iris > 100)
                                    damage = (int)(damage * (pc.Status.animal_dmg_up_iris / 100.0f));
                                else if (target.Race == Race.MAGIC_CREATURE && pc.Status.magic_c_dmg_up_iris > 100)
                                    damage = (int)(damage * (pc.Status.magic_c_dmg_up_iris / 100.0f));
                                else if (target.Race == Race.PLANT && pc.Status.plant_dmg_up_iris > 100)
                                    damage = (int)(damage * (pc.Status.plant_dmg_up_iris / 100.0f));
                                else if (target.Race == Race.WATER_ANIMAL && pc.Status.water_a_dmg_up_iris > 100)
                                    damage = (int)(damage * (pc.Status.water_a_dmg_up_iris / 100.0f));
                                else if (target.Race == Race.MACHINE && pc.Status.machine_dmg_up_iris > 100)
                                    damage = (int)(damage * (pc.Status.machine_dmg_up_iris / 100.0f));
                                else if (target.Race == Race.ROCK && pc.Status.rock_dmg_up_iris > 100)
                                    damage = (int)(damage * (pc.Status.rock_dmg_up_iris / 100.0f));
                                else if (target.Race == Race.ELEMENT && pc.Status.element_dmg_up_iris > 100)
                                    damage = (int)(damage * (pc.Status.element_dmg_up_iris / 100.0f));
                                else if (target.Race == Race.UNDEAD && pc.Status.undead_dmg_up_iris > 100)
                                    damage = (int)(damage * (pc.Status.undead_dmg_up_iris / 100.0f));
                            }

                            if (sActor.WeaponElement == Elements.Holy)
                                if (target.Status.Additions.ContainsKey("Oratio"))
                                    damage = (int)(damage / 0.8f);
                            //if (sActor.Status.Additions.ContainsKey("ホークアイ"))//HAW站桩
                            //{
                            //    damage *= (int)((sActor.Status.Additions["ホークアイ"] as DefaultBuff).Variable["ホークアイ"] / 100.0f);
                            //}
                        }

                        //最终伤害放大处理结束
                        //金刚不坏处理
                        if (i.Status.Additions.ContainsKey("MentalStrength") && MATKBonus > 0) {
                            var rate = (i.Status.Additions["MentalStrength"] as DefaultBuff).Variable["MentalStrength"];
                            damage = (int)(damage * (1.0f - (double)rate / 100.0f));
                        }

                        if (i.Status.NeutralDamegeDown_rate > 0 && element == Elements.Neutral)
                            damage = (int)(damage * (1.0f - i.Status.NeutralDamegeDown_rate / 100.0f));
                        if (i.Status.NeutralDamegeDown_rate > 0 && element != Elements.Neutral)
                            damage = (int)(damage * (1.0f - i.Status.ElementDamegeDown_rate / 100.0f));

                        if (i.Status.Additions.ContainsKey("BarrierShield") && MATKBonus > 0) damage = 0;
                        //处理bonus的技能伤害控制
                        uint skid = 0;
                        if (arg != null)
                            if (arg.skill != null)
                                skid = arg.skill.ID;
                        if (skid != 0) {
                            if (sActor.Status.SkillRate.ContainsKey(skid))
                                damage = (int)(damage * (1.0f + sActor.Status.SkillRate[skid] / 100.0f));
                            if (sActor.Status.SkillDamage.ContainsKey(skid))
                                damage += (int)sActor.Status.SkillDamage[skid];
                        }

                        //吸血效果下
                        if (SuckBlood != 0 && damage != 0 && !sActor.Buff.NoRegen) //吸血效果
                            if (sActor.type == ActorType.PC) {
                                var hp = (int)(damage * SuckBlood);
                                sActor.HP += (uint)hp;
                                if (sActor.HP > sActor.MaxHP)
                                    sActor.HP = sActor.MaxHP;
                                Instance.ShowVessel(sActor, -hp);

                                try {
                                    var y1 = "攻击";
                                    if (arg != null)
                                        if (arg.skill != null)
                                            y1 = arg.skill.Name;
                                    SendAttackMessage(1, target, "从 " + sActor.Name + " 处的 " + y1 + "",
                                        "受到了 " + -damage + " 点" + "治疗效果");
                                }
                                catch (Exception ex) {
                                    Logger.GetLogger().Error(ex, ex.Message);
                                }
                            }
                        //吸血效果上

                        try {
                            var s = "魔法伤害";
                            var y = "攻击";
                            if (arg != null)
                                if (arg.skill != null)
                                    y = arg.skill.Name;
                            if (damage < 0) {
                                if (target.Buff.NoRegen)
                                    damage = 0;
                                s = "治疗效果";
                                SendAttackMessage(1, target, "从 " + sActor.Name + " 处的 " + y + "",
                                    "接受了 " + -damage + " 点" + s);
                            }
                            else {
                                SendAttackMessage(2, target, "从 " + sActor.Name + " 处的 " + y + "",
                                    "受到了 " + damage + " 点" + s);
                            }

                            SendAttackMessage(3, sActor, "你的 " + y + " 对 " + target.Name + "",
                                "造成了 " + (damage >= 0 ? damage.ToString() : (-damage).ToString()) + " 点" + s);
                        }
                        catch (Exception ex) {
                            Logger.GetLogger().Error(ex, ex.Message);
                        }

                        arg.hp[index + counter] = damage;

                        if (target.HP != 0) {
                            if (damage > 0) {
                                if (target.Status.PlantShield > 0) {
                                    var dmgleft = target.Status.PlantShield - damage;
                                    if (dmgleft <= 0) {
                                        target.Status.PlantShield = 0;
                                        target.Status.Additions["PlantShield"].AdditionEnd();

                                        if (target.HP > Math.Abs(dmgleft))
                                            target.HP = (uint)(target.HP + dmgleft);
                                        else
                                            target.HP = 0;
                                    }
                                    else {
                                        target.Status.PlantShield -= (uint)damage;
                                    }
                                }
                                else {
                                    if (damage > target.HP)
                                        target.HP = 0;
                                    else
                                        target.HP = (uint)(target.HP - damage);
                                }
                            }
                            else {
                                target.HP = (uint)(target.HP - damage);
                            }
                        }

                        if (damage > 0) {
                            if (target.HP > 0) {
                                arg.flag[index + counter] = AttackFlag.HP_DAMAGE;
                            }
                            else {
                                if (!ride && !target.Buff.Reborn)
                                    arg.flag[index + counter] =
                                        AttackFlag.DIE | AttackFlag.HP_DAMAGE | AttackFlag.ATTACK_EFFECT;
                                else
                                    arg.flag[index + counter] = AttackFlag.HP_DAMAGE | AttackFlag.ATTACK_EFFECT;
                            }
                        }
                        else {
                            arg.flag[index + counter] = AttackFlag.HP_HEAL | AttackFlag.NO_DAMAGE;
                        }

                        if (target.HP > target.MaxHP)
                            target.HP = target.MaxHP;
                    }
                }

                ApplyDamage(sActor, target, damage, arg);
                if (sActor.Status.Additions.ContainsKey("Relement") && element != Elements.Neutral &&
                    element != Elements.Holy && element != Elements.Dark) //ASJOB20技能
                {
                    int Skilllevel = (sActor.Status.Additions["Relement"] as DefaultBuff).skill.Level;
                    thispc = (ActorPC)sActor;
                    RemoveAddition(target, "HolyShield");
                    RemoveAddition(target, "DarkShield");
                    RemoveAddition(target, "FireShield");
                    RemoveAddition(target, "WaterShield");
                    RemoveAddition(target, "WindShield");
                    RemoveAddition(target, "EarthShield");
                    target.Buff.BodyDarkElementUp = false;
                    target.Buff.BodyEarthElementUp = false;
                    target.Buff.BodyFireElementUp = false;
                    target.Buff.BodyWaterElementUp = false;
                    target.Buff.BodyWindElementUp = false;
                    target.Buff.BodyHolyElementUp = false;
                    var life = 150000 + 30000 * Skilllevel;
                    if (element == Elements.Earth) //魔法属性
                        Toelements = Elements.Wind;
                    else if (element == Elements.Wind)
                        Toelements = Elements.Fire;
                    else if (element == Elements.Fire)
                        Toelements = Elements.Water;
                    else if (element == Elements.Water) Toelements = Elements.Earth;
                    var skill = new DefaultBuff(arg.skill, target, Toelements + "Shield", life);
                    skill.OnAdditionStart += StartEventHandlerSele;
                    skill.OnAdditionEnd += EndEventHandlerSele;
                    ApplyAddition(target, skill);
                }

                if (i.Status.Additions.ContainsKey("TranceBody") && element != Elements.Neutral &&
                    element != Elements.Holy && element != Elements.Dark) //ASJOB13技能
                {
                    int Skilllevel = (i.Status.Additions["TranceBody"] as DefaultBuff).skill.Level;
                    //(sActor.Status.Additions["DragonEyeOpen"] as DefaultBuff)
                    thispc = (ActorPC)i;
                    RemoveAddition(i, "HolyShield");
                    RemoveAddition(i, "DarkShield");
                    RemoveAddition(i, "FireShield");
                    RemoveAddition(i, "WaterShield");
                    RemoveAddition(i, "WindShield");
                    RemoveAddition(i, "EarthShield");
                    i.Buff.BodyDarkElementUp = false;
                    i.Buff.BodyEarthElementUp = false;
                    i.Buff.BodyFireElementUp = false;
                    i.Buff.BodyWaterElementUp = false;
                    i.Buff.BodyWindElementUp = false;
                    i.Buff.BodyHolyElementUp = false;
                    var life = 150000 + 30000 * Skilllevel;
                    if (element == Elements.Earth) //魔法属性
                        Toelements = Elements.Earth;
                    else if (element == Elements.Wind)
                        Toelements = Elements.Wind;
                    else if (element == Elements.Fire)
                        Toelements = Elements.Fire;
                    else if (element == Elements.Water) Toelements = Elements.Water;
                    var skill = new DefaultBuff(arg.skill, i, Toelements + "Shield", life);
                    skill.OnAdditionStart += StartEventHandlerMele;
                    skill.OnAdditionEnd += EndEventHandlerMele;
                    ApplyAddition(i, skill);
                }

                MapManager.Instance.GetMap(sActor.MapID)
                    .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.HPMPSP_UPDATE, null, target, true);
                counter++;
                //return ;
            }

            RemoveAddition(sActor, "Relement");
            return damage;
            //magicalCounter--;
        }

        private void StartEventHandlerSele(Actor actor, DefaultBuff skill) {
            var atk1 = (thispc.Status.Additions["Relement"] as DefaultBuff).skill.Level * 5;
            uint SkillID = 0;
            if (Toelements == Elements.Earth)
                SkillID = 3110;
            else if (Toelements == Elements.Wind)
                SkillID = 3108;
            else if (Toelements == Elements.Fire)
                SkillID = 3107;
            else if (Toelements == Elements.Water) SkillID = 3109;

            if (thispc.Skills2_2.ContainsKey(SkillID) || thispc.DualJobSkills.Exists(x => x.ID == SkillID)) {
                //这里取副职等级
                var duallv = 0;
                if (thispc.DualJobSkills.Exists(x => x.ID == SkillID))
                    duallv = Enumerable.FirstOrDefault<SagaDB.Skill.Skill>(thispc.DualJobSkills, x => x.ID == SkillID)
                        .Level;

                //这里取主职等级
                var mainlv = 0;
                if (thispc.Skills2_2.ContainsKey(SkillID))
                    mainlv = thispc.Skills2_2[SkillID].Level;

                //这里取等级最高等级用来做倍率加成
                atk1 += 10 * Math.Max(duallv, mainlv);
            }

            if (skill.Variable.ContainsKey("ElementShield"))
                skill.Variable.Remove("ElementShield");
            skill.Variable.Add("ElementShield", atk1);
            actor.Status.elements_skill[Toelements] += atk1;

            var type = actor.Buff.GetType();
            var propertyInfo = type.GetProperty("Body" + Toelements + "ElementUp");
            propertyInfo.SetValue(actor.Buff, true, null);

            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandlerSele(Actor actor, DefaultBuff skill) {
            var value = skill.Variable["ElementShield"];
            actor.Status.elements_skill[Toelements] -= (short)value;

            var type = actor.Buff.GetType();
            var propertyInfo = type.GetProperty("Body" + Toelements + "ElementUp");
            propertyInfo.SetValue(actor.Buff, false, null);
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }


        private void StartEventHandlerMele(Actor actor, DefaultBuff skill) {
            var atk1 = 15 + (thispc.Status.Additions["TranceBody"] as DefaultBuff).skill.Level * 5;
            uint SkillID = 0;
            if (Toelements == Elements.Earth)
                SkillID = 3042;
            else if (Toelements == Elements.Wind)
                SkillID = 3018;
            else if (Toelements == Elements.Fire)
                SkillID = 3007;
            else if (Toelements == Elements.Water) SkillID = 3030;

            if (thispc.Skills.ContainsKey(SkillID) || thispc.DualJobSkills.Exists(x => x.ID == SkillID)) {
                //这里取副职等级
                var duallv = 0;
                if (thispc.DualJobSkills.Exists(x => x.ID == SkillID))
                    duallv = Enumerable.FirstOrDefault<SagaDB.Skill.Skill>(thispc.DualJobSkills, x => x.ID == SkillID)
                        .Level;

                //这里取主职等级
                var mainlv = 0;
                if (thispc.Skills.ContainsKey(SkillID))
                    mainlv = thispc.Skills[SkillID].Level;

                //这里取等级最高等级用来做倍率加成
                atk1 += 5 * Math.Max(duallv, mainlv);
            }

            if (skill.Variable.ContainsKey("ElementShield"))
                skill.Variable.Remove("ElementShield");
            skill.Variable.Add("ElementShield", atk1);
            actor.Status.elements_skill[Toelements] += atk1;

            var type = actor.Buff.GetType();
            var propertyInfo = type.GetProperty("Body" + Toelements + "ElementUp");
            propertyInfo.SetValue(actor.Buff, true, null);
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandlerMele(Actor actor, DefaultBuff skill) {
            var value = skill.Variable["ElementShield"];
            actor.Status.elements_skill[Toelements] -= (short)value;

            var type = actor.Buff.GetType();
            var propertyInfo = type.GetProperty("Body" + Toelements + "ElementUp");
            propertyInfo.SetValue(actor.Buff, false, null);
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }
        //public int MagicAttack(Actor sActor, List<Actor> dActor, SkillArg arg, DefType defType, Elements element, int elementValue, float MATKBonus, int mcridamagebonus, int index, bool setAtk, bool noReflect, float SuckBlood, bool WeaponAttack = false, float ignore = 0)
        //{
        //    if (dActor.Count == 0) return 0;
        //    if (sActor.Status == null)
        //        return 0;
        //    if (dActor.Count > 10)
        //    {
        //        foreach (var item in dActor)
        //            DoDamage(false, sActor, item, arg, defType, element, index, MATKBonus);
        //        return 0;
        //    }

        //    if (!CheckStatusCanBeAttact(sActor, 0))
        //        return 0;

        //    int damage = 0;

        //    int matk;
        //    int mindamage = 0;
        //    int maxdamage = 0;

        //    int counter = 0;
        //    Map map = Manager.MapManager.Instance.GetMap(dActor[0].MapID);
        //    if (index == 0)
        //    {
        //        arg.affectedActors = new List<Actor>();
        //        foreach (Actor i in dActor)
        //            arg.affectedActors.Add(i);
        //        arg.Init();
        //    }
        //    mindamage = sActor.Status.min_matk;
        //    maxdamage = sActor.Status.max_matk;

        //    if (mindamage > maxdamage) maxdamage = mindamage;

        //    foreach (Actor i in dActor)
        //    {
        //        bool isPossession = false;
        //        bool isHost = false;
        //        Actor target = i;
        //        if (i.Status == null)
        //            continue;

        //        //NOTOUCH類MOB 跳過判定
        //        if (i.type == ActorType.MOB)
        //        {
        //            ActorMob checkmob = (ActorMob)i;
        //            switch (checkmob.BaseData.mobType)
        //            {
        //                case SagaDB.Mob.MobType.ANIMAL_NOTOUCH:
        //                case SagaDB.Mob.MobType.BIRD_NOTOUCH:
        //                case SagaDB.Mob.MobType.ELEMENT_BOSS_NOTOUCH:
        //                case SagaDB.Mob.MobType.HUMAN_NOTOUCH:
        //                case SagaDB.Mob.MobType.ELEMENT_NOTOUCH:
        //                case SagaDB.Mob.MobType.PLANT_NOTOUCH:
        //                case SagaDB.Mob.MobType.MACHINE_NOTOUCH:
        //                case SagaDB.Mob.MobType.NONE_NOTOUCH:
        //                case SagaDB.Mob.MobType.UNDEAD_NOTOUCH:
        //                case SagaDB.Mob.MobType.WATER_ANIMAL_NOTOUCH:
        //                case SagaDB.Mob.MobType.PLANT_BOSS_NOTOUCH:
        //                    continue;

        //            }

        //        }

        //        if (i.type == ActorType.PC)
        //        {
        //            ActorPC pc = (ActorPC)i;
        //            if (pc.PossesionedActors.Count > 0 && pc.PossessionTarget == 0)
        //            {
        //                isPossession = true;
        //                isHost = true;
        //            }
        //            if (pc.PossessionTarget != 0)
        //            {
        //                isPossession = true;
        //                isHost = false;
        //            }
        //        }

        //        //处理凭依伤害
        //        if (isHost && isPossession && MATKBonus > 0)
        //        {
        //            List<Actor> possessionDamage = ProcessAttackPossession(i);
        //            if (possessionDamage.Count > 0)
        //            {
        //                arg.Remove(i);
        //                int oldcount = arg.flag.Count;
        //                arg.Extend(possessionDamage.Count);
        //                foreach (Actor j in possessionDamage)
        //                {
        //                    if (Global.Random.Next(0, 99) < i.Status.possessionTakeOver)
        //                        arg.affectedActors.Add(i);
        //                    else
        //                        arg.affectedActors.Add(j);
        //                }
        //                MagicAttack(sActor, possessionDamage, arg, element, elementValue, MATKBonus, oldcount);
        //                continue;
        //            }
        //        }
        //        if (i.Status.Additions.ContainsKey("MagicReflect") && i != sActor && !noReflect)
        //        {
        //            arg.Remove(i);
        //            int oldcount = arg.flag.Count;
        //            arg.Extend(1);
        //            arg.affectedActors.Add(sActor);
        //            List<Actor> dst = new List<Actor>();
        //            dst.Add(sActor);
        //            RemoveAddition(i, "MagicReflect");
        //            MagicAttack(sActor, dst, arg, DefType.MDef, element, elementValue, MATKBonus, oldcount, false, true);
        //            continue;
        //        }

        //        if (!setAtk)
        //        {
        //            matk = Global.Random.Next(mindamage, maxdamage);
        //            if (element != Elements.Neutral)
        //            {
        //                float eleBonus = CalcElementBonus(sActor, i, element, 1, ((MATKBonus < 0) && !((i.Status.undead == true) && (element == Elements.Holy))));
        //                matk = (int)(matk * eleBonus * MATKBonus);
        //            }
        //            else
        //                matk = (int)(matk * 1f * MATKBonus);
        //            if (sActor.Status.zenList.Contains((ushort)arg.skill.ID))
        //            {
        //                matk *= 2;
        //            }
        //            if (sActor.Status.darkZenList.Contains((ushort)arg.skill.ID))
        //            {
        //                matk *= 2;
        //            }
        //        }
        //        else
        //            matk = (int)MATKBonus;
        //        if (MATKBonus > 0)
        //        {
        //            damage = CalcPhyDamage(sActor, i, defType, matk, ignore);
        //        }
        //        else
        //        {
        //            damage = matk;
        //        }


        //        //魔法会心判定
        //        AttackResult res = AttackResult.Hit;

        //        damage = checkbuff(sActor, target, arg, 1, damage);
        //        damage = checkirisbuff(sActor, target, arg, 1, damage);
        //        if (i.Buff.Frosen == true && element == Elements.Fire)
        //        {
        //            RemoveAddition(i, i.Status.Additions["Frosen"]);
        //        }
        //        if (i.Buff.Stone == true && element == Elements.Water)
        //        {
        //            RemoveAddition(i, i.Status.Additions["Stone"]);
        //        }

        //        if (i.Buff.Frosen == true && element == Elements.Fire)
        //        {
        //            RemoveAddition(i, i.Status.Additions["WaterFrosenElement"]);
        //        }
        //        if (i.Buff.Stone == true && element == Elements.Water)
        //        {
        //            RemoveAddition(i, i.Status.Additions["StoneFrosenElement"]);
        //        }

        //        if (sActor.type == ActorType.PC && target.type == ActorType.PC)
        //        {
        //            if (damage > 0)
        //                damage = (int)(damage * Configuration.Instance.PVPDamageRateMagic);
        //        }

        //        if (target.Status.Additions.ContainsKey("DamageUp"))//伤害标记
        //        {
        //            float DamageUpRank = target.Status.Damage_Up_Lv * 0.1f + 1.1f;
        //            damage = (int)(damage * DamageUpRank);
        //        }
        //        if (target.Status.Additions.ContainsKey("DamageNullify"))//boss状态
        //            damage = (int)(damage * (float)0f);

        //        if (target.Status.MagicRuduceRate > 0)//魔法抵抗力
        //        {
        //            if (target.Status.MagicRuduceRate > 1)
        //                damage = (int)((float)damage / target.Status.MagicRuduceRate);
        //            else
        //                damage = (int)((float)damage / (1.0f + target.Status.MagicRuduceRate));
        //        }

        //        if (damage <= 0 && MATKBonus >= 0)
        //            damage = 1;

        //        if (isPossession && isHost && target.Status.Additions.ContainsKey("DJoint"))
        //        {
        //            Additions.Global.DefaultBuff buf = (Additions.Global.DefaultBuff)target.Status.Additions["DJoint"];
        //            if (Global.Random.Next(0, 99) < buf["Rate"])
        //            {
        //                Actor dst = map.GetActor((uint)buf["Target"]);
        //                if (dst != null)
        //                {
        //                    target = dst;
        //                    arg.affectedActors[index + counter] = target;
        //                }
        //            }
        //        }
        //        if (sActor.type == ActorType.PET)
        //            ProcessPetGrowth(sActor, PetGrowthReason.SkillHit);
        //        if (i.type == ActorType.PET && damage > 0)
        //            ProcessPetGrowth(i, PetGrowthReason.MagicalBeenHit);

        //        bool ride = false;
        //        if (target.type == ActorType.PC)
        //        {
        //            ActorPC pc = (ActorPC)target;
        //            if (pc.Pet != null)
        //                ride = pc.Pet.Ride;
        //        }

        //        if (sActor.type == ActorType.PC && target.type == ActorType.MOB)
        //        {
        //            ActorMob mob = (ActorMob)target;
        //            if (mob.BaseData.mobType.ToString().Contains("CHAMP") && !sActor.Buff.StateOfMonsterKillerChamp)
        //                damage = damage / 10;
        //        }


        //        if (sActor.type == ActorType.PC)
        //        {
        //            int score = damage / 100;
        //            if (score == 0 && damage != 0)
        //                score = 1;
        //            ODWarManager.Instance.UpdateScore(sActor.MapID, sActor.ActorID, Math.Abs(score));
        //        }

        //        //加伤处理下
        //        if (target.Seals > 0)
        //            damage = (int)(damage * (float)(1f + 0.05f * target.Seals));//圣印
        //                                                                        //加伤处理上
        //                                                                        //减伤处理下
        //        if (target.Status.Additions.ContainsKey("无敌"))
        //            damage = 0;
        //        if (target.Status.Additions.ContainsKey("MagicShield"))//魔力加护
        //        {
        //            if (target.type == ActorType.PC)
        //                damage = (int)(damage * (float)(1f - 0.02f * ((ActorPC)target).TInt["MagicShieldlv"]));
        //            else
        //                damage = (int)(damage * (float)0.9f);
        //        }
        //        if (target.Status.MagicRuduceRate != 0)
        //        {
        //            damage = (int)(damage * (float)1f - target.Status.MagicRuduceRate);
        //        }
        //        //减伤处理上
        //        //开始处理最终伤害放大

        //        if (!setAtk)
        //        {
        //            //杀戮放大
        //            if (sActor.Status.Additions.ContainsKey("Efuikasu"))
        //                damage = (int)((float)damage * (1.0f + (float)sActor.KillingMarkCounter * 0.05f));

        //            //火心放大
        //            if (sActor.Status.Additions.ContainsKey("FrameHart"))
        //            {
        //                int rate = (sActor.Status.Additions["FrameHart"] as DefaultBuff).Variable["FrameHart"];
        //                damage = (int)((double)damage * (double)((double)rate / 100));
        //            }
        //            //竜眼放大
        //            if (sActor.Status.Additions.ContainsKey("DragonEyeOpen"))
        //            {
        //                int rate = (sActor.Status.Additions["DragonEyeOpen"] as DefaultBuff).Variable["DragonEyeOpen"];
        //                damage = (int)((double)damage * (double)((double)rate / 100));
        //            }
        //            //极大放大
        //            if (sActor.Status.Additions.ContainsKey("Zensss") && !sActor.ZenOutLst.Contains(arg.skill.ID))
        //            {
        //                float zenbonus = (float)((sActor.Status.Additions["Zensss"] as DefaultBuff).Variable["Zensss"] / 10);
        //                //MATKBonus *= zenbonus;
        //                damage = (int)((float)damage * zenbonus);
        //            }
        //        }

        //        //最终伤害放大处理结束

        //        //吸血效果下
        //        if (SuckBlood != 0 && damage != 0)//吸血效果
        //        {
        //            if (sActor.type == ActorType.PC)
        //            {
        //                int hp = (int)(damage * SuckBlood);
        //                sActor.HP += (uint)hp;
        //                if (sActor.HP > sActor.MaxHP)
        //                    sActor.HP = sActor.MaxHP;
        //                Instance.ShowVessel(sActor, -hp);
        //            }
        //        }
        //        //吸血效果上
        //        if (target.Status.Additions.ContainsKey("Sacrifice") && damage < 0)
        //            damage = 0;

        //        arg.hp[index + counter] += damage;
        //        if (damage > 0)
        //        {
        //            if (target.HP > damage)
        //            {
        //                arg.flag[index + counter] = AttackFlag.HP_DAMAGE;
        //            }
        //            else
        //            {
        //                damage = (int)target.HP;
        //                if (!ride && !target.Buff.Reborn)
        //                    arg.flag[index + counter] = AttackFlag.DIE | AttackFlag.HP_DAMAGE | AttackFlag.ATTACK_EFFECT;
        //                else
        //                    arg.flag[index + counter] = AttackFlag.HP_DAMAGE | AttackFlag.ATTACK_EFFECT;
        //            }
        //        }
        //        else
        //        {
        //            arg.flag[index + counter] = AttackFlag.HP_HEAL | AttackFlag.NO_DAMAGE;
        //        }

        //        if (target.HP != 0)
        //            target.HP = (uint)(target.HP - damage);
        //        if (target.HP > target.MaxHP)
        //            target.HP = target.MaxHP;

        //        counter++;
        //        ApplyDamage(sActor, target, damage, arg);
        //        MapManager.Instance.GetMap(sActor.MapID).SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.HPMPSP_UPDATE, null, target, true);
        //    }
        //    return damage;
        //    //magicalCounter--;
        //}

        /// <summary>
        ///     对指定目标附加伤害
        /// </summary>
        /// <param name="sActor">原目标</param>
        /// <param name="dActor">对象目标</param>
        /// <param name="damage">伤害值</param>
        public void ApplyDamage(Actor sActor, Actor dActor, int damage, SkillArg arg2 = null) {
            ApplyDamage(sActor, dActor, damage, false, arg2);
        }


        //public List<int> range = new List<int>();
        //public void GravityGL()
        //{
        //    range.Add(SkillHandler.Instance.CalcPosHashCode(1, 0, 2));
        //    range.Add(SkillHandler.Instance.CalcPosHashCode(2, 0, 2));
        //    range.Add(SkillHandler.Instance.CalcPosHashCode(-1, 0, 2));
        //    range.Add(SkillHandler.Instance.CalcPosHashCode(-2, 0, 2));
        //    range.Add(SkillHandler.Instance.CalcPosHashCode(1, 1, 2));
        //    range.Add(SkillHandler.Instance.CalcPosHashCode(0, 1, 2));
        //    range.Add(SkillHandler.Instance.CalcPosHashCode(-1, 1, 2));
        //    range.Add(SkillHandler.Instance.CalcPosHashCode(0, 2, 2));
        //    range.Add(SkillHandler.Instance.CalcPosHashCode(1, -1, 2));
        //    range.Add(SkillHandler.Instance.CalcPosHashCode(0, -1, 2));
        //    range.Add(SkillHandler.Instance.CalcPosHashCode(-1, -1, 2));
        //    range.Add(SkillHandler.Instance.CalcPosHashCode(0, -2, 2));
        //}

        /// <summary>
        ///     对指定目标附加伤害
        /// </summary>
        /// <param name="sActor">原目标</param>
        /// <param name="dActor">对象目标</param>
        /// <param name="damage">伤害值</param>
        public void ApplyDamage(Actor sActor, Actor dActor, int damage, bool doublehate, SkillArg arg2 = null) {
            if ((DateTime.Now - dActor.Status.attackStamp).TotalSeconds > 5) {
                dActor.Status.attackStamp = DateTime.Now;
                dActor.Status.attackingActors.Clear();
                if (!dActor.Status.attackingActors.Contains(sActor))
                    dActor.Status.attackingActors.Add(sActor);
            }
            else {
                if (!dActor.Status.attackingActors.Contains(sActor))
                    dActor.Status.attackingActors.Add(sActor);
            }

            if (sActor.type == ActorType.PC) WeaponWorn((ActorPC)sActor);
            if (dActor.type == ActorType.PC && damage > dActor.MaxHP / 100) ArmorWorn((ActorPC)dActor);

            if (arg2 != null && arg2.skill != null && arg2.skill.ID == 2541 && !arg2.flag.Contains(AttackFlag.MISS) &&
                !arg2.flag.Contains(AttackFlag.AVOID) && !arg2.flag.Contains(AttackFlag.GUARD) &&
                !arg2.flag.Contains(AttackFlag.AVOID2))
                //this.CreateAutoCastInfo()
                arg2.autoCast.Add(Instance.CreateAutoCastInfo(2542, arg2.skill.Level, 1000));

            //3转剑35级技能(已完成副职逻辑)
            if (dActor.Status.Pressure_lv > 0) {
                var level = dActor.Status.Pressure_lv;
                float[] hprank = { 0.2f, 0.2f, 0.25f, 0.25f, 0.3f };
                var factor = 3f + 0.3f * level;
                var pc = (ActorPC)dActor;
                //不管是主职还是副职,确定技能存在
                if (pc.Skills3.ContainsKey(1113) || pc.DualJobSkills.Exists(x => x.ID == 1113))
                    for (var i = 1; i < level; i++)
                        if (pc.HP < (uint)(pc.MaxHP * hprank[i - 1]) && pc.HP > damage)
                            if (Global.Random.Next(0, 100) <= level * 10) {
                                var map = MapManager.Instance.GetMap(dActor.MapID);
                                var affected = map.GetActorsArea(dActor, 400, false);
                                var realAffected = new List<Actor>();
                                arg2 = new SkillArg();
                                arg2.Init();
                                //获取副职AutoHeal的等级
                                var duallv = 0;
                                if (pc.DualJobSkills.Exists(x => x.ID == 1113))
                                    duallv = pc.DualJobSkills.FirstOrDefault(x => x.ID == 1113).Level;

                                //这里取主职的AutoHeal等级
                                var mainlv = 0;
                                if (pc.Skills3.ContainsKey(1113))
                                    mainlv = pc.Skills3[1113].Level;

                                //这里取等级最高的AutoHeal等级
                                var level2 = Math.Max(duallv, mainlv);

                                arg2.skill = SkillFactory.Instance.GetSkill(1113, (byte)level2);
                                var duallv2 = 0;
                                //不屈意志触发模块
                                if (pc.DualJobSkills.Exists(x => x.ID == 1100))
                                    duallv2 = pc.DualJobSkills.FirstOrDefault(x => x.ID == 1100).Level;

                                //这里取主职的不屈斗志等级
                                var mainlv2 = 0;
                                if (pc.Skills3.ContainsKey(1100))
                                    mainlv2 = pc.Skills3[1100].Level;

                                //这里取等级最高的不屈斗志等级
                                var level1100 = Math.Max(duallv2, mainlv2);
                                if (level1100 != 0 && pc.Buff.NoRegen == false) {
                                    var recoveryrate = new[] { 0.24f, 0.22f, 0.20f, 0.18f, 0.16f };
                                    arg2 = new SkillArg();
                                    arg2.Init();
                                    arg2.skill = SkillFactory.Instance.GetSkill(2066, 5);
                                    var hpheal = (int)(pc.MaxHP * recoveryrate[level2 - 1]);
                                    var mpheal = (int)(pc.MaxMP * recoveryrate[level2 - 1]);
                                    var spheal = (int)(pc.MaxSP * recoveryrate[level2 - 1]);
                                    arg2.hp.Add(hpheal);
                                    arg2.mp.Add(mpheal);
                                    arg2.sp.Add(spheal);
                                    arg2.flag.Add(AttackFlag.HP_HEAL | AttackFlag.SP_HEAL | AttackFlag.MP_HEAL |
                                                  AttackFlag.NO_DAMAGE);
                                    pc.HP += (uint)hpheal;
                                    pc.MP += (uint)mpheal;
                                    pc.SP += (uint)spheal;
                                    if (pc.HP > pc.MaxHP)
                                        pc.HP = pc.MaxHP;
                                    if (pc.MP > pc.MaxMP)
                                        pc.MP = pc.MaxMP;
                                    if (pc.SP > pc.MaxSP)
                                        pc.SP = pc.MaxSP;
                                    ShowEffect(pc, pc, 4321);
                                    Instance.ShowVessel(pc, -hpheal, -mpheal, -spheal);
                                    MapManager.Instance.GetMap(pc.MapID)
                                        .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.HPMPSP_UPDATE, arg2, pc,
                                            true);
                                }

                                foreach (var act in affected) {
                                    arg2.argType = SkillArg.ArgType.Attack;
                                    arg2.flag.Add(AttackFlag.HP_DAMAGE);
                                    arg2.type = ATTACK_TYPE.BLOW;
                                    arg2.hp.Add(damage);
                                    arg2.sp.Add(0);
                                    arg2.mp.Add(0);
                                    arg2.affectedActors.Add(act);
                                    Instance.PhysicalAttack(dActor, realAffected, arg2, Elements.Neutral, factor);
                                    ShowEffect((ActorPC)dActor, act, 4002);
                                    ShowEffect((ActorPC)dActor, dActor, 4321);
                                    Instance.PushBack(dActor, act, 4);
                                    if (Instance.CanAdditionApply(sActor, act, DefaultAdditions.Stun, level * 10)) {
                                        var stun = new Stun(arg2.skill, act, 4000);
                                        ApplyAddition(act, stun);
                                    }
                                }
                            }
            }

            //如果玩家有被动buff AutoHeal
            //自动治疗(已完成副职逻辑)
            if (dActor.type == ActorType.PC && dActor.Status.Additions.ContainsKey("AutoHeal") &&
                !dActor.Buff.NoRegen) {
                var pc = (ActorPC)dActor;
                //不管是主职还是副职
                //如果玩家加了被动技能AutoHeal,并且玩家有AutoHeal这个技能
                if (pc.Skills3.ContainsKey(1109) || pc.DualJobSkills.Exists(x => x.ID == 1109)) {
                    //获取AutoHeal的等级
                    //int level = pc.Skills3[1109].Level;
                    //获取副职AutoHeal的等级
                    var duallv = 0;
                    if (pc.DualJobSkills.Exists(x => x.ID == 1109))
                        duallv = pc.DualJobSkills.FirstOrDefault(x => x.ID == 1109).Level;

                    //这里取主职的AutoHeal等级
                    var mainlv = 0;
                    if (pc.Skills3.ContainsKey(1109))
                        mainlv = pc.Skills3[1109].Level;

                    //这里取等级最高的AutoHeal等级
                    var level = Math.Max(duallv, mainlv);
                    //声明触发治疗标记
                    var active = false;
                    //触发治疗的血线
                    var activerate = new[] { 0.2f, 0.4f, 0.6f, 0.7f, 0.8f };

                    //遍历触发血线数组
                    for (var i = 1; i <= level; i++)
                        //如果玩家当前的血量 小于触发血线
                        if (pc.HP < (uint)(dActor.MaxHP * activerate[i - 1]) && pc.HP > damage)
                            // 40%机率触发治疗
                            if (Global.Random.Next(1, 100) <= 40) {
                                active = true;
                                break;
                            }

                    if (active) {
                        //自动咏唱Healing
                        var autoheal = new SkillArg();
                        //获取副职Healing的等级
                        var duallv2 = 0;
                        if (pc.DualJobSkills.Exists(x => x.ID == 3054))
                            duallv2 = pc.DualJobSkills.FirstOrDefault(x => x.ID == 3054).Level;

                        //这里取主职的Healing等级
                        var mainlv2 = 0;
                        if (pc.Skills.ContainsKey(3054))
                            mainlv2 = pc.Skills[3054].Level;

                        //这里取等级最高的Healing等级
                        var level2 = Math.Max(duallv2, mainlv2);
                        if (level2 == 0) level2 = 1; //習得していない場合はヒーリングLv1×0.8倍の回復量になる-wiki
                        var skill = SkillFactory.Instance.GetSkill(3054, (byte)level2);
                        //SagaDB.Skill.Skill skill = SagaDB.Skill.SkillFactory.Instance.GetSkill(3054, pc.Skills[3054].Level);
                        autoheal.sActor = pc.ActorID;
                        autoheal.dActor = pc.ActorID;
                        autoheal.skill = skill;
                        autoheal.argType = SkillArg.ArgType.Cast;
                        autoheal.useMPSP = false;
                        MapClient.FromActorPC(pc).OnSkillCastComplete(autoheal);
                    }
                }
            }


            //不屈の闘志(已完成副职逻辑)
            if (dActor.type == ActorType.PC && dActor.Status.Additions.ContainsKey("不屈の闘志")) {
                var pc = (ActorPC)dActor;
                //不管是主职还是副职
                //如果玩家加了被动技能 不屈的斗志
                if (pc.Skills3.ContainsKey(1100) || pc.DualJobSkills.Exists(x => x.ID == 1100)) {
                    //获取不屈的斗志的等级
                    //int level = pc.Skills3[1100].Level;
                    //这里取副职的不屈的斗志等级
                    var duallv = 0;
                    if (pc.DualJobSkills.Exists(x => x.ID == 1100))
                        duallv = pc.DualJobSkills.FirstOrDefault(x => x.ID == 1100).Level;

                    //这里取主职的不屈的斗志等级
                    var mainlv = 0;
                    if (pc.Skills3.ContainsKey(1100))
                        mainlv = pc.Skills3[1100].Level;

                    //这里取等级最高的不屈的斗志进行判定
                    var level = Math.Max(duallv, mainlv);

                    //触发治疗的血线
                    var activerate = new[] { 0.08f, 0.16f, 0.24f, 0.32f, 0.4f };
                    //治疗量
                    var recoveryrate = new[] { 0.24f, 0.22f, 0.20f, 0.18f, 0.16f };
                    //遍历触发血线数组
                    for (var i = 1; i < level; i++)
                        //如果玩家当前的血量 小于触发血线
                        if (pc.HP < (uint)(dActor.MaxHP * activerate[i - 1]) && pc.HP > damage) {
                            if (dActor.Buff.NoRegen)
                                break;
                            arg2 = new SkillArg();
                            arg2.Init();
                            ShowEffect(pc, pc, 4321);
                            var hpheal = (int)(dActor.MaxHP * recoveryrate[i - 1]);
                            var mpheal = (int)(dActor.MaxMP * recoveryrate[i - 1]);
                            var spheal = (int)(dActor.MaxSP * recoveryrate[i - 1]);
                            arg2.hp.Add(hpheal);
                            arg2.mp.Add(mpheal);
                            arg2.sp.Add(spheal);
                            arg2.flag.Add(AttackFlag.HP_HEAL | AttackFlag.SP_HEAL | AttackFlag.MP_HEAL |
                                          AttackFlag.NO_DAMAGE);
                            dActor.HP += (uint)hpheal;
                            dActor.MP += (uint)mpheal;
                            dActor.SP += (uint)spheal;
                            if (dActor.HP > dActor.MaxHP)
                                dActor.HP = dActor.MaxHP;
                            if (dActor.MP > dActor.MaxMP)
                                dActor.MP = dActor.MaxMP;
                            if (dActor.SP > dActor.MaxSP)
                                dActor.SP = dActor.MaxSP;
                            Instance.ShowVessel(pc, -hpheal, -mpheal, -spheal);
                            MapManager.Instance.GetMap(dActor.MapID)
                                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.HPMPSP_UPDATE, arg2, dActor, true);
                        }
                }
            }

            if ((dActor.type == ActorType.MOB || dActor.type == ActorType.PET) && damage >= 0) {
                Actor attacker;

                //凭依中仇恨转移到寄主
                if (sActor.type == ActorType.PC) {
                    var pc = (ActorPC)sActor;
                    if (pc.PossessionTarget != 0) {
                        var possession = MapManager.Instance.GetMap(pc.MapID).GetActor(pc.PossessionTarget);
                        if (possession != null) {
                            if (possession.type == ActorType.PC)
                                attacker = possession;
                            else
                                attacker = sActor;
                        }
                        else {
                            attacker = sActor;
                        }
                    }
                    else {
                        attacker = sActor;
                    }
                }
                else {
                    attacker = sActor;
                }

                if (dActor.type == ActorType.MOB) {
                    var mob = (MobEventHandler)dActor.e;
                    if (sActor.Status.Additions.ContainsKey("柔和魔法"))
                        mob.AI.OnAttacked(attacker, damage / 2);
                    else
                        mob.AI.OnAttacked(attacker, damage);
                    if (doublehate)
                        mob.AI.OnAttacked(attacker, damage * 2);
                }
                else {
                    var mob = (PetEventHandler)dActor.e;
                    if (sActor.Status.Additions.ContainsKey("柔和魔法"))
                        mob.AI.OnAttacked(attacker, damage / 2);
                    else
                        mob.AI.OnAttacked(attacker, damage);
                    if (doublehate)
                        mob.AI.OnAttacked(attacker, damage * 2);
                }
            }

            if (dActor.type == ActorType.PC) {
                //如果凭依中受攻击解除凭依
                //TODO: 支援スキル使用時の憑依解除設定
                var pc = (ActorPC)dActor;
                if (pc.Online) {
                    var client = MapClient.FromActorPC(pc);
                    if (client.Character.Buff.GetReadyPossession) {
                        client.Character.Buff.GetReadyPossession = false;
                        client.Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null,
                            client.Character, true);
                        if (client.Character.Tasks.ContainsKey("Possession")) {
                            client.Character.Tasks["Possession"].Deactivate();
                            client.Character.Tasks.Remove("Possession");
                        }
                    }

                    if (dActor.Status.Additions.ContainsKey("Hiding")) {
                        dActor.Status.Additions["Hiding"].AdditionEnd();
                        dActor.Status.Additions.Remove("Hiding");
                    }

                    if (dActor.Status.Additions.ContainsKey("fish")) {
                        dActor.Status.Additions["fish"].AdditionEnd();
                        dActor.Status.Additions.Remove("fish");
                    }

                    if (dActor.Status.Additions.ContainsKey("Cloaking")) {
                        dActor.Status.Additions["Cloaking"].AdditionEnd();
                        dActor.Status.Additions.Remove("Cloaking");
                    }

                    if (dActor.Status.Additions.ContainsKey("IAmTree")) {
                        dActor.Status.Additions["IAmTree"].AdditionEnd();
                        dActor.Status.Additions.Remove("IAmTree");
                    }

                    if (dActor.Status.Additions.ContainsKey("Invisible")) {
                        dActor.Status.Additions["Invisible"].AdditionEnd();
                        dActor.Status.Additions.Remove("Invisible");
                    }
                }
            }

            //魔力吸收
            if (dActor.Status.Additions.ContainsKey("Desist") && damage >= 0) {
                var desistfactor = (dActor.Status.Additions["Desist"] as DefaultBuff).Variable["Desist"] / 100.0f;
                var mpdesist = (int)Math.Floor(damage * desistfactor);
                if (dActor.MaxMP < dActor.MP + mpdesist)
                    dActor.MP = dActor.MaxMP;
                else
                    dActor.MP += (uint)mpdesist;
                MapManager.Instance.GetMap(dActor.MapID)
                    .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.HPMPSP_UPDATE, null, dActor, true);
            }

            //如果对象目标死亡
            if (dActor.HP == 0) {
                if (sActor.Status.Additions.ContainsKey("Efuikasu")) {
                    if (sActor.KillingMarkSoulUse)
                        sActor.KillingMarkCounter = Math.Min(++sActor.KillingMarkCounter, 10);
                    else
                        sActor.KillingMarkCounter = Math.Min(++sActor.KillingMarkCounter, 20);
                }

                if (!dActor.Buff.Dead) {
                    if (dActor.type == ActorType.PC) {
                        var pc = (ActorPC)dActor;
                        if (pc.Pet != null)
                            if (pc.Pet.Ride) {
                                var eh = (PCEventHandler)pc.e;
                                var p = new CSMG_ITEM_MOVE();
                                p.data = new byte[11];
                                if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.PET)) {
                                    var item = pc.Inventory.Equipments[EnumEquipSlot.PET];
                                    if (item.Durability != 0) item.Durability--;
                                    eh.Client.SendItemInfo(item);
                                    eh.Client.SendSystemMessage(string.Format(
                                        LocalManager.Instance.Strings.PET_FRIENDLY_DOWN, item.BaseData.name));
                                    var arg = new EffectArg();
                                    arg.actorID = eh.Client.Character.ActorID;
                                    arg.effectID = 8044;
                                    eh.OnShowEffect(eh.Client.Character, arg);
                                    p.InventoryID = item.Slot;
                                    p.Target = ContainerType.BODY;
                                    p.Count = 1;
                                    eh.Client.OnItemMove(p);
                                }
                                //return;
                            }

                        //凭依者死亡自动解除凭依
                        if (pc.PossessionTarget != 0) {
                            var p = new CSMG_POSSESSION_CANCEL();
                            p.PossessionPosition = PossessionPosition.NONE;
                            MapClient.FromActorPC(pc).OnPossessionCancel(p);
                        }

                        if (((ActorPC)dActor).Mode != PlayerMode.COLISEUM_MODE &&
                            ((ActorPC)dActor).Mode != PlayerMode.KNIGHT_WAR)
                            ExperienceManager.Instance.DeathPenalty((ActorPC)dActor);
                        //处理WRP
                        if (sActor.type == ActorType.PC) {
                            var map = MapManager.Instance.GetMap(pc.MapID);
                            if (map.Info.Flag.Test(MapFlags.Wrp))
                                ExperienceManager.Instance.ProcessWrp((ActorPC)sActor, pc);
                        }
                    }

                    if (dActor.type == ActorType.MOB) {
                        var mob = (ActorMob)dActor;
                        if (sActor.type == ActorType.PC) {
                            var pc = (ActorPC)sActor;
                            var eh = (PCEventHandler)pc.e;
                            Map map; // = SagaMap.Manager.MapManager.Instance.GetMap(dActor.MapID);
                            List<Actor> eventactors; // = map.GetActorsArea(dActor, 3000, false, true);
                            List<ActorPC> owners; // = new List<ActorPC>();
                            if (Instance.isBossMob(mob) && ((MobEventHandler)mob.e).AI.SpawnDelay >= 1800000) {
                                map = MapManager.Instance.GetMap(dActor.MapID);
                                eventactors = map.GetActorsArea(dActor, 12700, false, true);
                                eventactors = eventactors.Where(x => x.type == ActorType.PC && (x as ActorPC).Online)
                                    .ToList();
                                foreach (var item in eventactors) {
                                    MapClient.FromActorPC((ActorPC)item)
                                        .SendAnnounce("本次: [" + mob.Name + "]已经完成击杀,设置系召唤系技能造成的伤害不被统计为你的个人伤害.");
                                    MapClient.FromActorPC((ActorPC)item).SendAnnounce("正在分配击杀......");
                                }

                                owners = new List<ActorPC>();
                                foreach (var item in ((MobEventHandler)mob.e).AI.DamageTable) {
                                    var act = eventactors.FirstOrDefault(x => x.ActorID == item.Key);
                                    if (act != null && act.type == ActorType.PC && (act as ActorPC).Online) {
                                        MapClient.FromActorPC((ActorPC)act)
                                            .SendAnnounce(string.Format("本次击杀: [{0}],你贡献了: {1}点伤害", mob.Name,
                                                item.Value));
                                        owners.Add((ActorPC)act);
                                        if ((act as ActorPC).PossesionedActors.Count > 0)
                                            foreach (var pitem in (act as ActorPC).PossesionedActors) {
                                                if (!pitem.Online)
                                                    continue;
                                                if (((MobEventHandler)mob.e).AI.DamageTable
                                                    .Where(x => x.Key == pitem.ActorID).ToList().Count == 0 &&
                                                    pitem.MapID == mob.MapID) {
                                                    MapClient.FromActorPC(pitem)
                                                        .SendAnnounce(string.Format("本次击杀: [{0}],你未提供伤害贡献,但是你混到了凭依击杀",
                                                            mob.Name));
                                                    owners.Add(pitem);
                                                }
                                            }

                                        if ((act as ActorPC).PossessionTarget != 0)
                                            if (((MobEventHandler)mob.e).AI.DamageTable
                                                .Where(x => x.Key == (act as ActorPC).PossessionTarget).ToList()
                                                .Count == 0 && act.MapID == mob.MapID) {
                                                var cli = MapClient.FromActorPC((ActorPC)eventactors.FirstOrDefault(x =>
                                                    x.ActorID == (act as ActorPC).PossessionTarget));
                                                if (cli != null) {
                                                    cli.SendAnnounce(string.Format("本次击杀: [{0}],你没有提供伤害贡献,但是你混到了凭依跑者击杀",
                                                        mob.Name));
                                                    owners.Add((ActorPC)eventactors.First(x =>
                                                        x.ActorID == (act as ActorPC).PossessionTarget));
                                                }
                                            }

                                        if ((act as ActorPC).Party != null)
                                            foreach (var ptitem in (act as ActorPC).Party.Members)
                                                if (((MobEventHandler)mob.e).AI.DamageTable
                                                    .Where(x => x.Key == ptitem.Value.ActorID).ToList().Count == 0 &&
                                                    ptitem.Value.Online && ptitem.Value.MapID == mob.MapID) {
                                                    MapClient.FromActorPC(ptitem.Value)
                                                        .SendAnnounce(string.Format("本次击杀: [{0}],你没有提供伤害贡献,但是你混到了组队击杀",
                                                            mob.Name));
                                                    owners.Add(ptitem.Value);
                                                }
                                    }
                                }

                                foreach (var ac in owners) {
                                    if (ac == null)
                                        continue;
                                    if (!ac.Online)
                                        continue;
                                    var ieh = (PCEventHandler)ac.e;
                                    ieh.Client.EventMobKilled(mob);
                                    ieh.Client.QuestMobKilled(mob, false);
                                }
                            }
                            else {
                                //处理任务信息
                                if (pc.Party != null) {
                                    foreach (var tmp in pc.Party.Members.Values) {
                                        if (!tmp.Online) continue;
                                        if (tmp == pc) continue;
                                        ((PCEventHandler)tmp.e).Client.QuestMobKilled(mob, true);
                                    }

                                    eh.Client.QuestMobKilled(mob, false);
                                }
                                else {
                                    eh.Client.QuestMobKilled(mob, false);
                                }
                            }

                            map = MapManager.Instance.GetMap(dActor.MapID);
                            eventactors = map.GetActorsArea(dActor, 3000, false, true);
                            owners = new List<ActorPC>();
                            foreach (var ac in eventactors)
                                if (((MobEventHandler)mob.e).AI.DamageTable.ContainsKey(ac.ActorID) &&
                                    ac.type == ActorType.PC)
                                    owners.Add((ActorPC)ac);
                            foreach (var i in owners) {
                                if (!i.Online) continue;
                                if (i == null) continue;
                                var ieh = (PCEventHandler)i.e;
                                ieh.Client.EventMobKilled(mob);
                            }
                        }
                        //ExperienceManager.Instance.ProcessMobExp(mob);
                    }
                    //if (sActor.type == ActorType.PC && dActor.type == ActorType.PC && sActor != dActor)
                    //{
                    //    ActorPC pc = (ActorPC)sActor;
                    //    ActorPC dpc = (ActorPC)dActor;
                    //    Map map = Manager.MapManager.Instance.GetMap(sActor.MapID);
                    //    if (pc.Mode == PlayerMode.KNIGHT_EAST || pc.Mode == PlayerMode.KNIGHT_WEST)
                    //        pc.Mode = PlayerMode.NORMAL;
                    //    if (dpc.Mode == PlayerMode.KNIGHT_EAST || dpc.Mode == PlayerMode.KNIGHT_WEST)
                    //        dpc.Mode = PlayerMode.NORMAL;
                    //    map.Announce("玩家 " + dActor.Name + " 被 " + sActor.Name + " 擊殺了。");
                    //    pc.TInt["PVP连杀"]++;
                    //    if (pc.TInt["PVP连杀"] > pc.TInt["PVP最大连杀"]) pc.TInt["PVP最大连杀"] = pc.TInt["PVP连杀"];
                    //    if (pc.TInt["PVP连杀"] > 2)
                    //        map.Announce(sActor.Name + "連續擊殺了 " + pc.TInt["PVP连杀"].ToString() + " 人！");
                    //    if (dpc.TInt["PVP连杀"] > 2)
                    //        map.Announce(sActor.Name + "結束了 " + dpc.Name + " 的" + dpc.TInt["PVP连杀"].ToString() + " 連續擊殺。");

                    //    if (sActor.type == ActorType.PC)
                    //    {
                    //        ActorPC spc = (ActorPC)sActor;
                    //        if (spc.Mode == PlayerMode.NORMAL)
                    //        {
                    //            MapClient.FromActorPC(spc).TitleProccess(spc, 10, 1);
                    //        }
                    //    }

                    //}
                    //if (dActor.type == ActorType.PC && sActor != dActor)
                    //{
                    //    ActorPC pc = (ActorPC)dActor;
                    //    if (pc.Online)
                    //        pc.TInt["死亡统计"]++;
                    //    if (sActor.type == ActorType.PC)
                    //    {
                    //        ActorPC pc2 = (ActorPC)sActor;
                    //        if (pc2.Online)
                    //        {
                    //            pc2.TInt["击杀统计"]++;
                    //            MapClient.FromActorPC(pc2).SendSystemMessage("击杀数：" + pc2.TInt["击杀统计"].ToString() +
                    //                " 最高连杀数：" + pc2.TInt["PVP最大连杀"].ToString() + " 死亡数：" + pc2.TInt["死亡统计"].ToString() +
                    //                " 已造成伤害：" + pc2.TInt["伤害统计"].ToString() + " 已治疗：" + pc2.TInt["治疗统计"].ToString());
                    //        }
                    //    }
                    //}
                    dActor.e.OnDie();
                }
            }
        }

        private void GetlongARROW(ActorPC pc, SkillArg arg) {
            if (pc.Skills.ContainsKey(2035) || pc.Skills2_2.ContainsKey(2035) ||
                pc.DualJobSkills.Exists(x => x.ID == 2035)) {
                if ((pc.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].BaseData.itemType == ItemType.ARROW ||
                     pc.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].BaseData.itemType == ItemType.BULLET) &&
                    pc.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].Stack > 0) {
                    var duallv = 0;
                    if (pc.DualJobSkills.Exists(x => x.ID == 2035))
                        duallv = pc.DualJobSkills.FirstOrDefault(x => x.ID == 2035).Level;

                    var mainlv = 0;
                    if (pc.Skills.ContainsKey(2035))
                        mainlv = pc.Skills[2035].Level;

                    var mainlv2 = 0;
                    if (pc.Skills2_2.ContainsKey(2035))
                        mainlv2 = pc.Skills2_2[2035].Level;

                    var maxlv = Math.Max(duallv, mainlv);
                    maxlv = Math.Max(maxlv, mainlv2);
                    if (arg.skill == null) //普通攻击
                    {
                        if (Global.Random.Next(0, 99) >= maxlv * 10)
                            MapClient.FromActorPC(pc).DeleteItem(pc.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].Slot,
                                1, false);
                    }
                    else {
                        if (Global.Random.Next(0, 99) >= maxlv * 10)
                            MapClient.FromActorPC(pc).DeleteItem(pc.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].Slot,
                                1, false);
                    }
                }
            }
            else {
                MapClient.FromActorPC(pc).DeleteItem(pc.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].Slot, 1, false);
            }
        }

        private void GetlongCARD(ActorPC pc, SkillArg arg) {
            if (pc.Skills.ContainsKey(2035) || pc.Skills2_2.ContainsKey(2035) ||
                pc.DualJobSkills.Exists(x => x.ID == 2035)) {
                if ((pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.THROW ||
                     pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.CARD) &&
                    pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].Stack > 0) {
                    var duallv = 0;
                    if (pc.DualJobSkills.Exists(x => x.ID == 2035))
                        duallv = pc.DualJobSkills.FirstOrDefault(x => x.ID == 2035).Level;

                    var mainlv = 0;
                    if (pc.Skills.ContainsKey(2035))
                        mainlv = pc.Skills[2035].Level;

                    var mainlv2 = 0;
                    if (pc.Skills2_2.ContainsKey(2035))
                        mainlv2 = pc.Skills2_2[2035].Level;

                    var maxlv = Math.Max(duallv, mainlv);
                    maxlv = Math.Max(maxlv, mainlv2);
                    if (arg.skill == null) //普通攻击
                    {
                        if (Global.Random.Next(0, 99) >= maxlv * 10)
                            MapClient.FromActorPC(pc).DeleteItem(pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].Slot,
                                1, false);
                    }
                    else {
                        if (Global.Random.Next(0, 99) >= maxlv * 10)
                            MapClient.FromActorPC(pc).DeleteItem(pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].Slot,
                                1, false);
                    }
                }
            }
            else {
                MapClient.FromActorPC(pc).DeleteItem(pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].Slot, 1, false);
            }
        }

        /// <summary>
        ///     取得锁定的目标
        /// </summary>
        /// <param name="map"></param>
        /// <param name="actor"></param>
        /// <returns></returns>
        public Actor GetdActor(Actor sActor, SkillArg arg) {
            if (sActor.type != ActorType.PC) return null;
            Actor target = null;
            var map = MapManager.Instance.GetMap(sActor.MapID);
            target = map.GetActor((uint)sActor.TInt["targetID"]);
            if (target == null || target.HP <= 0) {
                if (((ActorPC)sActor).CInt["自动锁定模式"] == 0) {
                    MapClient.FromActorPC((ActorPC)sActor).SendSystemMessage("没有锁定的目标，输入/autolock开启与关闭自动锁定模式。");
                    return null;
                }

                var actors = map.GetActorsArea(sActor, (short)((arg.skill.Range + 2) * 100), false);
                var Targets = new List<Actor>();
                foreach (var item in actors)
                    if (Instance.CheckValidAttackTarget(sActor, item))
                        Targets.Add(item);
                if (Targets.Count < 1) {
                    MapClient.FromActorPC((ActorPC)sActor).SendSystemMessage("技能范围内没有目标可以锁定。");
                    return null;
                }

                target = Targets[0];
            }

            if (arg.skill.Range + 2 <
                Math.Max(Math.Abs(sActor.X - target.X) / 100, Math.Abs(sActor.Y - target.Y) / 100)) {
                MapClient.FromActorPC((ActorPC)sActor).SendSystemMessage("【你当前锁定的目标】超出了技能的极限范围。");
                return null;
            }

            if (target.HP <= 0) {
                MapClient.FromActorPC((ActorPC)sActor).SendSystemMessage("锁定的目标已死亡或已不存在。");
                return null;
            }

            var dir = map.CalcDir(sActor.X, sActor.Y, target.X, target.Y);
            map.MoveActor(Map.MOVE_TYPE.STOP, sActor, new short[2] { sActor.X, sActor.Y }, dir, sActor.Speed, true);

            return target;
        }

        public static void SendSystemMessage(Actor pc, string message) {
            if (pc.type == ActorType.PC)
                MapClient.FromActorPC((ActorPC)pc).SendSystemMessage(message);
        }

        /// <summary>
        ///     检查是否有攻击者对目标释放的DEBUFF
        /// </summary>
        /// <param name="sActor">攻击者</param>
        /// <param name="dActor">目标</param>
        /// <param name="type">0仅物理   1仅魔法</param>
        public void checkdebuff(Actor sActor, Actor dActor, SkillArg arg, byte type) {
            if (type == 0) //物理
            {
                if (sActor.Status.Additions.ContainsKey("ApplyPoison"))
                    if (Global.Random.Next(0, 100) < sActor.TInt["ApplyPoisonRate"]) {
                        var factor = 1f;
                        if (sActor.TInt["毒素研究提升"] != 0)
                            factor = 1f + sActor.TInt["毒素研究提升"] * 0.5f;
                        var damage = Instance.CalcDamage(false, sActor, dActor, arg, DefType.MDef, Elements.Holy, 50,
                            factor);
                        var p = new Poison(arg.skill, dActor, damage);
                        ApplyAddition(dActor, p);
                        ShowEffectOnActor(dActor, 4126);
                    }
            }
            else if (type == 2) //魔法
            {
            }
            //通用
        }

        /// <summary>
        ///     检查卡片对伤害的影响
        /// </summary>
        /// <param name="sActor">攻击者</param>
        /// <param name="dActor">目标</param>
        /// <param name="type">0仅物理   1仅魔法</param>
        public int checkirisbuff(Actor sActor, Actor dActor, SkillArg arg, byte type, int damage) {
            if (damage > 0 && dActor.Status.heal_attacked_iris > 0 && dActor.HP != damage) //圣母的加护
                if (dActor.Status.heal_attacked_iris * 1 >= Global.Random.Next(1, 100)) {
                    var heal = (uint)(dActor.MaxHP * 0.1f);
                    dActor.HP += heal;
                    if (dActor.HP > dActor.MaxHP) dActor.HP = dActor.MaxHP;
                    ShowVessel(dActor, (int)-heal);
                    ShowEffectOnActor(dActor, 4345);
                }

            if (sActor.type != ActorType.PC) return damage;
            var spc = (ActorPC)sActor;
            if (type == 0) //物理
            {
            }
            else if (type == 2) //魔法
            {
                if (damage < 0 && sActor.Status.heal_70up_iris > 0 && sActor.MP > sActor.MaxMP * 0.7) //治愈之音
                    damage += (int)(damage * 0.05 * sActor.Status.heal_70up_iris);
            }

            if (damage > 0 && sActor.Status.atk_70up_iris > 0 && sActor.HP > sActor.MaxHP * 0.7) //全力以赴
                damage += (int)(damage * 0.01 * sActor.Status.atk_70up_iris);
            if (damage > 0 && sActor.Status.atkup_job40_iris > 0 && ((ActorPC)sActor).JobLevel3 < 40) //勤奋好学
                damage += (int)(damage * 0.01 * sActor.Status.atkup_job40_iris);
            if (damage > 0 && sActor.Status.spweap_atkup_iris > 0 &&
                spc.Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND)) //玩具达人
            {
                var it = spc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType;
                if (it == ItemType.EXSWORD || it == ItemType.EXGUN || it == ItemType.ETC_WEAPON)
                    damage += (int)(damage * 0.01 * sActor.Status.spweap_atkup_iris);
            }

            /*if(damage > 0 && spc.PlayerTitleID >=21 && spc.PlayerTitleID <= 23)
            {
                if (10 >= Global.Random.Next(1, 100) && !sActor.Status.Additions.ContainsKey("黑喵称号CD"))
                {
                    OtherAddition skill = new OtherAddition(null, sActor, "黑喵称号CD", 10000);
                    ApplyAddition(sActor, skill);
                    Seals(sActor, dActor, 1);
                    ShowEffectOnActor(dActor, 5270); ;
                }
            }
            if (damage > 0 && sActor.Status.Mammon_iris > 0)
            {
                if(!sActor.Status.Additions.ContainsKey("玛蒙之欲") && spc.TInt["玛蒙之欲未解放"] == 0)
                {
                    if (sActor.Status.Mammon_iris * 1 >= Global.Random.Next(1, 500))
                    {
                        OtherAddition skill = new OtherAddition(null, sActor, "玛蒙之欲", 10000);
                        skill.OnAdditionEnd += (s, e) =>
                        {
                            OtherAddition s2 = new OtherAddition(null, sActor, "玛蒙之欲解放", 5000);
                            s2.OnAdditionEnd += (t, y) => spc.TInt["玛蒙之欲未解放"] = 0;
                            ApplyAddition(sActor, s2);
                        };
                        ApplyAddition(sActor, skill);
                        spc.TInt["玛蒙之欲未解放"] = 1;
                        ShowEffectOnActor(sActor, 4469); ;
                    }
                }
                else if(sActor.Status.Additions.ContainsKey("玛蒙之欲解放"))
                {
                    RemoveAddition(sActor, "玛蒙之欲解放");
                    spc.TInt["玛蒙之欲未解放"] = 0;
                    int damagebouns = spc.TInt["玛蒙之欲伤害"] / 5;
                    damage += damagebouns;
                    ShowEffectOnActor(dActor, 5282);
                }
            }
            */
            //if (damage > dActor.HP) damage = (int)dActor.HP;
            return damage;
        }

        /// <summary>
        ///     检查各类BUFF对伤害的影响
        /// </summary>
        /// <param name="sActor">攻击者</param>
        /// <param name="dActor">目标</param>
        /// <param name="type">0仅物理   1仅魔法</param>
        public int checkbuff(Actor sActor, Actor dActor, SkillArg arg, byte type, int damage) {
            try {
                var d = damage;
                d = (int)(d * sActor.Status.DamageRate);

                if (dActor.type == ActorType.PC) {
                    var pc = (ActorPC)dActor;
                    if (pc.Pet != null)
                        if (pc.Pet.Ride) {
                            if (damage > 0)
                                d = (int)(damage * 2f);
                            else
                                d = (int)(damage * 0.3);
                        }

                    if (pc.Fictitious) {
                        ShowVessel(dActor, 0, damage);
                        ShowEffectOnActor(dActor, 4174);
                        if (damage > 0)
                            d = -damage;
                        else
                            d = damage;
                    }
                }

                if (dActor.Status.Additions.ContainsKey("替身术") && damage > 0)
                    if (dActor.TInt["替身术记录X"] != 0 && dActor.TInt["替身术记录Y"] != 0) {
                        var map = MapManager.Instance.GetMap(dActor.MapID);
                        d = 0;
                        var x = (byte)dActor.TInt["替身术记录X"];
                        var y = (byte)dActor.TInt["替身术记录Y"];
                        RemoveAddition(dActor, "替身术");
                        dActor.TInt["替身术记录X"] = 0;
                        dActor.TInt["替身术记录X"] = 0;
                        var px = Global.PosX8to16(x, map.Width);
                        var py = Global.PosY8to16(y, map.Height);
                        var x1 = Global.PosX16to8(dActor.X, map.Width);
                        var y1 = Global.PosY16to8(dActor.Y, map.Height);
                        ShowEffect(map, dActor, x1, y1, 4251);
                        var inv = new Invisible(null, dActor, 10000);
                        ApplyAddition(sActor, inv);
                        map.TeleportActor(dActor, px, py);
                    }

                if (dActor.Status.Additions.ContainsKey("ShieldReflect") &&
                    !(dActor.Status.Additions.ContainsKey("ShieldReflect") &&
                      sActor.Status.Additions.ContainsKey("ShieldReflect")) &&
                    sActor.type == ActorType.PC) //盾牌反射
                {
                    ShowEffectByActor(dActor, 5092);
                    //sActor.EP += 500;
                    CauseDamage(dActor, sActor, damage);
                    ShowVessel(sActor, damage);
                    d = 0;
                }

                if (sActor.Status.Additions.ContainsKey("ApplyPoison")) {
                    var fac = sActor.TInt["PoisonDamageUP"] / 100f;
                    if (dActor.Status.Additions.ContainsKey("Poison1"))
                        fac *= 1.5f;
                    var dp = CalcDamage(false, sActor, dActor, null, DefType.MDef, Elements.Dark, 50, fac);
                    d += dp;
                    Instance.ShowEffectOnActor(dActor, 8048);
                }

                if (sActor.Buff.魂之手)
                    d = damage * 2;
                if (dActor.Buff.魂之手) {
                    if (damage > 0)
                        d = damage * 3;
                    else
                        d = (int)(damage * 0.2f);
                }

                return d;
            }
            catch (Exception ex) {
                Logger.GetLogger().Error(ex, ex.Message);
                return 0;
            }
        }

        /// <summary>
        ///     对目标造成伤害（该函数统筹了CalcDamage() CauseDamage() ShowVessel()）
        /// </summary>
        /// <param name="IsPhyDamage">是否為物理傷害 1是 2不是</param>
        /// <param name="sActor">攻擊者</param>
        /// <param name="dActor">目標</param>
        /// <param name="arg"></param>
        /// <param name="defType">防禦類型</param>
        /// <param name="element">元素</param>
        /// <param name="eleValue">元素值</param>
        /// <param name="ATKBonus">倍率</param>
        /// <param name="ignore">無視防禦率</param>
        public void DoDamage(bool IsPhyDamage, Actor sActor, Actor dActor, SkillArg arg, DefType defType,
            Elements element, int eleValue, float ATKBonus, float ignore = 0) {
            DoDamage(IsPhyDamage, sActor, dActor, arg, defType, element, eleValue, ATKBonus, 0, 0, ignore = 0);
        }

        public void DoDamage(bool IsPhyDamage, Actor sActor, Actor dActor, SkillArg arg, DefType defType,
            Elements element, int eleValue, float ATKBonus, int scribonus, int cridamagebonusfloat, float ignore = 0) {
            try {
                var res = AttackResult.Hit;
                var damage = CalcDamage(IsPhyDamage, sActor, dActor, arg, defType, element, eleValue, ATKBonus, out res,
                    scribonus, cridamagebonusfloat, ignore);
                CauseDamage(sActor, dActor, damage);
                ShowVessel(dActor, damage, 0, 0, res);
                RemoveAddition(sActor, "Relement");
            }
            catch (Exception ex) {
                Logger.GetLogger().Error(ex, ex.Message);
            }
        }

        /// <summary>
        ///     計算傷害（實際不造成傷害！！）
        /// </summary>
        /// <param name="IsPhyDamage">是否為物理傷害 1是 2不是</param>
        /// <param name="sActor">攻擊者</param>
        /// <param name="dActor">目標</param>
        /// <param name="arg"></param>
        /// <param name="defType">防禦類型</param>
        /// <param name="element">元素</param>
        /// <param name="eleValue">元素值</param>
        /// <param name="ATKBonus">倍率</param>
        /// <param name="ignore">無視防禦率</param>
        /// <returns>傷害</returns>
        public int CalcDamage(bool IsPhyDamage, Actor sActor, Actor dActor, SkillArg arg, DefType defType,
            Elements element, int eleValue, float ATKBonus, float ignore = 0) {
            var res = AttackResult.Hit;
            return CalcDamage(IsPhyDamage, sActor, dActor, arg, defType, element, eleValue, ATKBonus, out res, 0, 0,
                ignore = 0);
        }

        public int CalcDamage(bool IsPhyDamage, Actor sActor, Actor dActor, SkillArg arg, DefType defType,
            Elements element, int eleValue, float ATKBonus, int scribonus, int cridamagebonus, float ignore = 0) {
            var res = AttackResult.Hit;
            return CalcDamage(IsPhyDamage, sActor, dActor, arg, defType, element, eleValue, ATKBonus, out res,
                scribonus, cridamagebonus, ignore);
        }

        /// <summary>
        ///     計算傷害（實際不造成傷害！！）
        /// </summary>
        /// <param name="IsPhyDamage">是否為物理傷害 1是 2不是</param>
        /// <param name="sActor">攻擊者</param>
        /// <param name="dActor">目標</param>
        /// <param name="arg"></param>
        /// <param name="defType">防禦類型</param>
        /// <param name="element">元素</param>
        /// <param name="eleValue">元素值</param>
        /// <param name="ATKBonus">倍率</param>
        /// <param name="ignore">無視防禦率</param>
        /// <returns>傷害</returns>
        public int CalcDamage(bool IsPhyDamage, Actor sActor, Actor dActor, SkillArg arg, DefType defType,
            Elements element, int eleValue, float ATKBonus, out AttackResult res, int scribonus, int cridamagebonus,
            float ignore = 0) {
            try {
                var damage = 0;
                int atk;
                var mindamage = 0;
                var maxdamage = 0;

                res = CalcAttackResult(sActor, dActor, sActor.Range > 3, 0, 0);
                if (dActor.Status.Additions.ContainsKey("Warn")) //警戒
                    if (res == AttackResult.Critical)
                        res = AttackResult.Hit;

                if (res == AttackResult.Critical) {
                    damage = (int)(damage * CalcCritBonus(sActor, dActor));
                    if (sActor.Status.Additions.ContainsKey("CriDamUp")) {
                        var rate = (sActor.Status.Additions["CriDamUp"] as DefaultPassiveSkill).Variable["CriDamUp"] /
                            100.0f + 1.0f;
                        damage = (int)(damage * rate);
                    }
                }

                if (IsPhyDamage) {
                    if (arg == null) {
                        mindamage = sActor.Status.min_atk1;
                        maxdamage = sActor.Status.max_atk1;
                    }
                    else {
                        //獲取攻擊力
                        switch (arg.type) {
                            case ATTACK_TYPE.BLOW:
                                mindamage = sActor.Status.min_atk1;
                                maxdamage = sActor.Status.max_atk1;
                                break;
                            case ATTACK_TYPE.SLASH:
                                mindamage = sActor.Status.min_atk2;
                                maxdamage = sActor.Status.max_atk2;
                                break;
                            case ATTACK_TYPE.STAB:
                                mindamage = sActor.Status.min_atk3;
                                maxdamage = sActor.Status.max_atk3;
                                break;
                        }
                    }

                    if (mindamage > maxdamage) maxdamage = mindamage;
                    //atk = (short)Global.Random.Next(mindamage, maxdamage);
                    //atk = (short)(atk * CalcElementBonus(sActor, dActor, element, 0, false) * ATKBonus);
                    atk = Global.Random.Next(mindamage, maxdamage);
                    //TODO: element bonus, range bonus
                    var eleBonus = CalcElementBonus(sActor, dActor, element, 0, false);

                    if (dActor.Status.Contract_Lv != 0) {
                        var tmpele = Elements.Neutral;
                        switch (dActor.Status.Contract_Lv) {
                            case 1:
                                tmpele = Elements.Fire;
                                break;
                            case 2:
                                tmpele = Elements.Water;
                                break;
                            case 3:
                                tmpele = Elements.Earth;
                                break;
                            case 4:
                                tmpele = Elements.Wind;
                                break;
                        }

                        if (tmpele == element)
                            eleBonus -= 0.15f;
                        else
                            eleBonus += 1.0f;
                    }

                    atk = (int)Math.Ceiling(atk * eleBonus * ATKBonus);
                    damage = CalcPhyDamage(sActor, dActor, defType, atk, ignore);

                    damage = CheckBuffForDamage(sActor, dActor, damage);
                    if (sActor.Status.Additions.ContainsKey("FrameHart")) //火心
                    {
                        var rate = (sActor.Status.Additions["FrameHart"] as DefaultBuff).Variable["FrameHart"];
                        damage = (int)(damage * ((double)rate / 100.0f));
                    }

                    if (sActor.Status.Additions.ContainsKey("ホークアイ")) //HAW站桩
                        damage = (int)(damage * ((sActor.Status.Additions["ホークアイ"] as DefaultBuff).Variable["ホークアイ"] /
                                                 100.0f));

                    if (sActor.type == ActorType.PC) {
                        var pc = (ActorPC)sActor;
                        if (pc.Skills2_1.ContainsKey(310) || pc.DualJobSkills.Exists(x => x.ID == 310)) //HAW2-1追魂箭
                        {
                            var duallv = 0;
                            if (pc.DualJobSkills.Exists(x => x.ID == 310))
                                duallv = pc.DualJobSkills.FirstOrDefault(x => x.ID == 310).Level;

                            var mainlv = 0;
                            if (pc.Skills2_1.ContainsKey(310))
                                mainlv = pc.Skills2_1[310].Level;

                            var level = Math.Max(duallv, mainlv);
                            if (dActor.Buff.Stun ||
                                dActor.Buff.Stone ||
                                dActor.Buff.Frosen ||
                                dActor.Buff.Poison ||
                                dActor.Buff.Sleep ||
                                dActor.Buff.SpeedDown ||
                                dActor.Buff.Confused ||
                                dActor.Buff.Paralysis ||
                                dActor.Buff.STRDown ||
                                dActor.Buff.VITDown ||
                                dActor.Buff.INTDown ||
                                dActor.Buff.DEXDown ||
                                dActor.Buff.AGIDown ||
                                dActor.Buff.MAGDown ||
                                dActor.Buff.MaxHPDown ||
                                dActor.Buff.MaxMPDown ||
                                dActor.Buff.MaxSPDown ||
                                dActor.Buff.MinAtkDown ||
                                dActor.Buff.MaxAtkDown ||
                                dActor.Buff.MinMagicAtkDown ||
                                dActor.Buff.MaxMagicAtkDown ||
                                dActor.Buff.DefDown ||
                                dActor.Buff.DefRateDown ||
                                dActor.Buff.MagicDefDown ||
                                dActor.Buff.MagicDefRateDown ||
                                dActor.Buff.ShortHitDown ||
                                dActor.Buff.LongHitDown ||
                                dActor.Buff.MagicHitDown ||
                                dActor.Buff.ShortDodgeDown ||
                                dActor.Buff.LongDodgeDown ||
                                dActor.Buff.MagicAvoidDown ||
                                dActor.Buff.CriticalRateDown ||
                                dActor.Buff.CriticalDodgeDown ||
                                dActor.Buff.HPRegenDown ||
                                dActor.Buff.MPRegenDown ||
                                dActor.Buff.SPRegenDown ||
                                dActor.Buff.AttackSpeedDown ||
                                dActor.Buff.CastSpeedDown ||
                                dActor.Buff.SpeedDown ||
                                dActor.Buff.Berserker)
                                if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND))
                                    if (pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType ==
                                        ItemType.BOW)
                                        damage = (int)(damage * (1.1f + 0.02f * level));
                        }

                        if (pc.Skills2_2.ContainsKey(314) || pc.DualJobSkills.Exists(x => x.ID == 314)) //GU2-2追魂刃
                        {
                            var duallv = 0;
                            if (pc.DualJobSkills.Exists(x => x.ID == 314))
                                duallv = pc.DualJobSkills.FirstOrDefault(x => x.ID == 314).Level;

                            var mainlv = 0;
                            if (pc.Skills2_2.ContainsKey(314))
                                mainlv = pc.Skills2_2[314].Level;

                            var level = Math.Max(duallv, mainlv);
                            if (dActor.Buff.Stun ||
                                dActor.Buff.Stone ||
                                dActor.Buff.Frosen ||
                                dActor.Buff.Poison ||
                                dActor.Buff.Sleep ||
                                dActor.Buff.SpeedDown ||
                                dActor.Buff.Confused ||
                                dActor.Buff.Paralysis ||
                                dActor.Buff.STRDown ||
                                dActor.Buff.VITDown ||
                                dActor.Buff.INTDown ||
                                dActor.Buff.DEXDown ||
                                dActor.Buff.AGIDown ||
                                dActor.Buff.MAGDown ||
                                dActor.Buff.MaxHPDown ||
                                dActor.Buff.MaxMPDown ||
                                dActor.Buff.MaxSPDown ||
                                dActor.Buff.MinAtkDown ||
                                dActor.Buff.MaxAtkDown ||
                                dActor.Buff.MinMagicAtkDown ||
                                dActor.Buff.MaxMagicAtkDown ||
                                dActor.Buff.DefDown ||
                                dActor.Buff.DefRateDown ||
                                dActor.Buff.MagicDefDown ||
                                dActor.Buff.MagicDefRateDown ||
                                dActor.Buff.ShortHitDown ||
                                dActor.Buff.LongHitDown ||
                                dActor.Buff.MagicHitDown ||
                                dActor.Buff.ShortDodgeDown ||
                                dActor.Buff.LongDodgeDown ||
                                dActor.Buff.MagicAvoidDown ||
                                dActor.Buff.CriticalRateDown ||
                                dActor.Buff.CriticalDodgeDown ||
                                dActor.Buff.HPRegenDown ||
                                dActor.Buff.MPRegenDown ||
                                dActor.Buff.SPRegenDown ||
                                dActor.Buff.AttackSpeedDown ||
                                dActor.Buff.CastSpeedDown ||
                                dActor.Buff.SpeedDown ||
                                dActor.Buff.Berserker)
                                if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND))
                                    if (pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType ==
                                        ItemType.SHORT_SWORD ||
                                        pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType ==
                                        ItemType.SWORD ||
                                        pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType ==
                                        ItemType.RAPIER)
                                        damage = (int)(damage * (1.1f + 0.02f * level));
                        }
                    }

                    if (dActor.Status.NeutralDamegeDown_rate > 0 && element == Elements.Neutral)
                        damage = (int)(damage * (1.0f - dActor.Status.NeutralDamegeDown_rate / 100.0f));
                    if (dActor.Status.NeutralDamegeDown_rate > 0 && element != Elements.Neutral)
                        damage = (int)(damage * (1.0f - dActor.Status.ElementDamegeDown_rate / 100.0f));
                    if (arg.skill != null)
                        if (sActor.Status.doubleUpList.Contains((ushort)arg.skill.ID))
                            atk *= 2;

                    //if (damage > atk)
                    //    damage = atk;
                    if (arg == null)
                        damage = (int)(damage * (1f - sActor.Status.damage_atk1_discount));
                    else
                        switch (arg.type) {
                            case ATTACK_TYPE.BLOW:
                                damage = (int)(damage * (1f - sActor.Status.damage_atk1_discount));
                                break;
                            case ATTACK_TYPE.SLASH:
                                damage = (int)(damage * (1f - sActor.Status.damage_atk2_discount));
                                break;
                            case ATTACK_TYPE.STAB:
                                damage = (int)(damage * (1f - sActor.Status.damage_atk3_discount));
                                break;
                        }

                    if (sActor.type == ActorType.PC && dActor.type == ActorType.PC)
                        damage = (int)(damage * Configuration.Configuration.Instance.PVPDamageRatePhysic);
                    if (damage <= 0) damage = 1;


                    //计算暴击增益
                    if (scribonus != 0)
                        if (res == AttackResult.Critical) {
                            damage = (int)(damage * (CalcCritBonus(sActor, dActor, scribonus) + cridamagebonus));
                            if (sActor.Status.Additions.ContainsKey("CriDamUp")) {
                                var rate =
                                    (sActor.Status.Additions["CriDamUp"] as DefaultPassiveSkill).Variable["CriDamUp"] /
                                    100.0f + 1.0f;
                                damage = (int)(damage * rate);
                            }
                        }

                    checkdebuff(sActor, dActor, arg, 0);
                }
                else {
                    mindamage = sActor.Status.min_matk;
                    maxdamage = sActor.Status.max_matk;
                    if (mindamage > maxdamage) maxdamage = mindamage;
                    atk = Global.Random.Next(mindamage, maxdamage);
                    if (sActor.type == ActorType.PC) {
                        //wiz3转JOB3,属性对无属性魔法增伤
                        var pci = sActor as ActorPC;
                        float rates = 0;
                        //不管是主职还是副职, 只要习得技能,则进入增伤判定
                        if ((pci.Skills3.ContainsKey(986) || pci.DualJobSkills.Exists(x => x.ID == 986)) &&
                            element == Elements.Neutral) {
                            //这里取副职的等级
                            var duallv = 0;
                            if (pci.DualJobSkills.Exists(x => x.ID == 986))
                                duallv = pci.DualJobSkills.FirstOrDefault(x => x.ID == 986).Level;

                            //这里取主职的等级
                            var mainlv = 0;
                            if (pci.Skills3.ContainsKey(986))
                                mainlv = pci.Skills3[986].Level;
                            rates = 0.02f + 0.002f * mainlv;
                            //int elements = (int)pci.WeaponElement[pci.WeaponElement];
                            var elements = pci.Status.attackElements_item[pci.WeaponElement]
                                           + pci.Status.attackElements_skill[pci.WeaponElement]
                                           + pci.Status.attackelements_iris[pci.WeaponElement];

                            //int elements = pci.Inventory.Equipments[SagaDB.Item.EnumEquipSlot.RIGHT_HAND].BaseData.element[SagaLib.Elements.Dark] +
                            //pci.Inventory.Equipments[SagaDB.Item.EnumEquipSlot.RIGHT_HAND].BaseData.element[SagaLib.Elements.Earth] +
                            //pci.Inventory.Equipments[SagaDB.Item.EnumEquipSlot.RIGHT_HAND].BaseData.element[SagaLib.Elements.Fire] +
                            //pci.Inventory.Equipments[SagaDB.Item.EnumEquipSlot.RIGHT_HAND].BaseData.element[SagaLib.Elements.Holy] +
                            //pci.Inventory.Equipments[SagaDB.Item.EnumEquipSlot.RIGHT_HAND].BaseData.element[SagaLib.Elements.Neutral] +
                            //pci.Inventory.Equipments[SagaDB.Item.EnumEquipSlot.RIGHT_HAND].BaseData.element[SagaLib.Elements.Water] +
                            //pci.Inventory.Equipments[SagaDB.Item.EnumEquipSlot.RIGHT_HAND].BaseData.element[SagaLib.Elements.Wind];

                            if (elements > 0) ATKBonus += rates * elements;
                        }
                    }

                    if (element != Elements.Neutral) {
                        var eleBonuss = CalcElementBonus(sActor, dActor, element, 1,
                            ATKBonus < 0 && !(dActor.Status.undead && element == Elements.Holy));
                        if (sActor.Status.Contract_Lv != 0) //CAJOB40
                        {
                            var tmpele = Elements.Neutral;
                            switch (sActor.Status.Contract_Lv) {
                                case 1:
                                    tmpele = Elements.Fire;
                                    break;
                                case 2:
                                    tmpele = Elements.Water;
                                    break;
                                case 3:
                                    tmpele = Elements.Earth;
                                    break;
                                case 4:
                                    tmpele = Elements.Wind;
                                    break;
                            }

                            if (tmpele == element)
                                eleBonuss += 0.5f;
                            else
                                eleBonuss -= 0.65f;
                        }

                        if (dActor.Status.Contract_Lv != 0) {
                            var tmpele = Elements.Neutral;
                            switch (dActor.Status.Contract_Lv) {
                                case 1:
                                    tmpele = Elements.Fire;
                                    break;
                                case 2:
                                    tmpele = Elements.Water;
                                    break;
                                case 3:
                                    tmpele = Elements.Earth;
                                    break;
                                case 4:
                                    tmpele = Elements.Wind;
                                    break;
                            }

                            if (tmpele == element)
                                eleBonuss -= 0.15f;
                            else
                                eleBonuss += 1.0f;
                        }

                        atk = (int)(atk * eleBonuss * ATKBonus);
                    }
                    else {
                        atk = (int)(atk * 1f * ATKBonus);
                    }

                    damage = CalcMagDamage(sActor, dActor, defType, atk, ignore);

                    damage = CheckBuffForDamage(sActor, dActor, damage);
                    if (dActor.Status.NeutralDamegeDown_rate > 0 && element == Elements.Neutral)
                        damage = (int)(damage * (1.0f - dActor.Status.NeutralDamegeDown_rate / 100.0f));
                    if (dActor.Status.NeutralDamegeDown_rate > 0 && element != Elements.Neutral)
                        damage = (int)(damage * (1.0f - dActor.Status.ElementDamegeDown_rate / 100.0f));
                    if (sActor.type == ActorType.PC && dActor.type == ActorType.PC)
                        if (damage > 0)
                            damage = (int)(damage * Configuration.Configuration.Instance.PVPDamageRateMagic);
                    if (dActor.Status.Additions.ContainsKey("BradStigma")) {
                        var rate = (dActor.Status.Additions["BradStigma"] as DefaultBuff).Variable["BradStigma"];
                        //MapClient.FromActorPC((ActorPC)sActor).SendSystemMessage("你的血印技能，使你的暗屬攻擊加成(" + rate + "%)。");
                        damage = (int)(damage * ((double)rate / 100.0f));
                    }


                    if ((res == AttackResult.Critical || res == AttackResult.Hit) &&
                        sActor.Status.Additions.ContainsKey("WithinWeeks") && sActor.type == ActorType.PC) {
                        var thispc = (ActorPC)sActor;
                        var level = thispc.CInt["WithinWeeksLevel"];
                        switch (thispc.CInt["WithinWeeksLevel"]) {
                            case 1:
                                if (Instance.CanAdditionApply(sActor, dActor, DefaultAdditions.Silence, 5)) {
                                    var skill = new Silence(arg.skill, dActor, 750 + 250 * level);
                                    ApplyAddition(dActor, skill);
                                }

                                break;
                            case 2:
                                if (Instance.CanAdditionApply(sActor, dActor, DefaultAdditions.CannotMove, 5)) {
                                    var skill = new CannotMove(arg.skill, dActor, 1000);
                                    ApplyAddition(dActor, skill);
                                }

                                break;
                            case 3:
                                if (Instance.CanAdditionApply(sActor, dActor, DefaultAdditions.Stiff, 5)) {
                                    var skill = new Stiff(arg.skill, dActor, 1000);
                                    ApplyAddition(dActor, skill);
                                }

                                break;
                            case 4:
                                if (Instance.CanAdditionApply(sActor, dActor, DefaultAdditions.Confuse, 5)) {
                                    var skill = new Confuse(arg.skill, dActor, 3000);
                                    ApplyAddition(dActor, skill);
                                }

                                break;
                            case 5:
                                if (Instance.CanAdditionApply(sActor, dActor, DefaultAdditions.Stun, 10 * level)) {
                                    var skill = new Stun(arg.skill, dActor, 2000);
                                    ApplyAddition(dActor, skill);
                                }

                                break;
                        }
                    }


                    checkdebuff(sActor, dActor, arg, 1);
                }

                if (sActor.type == ActorType.PC) {
                    var pc = (ActorPC)sActor;
                    if (pc.Party != null && sActor.Status.pt_dmg_up_iris > 100)
                        damage = (int)(damage * (sActor.Status.pt_dmg_up_iris / 100.0f));
                    //iris卡种族增伤部分
                    if (dActor.Race == Race.HUMAN && pc.Status.human_dmg_up_iris > 100)
                        damage = (int)(damage * (pc.Status.human_dmg_up_iris / 100.0f));
                    else if (dActor.Race == Race.BIRD && pc.Status.bird_dmg_up_iris > 100)
                        damage = (int)(damage * (pc.Status.bird_dmg_up_iris / 100.0f));
                    else if (dActor.Race == Race.ANIMAL && pc.Status.animal_dmg_up_iris > 100)
                        damage = (int)(damage * (pc.Status.animal_dmg_up_iris / 100.0f));
                    else if (dActor.Race == Race.MAGIC_CREATURE && pc.Status.magic_c_dmg_up_iris > 100)
                        damage = (int)(damage * (pc.Status.magic_c_dmg_up_iris / 100.0f));
                    else if (dActor.Race == Race.PLANT && pc.Status.plant_dmg_up_iris > 100)
                        damage = (int)(damage * (pc.Status.plant_dmg_up_iris / 100.0f));
                    else if (dActor.Race == Race.WATER_ANIMAL && pc.Status.water_a_dmg_up_iris > 100)
                        damage = (int)(damage * (pc.Status.water_a_dmg_up_iris / 100.0f));
                    else if (dActor.Race == Race.MACHINE && pc.Status.machine_dmg_up_iris > 100)
                        damage = (int)(damage * (pc.Status.machine_dmg_up_iris / 100.0f));
                    else if (dActor.Race == Race.ROCK && pc.Status.rock_dmg_up_iris > 100)
                        damage = (int)(damage * (pc.Status.rock_dmg_up_iris / 100.0f));
                    else if (dActor.Race == Race.ELEMENT && pc.Status.element_dmg_up_iris > 100)
                        damage = (int)(damage * (pc.Status.element_dmg_up_iris / 100.0f));
                    else if (dActor.Race == Race.UNDEAD && pc.Status.undead_dmg_up_iris > 100)
                        damage = (int)(damage * (pc.Status.undead_dmg_up_iris / 100.0f));
                }

                //友情的一击
                if (sActor.Status.Additions.ContainsKey("BlowOfFriendship")) damage = (int)(damage * 1.15f);

                //竜眼放大
                if (sActor.Status.Additions.ContainsKey("DragonEyeOpen")) {
                    var rate = (sActor.Status.Additions["DragonEyeOpen"] as DefaultBuff).Variable["DragonEyeOpen"];
                    damage = (int)(damage * ((double)rate / 100.0f));
                }

                if (dActor.type == ActorType.PC) {
                    var pc = (ActorPC)dActor;
                    if (pc.Party != null && pc.Status.pt_dmg_down_iris < 100)
                        damage = (int)(damage * (pc.Status.pt_dmg_up_iris / 100.0f));
                    if (pc.Status.Element_down_iris < 100 && element != Elements.Neutral)
                        damage = (int)(damage * (pc.Status.Element_down_iris / 100.0f));

                    //iris卡种族减伤部分
                    if (sActor.Race == Race.HUMAN && pc.Status.human_dmg_down_iris < 100)
                        damage = (int)(damage * (pc.Status.human_dmg_down_iris / 100.0f));

                    else if (sActor.Race == Race.BIRD && pc.Status.bird_dmg_down_iris < 100)
                        damage = (int)(damage * (pc.Status.bird_dmg_down_iris / 100.0f));
                    else if (sActor.Race == Race.ANIMAL && pc.Status.animal_dmg_down_iris < 100)
                        damage = (int)(damage * (pc.Status.animal_dmg_down_iris / 100.0f));
                    else if (sActor.Race == Race.MAGIC_CREATURE && pc.Status.magic_c_dmg_down_iris < 100)
                        damage = (int)(damage * (pc.Status.magic_c_dmg_down_iris / 100.0f));
                    else if (sActor.Race == Race.PLANT && pc.Status.plant_dmg_down_iris < 100)
                        damage = (int)(damage * (pc.Status.plant_dmg_down_iris / 100.0f));
                    else if (sActor.Race == Race.WATER_ANIMAL && pc.Status.water_a_dmg_down_iris < 100)
                        damage = (int)(damage * (pc.Status.water_a_dmg_down_iris / 100.0f));
                    else if (sActor.Race == Race.MACHINE && pc.Status.machine_dmg_down_iris < 100)
                        damage = (int)(damage * (pc.Status.machine_dmg_down_iris / 100.0f));
                    else if (sActor.Race == Race.ROCK && pc.Status.rock_dmg_down_iris < 100)
                        damage = (int)(damage * (pc.Status.rock_dmg_down_iris / 100.0f));
                    else if (sActor.Race == Race.ELEMENT && pc.Status.element_dmg_down_iris < 100)
                        damage = (int)(damage * (pc.Status.element_dmg_down_iris / 100.0f));
                    else if (sActor.Race == Race.UNDEAD && pc.Status.undead_dmg_down_iris < 100)
                        damage = (int)(damage * (pc.Status.undead_dmg_down_iris / 100.0f));
                }

                if (res == AttackResult.Miss) //取消MISS
                {
                    damage = (int)(damage * 0.6f);
                    res = AttackResult.Hit;
                }

                if ((res == AttackResult.Avoid && IsPhyDamage) ||
                    res == AttackResult.Guard) //res == AttackResult.Miss || 
                    damage = 0;
                return damage;
            }
            catch (Exception ex) {
                Logger.GetLogger().Error(ex, ex.Message);
                res = AttackResult.Miss;
                return 0;
            }
        }

        public void ChangdeWeapons(Actor sActor, byte type) {
            var map = MapManager.Instance.GetMap(sActor.MapID);
            if (sActor.type == ActorType.PC) {
                var pc = (ActorPC)sActor;
                if (sActor.Status.Additions.ContainsKey("自由射击"))
                    RemoveAddition(sActor, "自由射击");
                if (sActor.Status.Additions.ContainsKey("弓术专注提升"))
                    RemoveAddition(sActor, "弓术专注提升");

                StatusFactory.Instance.CalcRange(pc);
                if (pc.TInt["斥候远程模式"] == type) return;
                pc.TInt["斥候远程模式"] = type;
                var sp = pc.SP;
                StatusFactory.Instance.CalcStatus(pc);

                MapClient.FromActorPC(pc).SendStatusExtend();
                MapClient.FromActorPC(pc).SendRange();
                Instance.CastPassiveSkills(pc);
                pc.SP = sp;
                map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.CHANGE_EQUIP, null, pc, true);
                map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.HPMPSP_UPDATE, null, pc, true);
            }
        }

        /// <summary>
        ///     實現傷害（沒有任何視覺特效！）
        /// </summary>
        /// <param name="sActor">攻擊者</param>
        /// <param name="dActor">目標者</param>
        /// <param name="damage">傷害</param>
        public void CauseDamage(Actor sActor, Actor dActor, int damage, bool ignoreShield = false) {
            if (dActor.HP < 1) return;
            damage = checkbuff(sActor, dActor, null, 3, damage);


            if (dActor.type == ActorType.PC) {
                var pc = (ActorPC)dActor;
                if (pc.Status.Additions.ContainsKey("HolyVolition")) //7月16日更新的光之意志BUFF
                {
                    dActor.HP = 1;
                    ShowEffectOnActor(pc, 4173);
                    damage = 0;
                }

                if (damage > dActor.HP && pc.TInt["副本复活标记"] == 4 && pc.TInt["单人复活次数"] > 0) {
                    pc.TInt["单人复活次数"] -= 1;
                    dActor.HP = dActor.MaxHP;
                    dActor.MP = dActor.MaxMP;
                    dActor.SP = dActor.MaxSP;
                    var actors = GetActorsAreaWhoCanBeAttackedTargets(dActor, 300);
                    foreach (var item in actors)
                        if (CheckValidAttackTarget(dActor, item)) {
                            PushBack(dActor, item, 3);
                            ShowEffectOnActor(item, 5275);
                            if (!item.Status.Additions.ContainsKey("Stun")) {
                                var stun = new Stun(null, item, 3000);
                                ApplyAddition(item, stun);
                            }
                        }

                    ShowEffectOnActor(pc, 4243);
                    damage = 0;
                    SendSystemMessage(pc, "你被使用了一次复活机会！剩余次数：" + pc.TInt["单人复活次数"]);

                    CastPassiveSkills(pc); //重新计算被动

                    /*if (!pc.Tasks.ContainsKey("Recover"))//自然恢复
                    {
                        Tasks.PC.Recover reg = new Tasks.PC.Recover(MapClient.FromActorPC(pc));
                        pc.Tasks.Add("Recover", reg);
                        reg.Activate();
                    }*/

                    if (!pc.Status.Additions.ContainsKey("HolyVolition")) {
                        var skill = new DefaultBuff(null, pc, "HolyVolition", 2000);
                        ApplyAddition(pc, skill);
                    }
                }
            }

            if (damage > dActor.HP)
                dActor.HP = 0;
            else
                dActor.HP = (uint)(dActor.HP - damage);

            if (dActor.HP > dActor.MaxHP)
                dActor.HP = dActor.MaxHP;


            //if (dActor.type == ActorType.PC && dActor.HP < 1)
            //ClientManager.EnterCriticalArea();
            ApplyDamage(sActor, dActor, damage);

            MapManager.Instance.GetMap(sActor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.HPMPSP_UPDATE, null, dActor, true);
            //MapClient.FromActorPC((ActorPC)sActor).SendPartyMemberHPMPSP((ActorPC)sActor);

            //ClientManager.LeaveCriticalArea();
        }

        /// <summary>
        ///     搜索道具欄前20個，尋找可裝備的武器
        /// </summary>
        /// <param name="pc">玩家</param>
        /// <param name="requestType">可行的武器類型</param>
        /// <returns>bool</returns>
        public bool CheckWeapon(ActorPC pc, List<ItemType> requestType) {
            return false; //暂时去掉
            try {
                if (requestType.Count > 0)
                    for (var y = 0; y < requestType.Count; y++) {
                        Item item = null;
                        for (var i = 0; i < 20; i++)
                            if (pc.Inventory.Items[ContainerType.BODY][i].BaseData.itemType == requestType[y]) {
                                item = pc.Inventory.Items[ContainerType.BODY][i];
                                var client = MapClient.FromActorPC(pc);
                                if (client.CheckEquipRequirement(item) == 0) {
                                    if (item.EquipSlot.Contains(EnumEquipSlot.LEFT_HAND) &&
                                        item.EquipSlot.Contains(EnumEquipSlot.RIGHT_HAND)
                                        && item.EquipSlot.Count == 1
                                        && pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND)
                                        && !pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.doubleHand)
                                        client.OnItemEquipt(item.Slot, 15);
                                    else client.OnItemEquipt(item.Slot, 0);
                                    var arg = new EffectArg();
                                    arg.effectID = 4177;
                                    arg.actorID = pc.ActorID;
                                    client.map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SHOW_EFFECT, arg, pc,
                                        true);
                                    return true;
                                }
                            }
                    }
            }
            catch (Exception ex) {
                Logger.GetLogger().Error(ex, ex.Message);
                return false;
            }

            return false;
        }

        public void CancelSkillCast(Actor actor) {
            //if(actor.type == ActorType.MOB) return;
            //if (Global.Random.Next(0, 100) < 33) 
            /*if (actor.type == ActorType.PC)
            {
                Network.Client.MapClient.FromActorPC((ActorPC)actor).SendSkillDummy();
            }
            else
            {*/
            if (actor.Tasks.ContainsKey("SkillCast") && actor.TInt["CanNotInterrupted"] != 1) {
                if (actor.Tasks["SkillCast"].getActivated())
                    if ((actor.Tasks["SkillCast"].NextUpdateTime - DateTime.Now).TotalMilliseconds > 200) {
                        actor.Tasks["SkillCast"].Deactivate();
                        actor.Tasks.Remove("SkillCast");
                    }

                /*SkillArg arg = new SkillArg();
                arg.sActor = actor.ActorID;
                arg.dActor = 0;
                arg.skill = SkillFactory.Instance.GetSkill(3311, 1);
                arg.x = 0;
                arg.y = 0;
                arg.hp = new List<int>();
                arg.sp = new List<int>();
                arg.mp = new List<int>();
                arg.hp.Add(0);
                arg.sp.Add(0);
                arg.mp.Add(0);
                arg.flag.Add(AttackFlag.NONE);
                //arg.affectedActors.Add(this.Character);
                arg.argType = SkillArg.ArgType.Active;*/
                MapManager.Instance.GetMap(actor.MapID)
                    .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SKILL_CANCEL, null, actor, true);
                //}
            }
        }

        public void SendAttackMessage(byte type, Actor sActor, string Sender, string Content) {
            if (sActor.type == ActorType.PC) {
                var p = new SSMG_CHAT_JOB();
                p.Type = type;
                p.Sender = Sender;
                p.Content = Content;
                MapClient.FromActorPC((ActorPC)sActor).NetIo.SendPacket(p);
            }
        }

        /// <summary>
        ///     附加圣印
        /// </summary>
        /// <param name="dActor">目标</param>
        public void Seals(Actor sActor, Actor dActor) {
            Seals(sActor, dActor, 1);
        }

        public void Seals(Actor sActor, Actor dActor, byte count) {
            if (sActor.type == ActorType.PC)
                if (((ActorPC)sActor).PossessionTarget != 0) //凭依时无效
                    return;
            if (sActor.Status.Additions.ContainsKey("EvilSoul")) return;
            if (dActor != null)
                if (sActor.Status.Additions.ContainsKey("Seals")) {
                    var arg = new EffectArg();
                    arg.effectID = 4238;
                    arg.actorID = dActor.ActorID;
                    if (sActor.type == ActorType.PC)
                        MapClient.FromActorPC((ActorPC)sActor).map
                            .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SHOW_EFFECT, arg, (ActorPC)sActor, true);
                    dActor.IsSeals = 1;
                    for (var i = 0; i < count; i++) {
                        var Seals = new Seals(null, dActor, 15000);
                        ApplyAddition(dActor, Seals);
                    }
                }
        }

        /// <summary>
        ///     让Actor说话
        /// </summary>
        /// <param name="actor">说话者</param>
        /// <param name="message">内容</param>
        public void ActorSpeak(Actor actor, string message) {
            var s = new Activator2(actor, message, 500);
            s.Activate();
        }

        public void ActorSpeak(Actor actor, string message, int duetime) {
            var s = new Activator2(actor, message, duetime);
            s.Activate();
        }

        /// <summary>
        ///     让actor跳数值
        /// </summary>
        /// <param name="actor">目标</param>
        /// <param name="hp">血量，正值为伤害，负值为恢复</param>
        /// <param name="mp">法力，正值为伤害，负值为恢复</param>
        /// <param name="sp">SP，正值为伤害，负值为恢复</param>
        public void ShowVessel(Actor actor, int hp = 0, int mp = 0, int sp = 0, AttackResult res = AttackResult.Hit) {
            var tome = true;
            var arg = new SkillArg();
            arg.affectedActors.Add(actor);
            arg.Init();
            arg.sActor = actor.ActorID;
            arg.argType = SkillArg.ArgType.Item_Active;
            var item0 = ItemFactory.Instance.GetItem(10000000);
            arg.item = item0;
            arg.hp[0] = hp;
            arg.mp[0] = mp;
            arg.sp[0] = sp;

            if (actor.HP == 0) {
                arg.flag[0] = AttackFlag.DIE | AttackFlag.HP_DAMAGE | AttackFlag.ATTACK_EFFECT;
                arg.argType = SkillArg.ArgType.Attack;
            }
            else if (hp > 0) {
                arg.flag[0] |= AttackFlag.HP_DAMAGE;
            }
            else if (hp < 0) {
                arg.item = ItemFactory.Instance.GetItem(10000000);
                arg.flag[0] |= AttackFlag.HP_HEAL;
                arg.argType = SkillArg.ArgType.Item_Active;
            }

            /*if (res == AttackResult.Critical)
            {
                arg.flag[0] |= AttackFlag.CRITICAL;
                arg.argType = SkillArg.ArgType.Attack;
            }
            if (res == AttackResult.Miss || res == AttackResult.Avoid || res == AttackResult.Guard)
            {
                if (res == AttackResult.Miss)
                    arg.flag[0] = AttackFlag.MISS;
                else if (res == AttackResult.Avoid)
                    arg.flag[0] = AttackFlag.AVOID;
                else
                    arg.flag[0] = AttackFlag.GUARD;
                arg.argType = SkillArg.ArgType.Attack;
            }*/

            if (mp > 0)
                arg.flag[0] |= AttackFlag.MP_DAMAGE;
            else if (mp < 0)
                arg.flag[0] |= AttackFlag.MP_HEAL;
            if (sp > 0)
                arg.flag[0] |= AttackFlag.SP_DAMAGE;
            else if (sp < 0)
                arg.flag[0] |= AttackFlag.SP_HEAL;
            if (actor.HP == 0) {
                if (actor.type == ActorType.PC) {
                    arg.argType = SkillArg.ArgType.Item_Active;
                    actor.e.OnActorSkillUse(actor, arg);
                    tome = false;
                    arg.argType = SkillArg.ArgType.Attack;
                }

                if (actor.Status.Additions.ContainsKey("HolyVolition") && hp > 0)
                    arg.flag[0] = AttackFlag.HP_DAMAGE | AttackFlag.ATTACK_EFFECT;
            }


            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SKILL, arg, actor, tome);
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.HPMPSP_UPDATE, arg, actor, true);
        }

        /// <summary>
        ///     武器装备破损
        /// </summary>
        /// <param name="pc">玩家</param>
        public void WeaponWorn(ActorPC pc) {
            if (!pc.Status.Additions.ContainsKey("DurDownCancel")) //试运行“防护保养”-2261
                return;
            uint rate = 2;
            if (pc.Status.Additions.ContainsKey("fish"))
                rate = 60;
            if (Global.Random.Next(0, 6000) < rate)
                if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND)) {
                    if (pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].PossessionedActor != null)
                        return;
                    if (pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].maxDurability == 0)
                        return;
                    var arg = new EffectArg();
                    var client = MapClient.FromActorPC(pc);
                    if (pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].Durability <= 0 ||
                        pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].Durability >
                        pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].maxDurability) {
                        client.SendSystemMessage("武器[" +
                                                 pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.name +
                                                 "]损毁！");
                        SSMG_ITEM_DELETE p2;
                        p2 = new SSMG_ITEM_DELETE();
                        p2.InventorySlot = pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].Slot;
                        client.NetIo.SendPacket(p2);
                        var oriItem = pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND];
                        client.ItemMoveSub(oriItem, ContainerType.BODY, oriItem.Stack);
                        if (oriItem.BaseData.repairItem == 0)
                            client.DeleteItem(pc.Inventory.LastItem.Slot, pc.Inventory.LastItem.Stack, true);
                        return;
                    }

                    arg.actorID = client.Character.ActorID;
                    arg.effectID = 8044;
                    client.Character.e.OnShowEffect(client.Character, arg);
                    pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].Durability -= 1;
                    client.SendSystemMessage("武器[" + pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.name +
                                             "]耐久度下降！(" + pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].Durability +
                                             "/" + pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].maxDurability +
                                             ")");
                    client.SendItemInfo(pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND]);
                }
        }

        /// <summary>
        ///     防具装备破损
        /// </summary>
        /// <param name="pc">玩家</param>
        public void ArmorWorn(ActorPC pc) {
            if (!pc.Status.Additions.ContainsKey("DurDownCancel")) //试运行“防护保养”-2261
                return;
            if (Global.Random.Next(0, 3500) < 2) {
                var arg = new EffectArg();
                var client = MapClient.FromActorPC(pc);
                var ArmorEnum = new EnumEquipSlot();
                switch (Global.Random.Next(1, 11)) {
                    case 1:
                        ArmorEnum = EnumEquipSlot.BACK;
                        break;
                    case 2:
                        ArmorEnum = EnumEquipSlot.CHEST_ACCE;
                        break;
                    case 3:
                        ArmorEnum = EnumEquipSlot.FACE;
                        break;
                    case 4:
                        ArmorEnum = EnumEquipSlot.FACE_ACCE;
                        break;
                    case 5:
                        ArmorEnum = EnumEquipSlot.HEAD;
                        break;
                    case 6:
                        ArmorEnum = EnumEquipSlot.HEAD_ACCE;
                        break;
                    case 7:
                        ArmorEnum = EnumEquipSlot.LEFT_HAND;
                        break;
                    case 8:
                        ArmorEnum = EnumEquipSlot.LOWER_BODY;
                        break;
                    case 9:
                        ArmorEnum = EnumEquipSlot.SHOES;
                        break;
                    case 10:
                        ArmorEnum = EnumEquipSlot.SOCKS;
                        break;
                    case 11:
                        ArmorEnum = EnumEquipSlot.UPPER_BODY;
                        break;
                }

                if (pc.Inventory.Equipments.ContainsKey(ArmorEnum)) {
                    if (pc.Inventory.Equipments[ArmorEnum].PossessionedActor != null)
                        return;
                    if (pc.Inventory.Equipments[ArmorEnum].maxDurability == 0)
                        return;
                    if (pc.Inventory.Equipments[ArmorEnum].Durability <= 0 ||
                        pc.Inventory.Equipments[ArmorEnum].Durability >
                        pc.Inventory.Equipments[ArmorEnum].maxDurability) {
                        client.SendSystemMessage("装备[" + pc.Inventory.Equipments[ArmorEnum].BaseData.name + "]损毁！");
                        SSMG_ITEM_DELETE p2;
                        p2 = new SSMG_ITEM_DELETE();
                        p2.InventorySlot = pc.Inventory.Equipments[ArmorEnum].Slot;
                        client.NetIo.SendPacket(p2);
                        pc.Inventory.Equipments.Remove(ArmorEnum);
                        client.SendItems();
                        client.SendEquip();
                        return;
                    }

                    arg.actorID = client.Character.ActorID;
                    arg.effectID = 8044;
                    client.Character.e.OnShowEffect(client.Character, arg);
                    pc.Inventory.Equipments[ArmorEnum].Durability -= 1;
                    client.SendSystemMessage("装备[" + pc.Inventory.Equipments[ArmorEnum].BaseData.name + "]耐久度下降！(" +
                                             pc.Inventory.Equipments[ArmorEnum].Durability +
                                             "/" + pc.Inventory.Equipments[ArmorEnum].maxDurability + ")");
                    client.SendItemInfo(pc.Inventory.Equipments[ArmorEnum]);
                }
            }
        }

        /// <summary>
        ///     特定装备耐久下降1
        /// </summary>
        /// <param name="pc"></param>
        public void EquipWorn(ActorPC pc, Item wornequip) {
            var client = MapClient.FromActorPC(pc);
            if (client.Character.Account.GMLevel > 200) return;
            var arg = new EffectArg();
            if (wornequip.Durability < 2) {
                wornequip.Durability = 0;
                client.SendSystemMessage("装备[" + wornequip.BaseData.name + "]损坏！");
                client.OnItemMove(wornequip.Slot, ContainerType.BODY, wornequip.Stack, false);
            }
            else {
                wornequip.Durability--;
                client.SendSystemMessage("装备[" + wornequip.BaseData.name + "]耐久度下降！(" + wornequip.Durability + "/" +
                                         wornequip.maxDurability + ")");
            }

            //client.SendItems();
            client.SendEquip();
            arg.actorID = client.Character.ActorID;
            arg.effectID = 8044;
            client.Character.e.OnShowEffect(client.Character, arg);
            client.SendItemInfo(wornequip);
        }

        /// <summary>
        ///     随机装备耐久下降1
        /// </summary>
        /// <param name="pc"></param>
        public void RandomEquipWorn(ActorPC pc) {
            if (pc == null) return;
            var equips = new List<Item>();
            var client = MapClient.FromActorPC(pc);
            var arg = new EffectArg();
            if (Global.Random.Next(0, 100) < 80 && (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.UPPER_BODY) ||
                                                    pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND))) {
                if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.UPPER_BODY))
                    equips.Add(pc.Inventory.Equipments[EnumEquipSlot.UPPER_BODY]);
                if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND))
                    equips.Add(pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND]);
            }
            else {
                foreach (var i in pc.Inventory.Equipments.Values) {
                    if (i.Stack == 0)
                        continue;
                    equips.Add(i);
                }
            }

            if (equips.Count < 1) return;
            var wornequip = equips[Global.Random.Next(0, equips.Count - 1)];
            client.SendSystemMessage("装备[" + wornequip.BaseData.name + "]耐久度下降！");
            if (wornequip.Durability < 2) {
                wornequip.Durability = 0;
                client.SendSystemMessage("装备[" + wornequip.BaseData.name + "]损坏！");
                client.OnItemMove(wornequip.Slot, ContainerType.BODY, wornequip.Stack, false);
            }
            else {
                wornequip.Durability--;
                client.SendSystemMessage("装备[" + wornequip.BaseData.name + "]耐久度下降！(" + wornequip.Durability + "/" +
                                         wornequip.maxDurability + ")");
            }

            //client.SendItems();
            client.SendEquip();
            arg.actorID = client.Character.ActorID;
            arg.effectID = 8044;
            client.Character.e.OnShowEffect(client.Character, arg);
            client.SendItemInfo(wornequip);
        }

        public void Attack(Actor sActor, Actor dActor, SkillArg arg) {
            Attack(sActor, dActor, arg, 1f);
        }

        public void Attack(Actor sActor, Actor dActor, SkillArg arg, float factor) {
            if (!CheckStatusCanBeAttact(sActor, 1)) {
                if (sActor.type == ActorType.PC) MapClient.FromActorPC((ActorPC)sActor).SendSystemMessage("無法行動的狀態。");
                return;
            }

            int combo = GetComboCount(sActor);

            arg.sActor = sActor.ActorID;
            arg.dActor = dActor.ActorID;
            for (var i = 0; i < combo; i++) arg.affectedActors.Add(dActor);
            arg.type = sActor.Status.attackType;
            arg.delayRate = 1f + (float)combo / 2;
            PhysicalAttack(sActor, arg.affectedActors, arg, sActor.WeaponElement, factor);
        }

        public void CriAttack(Actor sActor, Actor dActor, SkillArg arg) {
        }

        public int TryCast(Actor sActor, Actor dActor, SkillArg arg) {
            if (skillHandlers.ContainsKey(arg.skill.ID)) {
                if (!CheckStatusCanBeAttact(sActor, 2)) {
                    if (sActor.type == ActorType.PC)
                        MapClient.FromActorPC((ActorPC)sActor).SendSystemMessage("無法行動的狀態。");

                    return 0;
                }

                if (sActor.type == ActorType.PC)
                    //if (dActor == null &&
                    //    arg.skill.ID != 3434//福音
                    //    )
                    //{
                    //    return 0;
                    //}
                    //Calc Direction Before Cast..
                    //Map map = Manager.MapManager.Instance.GetMap(sActor.MapID);
                    //map.MoveActor(Map.MOVE_TYPE.STOP, sActor, new short[2] { sActor.X, sActor.Y }, sActor.Dir, sActor.Speed);
                    //Cancel Cloaking Skill
                    //if(dActor!=null)
                    //{
                    //    ActorPC spc = (ActorPC)sActor;
                    //    if (spc.PossessionTarget != 0)
                    //    {
                    //        Map map = Manager.MapManager.Instance.GetMap(sActor.MapID);
                    //        Actor TargetPossessionActor = map.GetActor(spc.PossessionTarget);
                    //        if (TargetPossessionActor.Status.Additions.ContainsKey("Cloaking"))
                    //            RemoveAddition(TargetPossessionActor, "Cloaking");
                    //    }
                    //    if (dActor.Status.Additions.ContainsKey("Cloaking"))
                    //        RemoveAddition(dActor, "Cloaking");
                    //    if (sActor.Status.Additions.ContainsKey("Cloaking"))
                    //        RemoveAddition(sActor, "Cloaking");
                    //}
                    return skillHandlers[arg.skill.ID].TryCast((ActorPC)sActor, dActor, arg);

                return 0;
            }

            return 0;
        }


        public void SetNextComboSkill(Actor actor, uint id) {
            if (actor.type == ActorType.PC) {
                var pc = (ActorPC)actor;
                MapClient.FromActorPC(pc).nextCombo.Add(id);
            }
        }

        public void SkillCast(Actor sActor, Actor dActor, SkillArg arg) {
            arg.sActor = sActor.ActorID;
            if (arg.dActor != 0xFFFFFFFF)
                arg.dActor = dActor.ActorID;


            if (skillHandlers.ContainsKey(arg.skill.ID)) {
                skillHandlers[arg.skill.ID].Proc(sActor, dActor, arg, arg.skill.Level);
                if (arg.affectedActors.Count == 0 && arg.dActor != arg.sActor && arg.dActor != 0 &&
                    arg.dActor != 0xffffffff) {
                    arg.affectedActors.Add(dActor);
                    arg.Init();
                }
            }
            else if (MobskillHandlers.ContainsKey(arg.skill.ID)) {
                MobskillHandlers[arg.skill.ID].Proc(sActor, dActor, arg, arg.skill.Level);
                if (arg.affectedActors.Count == 0 && arg.dActor != arg.sActor && arg.dActor != 0 &&
                    arg.dActor != 0xffffffff) {
                    if (!CheckStatusCanBeAttact(sActor, 3)) return;
                    arg.affectedActors.Add(dActor);
                    arg.Init();
                }
            }
            else {
                arg.affectedActors.Add(dActor);
                arg.Init();
                Logger.GetLogger().Warning("No defination for skill:" + arg.skill.Name + "(ID:" + arg.skill.ID + ")",
                    null);
            }
        }

        private byte GetComboCount(Actor actor) {
            if (actor.type == ActorType.PC) {
                var pc = (ActorPC)actor;
                byte combo = 1;

                if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND)) {
                    var item = pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND];
                    switch (item.BaseData.itemType) {
                        case ItemType.DUALGUN:
                        case ItemType.CLAW:
                            combo = 2;
                            break;
                        default:
                            combo = 1;
                            break;
                    }
                }
                else {
                    combo = 1;
                }

                if (Global.Random.Next(0, 99) < actor.Status.combo_rate_skill)
                    combo = (byte)actor.Status.combo_skill;
                return combo;
            }

            return 1;
        }

        public void CastPassiveSkills(ActorPC pc, bool ReCalcState = true) {
            //PC.StatusFactory.Instance.CalcStatusOnSkillEffect(pc);
            var list = pc.Status.Additions.Keys.ToList();
            foreach (var i in list)
                if (pc.Status.Additions[i].GetType() == typeof(DefaultPassiveSkill))
                    RemoveAddition(pc, pc.Status.Additions[i]);

            foreach (var i in pc.Skills.Values)
                if (i.active == false)
                    if (skillHandlers.ContainsKey(i.ID)) {
                        var arg = new SkillArg();
                        arg.skill = i;
                        skillHandlers[i.ID].Proc(pc, pc, arg, i.Level);
                    }

            if (!pc.Rebirth || pc.Job != pc.Job3) {
                foreach (var i in pc.Skills2.Values)
                    if (i.active == false)
                        if (skillHandlers.ContainsKey(i.ID)) {
                            var arg = new SkillArg();
                            arg.skill = i;
                            skillHandlers[i.ID].Proc(pc, pc, arg, i.Level);
                        }

                foreach (var i in pc.SkillsReserve.Values)
                    if (i.active == false)
                        if (skillHandlers.ContainsKey(i.ID)) {
                            var arg = new SkillArg();
                            arg.skill = i;
                            skillHandlers[i.ID].Proc(pc, pc, arg, i.Level);
                        }
            }
            else {
                foreach (var i in pc.Skills2_1.Values)
                    if (i.active == false)
                        if (skillHandlers.ContainsKey(i.ID)) {
                            var arg = new SkillArg();
                            arg.skill = i;
                            skillHandlers[i.ID].Proc(pc, pc, arg, i.Level);
                        }

                foreach (var i in pc.Skills2_2.Values)
                    if (i.active == false)
                        if (skillHandlers.ContainsKey(i.ID)) {
                            var arg = new SkillArg();
                            arg.skill = i;
                            skillHandlers[i.ID].Proc(pc, pc, arg, i.Level);
                        }

                foreach (var i in pc.Skills3.Values)
                    if (i.active == false)
                        if (skillHandlers.ContainsKey(i.ID)) {
                            var arg = new SkillArg();
                            arg.skill = i;
                            skillHandlers[i.ID].Proc(pc, pc, arg, i.Level);
                        }

                foreach (var i in pc.DualJobSkills)
                    if (!i.active)
                        if (skillHandlers.ContainsKey(i.ID)) {
                            var arg = new SkillArg();
                            arg.skill = i;
                            skillHandlers[i.ID].Proc(pc, pc, arg, i.Level);
                        }
            }

            if (ReCalcState)
                StatusFactory.Instance.CalcStatusOnSkillEffect(pc);
        }

        public void CheckBuffValid(ActorPC pc) {
            var list = pc.Status.Additions.Keys.ToList();
            foreach (var i in list) {
                if (i == null)
                    continue;
                if (pc.Status.Additions[i].GetType() == typeof(DefaultBuff)) {
                    var buff = (DefaultBuff)pc.Status.Additions[i];
                    int result;
                    if (buff.OnCheckValid != null) {
                        buff.OnCheckValid(pc, pc, out result);
                        if (result != 0) RemoveAddition(pc, pc.Status.Additions[i]);
                    }
                }
            }
        }

        /// <summary>
        ///     Apply a addition to an actor
        /// </summary>
        /// <param name="actor">Actor which the addition should be applied to</param>
        /// <param name="addition">Addition to be applied</param>
        public static void ApplyAddition(Actor actor, Addition addition) {
            if (!addition.Enabled) return;
            if (actor.type == ActorType.PC)
                if (((ActorPC)actor).Fictitious)
                    return;
            if (actor.Status.Additions.ContainsKey(addition.Name)) {
                //return;
                var oldaddition = actor.Status.Additions[addition.Name];
                if (oldaddition.MyType == Addition.AdditionType.Buff ||
                    oldaddition.MyType == Addition.AdditionType.Debuff) {
                    var oldbuff = (DefaultBuff)oldaddition;
                    var newbuff = (DefaultBuff)addition;
                    if (oldbuff.Variable.ContainsKey(addition.Name) || newbuff.Variable.ContainsKey(addition.Name))
                        if (oldbuff.Variable[addition.Name] == newbuff.Variable[addition.Name]) {
                            oldbuff.TotalLifeTime += addition.TotalLifeTime;
                            return;
                        }
                }

                //if (oldaddition.MyType == Addition.AdditionType.Debuff)
                //{
                //    DefaultDeBuff oldbuff = (DefaultDeBuff)oldaddition;
                //    DefaultDeBuff newbuff = (DefaultDeBuff)addition;
                //    if (oldbuff.Variable.ContainsKey(addition.Name) || newbuff.Variable.ContainsKey(addition.Name))
                //    {
                //        if (oldbuff.Variable[addition.Name] == newbuff.Variable[addition.Name])
                //        {
                //            oldbuff.TotalLifeTime += addition.TotalLifeTime;
                //            return;
                //        }
                //    }
                //}
                if (oldaddition.Activated)
                    oldaddition.AdditionEnd();
                if (addition.IfActivate) {
                    addition.AdditionStart();
                    addition.StartTime = DateTime.Now;
                    addition.Activated = true;
                }

                var blocked = ClientManager.Blocked;
                if (!blocked)
                    ClientManager.EnterCriticalArea();

                actor.Status.Additions.Remove(addition.Name);
                actor.Status.Additions.Add(addition.Name, addition);

                if (!blocked)
                    ClientManager.LeaveCriticalArea();
            }
            else {
                if (addition.IfActivate) {
                    addition.AdditionStart();
                    addition.StartTime = DateTime.Now;
                    addition.Activated = true;
                }

                /*bool blocked = ClientManager.Blocked;
                if (!blocked)*/
                ClientManager.EnterCriticalArea();
                if (!actor.Status.Additions.ContainsKey(addition.Name))
                    actor.Status.Additions.Add(addition.Name, addition);

                //if (!blocked)
                ClientManager.LeaveCriticalArea();
            }
        }

        public static void RemoveAddition(Actor actor, string name) {
            var blocked = ClientManager.Blocked;
            if (!blocked)
                ClientManager.EnterCriticalArea();
            if (actor.Status.Additions.ContainsKey(name))
                RemoveAddition(actor, actor.Status.Additions[name]);
            if (!blocked)
                ClientManager.LeaveCriticalArea();
        }

        public static void RemoveAddition(Actor actor, string name, bool removeOnly) {
            var blocked = ClientManager.Blocked;
            if (!blocked)
                ClientManager.EnterCriticalArea();
            if (actor.Status.Additions.ContainsKey(name))
                RemoveAddition(actor, actor.Status.Additions[name], true);
            if (!blocked)
                ClientManager.LeaveCriticalArea();
        }

        public static void RemoveAddition(Actor actor, Addition addition) {
            var blocked = ClientManager.Blocked;
            if (!blocked)
                ClientManager.EnterCriticalArea();
            RemoveAddition(actor, addition, false);
            if (!blocked)
                ClientManager.LeaveCriticalArea();
        }

        public static void RemoveAddition(Actor actor, Addition addition, bool removeOnly) {
            if (actor.Status == null)
                return;
            if (actor.Status.Additions.ContainsKey(addition.Name)) {
                actor.Status.Additions.Remove(addition.Name);
                if (addition.Activated && !removeOnly) addition.AdditionEnd();
                addition.Activated = false;
            }
        }

        /// <summary>
        ///     击退函数
        /// </summary>
        /// <param name="ori">击退发动者</param>
        /// <param name="dest">被击退者</param>
        /// <param name="step">击退距离</param>
        public void PushBack(Actor ori, Actor dest, int step) {
            if (!dest.Status.Additions.ContainsKey("FortressCircleSEQ") &&
                !dest.Status.Additions.ContainsKey("SolidBody"))
                PushBack(ori, dest, step, 3000);
        }

        public void PushBack(Actor ori, Actor dest, int step, ushort speed, MoveType moveType = MoveType.RUN) {
            var map = MapManager.Instance.GetMap(ori.MapID);
            if (dest.type == ActorType.MOB) {
                var eh = (MobEventHandler)dest.e;
                if (eh.AI.Mode.Symbol || eh.AI.Mode.SymbolTrash)
                    return;
            }

            var x = Global.PosX16to8(dest.X, map.Width);
            var y = Global.PosY16to8(dest.Y, map.Height);
            var deltaX = x - Global.PosX16to8(ori.X, map.Width);
            var deltaY = y - Global.PosY16to8(ori.Y, map.Height);
            while (deltaX == 0 && deltaY == 0) {
                deltaX = Global.Random.Next(-1, 1);
                deltaY = Global.Random.Next(-1, 1);
            }

            if (deltaX != 0)
                deltaX /= Math.Abs(deltaX);
            if (deltaY != 0)
                deltaY /= Math.Abs(deltaY);
            for (var i = 0; i < step; i++) {
                x = (byte)(x + deltaX);
                y = (byte)(y + deltaY);
                if (x >= map.Width || y >= map.Height || map.Info.walkable[x, y] != 2) {
                    x = (byte)(x - deltaX);
                    y = (byte)(y - deltaY);
                    break;
                }
            }

            var pos = new short[2];
            pos[0] = Global.PosX8to16(x, map.Width);
            pos[1] = Global.PosY8to16(y, map.Height);
            if (moveType != MoveType.RUN)
                map.MoveActor(Map.MOVE_TYPE.START, dest, pos, speed, speed, true, moveType);
            else
                map.MoveActor(Map.MOVE_TYPE.START, dest, pos, speed, speed, true);
            if (dest.type == ActorType.MOB) {
                var mob = (MobEventHandler)dest.e;
                mob.AI.OnPathInterupt();
            }

            if (dest.type == ActorType.PET || dest.type == ActorType.SHADOW) {
                var mob = (PetEventHandler)dest.e;
                mob.AI.OnPathInterupt();
            }
        }

        public void JumpBack(Actor ori, int step, ushort speed, MoveType moveType = MoveType.RUN) {
            var map = MapManager.Instance.GetMap(ori.MapID);
            byte OutX, OutY;
            Instance.GetTFrontPos(map, ori, out OutX, out OutY);
            var x = Global.PosX16to8(ori.X, map.Width);
            var y = Global.PosY16to8(ori.Y, map.Height);
            var deltaX = x - OutX;
            var deltaY = y - OutY;
            if (deltaX != 0)
                deltaX /= Math.Abs(deltaX);
            if (deltaY != 0)
                deltaY /= Math.Abs(deltaY);
            for (var i = 0; i < step; i++) {
                x = (byte)(x + deltaX);
                y = (byte)(y + deltaY);
                if (x >= map.Width || y >= map.Height || map.Info.walkable[x, y] != 2) {
                    x = (byte)(x - deltaX);
                    y = (byte)(y - deltaY);
                    break;
                }
            }

            var pos = new short[2];
            pos[0] = Global.PosX8to16(x, map.Width);
            pos[1] = Global.PosY8to16(y, map.Height);
            if (moveType != MoveType.RUN)
                map.MoveActor(Map.MOVE_TYPE.START, ori, pos, speed, speed, true, moveType);
            else
                map.MoveActor(Map.MOVE_TYPE.START, ori, pos, speed, speed, true);
        }

        /// <summary>
        ///     检查技能是否符合装备条件
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="skill"></param>
        /// <returns></returns>
        public bool CheckSkillCanCastForWeapon(ActorPC pc, SkillArg arg) {
            if (arg.skill.equipFlag.Value == 0)
                return true;
            if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND)) {
                if (arg.skill.equipFlag.Test((EquipFlags)Enum.Parse(typeof(EquipFlags),
                        pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType.ToString())))
                    return true;
            }
            else if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.LEFT_HAND)) {
                if (arg.skill.equipFlag.Test((EquipFlags)Enum.Parse(typeof(EquipFlags),
                        pc.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].BaseData.itemType.ToString())))
                    return true;
            }
            else if (arg.skill.equipFlag.Test(EquipFlags.HAND)) {
                return true;
            }

            var its = new List<ItemType>();
            var flags = typeof(EquipFlags);
            foreach (EquipFlags item in Enum.GetValues(flags))
                if (arg.skill.equipFlag.Test(item) && Enum.IsDefined(typeof(ItemType), item.ToString()))
                    its.Add((ItemType)Enum.Parse(typeof(ItemType), item.ToString()));
            if (its.Count > 0) {
                if (arg.dActor != 0) {
                    var dActor = MapManager.Instance.GetMap(pc.MapID).GetActor(arg.dActor);
                    if (dActor == null)
                        return false;
                    var range = Math.Max(Math.Abs(pc.X - dActor.X) / 100, Math.Abs(pc.Y - dActor.Y) / 100);
                    if (arg.skill.Range >= range) {
                        if (CheckWeapon(pc, its)) return true;
                    }
                    else {
                        return false;
                    }
                }
                else if (CheckWeapon(pc, its)) {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        ///     返回范围内可被攻击的对象
        /// </summary>
        /// <param name="caster">实际攻击者</param>
        /// <param name="actor">计算范围的实体</param>
        /// <param name="range">范围</param>
        /// <returns>可被攻击的对象</returns>
        public List<Actor> GetActorsAreaWhoCanBeAttackedTargets(Actor caster, Actor actor, short range) {
            var map = MapManager.Instance.GetMap(caster.MapID);
            return GetVaildAttackTarget(caster, map.GetActorsArea(actor, range, false));
        }

        /// <summary>
        ///     返回范围内可被攻击的对象
        /// </summary>
        /// <param name="sActor">攻击者</param>
        /// <param name="range">范围</param>
        /// <returns>可被攻击的对象</returns>
        public List<Actor> GetActorsAreaWhoCanBeAttackedTargets(Actor sActor, short range) {
            var map = MapManager.Instance.GetMap(sActor.MapID);
            return GetVaildAttackTarget(sActor, map.GetActorsArea(sActor, range, false));
        }

        /// <summary>
        ///     返回可攻击的actors
        /// </summary>
        /// <param name="sActor">攻击者</param>
        /// <param name="dActors">被攻击者们</param>
        /// <returns>可攻击的actors</returns>
        public List<Actor> GetVaildAttackTarget(Actor sActor, List<Actor> dActors) {
            if (dActors.Count < 1) return dActors;
            var actors = new List<Actor>();
            foreach (var item in dActors)
                if (CheckValidAttackTarget(sActor, item))
                    actors.Add(item);

            return actors;
        }

        /// <summary>
        ///     检查是施放者能否施放技能或功擊
        /// </summary>
        /// <param name="sActor">攻击者</param>
        /// <param name="type">0=魔法功擊,1=物理功擊,2=技能施放</param>
        /// <returns></returns>
        public bool CheckStatusCanBeAttact(Actor sActor, int type) {
            switch (type) {
                case 0:
                    //Type 0 = Magic
                    //Slienced Confused Frozen Sleep stone stun paralyse
                    if (
                        sActor.Status.Additions.ContainsKey("Silence") ||
                        sActor.Status.Additions.ContainsKey("Confused") ||
                        sActor.Status.Additions.ContainsKey("Frosen") ||
                        sActor.Status.Additions.ContainsKey("Stone") ||
                        sActor.Status.Additions.ContainsKey("Stun") ||
                        sActor.Status.Additions.ContainsKey("Sleep") ||
                        sActor.Status.Additions.ContainsKey("Paralyse") ||
                        sActor.Status.Additions.ContainsKey("SkillForbid")
                    )
                        return false;
                    break;
                case 1: //Type 1 == Phy
                    //Confused Frozen Sleep stone stun paralyse +斷腕
                    if (
                        sActor.Status.Additions.ContainsKey("Confused") ||
                        sActor.Status.Additions.ContainsKey("Frosen") ||
                        sActor.Status.Additions.ContainsKey("Stone") ||
                        sActor.Status.Additions.ContainsKey("Stun") ||
                        sActor.Status.Additions.ContainsKey("Sleep") ||
                        sActor.Status.Additions.ContainsKey("Paralyse")
                    )
                        return false;
                    break;
                case 2:
                    //檢查能否施放
                    //Slienced Confused Frozen Sleep stone stun paralyse

                    if (
                        sActor.Status.Additions.ContainsKey("Silence") ||
                        sActor.Status.Additions.ContainsKey("Confused") ||
                        sActor.Status.Additions.ContainsKey("Frosen") ||
                        sActor.Status.Additions.ContainsKey("Stone") ||
                        sActor.Status.Additions.ContainsKey("Stun") ||
                        sActor.Status.Additions.ContainsKey("Sleep") ||
                        sActor.Status.Additions.ContainsKey("Paralyse") ||
                        sActor.Status.Additions.ContainsKey("SkillForbid")
                    )
                        return false;

                    break;
            }

            return true;
        }

        /// <summary>
        ///     检查是弓和箭是否通过条件
        /// </summary>
        /// <param name="pc">攻击者</param>
        /// <param name="number">消耗箭矢数量</param>
        /// <returns></returns>
        public int CheckPcBowAndArrow(ActorPC pc, int number = 1) {
            if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND)) {
                if (pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.BOW) {
                    if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.LEFT_HAND)) {
                        if (pc.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].BaseData.itemType == ItemType.ARROW) {
                            if (pc.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].Stack >= number) return 0;

                            return -55;
                        }

                        return -34;
                    }

                    return -34;
                }

                return -5;
            }

            return -5;
        }

        /// <summary>
        ///     消耗特定箭矢
        /// </summary>
        /// <param name="pc">攻击者</param>
        /// <param name="number">消耗箭矢数量</param>
        /// <returns></returns>
        public void PcArrowDown(Actor sActor, int number = 1) {
            if (sActor.type == ActorType.PC) {
                var pc = (ActorPC)sActor;
                if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND))
                    if (pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.BOW)
                        if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.LEFT_HAND))
                            if (pc.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].BaseData.itemType == ItemType.ARROW)
                                if (pc.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].Stack >= number)
                                    MapClient.FromActorPC(pc)
                                        .DeleteItem(pc.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].Slot,
                                            (ushort)number, false);
            }
        }

        /// <summary>
        ///     检查是枪和子弹是否通过条件
        /// </summary>
        /// <param name="pc">攻击者</param>
        /// <param name="number">消耗弹药数量</param>
        /// <returns></returns>
        public int CheckPcGunAndBullet(ActorPC pc, int number = 1) {
            if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND)) {
                if (pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.GUN ||
                    pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.DUALGUN ||
                    pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.RIFLE) {
                    if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.LEFT_HAND)) {
                        if (pc.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].BaseData.itemType == ItemType.BULLET) {
                            if (pc.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].Stack >= number) return 0;

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

        /// <summary>
        ///     消耗特定子弹
        /// </summary>
        /// <param name="pc">攻击者</param>
        /// <param name="number">消耗子弹数量</param>
        /// <returns></returns>
        public void PcBulletDown(Actor sActor, int number = 1) {
            if (sActor.type == ActorType.PC) {
                var pc = (ActorPC)sActor;
                if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND))
                    if (pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.GUN ||
                        pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.DUALGUN ||
                        pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.RIFLE)
                        if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.LEFT_HAND))
                            if (pc.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].BaseData.itemType == ItemType.BULLET)
                                if (pc.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].Stack >= number)
                                    MapClient.FromActorPC(pc)
                                        .DeleteItem(pc.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].Slot,
                                            (ushort)number, false);
            }
        }

        /// <summary>
        ///     检查是远程装备是否通过条件
        /// </summary>
        /// <param name="pc">攻击者</param>
        /// <param name="number">消耗弹药数量</param>
        /// <returns></returns>
        public int CheckPcLongAttack(ActorPC pc, int number = 1) {
            if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND)) {
                if (pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.BOW) {
                    if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.LEFT_HAND)) {
                        if (pc.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].BaseData.itemType == ItemType.ARROW) {
                            if (pc.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].Stack >= number) return 0;

                            return -55;
                        }

                        return -34;
                    }

                    return -34;
                }

                if (pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.GUN ||
                    pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.DUALGUN ||
                    pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.RIFLE) {
                    if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.LEFT_HAND)) {
                        if (pc.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].BaseData.itemType == ItemType.BULLET) {
                            if (pc.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].Stack >= number) return 0;

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


        /// <summary>
        ///     消耗特定远程武器弹药
        /// </summary>
        /// <param name="pc">攻击者</param>
        /// <param name="number">消耗弹药数量</param>
        /// <returns></returns>
        public void PcArrowAndBulletDown(Actor sActor, int number = 1) {
            if (sActor.type == ActorType.PC) {
                var pc = (ActorPC)sActor;
                if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND)) {
                    if (pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.BOW)
                        if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.LEFT_HAND))
                            if (pc.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].BaseData.itemType == ItemType.ARROW)
                                if (pc.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].Stack >= number)
                                    MapClient.FromActorPC(pc)
                                        .DeleteItem(pc.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].Slot,
                                            (ushort)number, false);

                    if (pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.GUN ||
                        pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.DUALGUN ||
                        pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.RIFLE)
                        if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.LEFT_HAND))
                            if (pc.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].BaseData.itemType == ItemType.BULLET)
                                if (pc.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].Stack >= number)
                                    MapClient.FromActorPC(pc)
                                        .DeleteItem(pc.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].Slot,
                                            (ushort)number, false);
                }
            }
        }

        /// <summary>
        ///     检查是否可攻击
        /// </summary>
        /// <param name="sActor">攻击者</param>
        /// <param name="dActor">被攻击者</param>
        /// <returns></returns>
        public bool CheckValidAttackTarget(Actor sActor, Actor dActor) {
            if (sActor == dActor)
                return false;
            if (sActor == null || dActor == null)
                return false;
            if (dActor.type == ActorType.PC)
                if (!((ActorPC)dActor).Online)
                    return false;
            if (dActor.type == ActorType.SKILL)
                return false;
            if (dActor.type == ActorType.ITEM)
                return false;
            if (dActor.Buff.Dead)
                return false;
            if (sActor.type == ActorType.PC) {
                var pc = (ActorPC)sActor;
                switch (dActor.type) {
                    case ActorType.MOB:
                        var eh = (MobEventHandler)dActor.e;
                        if (eh.AI.Mode.Symbol)
                            return false;
                        return true;
                    case ActorType.ITEM:
                    case ActorType.SKILL:
                        return false;
                    case ActorType.PC: {
                        //Logger.getLogger().Information("skillhandler");
                        var target = (ActorPC)dActor;
                        if ((pc.Mode == PlayerMode.COLISEUM_MODE && target.Mode == PlayerMode.COLISEUM_MODE) ||
                            (pc.Mode == PlayerMode.WRP && target.Mode == PlayerMode.WRP) ||
                            (pc.Mode == PlayerMode.KNIGHT_WAR && target.Mode == PlayerMode.KNIGHT_WAR) ||
                            ((pc.Mode == PlayerMode.KNIGHT_EAST || pc.Mode == PlayerMode.KNIGHT_FLOWER ||
                              pc.Mode == PlayerMode.KNIGHT_NORTH
                              || pc.Mode == PlayerMode.KNIGHT_ROCK || pc.Mode == PlayerMode.KNIGHT_SOUTH ||
                              pc.Mode == PlayerMode.KNIGHT_WEST)
                             && (target.Mode == PlayerMode.KNIGHT_EAST || target.Mode == PlayerMode.KNIGHT_FLOWER ||
                                 target.Mode == PlayerMode.KNIGHT_NORTH
                                 || target.Mode == PlayerMode.KNIGHT_ROCK || target.Mode == PlayerMode.KNIGHT_SOUTH ||
                                 target.Mode == PlayerMode.KNIGHT_WEST)
                            )) {
                            if ((pc.Mode == PlayerMode.KNIGHT_EAST || pc.Mode == PlayerMode.KNIGHT_FLOWER ||
                                 pc.Mode == PlayerMode.KNIGHT_NORTH
                                 || pc.Mode == PlayerMode.KNIGHT_ROCK || pc.Mode == PlayerMode.KNIGHT_SOUTH ||
                                 pc.Mode == PlayerMode.KNIGHT_WEST)
                                && (target.Mode == PlayerMode.KNIGHT_EAST || target.Mode == PlayerMode.KNIGHT_FLOWER ||
                                    target.Mode == PlayerMode.KNIGHT_NORTH
                                    || target.Mode == PlayerMode.KNIGHT_ROCK ||
                                    target.Mode == PlayerMode.KNIGHT_SOUTH || target.Mode == PlayerMode.KNIGHT_WEST)
                               )
                                //Logger.getLogger().Information("skillhandler2");
                                if (pc.Mode == target.Mode)
                                    return false;
                            //Logger.getLogger().Information("skillhandler3");
                            if (pc.Party == target.Party && pc.Party != null)
                                return false;
                            if (target.PossessionTarget == 0)
                                return true;
                            return false;
                            //Logger.getLogger().Information("skillhandler4");
                        }

                        return false;
                    }
                    case ActorType.PET: {
                        var pet = (ActorPet)dActor;
                        if ((pc.Mode == PlayerMode.COLISEUM_MODE && pet.Owner.Mode == PlayerMode.COLISEUM_MODE) ||
                            (pc.Mode == PlayerMode.WRP && pet.Owner.Mode == PlayerMode.WRP) ||
                            (pc.Mode == PlayerMode.KNIGHT_WAR && pet.Owner.Mode == PlayerMode.KNIGHT_WAR)) {
                            if (pc.Party == pet.Owner.Party)
                                return false;
                            return true;
                        }

                        return false;
                    }
                    case ActorType.SHADOW: {
                        var pet = (ActorShadow)dActor;
                        if ((pc.Mode == PlayerMode.COLISEUM_MODE && pet.Owner.Mode == PlayerMode.COLISEUM_MODE) ||
                            (pc.Mode == PlayerMode.WRP && pet.Owner.Mode == PlayerMode.WRP) ||
                            (pc.Mode == PlayerMode.KNIGHT_WAR && pet.Owner.Mode == PlayerMode.KNIGHT_WAR)) {
                            if (pc.Party == pet.Owner.Party)
                                return false;
                            return true;
                        }

                        return false;
                    }
                }
            }
            else if (sActor.type == ActorType.MOB) {
                var isSlaveOfPc = false;
                var eh = (MobEventHandler)sActor.e;

                if (eh.AI.Master != null) {
                    if (eh.AI.Master.type == ActorType.PC)
                        isSlaveOfPc = true;
                    if (dActor.type == ActorType.MOB) {
                        var deh = (MobEventHandler)dActor.e;
                        if (deh.AI.Master != null)
                            if (deh.AI.Master.ActorID == eh.AI.Master.ActorID)
                                return false;
                    }
                }

                if (!isSlaveOfPc)
                    switch (dActor.type) {
                        case ActorType.PC:
                            var pc = (ActorPC)dActor;
                            if (pc.PossessionTarget != 0)
                                return false;
                            return true;
                        case ActorType.PARTNER:
                        case ActorType.PET:
                        case ActorType.SHADOW:
                            return true;
                        case ActorType.MOB:
                            eh = (MobEventHandler)dActor.e;
                            if (eh.AI.Mode.Symbol)
                                return true;
                            return false;
                        default:
                            return false;
                    }

                switch (dActor.type) {
                    case ActorType.MOB:
                        return true;
                    case ActorType.PARTNER:
                        return true;
                    default:
                        return false;
                }
            }
            else if (sActor.type == ActorType.PARTNER) {
                switch (dActor.type) {
                    case ActorType.MOB:
                        return true;
                    case ActorType.PC:
                    case ActorType.PET:
                    case ActorType.PARTNER:
                        return false;
                }
            }
            else if (sActor.type == ActorType.GOLEM) {
                switch (dActor.type) {
                    case ActorType.MOB:
                        return true;
                    case ActorType.PC:
                    case ActorType.PET:
                        return false;
                }
            }

            return false;
        }

        private class Activator2 : MultiRunTask {
            private readonly Actor caster;
            private readonly string message;

            public Activator2(Actor caster, string message, int duetime) {
                this.caster = caster;
                this.message = message;
                DueTime = duetime;
            }

            public override void CallBack() {
                var arg = new ChatArg();
                arg.content = message;
                if (caster.type == ActorType.PC)
                    MapManager.Instance.GetMap(caster.MapID)
                        .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.CHAT, arg, caster, true);
                else
                    MapManager.Instance.GetMap(caster.MapID)
                        .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.CHAT, arg, caster, false);
                Deactivate();
            }
        }

        public enum AttackResult {
            Hit,
            Miss,
            Avoid,
            Critical,
            Guard,
            Parry
        }

        public AttackResult CalcAttackResult(Actor sActor, Actor dActor, bool ranged) {
            return CalcAttackResult(sActor, dActor, ranged, 0, 0);
        }

        public AttackResult CalcAttackResult(Actor sActor, Actor dActor, bool ranged, int shitbonus, int scribonus) {
            //命中判定
            var res = AttackResult.Miss;
            if (sActor.type == ActorType.PC) {
                var pc = (ActorPC)sActor;
                if (pc.Skills2_2.ContainsKey(973) || pc.DualJobSkills.Exists(x => x.ID == 973)) //厄运，一定概率强制回避物理技能
                {
                    //这里取副职的等级
                    var duallv = 0;
                    if (pc.DualJobSkills.Exists(x => x.ID == 973))
                        duallv = pc.DualJobSkills.FirstOrDefault(x => x.ID == 973).Level;

                    //这里取主职的等级
                    var mainlv = 0;
                    if (pc.Skills2_2.ContainsKey(973))
                        mainlv = pc.Skills2_2[973].Level;

                    var godness = 0;
                    if (pc.Skills3.ContainsKey(1114) || pc.DualJobSkills.Exists(x => x.ID == 1114)) //幸运女神
                    {
                        //这里取副职的等级
                        var duallv2 = 0;
                        if (pc.DualJobSkills.Exists(x => x.ID == 1114))
                            duallv2 = pc.DualJobSkills.FirstOrDefault(x => x.ID == 1114).Level;

                        //这里取主职的等级
                        var mainlv2 = 0;
                        if (pc.Skills2_2.ContainsKey(1114))
                            mainlv2 = pc.Skills2_2[1114].Level;

                        godness = Math.Max(duallv, mainlv) * 2;
                    }

                    if (Global.Random.Next(0, 99) < Math.Max(duallv, mainlv) * 4 + godness) return AttackResult.Avoid;
                }
            }


            if (sActor.Status.Additions.ContainsKey("PrecisionFire"))
                return AttackResult.Hit;
            if (sActor.Status.Additions.ContainsKey("RoyalDealer"))
                return AttackResult.Hit;


            if (dActor.Status.Additions.ContainsKey("FortressCircleSEQ")) return AttackResult.Miss;
            var cri = 0f;
            var criavd = 0f;
            int sHit = 0, dAvoid = 0;

            if (ranged) {
                sHit = sActor.Status.hit_ranged;
                dAvoid = dActor.Status.avoid_ranged;
            }
            else {
                sHit = sActor.Status.hit_melee;
                dAvoid = dActor.Status.avoid_melee;
            }

            //#region 计算是否自己自己丢失目标

            // 弃用, 来源于不知名的wiki
            //int leveldiff = Math.Abs(sActor.Level - dActor.Level);
            //hit = ((float)(shitbonus + sHit) / ((float)dAvoid * 0.7f)) * 100.0f;
            //if (hit > 100.0f)
            //    hit = 100.0f;
            //if (sActor.Level > dActor.Level)
            //    hit *= (1.0f + (float)((float)leveldiff / (float)100));
            //else
            //    hit *= (1.0f - (float)((float)leveldiff / (float)100));

            float hit = 0;
            //命中率的算法 by wiki http://eco.chouhuangbi.com/doku.php?id=ecowiki:战斗
            //等级差计算

            hit = Math.Min(100.0f, (shitbonus + sHit) / (dAvoid * 0.7f) * 100.0f);


            var leveldiff = sActor.Level - dActor.Level;


            if (leveldiff < 0)
                hit *= 1.0f - Math.Abs(sActor.Level - dActor.Level) / 100.0f *
                    (1.0f - sActor.Status.level_hit_iris / 100.0f);
            else
                hit *= 1.0f + Math.Abs(sActor.Level - dActor.Level) / 100.0f *
                    (1.0f - sActor.Status.level_avoid_iris / 100.0f);


            if (sActor.Status.Additions.ContainsKey("DarkLight"))
                hit *= 1.0f - (sActor.Status.Additions["DarkLight"] as DefaultBuff).Variable["DarkLight"] / 100.0f;
            if (sActor.Status.Additions.ContainsKey("FlashLight"))
                hit *= 1.0f - (sActor.Status.Additions["FlashLight"] as DefaultBuff).skill.Level * 6.0f / 100.0f;

            // 围攻不会降低目标的闪避率,也不会增加自身的命中率 by 野芙
            //if (dActor.Status.attackingActors.Count > 1)
            //{
            //    hit += (dActor.Status.attackingActors.Count - 1) * 10;
            //}

            cri = sActor.Status.hit_critical + sActor.Status.cri_skill + sActor.Status.cri_skill_rate +
                  sActor.Status.cri_item + sActor.Status.hit_critical_iris; //cri_skill_rate仅影响暴击率，不影响暴击伤害
            criavd = dActor.Status.avoid_critical + dActor.Status.criavd_skill + dActor.Status.criavd_item +
                     dActor.Status.avoid_critical_iris;

            if (dActor.Status.Additions.ContainsKey("CriUp")) //暴击标记
                cri += 5 + dActor.Status.Cri_Up_Lv * 5;
            if (sActor.type == ActorType.PC) {
                var pc = (ActorPC)sActor;
                //iris卡种族暴击率提升
                if (dActor.Race == Race.HUMAN && pc.Status.human_cri_up_iris > 0) cri += pc.Status.human_cri_up_iris;
                if (dActor.Race == Race.BIRD && pc.Status.bird_cri_up_iris > 0)
                    cri += pc.Status.bird_cri_up_iris;
                else if (dActor.Race == Race.ANIMAL && pc.Status.animal_cri_up_iris > 0)
                    cri += pc.Status.animal_cri_up_iris;
                else if (dActor.Race == Race.MAGIC_CREATURE && pc.Status.magic_c_cri_up_iris > 0)
                    cri += pc.Status.magic_c_cri_up_iris;
                else if (dActor.Race == Race.PLANT && pc.Status.plant_cri_up_iris > 0)
                    cri += pc.Status.plant_cri_up_iris;
                else if (dActor.Race == Race.WATER_ANIMAL && pc.Status.water_a_cri_up_iris > 0)
                    cri += pc.Status.water_a_cri_up_iris;
                else if (dActor.Race == Race.MACHINE && pc.Status.machine_cri_up_iris > 0)
                    cri += pc.Status.machine_cri_up_iris;
                else if (dActor.Race == Race.ROCK && pc.Status.rock_cri_up_iris > 0)
                    cri += pc.Status.rock_cri_up_iris;
                else if (dActor.Race == Race.ELEMENT && pc.Status.element_cri_up_iris > 0)
                    cri += pc.Status.element_cri_up_iris;
                else if (dActor.Race == Race.UNDEAD && pc.Status.undead_cri_up_iris > 0)
                    cri += pc.Status.undead_cri_up_iris;
            }

            var cribonus = scribonus + (cri > criavd ? cri - criavd : 1);

            if (Global.Random.Next(1, 100) <= cribonus) {
                res = AttackResult.Critical;
                hit += 35;
            }
            else {
                res = AttackResult.Hit;
            }

            if (sActor.type == ActorType.PC) {
                var pc = (ActorPC)sActor;
                //iris卡对种族命中率提升
                if (dActor.Race == Race.HUMAN && pc.Status.human_hit_up_iris > 100)
                    hit = hit * (pc.Status.human_hit_up_iris / 100.0f);
                if (dActor.Race == Race.BIRD && pc.Status.bird_hit_up_iris > 100)
                    hit = hit * (pc.Status.bird_hit_up_iris / 100.0f);
                else if (dActor.Race == Race.ANIMAL && pc.Status.animal_hit_up_iris > 100)
                    hit = hit * (pc.Status.animal_hit_up_iris / 100.0f);
                else if (dActor.Race == Race.MAGIC_CREATURE && pc.Status.magic_c_hit_up_iris > 100)
                    hit = hit * (pc.Status.magic_c_hit_up_iris / 100.0f);
                else if (dActor.Race == Race.PLANT && pc.Status.plant_hit_up_iris > 100)
                    hit = hit * (pc.Status.plant_hit_up_iris / 100.0f);
                else if (dActor.Race == Race.WATER_ANIMAL && pc.Status.water_a_hit_up_iris > 100)
                    hit = hit * (pc.Status.water_a_hit_up_iris / 100.0f);
                else if (dActor.Race == Race.MACHINE && pc.Status.machine_hit_up_iris > 100)
                    hit = hit * (pc.Status.machine_hit_up_iris / 100.0f);
                else if (dActor.Race == Race.ROCK && pc.Status.rock_hit_up_iris > 100)
                    hit = hit * (pc.Status.rock_hit_up_iris / 100.0f);
                else if (dActor.Race == Race.ELEMENT && pc.Status.element_hit_up_iris > 100)
                    hit = hit * (pc.Status.element_hit_up_iris / 100.0f);
                else if (dActor.Race == Race.UNDEAD && pc.Status.undead_hit_up_iris > 100)
                    hit = hit * (pc.Status.undead_hit_up_iris / 100.0f);
            }


            if (hit < 5.0f) hit = 5.0f;
            if (hit > 95.0f) hit = 95.0f;
            var hit_res = Global.Random.Next(1, 100);
            if (dActor.type == ActorType.PC) {
                var pc = (ActorPC)dActor;
                //iris卡种族回避率提升
                if (sActor.Race == Race.HUMAN && pc.Status.human_avoid_up_iris > 100)
                    hit_res = (int)(hit_res * (pc.Status.human_avoid_up_iris / 100.0f));
                if (sActor.Race == Race.BIRD && pc.Status.bird_avoid_up_iris > 100)
                    hit_res = (int)(hit_res * (pc.Status.bird_avoid_up_iris / 100.0f));
                else if (sActor.Race == Race.ANIMAL && pc.Status.animal_avoid_up_iris > 100)
                    hit_res = (int)(hit_res * (pc.Status.animal_avoid_up_iris / 100.0f));
                else if (sActor.Race == Race.MAGIC_CREATURE && pc.Status.magic_c_avoid_up_iris > 100)
                    hit_res = (int)(hit_res * (pc.Status.magic_c_avoid_up_iris / 100.0f));
                else if (sActor.Race == Race.PLANT && pc.Status.plant_avoid_up_iris > 100)
                    hit_res = (int)(hit_res * (pc.Status.plant_avoid_up_iris / 100.0f));
                else if (sActor.Race == Race.WATER_ANIMAL && pc.Status.water_a_avoid_up_iris > 100)
                    hit_res = (int)(hit_res * (pc.Status.water_a_avoid_up_iris / 100.0f));
                else if (sActor.Race == Race.MACHINE && pc.Status.machine_avoid_up_iris > 100)
                    hit_res = (int)(hit_res * (pc.Status.machine_avoid_up_iris / 100.0f));
                else if (sActor.Race == Race.ROCK && pc.Status.rock_avoid_up_iris > 100)
                    hit_res = (int)(hit_res * (pc.Status.rock_avoid_up_iris / 100.0f));
                else if (sActor.Race == Race.ELEMENT && pc.Status.element_avoid_up_iris > 100)
                    hit_res = (int)(hit_res * (pc.Status.element_avoid_up_iris / 100.0f));
                else if (sActor.Race == Race.UNDEAD && pc.Status.undead_avoid_up_iris > 100)
                    hit_res = (int)(hit_res * (pc.Status.undead_avoid_up_iris / 100.0f));
            }

            //#endregion

            if (hit_res > hit) return AttackResult.Miss;

            if (Global.Random.Next(1, 100) <= 5)
                return AttackResult.Avoid;

            var guard = false;
            if (dActor.type == ActorType.PC) {
                var pc = (ActorPC)dActor;
                if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.LEFT_HAND))
                    if (pc.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].BaseData.itemType == ItemType.SHIELD)
                        if (Global.Random.Next(1, 100) <=
                            pc.Status.guard_item + pc.Status.guard_skill + pc.Status.guard_iris)
                            guard = true;
            }
            else if (dActor.type == ActorType.MOB) {
                if (Global.Random.Next(1, 100) <= dActor.Status.guard_skill)
                    guard = true;
            }

            if (guard) return AttackResult.Guard;

            if (dActor.Status.Additions.ContainsKey("見切り") && Global.Random.Next(0, 100) < dActor.Status.Syaringan_rate)
                return AttackResult.Avoid;

            if (dActor.Status.Additions.ContainsKey("Parry"))
                if (Global.Random.Next(1, 100) <= 30)
                    return AttackResult.Parry;

            if (sActor.Status.Additions.ContainsKey("AffterCritical")) {
                res = AttackResult.Critical;
                RemoveAddition(sActor, "AffterCritical");
            }

            if (sActor.Status.MissRevenge_hit) {
                sActor.Status.MissRevenge_hit = false;
                res = AttackResult.Critical;
            }

            return res;
        }

        public AttackResult CalcMagicAttackResult(Actor sActor, Actor dActor) {
            //命中判定
            var res = AttackResult.Hit;

            if (sActor.type == ActorType.PC) {
                var pc = (ActorPC)sActor;
                if (pc.Skills2_2.ContainsKey(960) || pc.DualJobSkills.Exists(x => x.ID == 960)) //强运，一定概率强制回避魔法技能
                {
                    //这里取副职的等级
                    var duallv = 0;
                    if (pc.DualJobSkills.Exists(x => x.ID == 960))
                        duallv = pc.DualJobSkills.FirstOrDefault(x => x.ID == 960).Level;

                    //这里取主职的等级
                    var mainlv = 0;
                    if (pc.Skills2_2.ContainsKey(960))
                        mainlv = pc.Skills2_2[960].Level;

                    var godness = 0;
                    if (pc.Skills3.ContainsKey(1114) || pc.DualJobSkills.Exists(x => x.ID == 1114)) //幸运女神
                    {
                        //这里取副职的等级
                        var duallv2 = 0;
                        if (pc.DualJobSkills.Exists(x => x.ID == 1114))
                            duallv2 = pc.DualJobSkills.FirstOrDefault(x => x.ID == 1114).Level;

                        //这里取主职的等级
                        var mainlv2 = 0;
                        if (pc.Skills2_2.ContainsKey(1114))
                            mainlv2 = pc.Skills2_2[1114].Level;

                        godness = Math.Max(duallv, mainlv) * 2;
                    }

                    if (Global.Random.Next(0, 99) < Math.Max(duallv, mainlv) * 4 + godness) return AttackResult.Avoid;
                }
            }

            if (dActor.Status.Additions.ContainsKey("見切り") && Global.Random.Next(0, 100) < dActor.Status.Syaringan_rate)
                return AttackResult.Avoid;
            return res;
        }

        public float CalcCritBonus(Actor sActor, Actor dActor, int scribonus = 0) {
            var res = 1.0f;
            int cri = scribonus, criavd = 0;
            if (sActor.type == ActorType.PC)
                cri += ((ActorPC)sActor).Status.hit_critical + ((ActorPC)sActor).Status.cri_skill +
                       ((ActorPC)sActor).Status.cri_item + ((ActorPC)sActor).Status.hit_critical_iris;
            if (sActor.type == ActorType.MOB)
                cri += ((ActorMob)sActor).Status.hit_critical + ((ActorMob)sActor).Status.cri_skill;
            if (dActor.type == ActorType.PC)
                criavd = ((ActorPC)dActor).Status.avoid_critical + ((ActorPC)dActor).Status.criavd_skill +
                         ((ActorPC)dActor).Status.criavd_item + ((ActorPC)dActor).Status.avoid_critical_iris;
            if (dActor.type == ActorType.MOB)
                criavd = ((ActorMob)dActor).Status.avoid_critical + ((ActorMob)dActor).Status.criavd_skill;
            if (dActor.Status.Additions.ContainsKey("CriUp")) //暴击标记
                cri += 5 + dActor.Status.Cri_Up_Lv * 5;
            if (sActor.type == ActorType.PC) {
                var pc = (ActorPC)sActor;
                //iris卡种族暴击率提升
                if (dActor.Race == Race.HUMAN && pc.Status.human_cri_up_iris > 0) cri += pc.Status.human_cri_up_iris;
                if (dActor.Race == Race.BIRD && pc.Status.bird_cri_up_iris > 0)
                    cri += pc.Status.bird_cri_up_iris;
                else if (dActor.Race == Race.ANIMAL && pc.Status.animal_cri_up_iris > 0)
                    cri += pc.Status.animal_cri_up_iris;
                else if (dActor.Race == Race.MAGIC_CREATURE && pc.Status.magic_c_cri_up_iris > 0)
                    cri += pc.Status.magic_c_cri_up_iris;
                else if (dActor.Race == Race.PLANT && pc.Status.plant_cri_up_iris > 0)
                    cri += pc.Status.plant_cri_up_iris;
                else if (dActor.Race == Race.WATER_ANIMAL && pc.Status.water_a_cri_up_iris > 0)
                    cri += pc.Status.water_a_cri_up_iris;
                else if (dActor.Race == Race.MACHINE && pc.Status.machine_cri_up_iris > 0)
                    cri += pc.Status.machine_cri_up_iris;
                else if (dActor.Race == Race.ROCK && pc.Status.rock_cri_up_iris > 0)
                    cri += pc.Status.rock_cri_up_iris;
                else if (dActor.Race == Race.ELEMENT && pc.Status.element_cri_up_iris > 0)
                    cri += pc.Status.element_cri_up_iris;
                else if (dActor.Race == Race.UNDEAD && pc.Status.undead_cri_up_iris > 0)
                    cri += pc.Status.undead_cri_up_iris;
            }

            res = 1.0f + (cri > criavd ? cri - criavd : 0) / 100.0f;
            res *= sActor.Status.hit_critical_rate_iris / 100.0f;
            return res;
        }

        public void ItemUse(Actor sActor, Actor dActor, SkillArg arg) {
            var list = new List<Actor>();
            list.Add(dActor);
            ItemUse(sActor, list, arg);
        }

        public void ItemUse(Actor sActor, List<Actor> dActor, SkillArg arg) {
            var counter = 0;
            arg.affectedActors = dActor;
            arg.Init();

            if (arg.item.BaseData.duration == 0)
                foreach (var i in dActor) {
                    if (i.Buff.NoRegen)
                        continue;
                    uint itemhp, itemsp, itemmp, itemep;
                    if (arg.item.BaseData.isRate) {
                        var recover = 1.0f;
                        if (arg.item.BaseData.itemType == ItemType.FOOD) {
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

                        if (arg.item.BaseData.itemType == ItemType.POTION) {
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
                    else {
                        var recover = 1.0f;
                        if (arg.item.BaseData.itemType == ItemType.FOOD) {
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

                        if (arg.item.BaseData.itemType == ItemType.POTION) {
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

                    if (sActor is ActorPC) {
                        var pc = (ActorPC)sActor;

                        if ((pc.Skills.ContainsKey(103) || pc.DualJobSkills.Exists(x => x.ID == 103)) &&
                            arg.item.BaseData.hp > 0) {
                            var duallv = 0;
                            if (pc.DualJobSkills.Exists(x => x.ID == 103))
                                duallv = pc.DualJobSkills.FirstOrDefault(x => x.ID == 103).Level;

                            var mainlv = 0;
                            if (pc.Skills.ContainsKey(103))
                                mainlv = pc.Skills[103].Level;

                            itemhp += (uint)(15 + arg.item.BaseData.hp * Math.Max(duallv, mainlv) * 0.03f);
                        }

                        if ((pc.Skills.ContainsKey(104) || pc.DualJobSkills.Exists(x => x.ID == 104)) &&
                            arg.item.BaseData.mp > 0) {
                            var duallv = 0;
                            if (pc.DualJobSkills.Exists(x => x.ID == 104))
                                duallv = pc.DualJobSkills.FirstOrDefault(x => x.ID == 104).Level;

                            var mainlv = 0;
                            if (pc.Skills.ContainsKey(104))
                                mainlv = pc.Skills[104].Level;

                            itemmp += (uint)(15 + arg.item.BaseData.mp * Math.Max(duallv, mainlv) * 0.03f);
                        }

                        if ((pc.Skills.ContainsKey(105) || pc.DualJobSkills.Exists(x => x.ID == 105)) &&
                            arg.item.BaseData.sp > 0) {
                            var duallv = 0;
                            if (pc.DualJobSkills.Exists(x => x.ID == 105))
                                duallv = pc.DualJobSkills.FirstOrDefault(x => x.ID == 105).Level;

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

                    if (arg.item.BaseData.hp > 0) {
                        arg.flag[counter] |= AttackFlag.HP_HEAL;
                        arg.hp[counter] = (int)-itemhp;
                    }
                    else if (arg.item.BaseData.hp < 0) {
                        arg.flag[counter] |= AttackFlag.HP_DAMAGE;
                        arg.hp[counter] = (int)-itemhp;
                    }

                    if (arg.item.BaseData.sp > 0) {
                        arg.flag[counter] |= AttackFlag.SP_HEAL;
                        arg.sp[counter] = (int)-itemsp;
                    }
                    else if (arg.item.BaseData.sp < 0) {
                        arg.flag[counter] |= AttackFlag.SP_DAMAGE;
                        arg.sp[counter] = (int)-itemsp;
                    }

                    if (arg.item.BaseData.mp > 0) {
                        arg.flag[counter] |= AttackFlag.MP_HEAL;
                        arg.mp[counter] = (int)-itemmp;
                    }
                    else if (arg.item.BaseData.mp < 0) {
                        arg.flag[counter] |= AttackFlag.MP_DAMAGE;
                        arg.mp[counter] = (int)-itemmp;
                    }

                    counter++;
                    MapManager.Instance.GetMap(sActor.MapID)
                        .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.HPMPSP_UPDATE, null, i, true);
                }
            else if (arg.item.BaseData.duration > 0)
                foreach (var i in dActor) {
                    if (arg.item.BaseData.hp > 0) {
                        i.Status.hp_medicine = arg.item.BaseData.hp;
                        var skill1 = new Medicine1(null, i, (int)arg.item.BaseData.duration);
                        ApplyAddition(i, skill1);
                    }

                    if (arg.item.BaseData.mp > 0) {
                        i.Status.mp_medicine = arg.item.BaseData.mp;
                        var skill2 = new Medicine2(null, i, (int)arg.item.BaseData.duration);
                        ApplyAddition(i, skill2);
                    }

                    if (arg.item.BaseData.sp > 0) {
                        i.Status.sp_medicine = arg.item.BaseData.sp;
                        var skill3 = new Medicine3(null, i, (int)arg.item.BaseData.duration);
                        ApplyAddition(i, skill3);
                    }
                }
            //arg.delay = 5000;
        }

        /// <summary>
        ///     获取移动角度的坐标
        /// </summary>
        /// <param name="Dir">旋转角度</param>
        /// <param name="CirPoint">圆心坐标</param>
        /// <param name="Range">移动的距离</param>
        /// <returns>目标点的坐标</returns>
        public short[] GetNewPoint(double Dir, short X, short Y, int Range) {
            var pos = new short[2];
            Range *= 100;
            var Dir2 = (Dir - 270) / 180 * Math.PI;
            var newx = (int)(Range * Math.Cos(Dir2));
            var newy = (int)(Range * Math.Sin(Dir2));
            pos[0] = (short)(X + newx);
            pos[1] = (short)(Y - newy);
            return pos;
        }

        /// <summary>
        ///     获取两个坐标间的路径
        /// </summary>
        /// <param name="fromx">原x</param>
        /// <param name="fromy">原y</param>
        /// <param name="tox">目标x</param>
        /// <param name="toy">目标y</param>
        /// <returns>路径坐标集</returns>
        public List<Point> GetStraightPath(byte fromx, byte fromy, byte tox, byte toy) {
            var path = new List<Point>();
            if (fromx == tox && fromy == toy)
                return path;
            double k; // 
            double nowx = fromx;
            double nowy = fromy;
            int x = fromx;
            int y = fromy;
            sbyte addx, addy;
            if (Math.Abs(toy - fromy) <= Math.Abs(tox - fromx)) {
                if (tox == fromx) {
                    if (fromy < toy)
                        for (var i = fromy + 1; i <= toy; i++) {
                            var t = new Point();
                            t.x = fromx;
                            t.y = (byte)i;
                            path.Add(t);
                        }
                    else
                        for (var i = fromy - 1; i <= toy; i--) {
                            var t = new Point();
                            t.x = fromx;
                            t.y = (byte)i;
                            path.Add(t);
                        }
                }
                else {
                    k = Math.Abs((double)(toy - fromy) / (tox - fromx));
                    if (toy < fromy)
                        addy = -1;
                    else
                        addy = 1;
                    if (tox < fromx)
                        addx = -1;
                    else
                        addx = 1;
                    while (Math.Round(nowx) != tox) {
                        x += addx;
                        if (Math.Round(nowy) != Math.Round(nowy + k * addy))
                            y += addy;
                        nowx += addx;
                        nowy += k * addy;

                        var t = new Point();
                        t.x = (byte)x;
                        t.y = (byte)y;
                        path.Add(t);
                    }
                }
            }
            else {
                if (toy == fromy) {
                    if (fromx < tox)
                        for (var i = fromx + 1; i <= tox; i++) {
                            var t = new Point();
                            t.x = (byte)i;
                            t.y = fromy;
                            path.Add(t);
                        }
                    else
                        for (var i = fromx - 1; i <= tox; i--) {
                            var t = new Point();
                            t.x = (byte)i;
                            t.y = fromy;
                            path.Add(t);
                        }
                }
                else {
                    k = Math.Abs((double)(tox - fromx) / (toy - fromy));
                    if (toy < fromy)
                        addy = -1;
                    else
                        addy = 1;
                    if (tox < fromx)
                        addx = -1;
                    else
                        addx = 1;
                    while (Math.Round(nowy) != toy) {
                        y += addy;
                        if (Math.Round(nowx) != Math.Round(nowx + k * addx))
                            x += addx;
                        nowy += addy;
                        nowx += k * addx;

                        var t = new Point();
                        t.x = (byte)x;
                        t.y = (byte)y;
                        path.Add(t);
                    }
                }
            }

            return path;
        }

        public class Point {
            public byte x, y;
        }

        //放置Skill定義中所需的方向相關之Function
        //Place the direction functions which will used by SkillDefinations

        //#region Direction

        /// <summary>
        ///     人物的方向
        /// </summary>
        public enum ActorDirection {
            South = 0,
            SouthEast = 7,
            East = 6,
            NorthEast = 5,
            North = 4,
            NorthWest = 3,
            West = 2,
            SouthWest = 1
        }

        /// <summary>
        ///     取得人物方向
        /// </summary>
        /// //编程数学我完全不会啊！要是有更方便的方法就帮我写了吧orz
        /// <param name="sActor">人物</param>
        /// <returns>方向</returns>
        public ActorDirection GetDirection(Actor sActor) {
            return (ActorDirection)Math.Ceiling((double)(sActor.Dir / 45));
        }

        public ActorDirection GetDirection(ushort dir) {
            return (ActorDirection)Math.Ceiling((double)(dir / 45));
        }

        /// <summary>
        ///     判断目标是否背向发起者
        /// </summary>
        /// <param name="sActor">发起者</param>
        /// <param name="dActor">目标</param>
        /// <returns>结果</returns>
        public bool GetIsBack(Actor sActor, Actor dActor) {
            switch (GetDirection(sActor)) {
                case ActorDirection.East:
                    switch (GetDirection(dActor)) {
                        case ActorDirection.East:
                        case ActorDirection.NorthEast:
                        case ActorDirection.SouthEast:
                            return true;
                    }

                    return false;
                case ActorDirection.North:
                    switch (GetDirection(dActor)) {
                        case ActorDirection.North:
                        case ActorDirection.NorthWest:
                        case ActorDirection.NorthEast:
                            return true;
                    }

                    return false;
                case ActorDirection.South:
                    switch (GetDirection(dActor)) {
                        case ActorDirection.South:
                        case ActorDirection.SouthWest:
                        case ActorDirection.SouthEast:
                            return true;
                    }

                    return false;
                case ActorDirection.West:
                    switch (GetDirection(dActor)) {
                        case ActorDirection.West:
                        case ActorDirection.SouthWest:
                        case ActorDirection.NorthWest:
                            return true;
                    }

                    return false;
                case ActorDirection.NorthEast:
                    switch (GetDirection(dActor)) {
                        case ActorDirection.NorthEast:
                        case ActorDirection.North:
                        case ActorDirection.East:
                            return true;
                    }

                    return false;
                case ActorDirection.NorthWest:
                    switch (GetDirection(dActor)) {
                        case ActorDirection.NorthWest:
                        case ActorDirection.North:
                        case ActorDirection.West:
                            return true;
                    }

                    return false;
                case ActorDirection.SouthEast:
                    switch (GetDirection(dActor)) {
                        case ActorDirection.SouthEast:
                        case ActorDirection.South:
                        case ActorDirection.East:
                            return true;
                    }

                    return false;
                case ActorDirection.SouthWest:
                    switch (GetDirection(dActor)) {
                        case ActorDirection.SouthWest:
                        case ActorDirection.South:
                        case ActorDirection.West:
                            return true;
                    }

                    return false;
            }

            return false;
        }

        /// <summary>
        ///     隨機獲得actor周圍坐標
        /// </summary>
        /// <param name="map">地圖</param>
        /// <param name="Actor">目標</param>
        /// <param name="X">回傳X</param>
        /// <param name="Y">回傳Y</param>
        /// <param name="Round">範圍格</param>
        public void GetTRoundPos(Map map, Actor Actor, out byte X, out byte Y, byte Round) {
            byte iffx, iffy;
            iffx = Global.PosX16to8(Actor.X, map.Width);
            iffy = Global.PosY16to8(Actor.Y, map.Height);
            byte outx = 0, outy = 0;
            do {
                if (iffx + Global.Random.Next(-Round, Round) < 0)
                    outx = 0;
                else if (iffx + Global.Random.Next(-Round, Round) > 255)
                    outx = 255;
                else
                    outx = (byte)(iffx + Global.Random.Next(-Round, Round));
                if (iffy + Global.Random.Next(-Round, Round) < 0)
                    outy = 0;
                else if (iffx + Global.Random.Next(-Round, Round) > 255)
                    outy = 255;
                else
                    outy = (byte)(iffy + Global.Random.Next(-Round, Round));
            } while (iffx == outx && iffy == outy);

            X = outx;
            Y = outy;
        }

        /// <summary>
        ///     取得背后的坐标
        /// </summary>
        /// //编程数学我完全不会啊！要是有更方便的方法就帮我写了吧orz
        /// <param name="map">地圖</param>
        /// <param name="Actor">目标</param>
        /// <param name="XDiff">回传X</param>
        /// <param name="YDiff">回传Y</param>
        public void GetTBackPos(Map map, Actor Actor, out byte X, out byte Y) {
            GetTBackPos(map, Actor, out X, out Y, false);
        }

        public void GetTFrontPos(Map map, Actor Actor, out byte X, out byte Y) {
            GetTBackPos(map, Actor, out X, out Y, true);
        }

        public void GetTBackPos(Map map, Actor Actor, out byte X, out byte Y, bool front) {
            byte iffx, iffy;
            iffx = Global.PosX16to8(Actor.X, map.Width);
            iffy = Global.PosY16to8(Actor.Y, map.Height);
            switch (GetDirection(Actor.Dir)) {
                case ActorDirection.East:
                    if (front) {
                        X = (byte)(iffx + 1);
                        Y = iffy;
                    }
                    else {
                        X = (byte)(iffx - 1);
                        Y = iffy;
                    }

                    break;
                case ActorDirection.SouthEast:
                    if (front) {
                        X = (byte)(iffx + 1);
                        Y = (byte)(iffy + 1);
                    }
                    else {
                        X = (byte)(iffx - 1);
                        Y = (byte)(iffy - 1);
                    }

                    break;
                case ActorDirection.South:
                    if (front) {
                        X = iffx;
                        Y = (byte)(iffy + 1);
                    }
                    else {
                        X = iffx;
                        Y = (byte)(iffy - 1);
                    }

                    break;
                case ActorDirection.SouthWest:
                    if (front) {
                        X = (byte)(iffx - 1);
                        Y = (byte)(iffy + 1);
                    }
                    else {
                        X = (byte)(iffx + 1);
                        Y = (byte)(iffy - 1);
                    }

                    break;
                case ActorDirection.West:
                    if (front) {
                        X = (byte)(iffx - 1);
                        Y = iffy;
                    }
                    else {
                        X = (byte)(iffx + 1);
                        Y = iffy;
                    }

                    break;
                case ActorDirection.NorthWest:
                    if (front) {
                        X = (byte)(iffx - 1);
                        Y = (byte)(iffy - 1);
                    }
                    else {
                        X = (byte)(iffx + 1);
                        Y = (byte)(iffy + 1);
                    }

                    break;
                case ActorDirection.North:
                    if (front) {
                        X = iffx;
                        Y = (byte)(iffy - 1);
                    }
                    else {
                        X = iffx;
                        Y = (byte)(iffy + 1);
                    }

                    break;
                case ActorDirection.NorthEast:
                    if (front) {
                        X = (byte)(iffx + 1);
                        Y = (byte)(iffy - 1);
                    }
                    else {
                        X = (byte)(iffx - 1);
                        Y = (byte)(iffy + 1);
                    }

                    break;
                default:
                    X = iffx;
                    Y = iffy;
                    break;
            }
        }

        /// <summary>
        ///     取得座標差(-sActordActor)
        /// </summary>
        /// <param name="map">地圖</param>
        /// <param name="sActor">使用技能的角色</param>
        /// <param name="dActor">目標角色</param>
        /// <param name="XDiff">回傳X的差異(格)</param>
        /// <param name="YDiff">回傳Y的差異(格)</param>
        public void GetXYDiff(Map map, Actor sActor, Actor dActor, out int XDiff, out int YDiff) {
            XDiff = Global.PosX16to8(dActor.X, map.Width) - Global.PosX16to8(sActor.X, map.Width);
            YDiff = Global.PosY16to8(sActor.Y, map.Height) - Global.PosY16to8(dActor.Y, map.Height);
        }

        /// <summary>
        ///     計算座標差之Hash值
        /// </summary>
        /// <param name="x">X座標</param>
        /// <param name="y">Y座標</param>
        /// <param name="SkillRange">技能範圍(EX.3x3=3) </param>
        /// <returns>座標之Hash值</returns>
        public int CalcPosHashCode(int x, int y, int SkillRange) {
            var nx = x + SkillRange;
            var ny = y + SkillRange;
            return nx * 100 + ny;
        }

        /// <summary>
        ///     取得對應之座標
        /// </summary>
        /// <param name="sActor">基準人物(原點)</param>
        /// <param name="XDiff">X座標偏移量(單位：格)</param>
        /// <param name="YDiff">Y座標偏移量(單位：格)</param>
        /// <param name="nx">回傳X</param>
        /// <param name="ny">回傳Y</param>
        /// <returns>是否正常(無溢位發生)</returns>
        public bool GetRelatedPos(Actor sActor, int XDiff, int YDiff, out short nx, out short ny) {
            byte nbx, nby = 0;
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var ox = Global.PosX16to8(sActor.X, map.Width);
            var oy = Global.PosY16to8(sActor.Y, map.Height);
            var ret = GetRelatedPos(sActor, XDiff, YDiff, ox, oy, out nbx, out nby);
            nx = Global.PosX8to16(nbx, map.Width);
            ny = Global.PosY8to16(nby, map.Height);
            return ret;
        }

        /// <summary>
        ///     取得對應之座標
        /// </summary>
        /// <param name="sActor">基準人物(原點)</param>
        /// <param name="XDiff">X座標偏移量(單位：格)</param>
        /// <param name="YDiff">Y座標偏移量(單位：格)</param>
        /// <param name="nx">回傳X</param>
        /// <param name="ny">回傳Y</param>
        /// <returns>是否正常(無溢位發生)</returns>
        public bool GetRelatedPos(Actor sActor, int XDiff, int YDiff, out byte nx, out byte ny) {
            var map = MapManager.Instance.GetMap(sActor.MapID);
            //取得舊座標
            var ox = Global.PosX16to8(sActor.X, map.Width);
            var oy = Global.PosY16to8(sActor.Y, map.Height);
            return GetRelatedPos(sActor, XDiff, YDiff, ox, oy, out nx, out ny);
        }

        /// <summary>
        ///     取得對應之座標
        /// </summary>
        /// <param name="sActor">基準人物</param>
        /// <param name="XDiff">X座標偏移量(單位：格)</param>
        /// <param name="YDiff">Y座標偏移量(單位：格)</param>
        /// <param name="sx">原點X</param>
        /// <param name="sy">原點Y</param>
        /// <param name="nx">回傳X</param>
        /// <param name="ny">回傳Y</param>
        /// <returns>是否正常(無溢位發生)</returns>
        public bool GetRelatedPos(Actor sActor, int XDiff, int YDiff, byte sx, byte sy, out byte nx, out byte ny) {
            MapManager.Instance.GetMap(sActor.MapID);
            //取得舊座標
            var ox = sx;
            var oy = sy;
            //判斷溢位
            if ((ox == 0 && XDiff < 0) ||
                (ox == 0xff && XDiff > 0) ||
                (oy == 0 && YDiff < 0) ||
                (oy == 0xff && YDiff > 0)) {
                nx = ox;
                ny = oy;
                return false;
            }

            //計算新座標
            nx = (byte)(ox + XDiff);
            ny = (byte)(oy + YDiff);
            return true;
        }

        //#endregion

        public Dictionary<uint, MobISkill> MobskillHandlers = new Dictionary<uint, MobISkill>();

        private string path;
        public Dictionary<uint, ISkill> skillHandlers = new Dictionary<uint, ISkill>();
        private uint skillID;

        public void LoadSkill(string path) {
            Logger.GetLogger().Information("開始加載技能...");
            var dic = new Dictionary<string, string> { { "CompilerVersion", "v3.5" } };
            var provider = new CSharpCodeProvider(dic);
            var skillcount = 0;
            this.path = path;
            try {
                var files = Directory.GetFiles(path, "*cs", SearchOption.AllDirectories);
                Assembly newAssembly;
                int tmp;
                if (files.Length > 0) {
                    newAssembly = CompileScript(files, provider);
                    if (newAssembly != null) {
                        tmp = LoadAssembly(newAssembly);
                        Logger.GetLogger().Information(string.Format("Containing {0} Skills", tmp));
                        skillcount += tmp;
                    }
                }
            }
            catch (Exception ex) {
                Logger.GetLogger().Error(ex, ex.Message);
            }

            Logger.GetLogger().Information(string.Format("外置技能加載數：{0}", skillcount));
        }

        private Assembly CompileScript(string[] Source, CodeDomProvider Provider) {
            //ICodeCompiler compiler = Provider.;
            var parms = new CompilerParameters();
            CompilerResults results;

            // Configure parameters
            parms.CompilerOptions = "/target:library /optimize";
            parms.GenerateExecutable = false;
            parms.GenerateInMemory = true;
            parms.IncludeDebugInformation = true;
            //parms.ReferencedAssemblies.Add(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + @"\Reference Assemblies\Microsoft\Framework\v3.5\System.Data.DataSetExtensions.dll");
            //parms.ReferencedAssemblies.Add(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + @"\Reference Assemblies\Microsoft\Framework\v3.5\System.Core.dll");
            //parms.ReferencedAssemblies.Add(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + @"\Reference Assemblies\Microsoft\Framework\v3.5\System.Xml.Linq.dll");
            parms.ReferencedAssemblies.Add("System.dll");
            parms.ReferencedAssemblies.Add("SagaLib.dll");
            parms.ReferencedAssemblies.Add("SagaDB.dll");
            parms.ReferencedAssemblies.Add("SagaMap.exe");
            foreach (var i in Configuration.Configuration.Instance.ScriptReference) parms.ReferencedAssemblies.Add(i);
            // Compile
            results = Provider.CompileAssemblyFromFile(parms, Source);
            if (results.Errors.HasErrors) {
                foreach (CompilerError error in results.Errors)
                    if (!error.IsWarning) {
                        Logger.GetLogger().Error("Compile Error:" + error.ErrorText, null);
                        Logger.GetLogger().Error("File:" + error.FileName + ":" + error.Line, null);
                    }

                return null;
            }

            //get a hold of the actual assembly that was generated
            return results.CompiledAssembly;
        }

        private int LoadAssembly(Assembly newAssembly) {
            var newScripts = newAssembly.GetModules();
            var count = 0;
            foreach (var newScript in newScripts) {
                var types = newScript.GetTypes();
                foreach (var npcType in types) {
                    try {
                        if (npcType.IsAbstract) continue;
                        if (npcType.GetCustomAttributes(false).Length > 0) continue;
                        ISkill newEvent;

                        newEvent = (ISkill)Activator.CreateInstance(npcType);
                        var t = newEvent.ToString();
                        var id = t.Substring(t.LastIndexOf("S") + 1, t.Length - t.LastIndexOf("S") - 1);
                        skillID = uint.Parse(id);

                        if (!skillHandlers.ContainsKey(skillID) && skillID != 0) {
                            skillHandlers.Add(skillID, newEvent);
                        }
                        else {
                            if (skillID != 0)
                                Logger.GetLogger().Warning(string.Format("EventID:{0} already exists, Class:{1} droped",
                                    skillID, npcType.FullName));
                        }
                    }
                    catch (Exception ex) {
                        Logger.GetLogger().Error(ex, ex.Message);
                    }

                    count++;
                }
            }

            return count;
        }

        public void Init() {
            skillHandlers.Add(2178, new EmergencyAvoid());

            MobskillHandlers.Add(20017, new BlackHoleOfPP());

            skillHandlers.Add(9125, new DeathFiger());
            //skillHandlers.Add(2115, new SagaMap.Skill.SkillDefinations.Event.PressionKiller(true));

            //#region 巨大咕咕鸡

            MobskillHandlers.Add(20000, new BlackHole());
            MobskillHandlers.Add(20001, new GuguPoison());

            //#endregion

            //#region 熊爹

            MobskillHandlers.Add(20005, new IceHole()); //废弃
            MobskillHandlers.Add(20006, new Rowofcloudpalm()); //废弃
            MobskillHandlers.Add(20007, new Fengshenlegs()); //废弃
            MobskillHandlers.Add(20009, new Attack());

            //#endregion

            //#region 领主骑士

            MobskillHandlers.Add(20010, new KnightAttack());
            MobskillHandlers.Add(20011, new IceHeart());
            MobskillHandlers.Add(20012, new Iceroad());
            MobskillHandlers.Add(20013, new IceDef());

            //#endregion

            //#region 天骸鸢

            MobskillHandlers.Add(20015, new FireInfernal());

            //#endregion

            skillHandlers.Add(20002, new EnergyOneForWeapon());
            skillHandlers.Add(20004, new IaiForWeapon());
            skillHandlers.Add(402, new MaxHealMpForWeapon());
            skillHandlers.Add(20014, new Snipe());

            //#region c-1 new skill

            skillHandlers.Add(8900, new ShadowBlast());

            //#endregion

            //#region Royaldealer

            skillHandlers.Add(989, new DealerSkill()); //18.05.13 lv 3  
            skillHandlers.Add(3361, new CAPACommunion()); //12月2日实装,lv6
            skillHandlers.Add(3371, new RoyalDealer()); //18.05.13 lv 10
            skillHandlers.Add(2491, new DamageUp()); //12月3日实装,lv13
            //noLv20
            skillHandlers.Add(2501, new CriUp()); //12月3日实装,lv23
            skillHandlers.Add(2502, new PerfectRiotStamp()); //18.05.13 lv 25
            //noLv25
            skillHandlers.Add(3404, new Rhetoric()); //12月3日实装,lv30(强运系统未装)
            skillHandlers.Add(2517, new StraightFlush()); //18.05.13 lv 35
            skillHandlers.Add(2518, new StraightFlushSEQ()); //18.05.13 lv 35
            skillHandlers.Add(1114, new LuckyGoddess()); //18.08.04 lv 40
            skillHandlers.Add(2558, new FalseMoney()); //18.05.13 lv 47
            skillHandlers.Add(2559, new TimeIsMoney()); //16.06.08 lv50
            //no 35 40 45

            //#endregion

            //#region Joker

            skillHandlers.Add(2519, new JokerStyle()); //9月9日实装
            skillHandlers.Add(2523, new IkspiariArmusing()); //9月9日实装
            skillHandlers.Add(2494, new JokerDelay()); //9月9日实装
            skillHandlers.Add(3390, new JokerArt()); //9月9日实装
            skillHandlers.Add(2483, new JokerStrike()); //9月9日实装
            skillHandlers.Add(990, new FullWeaponMaster()); //9月9日实装
            skillHandlers.Add(991, new FullerMaster()); //9月9日实装
            skillHandlers.Add(3410, new DivineProtection()); //9月9日实装
            skillHandlers.Add(3437, new StyleChange()); //9月9日实装
            skillHandlers.Add(2560, new JokerNone()); //9月9日实装,未完成
            skillHandlers.Add(2562, new JokerTwoHead()); //9月9日实装,未完成
            skillHandlers.Add(2566, new Joker()); //9月9日实装,未完成

            //#endregion

            //#region Stryder

            skillHandlers.Add(2482, new Xusihaxambi()); //2018年1月8日实装，lv3
            skillHandlers.Add(3352, new SPCommunion()); //12月2日实装，lv6
            //lv10被动不知作用
            skillHandlers.Add(2490, new StrapFlurry()); //2018年1月8日实装，lv13
            skillHandlers.Add(3382, new BannedOutfit()); //12月2日实装，lv20（只实现显示BUFF效果）  
            //lv23被动不知作用
            skillHandlers.Add(3385, new SkillForbid()); //12月2日实装，lv25（只实现显示BUFF效果）
            skillHandlers.Add(3402, new PartyBivouac()); //12月2日实装，lv30
            skillHandlers.Add(3403, new FlurryThunderbolt()); //12月2日实装，lv35
            skillHandlers.Add(992, new TreasureMaster()); //2018/4/5实装,job45
            skillHandlers.Add(2551, new PillageAct()); //2018/4/5实装,job47
            skillHandlers.Add(2552, new ArtFullTrap()); //2018/5/14,job50
            //缺少35、40

            //#endregion

            //#region Maestro

            skillHandlers.Add(2480, new WeaponStrengthen()); //12月1日实装，lv3（未完成，需要封包）
            skillHandlers.Add(3353, new ATKCommunion()); //12月1日实装，lv6
            skillHandlers.Add(987, new GreatMaster()); //12月1日实装,lv10（加成不明确）
            skillHandlers.Add(2487, new PotentialWeapon()); //12月1日实装，lv13（未完成，需要封包）
            skillHandlers.Add(2489, new RobotAtkUp()); //12月1日实装，lv20
            skillHandlers.Add(2500, new RobotDefUp()); //12月1日实装，lv23
            skillHandlers.Add(2506, new RobotCSPDUp()); //12月1日实装，lv25
            skillHandlers.Add(3401, new WeaponAtkUp()); //12月1日实装，lv30
            //缺少35、40
            skillHandlers.Add(2524, new RobotLaser()); //12月2日实装，lv45
            skillHandlers.Add(2549, new LimitExceed()); //7月1日实装，lv47
            skillHandlers.Add(2550, new WasteThrowing()); //2018.1.17实装,lv50

            //#endregion

            //#region Guardian

            skillHandlers.Add(983, new SpearMaster()); //11月24日实装，LV3习得
            skillHandlers.Add(3355, new def_addCommunion());
            skillHandlers.Add(3363, new Guardian()); //11月24日实装，LV10习得
            skillHandlers.Add(1102, new ReflectionShield()); //11月24日实装，LV20习得
            skillHandlers.Add(3386, new SoulProtect()); //8月1日实装，lv25习得
            skillHandlers.Add(2512, new ShieldImpact()); //11月24日实装，lv30习得
            skillHandlers.Add(2513, new SpiralSpear()); //11月24日实装，lv35习得
            skillHandlers.Add(2533, new StrongBody()); //11月24日实装，lv40习得
            skillHandlers.Add(2532, new Blocking()); //11月24日实装，lv45习得
            skillHandlers.Add(1101, new HatredUp()); //11月25日实装，lv13习得
            skillHandlers.Add(2535, new FortressCircle()); //2018年1月10日实装,Lv47习得//未完善
            skillHandlers.Add(2536, new FortressCircleSEQ()); //后续技能,同上
            skillHandlers.Add(2537, new LightOfTheDarkness()); //16.02.02, lv50

            //#endregion

            //#region Eraser

            skillHandlers.Add(984, new EraserMaster()); //11月24日实装，lv3习得
            skillHandlers.Add(3358, new AVOIDCommunion());
            skillHandlers.Add(3364, new Purger()); //11月24日实装，lv10习得
            skillHandlers.Add(2486, new Efuikasu()); //11月24日实装，Lv20习得
            skillHandlers.Add(3387, new Syaringan()); //11月24日实装，lv25习得
            skillHandlers.Add(2516, new EvilSpirit()); //11月24日实装，lv30习得
            skillHandlers.Add(2508, new Demacia()); //11月24日实装，lv35习得
            skillHandlers.Add(2529, new ShadowSeam()); //11月24日实装，lv40习得//未完成
            skillHandlers.Add(994, new PoisonMaster()); //2018年1月11日实装，lv45习得
            skillHandlers.Add(2541, new VenomBlast()); //2018年实装，lv47习得
            skillHandlers.Add(2542, new VenomBlastSeq()); //2018年实装，lv47习得
            skillHandlers.Add(2543, new Instant()); //16.05.11实装,lv50

            //#endregion

            //#region Hawkeye

            skillHandlers.Add(3357, new HITCommunion());
            skillHandlers.Add(985, new HawkeyeMaster()); //11月24日实装，lv3习得
            skillHandlers.Add(3365, new EagleEye()); //11月24日实装，lv10习得
            skillHandlers.Add(1103, new Nooheito()); //11月25日实装，lv13习得
            skillHandlers.Add(2485, new SmokeBall()); //11月25日实装，lv20习得
            skillHandlers.Add(1107, new MissRevenge()); //11月25日实装，lv23习得
            skillHandlers.Add(2504, new WithinWeeks()); //11月25日实装，lv25习得
            skillHandlers.Add(2514, new TimeBomb()); //11月25日实装，lv30习得
            skillHandlers.Add(2515, new TimeBombSEQ()); //6月26日实装追加部分，lv30习得
            skillHandlers.Add(2507, new PointRain()); //11月25日实装，lv35习得
            skillHandlers.Add(2531, new LoboCall()); //11月25日实装，lv40习得
            skillHandlers.Add(2530, new SejiwuiPoint()); //11月25日实装，lv45习得
            skillHandlers.Add(2538, new ImpactShot()); //6月25日实装，lv47习得
            skillHandlers.Add(2539, new MirageShot()); //6月26日实装，lv50习得
            skillHandlers.Add(2540, new MirageShotSEQ()); //6月26日实装，lv50习得

            //#endregion

            //#region ForceMaster

            skillHandlers.Add(986, new PlusElement()); //11月25日实装
            skillHandlers.Add(3359, new CSPDCommunion());
            skillHandlers.Add(3366, new ForceMaster());
            skillHandlers.Add(3375, new DecreaseWeapon()); //11月25日实装
            skillHandlers.Add(1105, new ForceShield()); //11月25日实装
            skillHandlers.Add(3383, new DecreaseShield()); //11月26日实装
            skillHandlers.Add(3388, new BarrierShield()); //11月26日实装
            skillHandlers.Add(3395, new ForceWave()); //11月26日实装,lv30（未完成实装）
            skillHandlers.Add(3394, new ThunderSpray()); //11月26日实装，lv35
            skillHandlers.Add(3419, new AdobannaSubiritei()); //11月26日实装，lv40
            skillHandlers.Add(3418, new ShockWave()); //11月26日实装,lv45
            skillHandlers.Add(3430, new DispelField()); //16.02.08实装, lv47
            skillHandlers.Add(3428, new DeathTractionGlare()); //2016-01-30实装,lv50
            skillHandlers.Add(3429, new DeathTractionGlareSEQ());

            //#endregion

            //#region Astralist

            skillHandlers.Add(3372, new DelayOut()); //11月26日实装,lv3
            skillHandlers.Add(3351, new MPCommunion());
            skillHandlers.Add(3367, new Astralist()); //11月26日实装,lv10
            skillHandlers.Add(3377, new TranceBody()); //11月26日实装,lv13
            skillHandlers.Add(3378, new Relement()); //11月26日实装,lv20
            skillHandlers.Add(3384, new Amplement()); //11月26日实装,lv23
            skillHandlers.Add(3389, new YugenKeiyaku()); //11月26日实装,lv25
            skillHandlers.Add(3409, new ElementGun()); //11月29日实装,lv30
            skillHandlers.Add(3398, new EarthQuake()); //11月29日实装,lv35
            skillHandlers.Add(3417, new Contract()); //11月29日实装,lv40
            skillHandlers.Add(3433, new ElementMemory()); //03月29日实装,lv47
            skillHandlers.Add(3416, new WindExplosion()); //11月29日实装 lv45
            skillHandlers.Add(3432, new ElementStar()); //确定是JOB50技能，6.11修正

            //#endregion

            //#region Cardinal

            skillHandlers.Add(3373, new Frustrate()); //11月30日实装,lv3
            skillHandlers.Add(3356, new MDEFCommunion()); // lv6
            skillHandlers.Add(3379, new CureTheUndead()); //16.02.02实装,lv13
            skillHandlers.Add(3368, new Cardinal()); //11月30日实装 lv10
            skillHandlers.Add(3380, new CureAll()); //11月30日实装lv20
            skillHandlers.Add(1109, new AutoHeal()); //11月30日实装,lv25
            skillHandlers.Add(3399, new AngelRing()); //11月30日实装,lv30
            skillHandlers.Add(3393, new MysticShine()); //11月30日实装,lv35
            skillHandlers.Add(3415, new Recovery()); //11月30日实装,lv40
            skillHandlers.Add(3414, new DevineBreaker()); //11月30日实装,lv45
            skillHandlers.Add(3436, new Salvation()); // 16.01.08实装,lv47
            skillHandlers.Add(3434, new Gospel()); // 16.01.08实装,lv50

            //#endregion

            //#region SoulTaker

            skillHandlers.Add(3374, new MegaDarkBlaze());
            skillHandlers.Add(3354, new MATKCommunion());
            skillHandlers.Add(3369, new SoulTaker());
            skillHandlers.Add(2544, new SoulHunting()); //2018.1.17实装,lv47
            skillHandlers.Add(2545, new SoulHuntingSEQ()); //2018.1.17实装,lv47后续技能
            skillHandlers.Add(3376, new Transition()); //11月30日实装，lv20
            skillHandlers.Add(1110, new SoulTakerMaster()); //11月30日实装，lv23
            skillHandlers.Add(3397, new DarkChains()); //11月30日实装，lv30
            skillHandlers.Add(3392, new Chasm()); //11月30日实装，lv35
            skillHandlers.Add(3420, new DeathSickle()); //11月30实装,lv40
            skillHandlers.Add(2526, new Fuenriru()); //11月30实装,lv45
            skillHandlers.Add(3431, new Dammnation()); //16.01.08实装,lv50

            //#endregion

            skillHandlers.Add(1606, new Ryuugankakusen());
            skillHandlers.Add(1607, new DragonEyesOfGod());

            //#region Harvest

            skillHandlers.Add(2481, new EquipCompose()); //12月2日实装，lv3（未完成，需要封包）
            skillHandlers.Add(3360,
                new SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._3_0_Class.Harvest_收获者____far.
                    CAPACommunion()); //12月2日实装，lv6
            skillHandlers.Add(3370, new Twine()); //12月2日,lv10（未实装，可能需要新的debuff）
            skillHandlers.Add(2488, new PotentialArmor()); //12月2日，lv13（未完成，需要封包）
            skillHandlers.Add(3381, new TwineSleep()); //12月2日,lv20（未实装，可能需要新的debuff）
            skillHandlers.Add(2505, new EquipComposeCancel()); //12月2日实装，lv23（未完成，需要封包）
            skillHandlers.Add(2497, new Bounce()); //12月2日完成实装,lv25
            skillHandlers.Add(2510, new Winder()); //12月2日,lv30（未完成，需要封包）
            skillHandlers.Add(3400, new CreateNeko()); //12月2日,lv35（未完成，需要召唤物技能）
            skillHandlers.Add(3413, new TwistedPlant()); //未完成,Lv40
            skillHandlers.Add(993, new HarvestMaster()); //未完全完成,Lv45
            skillHandlers.Add(2547, new MistletoeShooting()); //2018.1.18实装,Lv47
            skillHandlers.Add(2548, new MistletoeShootingSEQ()); //2018.1.18实装,Lv47
            skillHandlers.Add(2546, new PlantField()); //2018.1.18实装,Lv50
            //缺少40、45

            //#endregion


            skillHandlers.Add(101, new MaxMPUp());
            skillHandlers.Add(103, new HPRecoverUP());
            skillHandlers.Add(104, new MPRecoverUP());
            skillHandlers.Add(105, new SPRecoverUP());
            skillHandlers.Add(108, new AxeMastery());
            skillHandlers.Add(111, new MaceMastery());
            skillHandlers.Add(121, new TwoMaceMastery());
            skillHandlers.Add(128, new TwoHandGunMastery());
            skillHandlers.Add(200, new SwordHitUP());
            skillHandlers.Add(202, new SpearHitUP());
            skillHandlers.Add(204, new AxeHitUP());
            skillHandlers.Add(206, new ShortSwordHitUP());
            skillHandlers.Add(208, new MaceHitUP());
            skillHandlers.Add(113, new BowMastery());
            skillHandlers.Add(903, new Identify());
            skillHandlers.Add(907, new ThrowHitUP());
            skillHandlers.Add(2021, new Synthese());
            skillHandlers.Add(4026, new A_T_PJoint());
            skillHandlers.Add(978, new AtkUpByPt());
            skillHandlers.Add(959, new RiskAversion());
            skillHandlers.Add(712, new Camp());

            skillHandlers.Add(1602, new EventMoveSkill());

            skillHandlers.Add(10502, new Fish());
            skillHandlers.Add(904, new FoodFighter());
            skillHandlers.Add(2070, new SpeedUpSkill());
            skillHandlers.Add(2078, new MetalRepair());

            //#region System

            skillHandlers.Add(3250, new FGRope());

            //#endregion

            //#region Mob

            skillHandlers.Add(7843, new ElementShield(Elements.Fire, true));
            skillHandlers.Add(7169, new StarLove());
            MobskillHandlers.Add(7866, new SomaMirage());
            skillHandlers.Add(6444, new Curse());
            skillHandlers.Add(7500, new PoisonPerfume());
            skillHandlers.Add(7501, new RockStone());
            skillHandlers.Add(7502, new ParaizBan());
            skillHandlers.Add(7503, new SleepStrike());
            skillHandlers.Add(7504, new SilentGreen());
            skillHandlers.Add(7505, new SlowLogic());
            skillHandlers.Add(7506, new ConfuseStack());
            skillHandlers.Add(7507, new IceFade());
            skillHandlers.Add(7508, new StunAttack());
            skillHandlers.Add(7509, new EnergyAttack());
            skillHandlers.Add(7510, new FireAttack());
            skillHandlers.Add(7511, new WaterAttack());
            skillHandlers.Add(7512, new WindAttack());
            skillHandlers.Add(7513, new EarthAttack());
            skillHandlers.Add(7514, new LightBallad());
            skillHandlers.Add(7515, new DarkBallad());
            skillHandlers.Add(7516, new Blow());
            skillHandlers.Add(7517, new ConfuseBlow());
            skillHandlers.Add(7518, new StunBlow());
            skillHandlers.Add(7519, new MobHealing());
            skillHandlers.Add(7520, new MobHealing1());
            skillHandlers.Add(7521, new MobAshibarai());
            skillHandlers.Add(7522, new Brandish());
            skillHandlers.Add(7523, new Rush());
            skillHandlers.Add(7524, new Iai());
            skillHandlers.Add(7525, new KabutoWari());
            skillHandlers.Add(7526, new MobBokeboke());
            skillHandlers.Add(7527, new SkillDefinations.Monster.ShockWave());
            skillHandlers.Add(7528, new MobStormSword());
            skillHandlers.Add(7529, new Phalanx());
            skillHandlers.Add(7530, new WarCry());
            skillHandlers.Add(7531, new ExciaMation());
            skillHandlers.Add(7532, new IceArrow());
            skillHandlers.Add(7533, new DarkOne());
            skillHandlers.Add(7534, new WaterStorm());
            skillHandlers.Add(7535, new DarkStorm());
            skillHandlers.Add(7536, new WaterGroove());
            skillHandlers.Add(7537, new MobParalyzeblow());
            skillHandlers.Add(7538, new MobFireart());
            skillHandlers.Add(7539, new MobWaterart());
            skillHandlers.Add(7540, new MobEarthart());
            skillHandlers.Add(7541, new MobWindart());
            skillHandlers.Add(7542, new MobLightart());
            skillHandlers.Add(7543, new MobDarkart());
            skillHandlers.Add(7544, new MobTrSilenceAtk());
            skillHandlers.Add(7545, new MobTrPoisonAtk());
            skillHandlers.Add(7546, new MobTrPoisonCircle());
            skillHandlers.Add(7547, new MobTrStuinCircle());
            skillHandlers.Add(7548, new MobTrSleepCircle());
            skillHandlers.Add(7549, new MobTrSilenceCircle());
            skillHandlers.Add(7550, new MagPoison());
            skillHandlers.Add(7551, new MagSleep());
            skillHandlers.Add(7552, new MagSlow());
            skillHandlers.Add(7553, new StoneCircle());
            skillHandlers.Add(7554, new HiPoisonCircie());
            skillHandlers.Add(7555, new IceCircle());
            skillHandlers.Add(7556, new HiPoison());
            skillHandlers.Add(7557, new DeadlyPoison());
            skillHandlers.Add(7558, new MobPerfectcritical());
            skillHandlers.Add(7559, new SumSlaveMob(10010100)); //古代咕咕雞
            skillHandlers.Add(7560, new SumSlaveMob(26000000)); //ブリキングRX１
            skillHandlers.Add(7561, new SumSlaveMob(10080100)); //テンタクル
            skillHandlers.Add(7562, new SumSlaveMob(10040100)); //ワスプ
            skillHandlers.Add(7563, new SumSlaveMob(10030400)); //ポーラーベア
            skillHandlers.Add(7564, new FireHighStorm());
            skillHandlers.Add(7565, new WindHighWave());
            skillHandlers.Add(7566, new WindHighStorm());
            skillHandlers.Add(7567, new FireOne());
            skillHandlers.Add(7568, new FireStorm());
            skillHandlers.Add(7569, new WindStorm());
            skillHandlers.Add(7570, new EarthStorm());
            skillHandlers.Add(7571, new LightOne());
            skillHandlers.Add(7572, new DarkHighOne());
            //skillHandlers.Add(7573, new SkillDefinations.Enchanter.PoisonMash(true));
            MobskillHandlers.Add(7573, new PoisonMash(true));
            skillHandlers.Add(7574, new MobAvoupSelf());
            skillHandlers.Add(7575, new EnergyShield(true));
            skillHandlers.Add(7576, new MagicShield(true));
            skillHandlers.Add(7577, new MobAtkupOne());
            skillHandlers.Add(7578, new MobCharge());
            skillHandlers.Add(7579, new SumSlaveMob(10030903)); //黑熊
            skillHandlers.Add(7580, new SumSlaveMob(26180002)); //皮格夫
            skillHandlers.Add(7581, new SumSlaveMob(26100003)); //木魚
            skillHandlers.Add(7582, new MobTrSleep());
            skillHandlers.Add(7583, new MobTrStun());
            skillHandlers.Add(7584, new MobTrSilence());
            skillHandlers.Add(7585, new MobConfPoisonCircle());
            skillHandlers.Add(7586, new ElementCircle(Elements.Fire, true));
            skillHandlers.Add(7587, new ElementCircle(Elements.Wind, true));
            skillHandlers.Add(7588, new ElementCircle(Elements.Water, true));
            skillHandlers.Add(7589, new ElementCircle(Elements.Earth, true));
            skillHandlers.Add(7590, new ElementCircle(Elements.Holy, true));
            skillHandlers.Add(7591, new ElementCircle(Elements.Dark, true));
            skillHandlers.Add(7592, new SumSlaveMob(26180000)); //得菩提
            skillHandlers.Add(7593, new SumSlaveMob(26100000)); //雷魚
            skillHandlers.Add(7594, new SumSlaveMob(10030900)); //黑熊
            skillHandlers.Add(7595, new SumSlaveMob(10310006)); //艾卡納J牌
            skillHandlers.Add(7596, new SumSlaveMob(10250003)); //得菩提
            skillHandlers.Add(7597, new SumSlaveMob(30490000, 1)); //巨大咕咕銅像
            skillHandlers.Add(7598, new SumSlaveMob(30500000, 1)); //破壞MkII銅像
            skillHandlers.Add(7599, new SumSlaveMob(30510000, 1)); //皇路普銅像
            skillHandlers.Add(7600, new SumSlaveMob(30520000, 1)); //螫針蜂銅像
            skillHandlers.Add(7601, new SumSlaveMob(30530000, 1)); //白熊銅像

            skillHandlers.Add(7605, new SumSlaveMob(30150005, 4)); //雑草
            skillHandlers.Add(7606, new MobMeteo());
            skillHandlers.Add(7607, new MobDoughnutFireWall());
            skillHandlers.Add(7608, new MobReflection());

            skillHandlers.Add(7609, new MobElementLoad(7664)); //燃燒的路
            skillHandlers.Add(7610, new MobElementLoad(7665)); //凍結的路
            skillHandlers.Add(7611, new MobElementLoad(7666)); //螺旋風！
            skillHandlers.Add(7612, new MobElementLoad(7667)); //私家路
            skillHandlers.Add(7613, new MobElementLoad(7668)); //死神
            skillHandlers.Add(7614, new MobElementRandcircle(7669, 5));
            skillHandlers.Add(7615, new MobElementRandcircle(7670, 5));
            skillHandlers.Add(7616, new MobElementRandcircle(7671, 5));
            skillHandlers.Add(7617, new MobElementRandcircle(7672, 5));
            skillHandlers.Add(7618, new MobElementRandcircle(7673, 5));
            skillHandlers.Add(7619, new MobElementRandcircle(7674, 3));
            skillHandlers.Add(7620, new MobElementRandcircle(7675, 3));
            skillHandlers.Add(7621, new MobElementRandcircle(7676, 3));
            skillHandlers.Add(7622, new MobElementRandcircle(7677, 3));
            skillHandlers.Add(7623, new MobElementRandcircle(7678, 3));

            skillHandlers.Add(7648, new MobVitdownOne());
            skillHandlers.Add(7649, new MobCircleAtkup());
            skillHandlers.Add(7650, new GravityFall(true));
            skillHandlers.Add(7651, new AReflection());
            skillHandlers.Add(7652, new DelayCancel());
            skillHandlers.Add(7653, new MobCharge3());
            skillHandlers.Add(7654, new SumMob(30150007));
            skillHandlers.Add(7655, new SumMob(30130003));
            skillHandlers.Add(7656, new SumMob(30130005));
            skillHandlers.Add(7657, new SumMob(30130007));
            skillHandlers.Add(7658, new SumMob(30070052));
            skillHandlers.Add(7659, new SumMob(30070054));
            skillHandlers.Add(7660, new SumMob(30070056));
            skillHandlers.Add(7661, new SumMob(30070058));
            skillHandlers.Add(7662, new SumMob(30070060));
            skillHandlers.Add(7663, new SumMob(30070062));
            skillHandlers.Add(7664, new MobElementLoadSeq(Elements.Fire)); //燃燒的路
            skillHandlers.Add(7665, new MobElementLoadSeq(Elements.Water)); //凍結的路
            skillHandlers.Add(7666, new MobElementLoadSeq(Elements.Wind)); //螺旋風！
            skillHandlers.Add(7667, new MobElementLoadSeq(Elements.Earth)); //私家路
            skillHandlers.Add(7668, new MobElementLoadSeq(Elements.Dark)); //死神

            skillHandlers.Add(7669, new MobElementRandcircleSeq(Elements.Fire));
            skillHandlers.Add(7670, new MobElementRandcircleSeq(Elements.Water));
            skillHandlers.Add(7671, new MobElementRandcircleSeq(Elements.Wind));
            skillHandlers.Add(7672, new MobElementRandcircleSeq(Elements.Earth));
            skillHandlers.Add(7673, new MobElementRandcircleSeq(Elements.Dark));
            skillHandlers.Add(7674, new MobElementRandcircleSeq(Elements.Fire));
            skillHandlers.Add(7675, new MobElementRandcircleSeq(Elements.Water));
            skillHandlers.Add(7676, new MobElementRandcircleSeq(Elements.Wind));
            skillHandlers.Add(7677, new MobElementRandcircleSeq(Elements.Earth));
            skillHandlers.Add(7678, new MobElementRandcircleSeq(Elements.Dark));
            skillHandlers.Add(7679, new FireArrow());
            skillHandlers.Add(7680, new WaterArrow());
            skillHandlers.Add(7681, new EarthArrow());
            skillHandlers.Add(7682, new WindArrow());
            skillHandlers.Add(7683, new LightArrow());
            skillHandlers.Add(7684, new DarkArrow());
            skillHandlers.Add(7685, new MobConArrow());
            skillHandlers.Add(7686, new MobChargeArrow());
            skillHandlers.Add(7687, new SumSlaveMob(26050003)); //ホウオウ
            skillHandlers.Add(7688, new MobTrDrop());
            skillHandlers.Add(7689, new MobConfCircle());
            skillHandlers.Add(7690, new SumMob(90010000));
            skillHandlers.Add(7691, new MobMedic());
            skillHandlers.Add(7692, new MobWindHighStorm2());
            skillHandlers.Add(7693, new MobElementLoad(7694));
            skillHandlers.Add(7694, new MobWindHighStorm2());
            skillHandlers.Add(7695, new MobElementRandcircle(7696, 5));
            skillHandlers.Add(7696, new MobWindRandcircleSeq2());
            skillHandlers.Add(7697, new MobElementRandcircle(7698, 5));
            skillHandlers.Add(7698, new MobWindCrosscircleSeq2());

            skillHandlers.Add(7706, new SumSlaveMob(10136901)); //魔狼
            skillHandlers.Add(7707, new SolidAura());
            //skillHandlers.Add(7709, new SkillDefinations.Sorcerer.Kyrie(SagaMap.Skill.SkillDefinations.Sorcerer.Kyrie.KyrieUser.Boss)); 
            skillHandlers.Add(7709, new MobComaStun());
            skillHandlers.Add(7710, new SumMob(90010000));
            skillHandlers.Add(7711, new MobSelfDarkHighStorm());
            skillHandlers.Add(4951, new MobSelfDarkHighStorm());
            skillHandlers.Add(7712,
                new SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._2_1_Class.Cabalist_秘术使____lock.
                    DarkStorm(true));
            skillHandlers.Add(7713, new MobSelfMagStun());
            skillHandlers.Add(7714, new TrDrop2());
            skillHandlers.Add(7715, new MobHpPerDown());
            skillHandlers.Add(7716, new MobDropOut());
            skillHandlers.Add(7717, new SumSlaveMob(26180000)); //玉桂巴巴

            skillHandlers.Add(7719, new MobTalkSnmnpapa());
            skillHandlers.Add(7720, new SumSlaveMob(10120100)); //クリムゾンバウ
            skillHandlers.Add(7721, new SumSlaveMob(10210002)); //タランチュラ
            skillHandlers.Add(7722, new SumSlaveMob(10431000)); //キメラ(灰)
            skillHandlers.Add(7723, new SumSlaveMob(10680900)); //ゴースト(黒)
            skillHandlers.Add(7724, new SumSlaveMob(10251000)); //デカデス
            skillHandlers.Add(7725, new SumSlaveMob(10000006)); //デカプルル
            skillHandlers.Add(7726, new SumSlaveMob(10001005)); //メタリカ
            skillHandlers.Add(7727, new SumSlaveMob(10211400)); //夜叉蜘蛛
            skillHandlers.Add(7728, new ThunderBall(true));
            skillHandlers.Add(7729, new EarthBlast(true));
            skillHandlers.Add(7730, new FireBolt(true));
            skillHandlers.Add(7731, new StoneSkin(true));
            skillHandlers.Add(7732, new MobCancelChgstateAll());
            skillHandlers.Add(7733, new SumSlaveMob(10580500, 4)); //曼陀蘿芥末
            skillHandlers.Add(7734, new MobCircleSelfAtk());
            skillHandlers.Add(7735, new SumSlaveMob(10431400, 1)); //アルビノキメラ
            skillHandlers.Add(7736, new SumSlaveMob(10030901, 1)); //最強の魔獣
            skillHandlers.Add(7737, new MobCircleSelfAtk());
            skillHandlers.Add(7738, new EnergyShock(true));
            skillHandlers.Add(7739, new Concentricity(true));
            skillHandlers.Add(7740, new MobComboConShot());
            skillHandlers.Add(7741, new MobConShot());
            skillHandlers.Add(7742, new MobComboConAtk());
            skillHandlers.Add(7743, new MobConAtk());
            skillHandlers.Add(7744, new SumSlaveMob(10580500)); //曼陀蘿芥末
            //skillHandlers.Add(7745, new SkillDefinations.Enchanter.AcidMist());
            //skillHandlers.Add(7745, new SkillDefinations.Monster.AcidMistMobUse());
            MobskillHandlers.Add(7745, new AcidMistMobUse());
            //skillHandlers.Add(7746, new SkillDefinations.Monster.MobEarthDurable());
            MobskillHandlers.Add(7746, new MobEarthDurable());
            skillHandlers.Add(7747, new LifeSteal(true));
            skillHandlers.Add(7748, new MobChargeCircle());
            skillHandlers.Add(7749, new PetPlantPoison(true));
            skillHandlers.Add(7750, new MobStrVitAgiDownOne());
            skillHandlers.Add(7751, new MobAtkupSelf());
            skillHandlers.Add(7752, new MobDefUpSelf(true));
            skillHandlers.Add(7753, new SolidAura(SolidAura.KyrieUser.Mob));
            skillHandlers.Add(7754, new MobHolyfeather());
            skillHandlers.Add(7755, new MobSalvoFire());
            skillHandlers.Add(7756, new MobAmobm());
            skillHandlers.Add(7757, new SumMob(90010001));
            skillHandlers.Add(7758, new EnergyStorm(true));
            skillHandlers.Add(7759, new EnergyBlast(true));
            skillHandlers.Add(7760, new SumSlaveMob(10990000)); //バルル
            skillHandlers.Add(7761, new SumSlaveMob(10960000)); //野生德拉古
            skillHandlers.Add(7764, new MobIsSeN()); //怪物用一闪
            skillHandlers.Add(7766, new SumSlaveMob(19070500)); //ＤＥＭ－スナイパー

            skillHandlers.Add(7798, new Caputrue());
            skillHandlers.Add(7878, new NothingNess());

            skillHandlers.Add(7805, new SumSlaveMob(14160500)); //ポイズンジェル
            skillHandlers.Add(7806, new SumSlaveMob(14160000)); //バルーンジェル
            skillHandlers.Add(7807, new SumSlaveMob(10060600)); //エンジェルフィッシュ
            skillHandlers.Add(7808, new SumSlaveMob(10060200)); //スイムフィッシュ


            skillHandlers.Add(7810, new Abusoryutoteritori());
            skillHandlers.Add(7811, new SumSlaveMob(14110003, 8)); //スレイブドラゴン召喚

            MobskillHandlers.Add(7813, new CanisterShot(true));
            MobskillHandlers.Add(7815, new HellFire(true)); //DEM龙&恶魔龙用地狱之火
            skillHandlers.Add(7818, new SumSlaveMob(14320203)); //ＤＥＭ－クリンゲ召喚,现在DEM龙召唤49级的Link
            skillHandlers.Add(7819, new SumSlaveMob(14330500)); //ＤＥＭ－ゲヴェーア召喚

            skillHandlers.Add(7820, new SoulAttack());
            skillHandlers.Add(7821, new VolcanoHall());
            //skillHandlers.Add(7822, new SkillDefinations.BountyHunter.IsSeN(true));
            skillHandlers.Add(7825, new MobDevineBarrier()); //怪物用光界
            skillHandlers.Add(7830, new SumSlaveMob(14560900)); //アルルーン召喚
            skillHandlers.Add(7831, new Animadorein()); //怪物用核弹
            skillHandlers.Add(7832, new MobNoHeal()); //再生技能无效化
            skillHandlers.Add(7849, new MobElementBless(Elements.Fire)); //怪物用火祝
            skillHandlers.Add(7850, new MobElementBless(Elements.Wind)); //怪物用风祝
            skillHandlers.Add(7851, new MobElementBless(Elements.Water)); //怪物用水祝
            skillHandlers.Add(7852, new MobElementBless(Elements.Earth)); //怪物用地祝
            skillHandlers.Add(7853, new MobElementBless(Elements.Holy)); //怪物用光祝
            skillHandlers.Add(7854, new MobElementBless(Elements.Dark)); //怪物用暗祝
            skillHandlers.Add(7858, new ElementNull()); //属性攻击无效化
            skillHandlers.Add(7867, new SumSlaveMob(10270500)); //黄色天使之羽
            skillHandlers.Add(7874, new Roar()); //咆哮
            skillHandlers.Add(7875, new HumanSeeleGuerrilla()); //灵魂审判(5*5沉默+凭依解除)
            MobskillHandlers.Add(7859, new AlterFate()); //オールターフェイト 属性变化
            skillHandlers.Add(7883, new SumSlaveMob(14570002)); //圆桌骑士
            skillHandlers.Add(7884, new SumSlaveMob(14310105)); //自动医疗机召唤
            skillHandlers.Add(7885, new SumSlaveMob(14550901)); //末路幽灵
            skillHandlers.Add(7886, new SumSlaveMob(14170501)); //花纹蛇
            skillHandlers.Add(7887, new SumSlaveMob(14170502)); //花纹蛇
            skillHandlers.Add(7888, new SumSlaveMob(10140402)); //维德佛尔尼尔
            skillHandlers.Add(7889, new SumSlaveMob(10140403)); //维德佛尔尼尔
            MobskillHandlers.Add(7890, new Mutation()); //ミューテイション 属性变化
            skillHandlers.Add(7897, new SumSlaveMob(10960005)); //步行龙僵尸
            skillHandlers.Add(7898, new SumSlaveMob(10251000)); //梦魇
            skillHandlers.Add(7899, new SumSlaveMob(10470006)); //巴力西卜
            skillHandlers.Add(7900, new SumSlaveMob(14280001)); //玛尔斯
            skillHandlers.Add(7901, new SumSlaveMob(10221500)); //粉色鬼火
            skillHandlers.Add(7902, new SumSlaveMob(14171400)); //海蛇（白）
            skillHandlers.Add(7903, new SumSlaveMob(26130013)); //冰元素W
            skillHandlers.Add(7904, new SumSlaveMob(26130106)); //暗元素B
            skillHandlers.Add(7920, new SumSlaveMob(15620001)); //ポロマタンキクル(红跳跳)
            skillHandlers.Add(7987, new ZillionBladeMob()); //无尽之刃Mob
            skillHandlers.Add(8077, new BooooomMob()); //Mob自爆
            skillHandlers.Add(20151, new ConvolutionMob()); //大车轮Mob
            skillHandlers.Add(20155, new Corona()); //コロナMob
            skillHandlers.Add(25200, new VerbalMob()); //かまいたちMob
            skillHandlers.Add(8500, new MobHpHeal());
            skillHandlers.Add(8501, new MobBerserk());

            skillHandlers.Add(9000, new CswarSleep(true));
            skillHandlers.Add(9001, new CswarSleep(true));
            skillHandlers.Add(9002, new SumMob(26160003)); //サークリス

            skillHandlers.Add(9004, new AreaHeal(true));
            skillHandlers.Add(9106, new EventSelfDarkStorm(true));

            skillHandlers.Add(8444, new DeathSickle(true)); //死鎌乱舞 By Kk
            skillHandlers.Add(8476, new Bounce(true)); //セルフミラー Bt Kk

            //#endregion

            //#region Marionette

            skillHandlers.Add(5008, new HPRecovery());
            skillHandlers.Add(5009, new SPRecovery());
            skillHandlers.Add(5010, new MPRecovery());

            skillHandlers.Add(5507, new MExclamation());

            skillHandlers.Add(5513, new MBokeboke());

            skillHandlers.Add(5515, new MMirror());
            skillHandlers.Add(5516, new MMirrorSkill());

            skillHandlers.Add(5522, new MDarkCrosscircle());
            skillHandlers.Add(5523, new MDarkCrosscircleSeq());
            skillHandlers.Add(5524, new MCharge3()); //该技能不属于木偶师

            //#endregion

            //#region Event

            skillHandlers.Add(1500, new WeaCreUp());
            skillHandlers.Add(1501, new HitUpRateDown());

            skillHandlers.Add(1603, new Ryuugankaihou());
            skillHandlers.Add(1604, new RyuugankaihouShin());
            skillHandlers.Add(1605, new MagicSP());
            skillHandlers.Add(2072, new MoneyTime());
            skillHandlers.Add(2265, new NormalAttack());
            skillHandlers.Add(2457, new SymbolRepair());

            skillHandlers.Add(3067, new PowerUP());
            skillHandlers.Add(3069, new HPUP());
            skillHandlers.Add(3071, new SpeedUP());
            skillHandlers.Add(3145, new MagicUP());
            skillHandlers.Add(3269, new ChgTrance());
            skillHandlers.Add(5509, new Colder());
            skillHandlers.Add(5510, new ConflictKick());

            skillHandlers.Add(6415, new MoveUp2());
            skillHandlers.Add(6428, new MoveUp3());
            skillHandlers.Add(6451, new MoveUp5()); //紫火
            skillHandlers.Add(9100, new MiniMum());
            skillHandlers.Add(9101, new MaxiMum());
            skillHandlers.Add(9102, new EventCampfire());
            skillHandlers.Add(9103,
                new SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._2_1_Class.Sorcerer_魔导师____wiz.
                    Invisible());
            skillHandlers.Add(9105, new EventCampfire());
            skillHandlers.Add(9108, new Dango());
            skillHandlers.Add(9109, new EventCampfire());
            skillHandlers.Add(9114,
                new SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._2_1_Class.Sorcerer_魔导师____wiz.
                    Invisible());
            skillHandlers.Add(9117, new ExpUp());
            skillHandlers.Add(9126, new EventCampfire());
            skillHandlers.Add(9127, new EventCampfire());
            skillHandlers.Add(9128,
                new SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._2_1_Class.Sorcerer_魔导师____wiz.
                    Invisible());
            skillHandlers.Add(9129, new SumMobCastSkill(19010001, 9130));
            skillHandlers.Add(9130, new HpRecoveryMax());
            skillHandlers.Add(9131, new SumMobCastSkill(19010002, 9132));
            skillHandlers.Add(9132, new HpRecoveryMax());
            skillHandlers.Add(9133, new SumMobCastSkill(19010003, 9134));
            skillHandlers.Add(9134, new HpRecoveryMax());

            skillHandlers.Add(9140, new SumMobCastSkill(19010008, 9143, 50, 9146, 50));
            skillHandlers.Add(9141, new SumMobCastSkill(19010009, 9144, 50, 9147, 50));
            skillHandlers.Add(9142, new SumMobCastSkill(19010010, 9145, 50, 9148, 50));
            skillHandlers.Add(9143, new DefMdefUp());
            skillHandlers.Add(9144, new DefMdefUp());
            skillHandlers.Add(9145, new DefMdefUp());
            skillHandlers.Add(9146, new DefMdefUp());
            skillHandlers.Add(9147, new DefMdefUp());
            skillHandlers.Add(9148, new DefMdefUp());


            skillHandlers.Add(9139, new EventCampfire());

            skillHandlers.Add(9151, new ILoveYou());
            skillHandlers.Add(9152, new HpRecoveryMax());
            skillHandlers.Add(9153, new HpRecoveryMax());
            skillHandlers.Add(9154, new HpRecoveryMax());
            skillHandlers.Add(9155, new Healing());
            skillHandlers.Add(9157, new EventCampfire());
            skillHandlers.Add(9162, new Healing());
            skillHandlers.Add(9163, new EventCampfire());
            skillHandlers.Add(9174, new EventCampfire());
            skillHandlers.Add(9178, new EventCampfire());
            skillHandlers.Add(9182, new EventCampfire());
            skillHandlers.Add(9185, new EventCampfire());
            skillHandlers.Add(9190, new SumMob(30740000));
            skillHandlers.Add(9191, new SumMobCastSkill(30750000, 9192, 90, 9193, 10));
            skillHandlers.Add(9192, new WeepingWillow1());
            skillHandlers.Add(9193, new WeepingWillow2());

            skillHandlers.Add(9197,
                new SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._2_1_Class.Sorcerer_魔导师____wiz.
                    Invisible());
            skillHandlers.Add(5520, new DarkSun());

            skillHandlers.Add(9208, new SumMobCastSkill(19010028, 9209));
            skillHandlers.Add(9209, new HpRecoveryMax());

            skillHandlers.Add(9219, new MoveUp4());
            skillHandlers.Add(9220, new RiceSeed());
            skillHandlers.Add(9221, new PanTick());

            skillHandlers.Add(9223, new Gravity());
            skillHandlers.Add(9224, new Kyrie());
            skillHandlers.Add(9225, new SkillDefinations.Event.AReflection());
            skillHandlers.Add(9226, new STR_VIT_AGI_UP());
            skillHandlers.Add(9227, new MAG_INT_DEX_UP());
            skillHandlers.Add(9228, new SkillDefinations.Event.AreaHeal());

            skillHandlers.Add(10500, new HerosProtection());

            //#endregion

            //#region Swordman

            skillHandlers.Add(2005, new SwordCancel());
            skillHandlers.Add(2100, new Parry());
            skillHandlers.Add(2101, new Counter());
            skillHandlers.Add(2102, new Feint());
            skillHandlers.Add(2107, new Provocation());
            skillHandlers.Add(2111, new BanishBlow());
            skillHandlers.Add(2114, new SlowBlow());
            skillHandlers.Add(2120, new Charge());
            skillHandlers.Add(2117, new CutDown());
            skillHandlers.Add(2115,
                new SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._1_0_Class.Swordman_剑士_.Iai());
            skillHandlers.Add(2201, new Iai2());
            skillHandlers.Add(2202, new Iai3());

            //#endregion

            //#region BladeMaster

            skillHandlers.Add(2134, new aEarthAngry());
            skillHandlers.Add(2231, new aWoodHack());
            skillHandlers.Add(2232, new aFalconLongSword());
            skillHandlers.Add(2233, new aMistBehead());
            skillHandlers.Add(2234, new aAnimalCrushing());
            skillHandlers.Add(2116, new StormSword());
            skillHandlers.Add(2235, new aHundredSpriteCry());
            skillHandlers.Add(700, new P_BERSERK());
            skillHandlers.Add(2066, new PetMeditatioon());
            skillHandlers.Add(2109, new PetWarCry());
            skillHandlers.Add(2236, new AtkFly());
            skillHandlers.Add(2237, new SwordEaseSp());
            skillHandlers.Add(2238, new EaseCt());
            skillHandlers.Add(2379, new DoubleCutDown());
            skillHandlers.Add(2380, new DoubleCutDownSeq());

            //#endregion

            //#region BountyHunter

            skillHandlers.Add(2272, new ArmSlash());
            skillHandlers.Add(2271, new BodySlash());
            skillHandlers.Add(2273, new ChestSlash());
            skillHandlers.Add(2122, new MiNeUChi());
            skillHandlers.Add(2274, new ShieldSlash());
            skillHandlers.Add(2269, new DefUpAvoDown());
            skillHandlers.Add(2268, new AtkUpHitDown());
            skillHandlers.Add(2352, new BeatUp());
            skillHandlers.Add(2401, new MuSoU());
            skillHandlers.Add(2402, new MuSoUSEQ());
            skillHandlers.Add(2396, new SwordAssail());
            skillHandlers.Add(2397, new SwordAssailSEQ());
            skillHandlers.Add(2136, new AShiBaRaI());
            skillHandlers.Add(956, new ConSword());
            //skillHandlers.Add(400, new SkillDefinations.Global.SomeKindDamUp("HumDamUp", SagaDB.Mob.MobType.HUMAN, SagaDB.Mob.MobType.HUMAN_BOSS, SagaDB.Mob.MobType.HUMAN_BOSS_SKILL, SagaDB.Mob.MobType.HUMAN_RIDE, SagaDB.Mob.MobType.HUMAN_SKILL));
            skillHandlers.Add(2275, new PartsSlash());
            skillHandlers.Add(2355, new StrikeBlow());
            skillHandlers.Add(2353, new SolidBody());
            skillHandlers.Add(2400, new IsSeN());
            skillHandlers.Add(2179, new EdgedSlash());
            skillHandlers.Add(2270, new ComboIai());

            //#endregion

            //#region Gladiator

            skillHandlers.Add(982, new SwordMaster());
            skillHandlers.Add(3350, new HPCommunion());
            skillHandlers.Add(3362, new Gladiator()); //11月23日增加，LV10习得
            skillHandlers.Add(1100, new Volunteers()); //11月23日增加，LV13习得
            skillHandlers.Add(2484, new Ekuviri());
            skillHandlers.Add(2498, new DevilStance());
            skillHandlers.Add(2503, new Convolution());
            skillHandlers.Add(3391, new Pressure()); //11月23日增加，LV30习得
            skillHandlers.Add(1113, new Sewaibu()); //11月23日增加，LV35习得
            skillHandlers.Add(2528, new Disarm()); //11月23日增加，LV40习得
            skillHandlers.Add(2527, new SpeedHit()); //11月23日增加，LV45习得
            skillHandlers.Add(1117, new KenSei()); //16.05.03增加,LV47
            skillHandlers.Add(2534, new ZillionBlade()); //16.05.03增加,LV50

            //#endregion

            //#region Scout

            skillHandlers.Add(2001,
                new SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._1_0_Class.Scout_盗贼_.CriUp());
            skillHandlers.Add(2008, new ShortSwordCancel());
            skillHandlers.Add(2139, new ConThrust());
            skillHandlers.Add(2143, new SummerSaltKick());
            skillHandlers.Add(2133, new WalkAround());
            skillHandlers.Add(110, new ShortSwordMastery());
            skillHandlers.Add(2004, new AvoidMeleeUp());
            skillHandlers.Add(112, new ThrowMastery());
            skillHandlers.Add(2127, new ConThrow());
            skillHandlers.Add(908, new ThrowRangeUp());

            //#endregion

            //#region Assassin

            skillHandlers.Add(2045, new PoisonReate());
            skillHandlers.Add(2046, new PosionReate2());
            skillHandlers.Add(2047, new PoisonReate3());
            skillHandlers.Add(2069, new Concentricity());
            skillHandlers.Add(947, new ConClaw());
            skillHandlers.Add(2062, new WillPower());
            skillHandlers.Add(910, new PoisonRegiUp());
            skillHandlers.Add(2250, new UTuSeMi());
            skillHandlers.Add(2044, new AppliePoison());
            skillHandlers.Add(2142, new ScatterPoison());
            skillHandlers.Add(920, new PoisonRateUp());
            skillHandlers.Add(2251, new EventSumNinJa());

            //#endregion

            //#region Command

            skillHandlers.Add(127, new HandGunDamUp());
            skillHandlers.Add(2137, new Tackle());
            skillHandlers.Add(125, new MartialArtDamUp());
            skillHandlers.Add(2141,
                new SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._2_2_Class.Command_特工____sco.Rush());
            skillHandlers.Add(2282, new FlashHandGrenade());
            skillHandlers.Add(2362, new SetBomb());
            skillHandlers.Add(2378, new SetBomb2());
            skillHandlers.Add(2359, new UpperCut());
            skillHandlers.Add(2413, new WildDance());
            skillHandlers.Add(2414, new WildDance2());
            skillHandlers.Add(3095, new LandTrap());
            skillHandlers.Add(2280, new FullNelson());
            skillHandlers.Add(2281, new Combination());
            skillHandlers.Add(3131, new ChokingGas());
            skillHandlers.Add(2283, new Sliding());
            skillHandlers.Add(2284, new ClayMore());
            skillHandlers.Add(2361, new SealHMSp());
            skillHandlers.Add(2409, new RushBom());
            skillHandlers.Add(2410, new RushBomSeq());
            skillHandlers.Add(2411, new RushBomSeq2());
            skillHandlers.Add(2408, new SumCommand());
            //skillHandlers.Add(401, new SkillDefinations.Command.HumHitUp());

            //#endregion

            //#region Wizard

            skillHandlers.Add(3001, new EnergyOne());
            skillHandlers.Add(3002, new EnergyGroove());
            skillHandlers.Add(3005, new Decoy());
            skillHandlers.Add(3113, new EnergyShield());
            skillHandlers.Add(3279, new EnergyShield());
            skillHandlers.Add(3114, new MagicShield());
            skillHandlers.Add(3280, new MagicShield());
            skillHandlers.Add(3123, new EnergyShock());
            skillHandlers.Add(3125, new DancingSword());
            skillHandlers.Add(3124, new EnergySpear());
            skillHandlers.Add(3127, new EnergyBlast());
            skillHandlers.Add(3135,
                new SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._1_0_Class.Wizard_魔法师_.MagPoison());
            skillHandlers.Add(3136, new MagStone());
            skillHandlers.Add(3139, new MagSilence());
            skillHandlers.Add(801, new MaGaNiInfo());
            skillHandlers.Add(3101, new Heating());
            skillHandlers.Add(3281, new MobEnergyShock());
            skillHandlers.Add(3100, new IntenseHeatSheld());
            skillHandlers.Add(3033, new Aqualung());
            skillHandlers.Add(3003, new EnergyWall());
            skillHandlers.Add(503, new ManDamUp());
            skillHandlers.Add(504, new ManHitUp());
            skillHandlers.Add(505, new ManAvoUp());
            //skillHandlers.Add(400, new SkillDefinations.Wizard.WandMaster());
            //Wrong SkillID !! KK 2018/4/9
            skillHandlers.Add(401, new EnergyExcess());

            //#endregion

            //#region Sorcerer

            skillHandlers.Add(3126, new LivingSword());
            skillHandlers.Add(3300, new DevineBarrier());
            skillHandlers.Add(3253, new Teleport());
            skillHandlers.Add(3097,
                new SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._2_1_Class.Sorcerer_魔导师____wiz.
                    Invisible());
            skillHandlers.Add(3275, new EnergyBarrier());
            skillHandlers.Add(3276, new MagicBarrier());
            skillHandlers.Add(3256, new Clutter());
            skillHandlers.Add(3255, new StrVitAgiDownOne());
            skillHandlers.Add(3298, new EnergyBarn());
            skillHandlers.Add(3299, new EnergyBarnSEQ());
            skillHandlers.Add(3158, new Desist());
            skillHandlers.Add(3254, new SolidAura());
            skillHandlers.Add(2208, new MaganiAnalysis());
            skillHandlers.Add(3171, new MobInvisibleBreak());
            skillHandlers.Add(3098, new MobInvisibleBreak());
            skillHandlers.Add(3004, new WallSweep());
            skillHandlers.Add(3094, new HexaGram());
            skillHandlers.Add(2252, new OverWork());

            //#endregion

            //#region Vates

            //skillHandlers.Add(3111, new SkillDefinations.Vates.HolyBlessing());
            skillHandlers.Add(3111, new ElementBless(Elements.Holy));
            //skillHandlers.Add(3080, new SkillDefinations.Vates.HolyHealing());
            skillHandlers.Add(3080, new ElementCircle(Elements.Holy)); //光之领域
            skillHandlers.Add(3054, new Healing());
            skillHandlers.Add(3165, new SmallHealing());
            skillHandlers.Add(3055, new Resurrection());
            skillHandlers.Add(3073,
                new SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._1_0_Class.Votes_祭司_.LightOne());
            //skillHandlers.Add(3075, new SkillDefinations.Vates.HolyWeapon());
            //skillHandlers.Add(3076, new SkillDefinations.Vates.HolyShield());
            skillHandlers.Add(3075, new ElementWeapon(Elements.Holy));
            skillHandlers.Add(3076, new ElementShield(Elements.Holy));
            skillHandlers.Add(3082, new HolyGroove());
            skillHandlers.Add(3066, new CureStatus("Sleep"));
            skillHandlers.Add(3060, new CureStatus("Poison"));
            skillHandlers.Add(3058, new CureStatus("Stun"));
            skillHandlers.Add(3062, new CureStatus("Silence"));
            skillHandlers.Add(3150, new CureStatus("Stone"));
            skillHandlers.Add(3064, new CureStatus("Confuse"));
            skillHandlers.Add(3152, new CureStatus("鈍足"));
            skillHandlers.Add(3154, new CureStatus("Frosen"));
            skillHandlers.Add(803, new UndeadInfo());
            skillHandlers.Add(3065, new StatusRegi("Sleep"));
            skillHandlers.Add(3059, new StatusRegi("Poison"));
            skillHandlers.Add(3057, new StatusRegi("Stun"));
            skillHandlers.Add(3061, new StatusRegi("Silence"));
            skillHandlers.Add(3149, new StatusRegi("Stone"));
            skillHandlers.Add(3063, new StatusRegi("Confuse"));
            skillHandlers.Add(3151, new StatusRegi("鈍足"));
            skillHandlers.Add(3153, new StatusRegi("Frosen"));
            skillHandlers.Add(3078, new TurnUndead());

            //#endregion

            //#region Shaman

            skillHandlers.Add(3006, new FireBolt());
            //skillHandlers.Add(3007, new SkillDefinations.Shaman.FireShield());
            //skillHandlers.Add(3008, new SkillDefinations.Shaman.FireWeapon());
            skillHandlers.Add(3007, new ElementShield(Elements.Fire));
            skillHandlers.Add(3008, new ElementWeapon(Elements.Fire));
            skillHandlers.Add(3009, new FireBlast());
            skillHandlers.Add(3029,
                new SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._1_0_Class.Shaman_精灵使_.IceArrow());
            //skillHandlers.Add(3030, new SkillDefinations.Shaman.WaterShield());
            //skillHandlers.Add(3031, new SkillDefinations.Shaman.WaterWeapon());
            skillHandlers.Add(3030, new ElementShield(Elements.Water));
            skillHandlers.Add(3031, new ElementWeapon(Elements.Water));
            skillHandlers.Add(3032, new ColdBlast());
            skillHandlers.Add(3041, new LandKlug());
            //skillHandlers.Add(3042, new SkillDefinations.Shaman.EarthShield());
            //skillHandlers.Add(3043, new SkillDefinations.Shaman.EarthWeapon());
            skillHandlers.Add(3042, new ElementShield(Elements.Earth));
            skillHandlers.Add(3043, new ElementWeapon(Elements.Earth));
            skillHandlers.Add(3044, new EarthBlast());
            skillHandlers.Add(3017, new ThunderBall());
            //skillHandlers.Add(3018, new SkillDefinations.Shaman.WindShield());
            //skillHandlers.Add(3019, new SkillDefinations.Shaman.WindWeapon());
            skillHandlers.Add(3018, new ElementShield(Elements.Wind));
            skillHandlers.Add(3019, new ElementWeapon(Elements.Wind));
            skillHandlers.Add(3020, new LightningBlast());
            skillHandlers.Add(3011, new FireWall());
            skillHandlers.Add(3047, new StoneWall());
            skillHandlers.Add(802, new ElementIInfo());
            skillHandlers.Add(3000, new SenseElement());
            skillHandlers.Add(3162, new PrayerToTheElf());

            //#endregion

            //#region Elementaler

            skillHandlers.Add(3016, new FireGroove());
            skillHandlers.Add(3028, new WindGroove());
            skillHandlers.Add(3040,
                new SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._2_1_Class.Elementaler_元素使____sha.
                    WaterGroove());
            skillHandlers.Add(3053, new EarthGroove());
            skillHandlers.Add(3265, new LavaFlow());
            skillHandlers.Add(3036, new ElementStorm(Elements.Water));
            skillHandlers.Add(3025, new ElementStorm(Elements.Wind));
            skillHandlers.Add(3049, new ElementStorm(Elements.Earth));
            skillHandlers.Add(3013, new ElementStorm(Elements.Fire));
            skillHandlers.Add(3261, new ChainLightning());
            skillHandlers.Add(3260, new CatlingGun());
            skillHandlers.Add(3262, new WaterNum());
            skillHandlers.Add(3263, new EarthNum());
            skillHandlers.Add(3264, new IcicleTempest());
            skillHandlers.Add(3311, new SpellCancel());
            skillHandlers.Add(3159, new Zen());
            skillHandlers.Add(2209, new ElementAnalysis());
            skillHandlers.Add(3301, new AquaWave());
            skillHandlers.Add(3306, new CycloneGrooveEarth());
            skillHandlers.Add(939, new ElementLimitUp(Elements.Earth)); //大地守護
            skillHandlers.Add(936, new ElementLimitUp(Elements.Fire)); //火焰守護
            skillHandlers.Add(937, new ElementLimitUp(Elements.Water)); //水靈守護
            skillHandlers.Add(938, new ElementLimitUp(Elements.Wind)); //神風守護

            //#endregion

            //#region Enchanter

            skillHandlers.Add(3318, new GravityFall());
            skillHandlers.Add(3319, new ElementalWrath());
            skillHandlers.Add(3294, new SpeedEnchant());
            skillHandlers.Add(3295, new AtkUp_DefUp_SpdDown_AvoDown());
            skillHandlers.Add(2305, new SoulOfEarth());
            skillHandlers.Add(2304, new SoulOfWind());
            skillHandlers.Add(2303, new SoulOfWater());
            skillHandlers.Add(2302, new SoulOfFire());
            skillHandlers.Add(3046, new PoisonMash());
            skillHandlers.Add(3155, new Bind());
            skillHandlers.Add(3052, new AcidMist());
            skillHandlers.Add(3010, new FirePillar());
            skillHandlers.Add(3048, new ElementCircle(Elements.Earth)); //大地結界
            skillHandlers.Add(3012, new ElementCircle(Elements.Fire)); //火焰結界
            skillHandlers.Add(3035, new ElementCircle(Elements.Water)); //寒冰結界
            skillHandlers.Add(3024, new ElementCircle(Elements.Wind)); //神風結界
            skillHandlers.Add(3296, new ElementBall());
            skillHandlers.Add(3317, new EnchantWeapon());
            skillHandlers.Add(3110, new ElementBless(Elements.Earth)); //大地祝福
            skillHandlers.Add(3107, new ElementBless(Elements.Fire)); //火焰祝福
            skillHandlers.Add(3109, new ElementBless(Elements.Water)); //寒氣祝福
            skillHandlers.Add(3108, new ElementBless(Elements.Wind)); //神風祝福

            //#endregion

            //#region Acher

            skillHandlers.Add(2050, new BowCancel());
            skillHandlers.Add(2128, new ConArrow());

            //#endregion

            //#region Warlock

            skillHandlers.Add(3083, new BlackWidow());
            skillHandlers.Add(3085,
                new SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._1_0_Class.Warlock_暗术使_.
                    ShadowBlast());
            //skillHandlers.Add(3088, new SkillDefinations.Warlock.DarkWeapon());
            skillHandlers.Add(3088, new ElementWeapon(Elements.Dark));
            skillHandlers.Add(3093, new DarkGroove());
            //skillHandlers.Add(3133, new SkillDefinations.Warlock.DarkShield());
            skillHandlers.Add(3133, new ElementShield(Elements.Dark));
            skillHandlers.Add(3134, new ChaosWidow());
            skillHandlers.Add(3140,
                new SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._1_0_Class.Warlock_暗术使_.MagSlow());
            skillHandlers.Add(3141, new MagConfuse());
            skillHandlers.Add(3142, new MagFreeze());
            skillHandlers.Add(3143, new MagStun());
            skillHandlers.Add(3112, new ElementBless(Elements.Dark)); //黑暗祝福
            skillHandlers.Add(941, new ElementLimitUp(Elements.Dark)); //黑暗守護
            skillHandlers.Add(8456, new SuckBlood());

            //#endregion

            //#region Cabalist

            skillHandlers.Add(2229, new GrimReaper());
            skillHandlers.Add(2230, new SoulSteal());
            skillHandlers.Add(3092, new ElementCircle(Elements.Dark)); //暗黑結界（ダークパワーサークル）
            skillHandlers.Add(3087, new Fanaticism());
            skillHandlers.Add(3089,
                new SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._2_1_Class.Cabalist_秘术使____lock.
                    DarkStorm());
            skillHandlers.Add(3274, new MoveDownCircle());
            skillHandlers.Add(3021, new SleepCloud());
            skillHandlers.Add(949, new AllRateUp());
            skillHandlers.Add(3167, new DarkChopMark());
            skillHandlers.Add(3166, new ChopMark());
            skillHandlers.Add(3270, new HitAndAway());
            skillHandlers.Add(3273, new StoneSkin());
            skillHandlers.Add(3272, new RandMark());
            skillHandlers.Add(3309, new ChgstRand());
            skillHandlers.Add(3310, new EventSelfDarkStorm());
            skillHandlers.Add(3346, new Sacrifice());
            skillHandlers.Add(10000, new EffDarkChopMark());

            //#endregion

            //#region Fencer

            skillHandlers.Add(2007, new SpearCancel());
            skillHandlers.Add(2003, new MobDefUpSelf());
            skillHandlers.Add(106, new GuardUp());
            skillHandlers.Add(2064, new AstuteStab());

            //#endregion

            //#region Knight

            skillHandlers.Add(2123,
                new SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._2_1_Class.Knight_圣骑士____fen.
                    ShockWave());
            skillHandlers.Add(2247, new AtkUnDead());
            skillHandlers.Add(946, new ConSpear());
            skillHandlers.Add(2065, new AstuteBlow());
            skillHandlers.Add(934, new ElementAddUp(Elements.Holy, "LightAddUp"));
            skillHandlers.Add(2041, new DifrectArrow());
            skillHandlers.Add(2063, new AstuteSlash());
            skillHandlers.Add(2245, new CutDownSpear());
            skillHandlers.Add(4025, new DJoint());
            skillHandlers.Add(4029, new AProtect());
            skillHandlers.Add(2248, new HoldShield());
            skillHandlers.Add(2246, new HitRow());
            skillHandlers.Add(2125, new Valkyrie());
            skillHandlers.Add(3251, new VicariouslyResu());
            skillHandlers.Add(2381, new DirlineRandSeq());
            skillHandlers.Add(2382, new DirlineRandSeq2());
            skillHandlers.Add(2061, new Revive());

            //#endregion

            //#region Tatarabe

            skillHandlers.Add(2009, new Synthese());
            skillHandlers.Add(2051, new Synthese());
            skillHandlers.Add(2083, new Synthese());
            skillHandlers.Add(2185, new AtkRow());
            skillHandlers.Add(800, new RockInfo());
            skillHandlers.Add(2200, new EventCampfire());
            skillHandlers.Add(2071, new PosturetorToise());
            skillHandlers.Add(905, new GoRiKi());
            skillHandlers.Add(2177, new StoneThrow());
            skillHandlers.Add(2135, new ThrowDirt());

            //#endregion

            //#region Blacksmith

            skillHandlers.Add(2010, new Synthese());
            skillHandlers.Add(2017, new Synthese());
            skillHandlers.Add(3342, new FrameHart());
            skillHandlers.Add(2224, new RockCrash());
            skillHandlers.Add(2198, new FirstAid());
            skillHandlers.Add(2194, new KnifeGrinder());
            skillHandlers.Add(2388, new ThrowNugget());
            skillHandlers.Add(2387, new EearthCrash());
            skillHandlers.Add(6050, new PetAttack());
            skillHandlers.Add(6051, new PetBack());
            skillHandlers.Add(2207, new RockAnalysis());
            skillHandlers.Add(6102, new PetCastSkill(6103, "MACHINE"));
            skillHandlers.Add(6103, new PetMacAtk());
            skillHandlers.Add(6101, new PetMacLHitUp());
            skillHandlers.Add(930, new FireAddup());
            skillHandlers.Add(6104, new PetCastSkill(6105, "MACHINE"));
            skillHandlers.Add(6105, new PetMacCircleAtk());
            skillHandlers.Add(2262, new SupportRockInfo());
            skillHandlers.Add(2253, new GuideRock());
            skillHandlers.Add(2261, new DurDownCancel());
            skillHandlers.Add(2395, new Balls());
            skillHandlers.Add(942, new BoostPower());
            skillHandlers.Add(409, new StoDamUp());
            skillHandlers.Add(410, new StoHitUp());
            skillHandlers.Add(411, new StoAvoUp());

            //#endregion

            //#region Machinery

            skillHandlers.Add(2039, new Synthese());
            skillHandlers.Add(809, new MachineInfo());
            skillHandlers.Add(132, new RobotDamUp());
            skillHandlers.Add(970, new RobotRecUp());
            skillHandlers.Add(964, new RobotHpUp());
            skillHandlers.Add(2326, new RobotAmobm());
            skillHandlers.Add(968, new RobotHitUp());
            skillHandlers.Add(966,
                new SkillDefinations.Player_Skills.Passive_skills.BackPackers_Class._2_2_Class.Machinery_机械师____tat.
                    RobotDefUp());
            skillHandlers.Add(2323, new RobotChaff());
            skillHandlers.Add(969, new RobotAvoUp());
            skillHandlers.Add(965,
                new SkillDefinations.Player_Skills.Passive_skills.BackPackers_Class._2_2_Class.Machinery_机械师____tat.
                    RobotAtkUp());
            skillHandlers.Add(2324, new MirrorSkill());
            skillHandlers.Add(2325, new RobotTeleport());
            skillHandlers.Add(2322, new RobotBerserk());
            skillHandlers.Add(2368, new RobotChillLaser());
            skillHandlers.Add(2327, new RobotSalvoFire());
            skillHandlers.Add(2369, new RobotEcm());
            skillHandlers.Add(2422, new RobotFireRadiation());
            skillHandlers.Add(2424, new RobotOverTune());
            skillHandlers.Add(2423, new RobotSparkBall());
            skillHandlers.Add(2425, new RobotLovageCannon());
            skillHandlers.Add(506, new MciDamUp());
            skillHandlers.Add(507, new MciHitUp());
            skillHandlers.Add(508, new MciAvoUp());

            //#endregion

            //#region Farmasist

            skillHandlers.Add(2020, new Synthese());
            skillHandlers.Add(2034, new Synthese());
            skillHandlers.Add(2040, new Synthese());
            skillHandlers.Add(2054, new Synthese());
            skillHandlers.Add(2085, new Synthese());
            skillHandlers.Add(2086, new Synthese());
            skillHandlers.Add(2089, new Synthese());
            skillHandlers.Add(3128, new Cultivation());
            skillHandlers.Add(807, new PlantInfo());
            skillHandlers.Add(804, new TreeInfo());
            skillHandlers.Add(2169, new GrassTrap());
            skillHandlers.Add(2170, new PitTrap());
            skillHandlers.Add(2196, new HealingTree());

            //#endregion

            //#region Alchemist

            skillHandlers.Add(2022, new Synthese());
            skillHandlers.Add(2118,
                new SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._2_1_Class.Alchemist_炼金术士____far.
                    Phalanx());
            skillHandlers.Add(3096, new DelayTrap());
            skillHandlers.Add(2389, new DustExplosion());
            skillHandlers.Add(2214, new PlantAnalysis());
            skillHandlers.Add(954, new FoodThrow());
            skillHandlers.Add(909, new PotionFighter());
            skillHandlers.Add(406, new PlaDamUp());
            skillHandlers.Add(407, new PlaHitUp());
            skillHandlers.Add(408, new PlaAvoUp());
            skillHandlers.Add(6202, new PetCastSkill(6203, "PLANT"));
            skillHandlers.Add(6203, new PetPlantAtk());
            skillHandlers.Add(6206, new PetCastSkill(6207, "PLANT"));
            skillHandlers.Add(6207, new PetPlantDefupSelf());
            skillHandlers.Add(6204, new PetCastSkill(6205, "PLANT"));
            skillHandlers.Add(6205, new PetPlantHealing());
            skillHandlers.Add(2263, new SupportPlantInfo());
            skillHandlers.Add(943, new BoostMagic());
            skillHandlers.Add(2390, new ThrowChemical());
            skillHandlers.Add(3343, new SumChemicalPlant());
            skillHandlers.Add(3344, new SumChemicalPlant2());
            skillHandlers.Add(6200, new PetCastSkill(6201, "PLANT"));
            skillHandlers.Add(6201, new PetPlantPoison());
            skillHandlers.Add(2211, new TreeAnalysis());
            skillHandlers.Add(4028, new Super_A_T_PJoint()); //强力援手,但技能内容空白

            //#endregion

            //#region Marionest

            skillHandlers.Add(2038, new Synthese());
            skillHandlers.Add(133, new MarioDamUp());
            skillHandlers.Add(2328, new MarioCtrl());
            skillHandlers.Add(967, new MarioCtrlMove());
            skillHandlers.Add(2329, new MarioOver());
            skillHandlers.Add(2331, new EnemyCharming());
            skillHandlers.Add(2335, new MarioEarthWater());
            skillHandlers.Add(2334, new MarioWindEarth());
            skillHandlers.Add(2333, new MarioFireWind());
            skillHandlers.Add(2332, new MarioWaterFire());
            skillHandlers.Add(2371, new Puppet());
            skillHandlers.Add(976, new MarioTimeUp());
            skillHandlers.Add(2370, new MarionetteHarmony()); //2-2JOB36,大概是个错误的实装……
            skillHandlers.Add(980, new ChangeMarionette());
            skillHandlers.Add(3333, new MarioCancel());
            skillHandlers.Add(981, new MariostateUp());
            skillHandlers.Add(3334, new SumMario(26040001, 3335));
            skillHandlers.Add(3335, new SumMarioCont(Elements.Fire));
            skillHandlers.Add(3336, new SumMario(26100009, 3337));
            skillHandlers.Add(3337, new SumMarioCont(Elements.Water));
            skillHandlers.Add(3338, new SumMario(26100009, 3339));
            skillHandlers.Add(3339, new SumMarioCont(Elements.Wind));
            skillHandlers.Add(3340, new SumMario(26070003, 3341));
            skillHandlers.Add(3341, new SumMarioCont(Elements.Earth));

            //#endregion

            //#region Ranger

            skillHandlers.Add(2088, new Synthese());
            skillHandlers.Add(713, new Bivouac());
            skillHandlers.Add(805, new InsectInfo());
            skillHandlers.Add(806, new BirdInfo());
            skillHandlers.Add(808, new AnimalInfo());
            skillHandlers.Add(812, new WataniInfo());
            skillHandlers.Add(816, new TreasureInfo());
            skillHandlers.Add(2197, new CswarSleep());
            skillHandlers.Add(2103, new Unlock());
            skillHandlers.Add(403, new AniDamUp());
            skillHandlers.Add(404, new AniHitUp());
            skillHandlers.Add(405, new AniAvoUp());
            skillHandlers.Add(415, new WanDamUp());
            skillHandlers.Add(416, new WanHitUp());
            skillHandlers.Add(417, new WanAvoUp());

            //#endregion

            //#region Druid

            skillHandlers.Add(3146,
                new SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._2_1_Class.Druid_神官____vote.
                    CureAll());
            skillHandlers.Add(3307, new RegiAllUp());
            skillHandlers.Add(3308, new AreaHeal());
            skillHandlers.Add(3257,
                new SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._2_1_Class.Druid_神官____vote.
                    STR_VIT_AGI_UP());
            skillHandlers.Add(3258,
                new SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._2_1_Class.Druid_神官____vote.
                    MAG_INT_DEX_UP());
            skillHandlers.Add(3056, new HolyFeather());
            skillHandlers.Add(3164, new FlashLight());
            //skillHandlers.Add(3080, new SkillDefinations.Enchanter.ElementCircle(Elements.Holy));//熾天使之翼（ホーリーパワーサークル
            skillHandlers.Add(3163, new SunLightShower());
            skillHandlers.Add(3268, new CriAvoDownOne());
            skillHandlers.Add(3266, new LightHigeCircle());
            skillHandlers.Add(3118, new Seal());
            skillHandlers.Add(2210, new UndeadAnalysis());
            skillHandlers.Add(3267, new UndeadMdefDownOne());
            skillHandlers.Add(3077, new ClairvoYance());
            skillHandlers.Add(3119, new SealMagic());
            skillHandlers.Add(950, new TranceSpdUp());
            skillHandlers.Add(940, new ElementLimitUp(Elements.Holy)); //光明守護
            skillHandlers.Add(509, new UndDamUp());
            skillHandlers.Add(510, new UndHitUp());
            skillHandlers.Add(511, new UndAvoUp());
            skillHandlers.Add(3345, new AllHealing());

            //#endregion

            //#region Bard

            skillHandlers.Add(2310, new Samba());
            skillHandlers.Add(3323, new DeadMarch());
            skillHandlers.Add(2313, new Classic());
            skillHandlers.Add(2311, new HeavyMetal());
            skillHandlers.Add(2312, new RockAndRoll());
            skillHandlers.Add(2309, new Transformer());
            skillHandlers.Add(2367, new MusicalBlow());
            skillHandlers.Add(2366, new Shout());
            skillHandlers.Add(2365, new Relaxation());
            skillHandlers.Add(2315, new BardSession());
            skillHandlers.Add(3321, new ORaToRiO());
            skillHandlers.Add(2308, new PopMusic());
            skillHandlers.Add(2307, new Fusion());
            skillHandlers.Add(2306, new ChangeMusic());
            skillHandlers.Add(131, new MusicalDamUp());
            skillHandlers.Add(2314, new Requiem());
            skillHandlers.Add(3322, new AttractMarch());
            skillHandlers.Add(3320, new LoudSong());

            //#endregion

            //#region Sage

            skillHandlers.Add(2030, new Synthese());
            skillHandlers.Add(2031, new Synthese());
            skillHandlers.Add(2032, new Synthese());
            skillHandlers.Add(2033, new Synthese());
            skillHandlers.Add(2296, new IntelRides());
            skillHandlers.Add(3169, new EnergyStorm());
            skillHandlers.Add(2330, new EnergyFreak());
            skillHandlers.Add(3291, new EnergyFlare());
            skillHandlers.Add(3292, new ChgstBlock());
            skillHandlers.Add(3312, new LuminaryNova());
            skillHandlers.Add(3315, new LastInQuest());
            skillHandlers.Add(3313, new AReflection());
            skillHandlers.Add(130, new ReadDamup());
            skillHandlers.Add(2295, new StaffCtrl());
            skillHandlers.Add(2297, new Provide());
            skillHandlers.Add(2294, new MonsterSketch());
            skillHandlers.Add(3293, new MagHitUpCircle());
            skillHandlers.Add(3314, new SumDop());

            //#endregion

            //#region Necromancer

            skillHandlers.Add(3331, new Dejion());
            skillHandlers.Add(2316, new SoulBurn());
            skillHandlers.Add(2317, new SpiritBurn());
            skillHandlers.Add(3288, new DarkLight());
            skillHandlers.Add(3330, new EvilSoul());
            skillHandlers.Add(3332, new ChaosGate());
            skillHandlers.Add(2318, new AbsorbHpWeapon());
            skillHandlers.Add(2320, new SummobLemures());
            skillHandlers.Add(961, new LemuresHpUp());
            skillHandlers.Add(963, new LemuresMatkUp());
            skillHandlers.Add(962, new LemuresAtkUp());
            skillHandlers.Add(2321, new HealLemures());
            skillHandlers.Add(2319, new Rebone());
            skillHandlers.Add(3121, new NeKuRoMaNShi());
            skillHandlers.Add(315, new ChgstDamUp());
            skillHandlers.Add(3122,
                new SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._2_2_Class.Necromancer_死灵使____lock.
                    TrDrop2());
            skillHandlers.Add(3297, new Terror());
            skillHandlers.Add(3324, new SumDeath());
            skillHandlers.Add(3325, new SumDeath2());
            skillHandlers.Add(3326, new SumDeath3());
            skillHandlers.Add(3327, new SumDeath4());
            skillHandlers.Add(3328, new SumDeath5());
            skillHandlers.Add(3329, new SumDeath6());

            //#endregion

            //#region Soul

            skillHandlers.Add(4450, new Soul()); //SKILL_P_T_SWORDMAN,光戰士之魂
            skillHandlers.Add(4451, new Soul()); //SKILL_P_T_KINGHT,聖騎士之魂
            skillHandlers.Add(4452, new Soul()); //SKILL_P_T_ASSASSIN,暗殺者之魂
            skillHandlers.Add(4453, new Soul()); //SKILL_P_T_ARCHER,弓手之魂
            skillHandlers.Add(4454, new Soul()); //SKILL_P_T_MAGIC,魔導士之魂
            skillHandlers.Add(4455, new Soul()); //SKILL_P_T_ELEMENTAL,元素術師之魂
            skillHandlers.Add(4460, new Soul()); //SKILL_P_T_DRUID,神官之魂
            skillHandlers.Add(4461, new Soul()); //SKILL_P_T_KABBALIST,暗黑神官之魂
            skillHandlers.Add(4462, new Soul()); //SKILL_P_T_BLACKSMITH,鐵匠之魂
            skillHandlers.Add(4463, new Soul()); //SKILL_P_T_ALCHEMIST,鍊金術師之魂
            skillHandlers.Add(4464, new Soul()); //SKILL_P_T_EXPLORER,探險家之魂
            skillHandlers.Add(4465, new Soul()); //SKILL_P_T_TRADER,拜金使之魂
            skillHandlers.Add(4466, new Soul()); //SKILL_P_T_JOKER,道化師之魂

            //#endregion

            //#region DarkStalker

            skillHandlers.Add(2357, new DarkMist());
            skillHandlers.Add(2356, new LifeSteal());
            skillHandlers.Add(3289, new DegradetionDarkFlare());
            skillHandlers.Add(3290, new DegradetionDarkFlare());
            skillHandlers.Add(2403, new FlareSting());
            skillHandlers.Add(2404, new FlareSting2());
            skillHandlers.Add(2405, new BloodAbsrd());
            skillHandlers.Add(3120, new Spell());
            skillHandlers.Add(957, new NecroResu());
            skillHandlers.Add(314, new ChgstSwoDamUp());
            skillHandlers.Add(2277, new CancelLightCircle());
            skillHandlers.Add(958, new DarkProtect());
            skillHandlers.Add(935, new ElementAddUp(Elements.Dark, "DarkAddUp"));
            skillHandlers.Add(2278, new LightSeal());
            skillHandlers.Add(2279, new BradStigma());
            skillHandlers.Add(2358, new HpLostDamUp());
            skillHandlers.Add(979, new HpDownToDamUp());
            skillHandlers.Add(2406, new DarknessOfNight());
            skillHandlers.Add(2407, new DarknessOfNight2());
            skillHandlers.Add(500, new EleDamUp());
            skillHandlers.Add(501, new EleHitUp());
            skillHandlers.Add(502, new EleAvoUp());

            //#endregion

            //#region Striker

            skillHandlers.Add(2149, new ElementArrow(Elements.Fire));
            skillHandlers.Add(2150, new ElementArrow(Elements.Water));
            skillHandlers.Add(2151, new ElementArrow(Elements.Earth));
            skillHandlers.Add(2152, new ElementArrow(Elements.Wind));
            skillHandlers.Add(2220, new PotionArrow());
            skillHandlers.Add(2190, new LightDarkArrow(Elements.Holy));
            skillHandlers.Add(2191, new LightDarkArrow(Elements.Dark));
            skillHandlers.Add(313, new HuntingTactics());
            skillHandlers.Add(2267, new BowCastCancelOne());
            skillHandlers.Add(2266, new BlastArrow());
            skillHandlers.Add(310, new ChgstArrDamUp());
            skillHandlers.Add(2385, new ArmBreak());
            skillHandlers.Add(6500, new PetBirdAtkRowCircle());
            skillHandlers.Add(6501, new PetBirdAtkRowCircle2());
            skillHandlers.Add(6306, new DogHateUpCircle());
            skillHandlers.Add(6307, new PetDogHateUpCircle());
            skillHandlers.Add(6502, new BirdAtk());
            skillHandlers.Add(6503, new PetBirdAtk());
            skillHandlers.Add(6308, new PetCastSkill(6309, "ANIMAL"));
            skillHandlers.Add(6309, new PetDogAtkCircle());
            skillHandlers.Add(6550, new BirdDamUp());
            skillHandlers.Add(6350, new DogHpUp());
            skillHandlers.Add(6310, new PetCastSkill(6311, "ANIMAL"));
            skillHandlers.Add(6311, new PetDogDefUp());

            //#endregion

            //#region Gunner

            skillHandlers.Add(2285, new FastDraw());
            skillHandlers.Add(2286, new PluralityShot());
            skillHandlers.Add(2287, new ChargeShot());
            skillHandlers.Add(2289, new GrenadeShot());
            skillHandlers.Add(2291, new GrenadeSlow());
            skillHandlers.Add(2292, new GrenadeStan());
            skillHandlers.Add(2163, new StunShot());
            skillHandlers.Add(2288, new BurstShot());
            skillHandlers.Add(974, new CQB());
            skillHandlers.Add(2364, new GunCancel());
            skillHandlers.Add(2418, new VitalShot());
            skillHandlers.Add(2419, new ClothCrest());
            skillHandlers.Add(126, new RifleGunDamUp());
            skillHandlers.Add(2290, new ApiBullet());
            skillHandlers.Add(210, new GunHitUp());
            skillHandlers.Add(2293, new OverRange());
            skillHandlers.Add(2363, new BulletDance());
            skillHandlers.Add(2420, new PrecisionFire());
            skillHandlers.Add(2421, new CanisterShot());

            //#endregion

            //#region Explorer

            skillHandlers.Add(2222, new CaveHiding());
            skillHandlers.Add(2392, new Blinding());
            skillHandlers.Add(2221, new CaveBivouac());
            skillHandlers.Add(2212, new InsectAnalysis());
            skillHandlers.Add(2213, new BirdAnalysis());
            skillHandlers.Add(2215, new AnimalAnalysis());
            skillHandlers.Add(2264, new SupportInfo());
            skillHandlers.Add(953, new BaitTrap());
            skillHandlers.Add(311, new TrapDamUp());
            skillHandlers.Add(944, new BoostHp());
            skillHandlers.Add(2391, new FakeDeath());
            skillHandlers.Add(6300, new PetCastSkill(6301, "ANIMAL"));
            skillHandlers.Add(6301, new PetDogSlash());
            skillHandlers.Add(6302, new PetCastSkill(6303, "ANIMAL"));
            skillHandlers.Add(6303, new PetDogStan());
            skillHandlers.Add(6304, new PetCastSkill(6305, "ANIMAL"));
            skillHandlers.Add(6305, new PetDogLineatk());
            skillHandlers.Add(2171, new BarbedTrap());
            skillHandlers.Add(2172, new Bungestac());
            skillHandlers.Add(412, new InsDamUp());
            skillHandlers.Add(413, new InsHitUp());
            skillHandlers.Add(414, new InsAvoUp());
            skillHandlers.Add(3347, new AbsorbSpWeapon());
            skillHandlers.Add(2478, new FascinationBox()); //JOB50

            //#endregion

            //#region TreasureHunter

            skillHandlers.Add(2336, new BackRush());
            skillHandlers.Add(2337, new Catch());
            skillHandlers.Add(2340, new ConthWhip());
            skillHandlers.Add(2373, new WhipFlourish());
            skillHandlers.Add(2426, new Caution());
            skillHandlers.Add(2372, new Snatch());
            skillHandlers.Add(2427, new PullWhip());
            skillHandlers.Add(2430, new SonicWhip());
            skillHandlers.Add(2431, new SonicWhipSEQ());
            skillHandlers.Add(2432, new SonicWhipSEQ2());
            skillHandlers.Add(129, new RopeDamUp());
            skillHandlers.Add(2341, new WeaponRemove());
            skillHandlers.Add(2428, new Warn()); //警戒
            skillHandlers.Add(2342, new ArmorRemove());
            skillHandlers.Add(134, new UnlockDamUp());
            skillHandlers.Add(2429, new Escape());
            skillHandlers.Add(960, new GoodLucky()); //2018/4/5实装
            skillHandlers.Add(2339, new SearchTreasure()); //2018/4/9實裝

            //#endregion

            //#region Merchant

            skillHandlers.Add(702, new Packing());
            skillHandlers.Add(703, new BuyRateDown());
            skillHandlers.Add(704, new SellRateUp());
            skillHandlers.Add(2173, new AtkUpOne());
            skillHandlers.Add(2180, new SunSofbley());
            skillHandlers.Add(2186, new Magrow());

            //#endregion

            //#region Trader

            skillHandlers.Add(2394, new BugRand());
            skillHandlers.Add(705, new Trust());
            skillHandlers.Add(906, new BagDamUp());
            skillHandlers.Add(811, new HumanInfo());
            skillHandlers.Add(2223, new Shift());
            skillHandlers.Add(2225, new AgiDexUpOne());
            skillHandlers.Add(948, new BagCapDamup());
            skillHandlers.Add(2227, new Abetment());
            skillHandlers.Add(706, new Connection());
            skillHandlers.Add(2218, new HumanAnalysis());
            skillHandlers.Add(945, new BoostCritical());
            skillHandlers.Add(6400, new HumCustomary());
            skillHandlers.Add(6401, new PetHumCustomary());
            skillHandlers.Add(6404, new PetAtkupSelf());
            skillHandlers.Add(6405, new PetHitupSelf());
            skillHandlers.Add(6406, new PetDefupSelf());
            skillHandlers.Add(6402, new HumAdditional());
            skillHandlers.Add(6403, new PetHumAdditional());
            skillHandlers.Add(6407, new PetSlash());
            skillHandlers.Add(6408, new PetIai());
            skillHandlers.Add(6409, new PetProvocation());
            skillHandlers.Add(6410, new PetSennpuuken());
            skillHandlers.Add(6411, new PetMeditation());
            skillHandlers.Add(6450, new HumHealRateUp());

            //#endregion

            //#region Gambler

            skillHandlers.Add(3286, new RandHeal());
            skillHandlers.Add(3287, new RouletteHeal());
            skillHandlers.Add(2348, new AtkDownOne());
            skillHandlers.Add(2374, new CardBoomEran());
            skillHandlers.Add(2350, new RandDamOne());
            skillHandlers.Add(2377, new DoubleUp());
            skillHandlers.Add(2375, new CoinShot());
            skillHandlers.Add(972, new Blackleg());
            skillHandlers.Add(2347, new Slot());
            skillHandlers.Add(973, new BadLucky());
            skillHandlers.Add(2376, new SkillBreak());
            skillHandlers.Add(2436, new TrickDice());
            skillHandlers.Add(2433, new SumArcanaCard());
            //skillHandlers.Add(2432, new SkillDefinations.Gambler.SumArcanaCard2());
            //skillHandlers.Add(2431, new SkillDefinations.Gambler.SumArcanaCard3());
            skillHandlers.Add(2434, new SumArcanaCard4());
            skillHandlers.Add(2435, new SumArcanaCard5());
            skillHandlers.Add(2351, new RandChgstateCircle());
            skillHandlers.Add(2439, new FlowerCard());
            skillHandlers.Add(2440, new FlowerCardSEQ());
            skillHandlers.Add(2441, new FlowerCardSEQ2());

            //#endregion

            //#region Pet

            skillHandlers.Add(6424, new Revive(2));
            skillHandlers.Add(6425, new Revive(5));

            //#endregion

            //#region Breeder

            skillHandlers.Add(1000, new GrowUp());
            skillHandlers.Add(1001, new Biology());
            skillHandlers.Add(1002, new LionPower());
            skillHandlers.Add(1003, new Reins());
            skillHandlers.Add(2442, new TheTrust());
            skillHandlers.Add(2443, new Metamorphosis());
            skillHandlers.Add(2444, new PetDelayCancel());
            skillHandlers.Add(2445, new Akurobattoibeijon());
            skillHandlers.Add(2446, new HealFire());
            skillHandlers.Add(2447, new Encouragement());

            //#endregion

            //#region Gardener

            skillHandlers.Add(2453, new IAmTree());
            skillHandlers.Add(2449, new GardenerSkill());
            skillHandlers.Add(1006, new MoogCoalUp());
            skillHandlers.Add(2025, new Synthese());
            skillHandlers.Add(2455, new Cabin());
            skillHandlers.Add(1004, new GadenMaster());
            skillHandlers.Add(1005, new Topiary());
            skillHandlers.Add(1007, new Gardening());
            skillHandlers.Add(2452, new WeatherControl());
            skillHandlers.Add(2451, new HeavenlyControl());
            skillHandlers.Add(2454, new Gathering());

            //#endregion

            //#region 新Boss

            skillHandlers.Add(22000, new B1());
            skillHandlers.Add(22008, new WaterBall());

            //#endregion

            //#region 旅者

            skillHandlers.Add(23000, new NewSkill.Traveler.ChainLightning());
            skillHandlers.Add(23001, new HartHeal());
            skillHandlers.Add(23002, new ThunderFall());
            skillHandlers.Add(23003, new ThunderFall_Effect());
            skillHandlers.Add(23004, new Silva());
            skillHandlers.Add(23005, new NewSkill.Traveler.EarthQuake());
            skillHandlers.Add(23006, new EarthQuake_Effect());

            //#endregion

            //#region 武器技能

            skillHandlers.Add(24000, new WA_Neutral());
            skillHandlers.Add(1508, new MinCriRateUp());

            //#endregion

            //#region FL1

            skillHandlers.Add(100, new MaxHPUp());
            skillHandlers.Add(107, new SwordMastery());
            skillHandlers.Add(109, new SpearMastery());
            skillHandlers.Add(122, new TwoAxeMastery());
            skillHandlers.Add(116, new ShieldMastery());
            skillHandlers.Add(2112,
                new SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._1_0_Class.Swordman_剑士_.ConfuseBlow());
            skillHandlers.Add(2138, new LightningSpear());
            skillHandlers.Add(2121, new ChargeStrike());

            //#endregion

            //#region FL2-1

            skillHandlers.Add(2354, new NewSkill.FL2_1.Gravity());
            skillHandlers.Add(2124, new Sinkuha());
            skillHandlers.Add(119, new TwoHandMastery());
            skillHandlers.Add(2002, new ATKUp());
            skillHandlers.Add(2239, new Transporter());
            skillHandlers.Add(25010, new FireSlash());
            skillHandlers.Add(25011, new ArmorBreaker());

            //#endregion

            //#region FL2-2

            skillHandlers.Add(2228, new HolyBlade());
            skillHandlers.Add(2276, new DarkVacuum());
            skillHandlers.Add(123, new RapierMastery());
            skillHandlers.Add(3170, new NewSkill.FL2_2.Healing());
            skillHandlers.Add(120, new TwoSpearMastery());
            skillHandlers.Add(2383, new Appeal());
            skillHandlers.Add(2249, new StrikeSpear());
            skillHandlers.Add(25020, new IceStab());
            skillHandlers.Add(25021, new ShieldDefence());
            skillHandlers.Add(25022, new ShieldBash());

            //#endregion

            //#region FR1

            skillHandlers.Add(2042, new Hiding());
            skillHandlers.Add(102, new MaxSPUp());
            skillHandlers.Add(2000, new HitMeleeUp());
            skillHandlers.Add(2110, new NewSkill.FR1.Blow());


            skillHandlers.Add(2035, new Synthese());

            skillHandlers.Add(2126, new VitalAttack());
            skillHandlers.Add(2119, new NewSkill.FR1.Brandish());
            skillHandlers.Add(114, new LAvoUp());
            skillHandlers.Add(2129, new ChargeArrow());
            skillHandlers.Add(2148, new PluralityArrow());

            //#endregion

            //#region FR2-1

            skillHandlers.Add(2113,
                new SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._1_0_Class.Swordman_剑士_.StunBlow());
            skillHandlers.Add(2043, new Cloaking());
            skillHandlers.Add(2384, new Raid());
            skillHandlers.Add(977, new AvoidUp());
            skillHandlers.Add(2068, new BackAtk());
            skillHandlers.Add(312, new CriDamUp());
            skillHandlers.Add(118, new ClawMastery());
            skillHandlers.Add(2360, new CyclOne());
            skillHandlers.Add(2140, new PosionNeedle());
            skillHandlers.Add(26010, new ThrowThrowThrow());

            //#endregion

            //#region FR2-2

            skillHandlers.Add(951, new ShotStance());
            skillHandlers.Add(2049, new LHitUp());
            //skillHandlers.Add(2130, new SkillDefinations.Striker.BlastArrow());//贯通之箭,技能体指定错误
            skillHandlers.Add(2386, new ArrowGroove());
            skillHandlers.Add(2144, new NewSkill.FR2_2.FireArrow());
            skillHandlers.Add(2145, new NewSkill.FR2_2.WaterArrow());
            skillHandlers.Add(2146, new NewSkill.FR2_2.EarthArrow());
            skillHandlers.Add(2147, new NewSkill.FR2_2.WindArrow());
            skillHandlers.Add(2206, new DistanceArrow());
            skillHandlers.Add(26020, new SlowArrow());

            //#endregion

            //#region 自定义技能

            skillHandlers.Add(4901, new ModeChange());
            skillHandlers.Add(25000, new PassiveStr());
            skillHandlers.Add(25001, new PassiveAgi());
            skillHandlers.Add(25002, new PassiveInt());
            skillHandlers.Add(25003, new PassiveVit());
            skillHandlers.Add(25004, new PassiveDex());
            skillHandlers.Add(25005, new PassiveMag());
            skillHandlers.Add(27052, new HolyLight());
            skillHandlers.Add(27053, new KyrieEleison());
            skillHandlers.Add(27054, new HighHeal());
            skillHandlers.Add(27055, new Assumptio());
            skillHandlers.Add(27056, new Judex());
            skillHandlers.Add(27057, new Adoramus());
            skillHandlers.Add(27058, new Oratio());
            skillHandlers.Add(27059, new SkillDefinations.SunFlowerAdditions.TurnUndead());
            skillHandlers.Add(27060, new EquipChange());

            //#endregion

            //#region Partner

            skillHandlers.Add(15482, new TrialsInHeavenAndHell()); //路西法专用技能
            skillHandlers.Add(15330, new Thousand()); //伊邪那美专用技能
            skillHandlers.Add(15331, new Yukimori()); //伊邪那美专用技能
            skillHandlers.Add(6873, new ShinStormSword()); //蝙蝠阿鲁玛专用技能
            skillHandlers.Add(20206, new ComeOn()); //清姬专用技能
            skillHandlers.Add(15005, new MachineNurseAria()); //亚里亚专用技能
            skillHandlers.Add(15007, new Emission()); //亚里亚专用技能
            skillHandlers.Add(15010, new BlowAway()); //亚里亚专用技能
            skillHandlers.Add(6890, new Urayahachinototo()); //美琴专用技能
            skillHandlers.Add(6891, new DontCareAnymore()); //美琴专用技能
            skillHandlers.Add(20048, new DarkWorldWind()); //阿露卡多专用技能
            skillHandlers.Add(6676, new SkillDefinations.Parnter.DarkMist()); //阿露卡多专用技能
            skillHandlers.Add(6718, new Accept()); //阿露卡多专用技能
            skillHandlers.Add(6766, new NoFlashPlayer()); //罗蕾莱专用技能
            skillHandlers.Add(6765, new GoodByeRendezvous()); //罗蕾莱专用技能
            skillHandlers.Add(6758, new LeaveIt()); //玉藻专用技能
            skillHandlers.Add(6760, new TheRevelationOfKuo()); //玉藻专用技能
            skillHandlers.Add(15173, new LikeAPie()); //ECO猪专用技能
            skillHandlers.Add(15177, new HowDidYouDoIt()); //ECO猪专用技能
            skillHandlers.Add(15178, new YouLikeMe()); //ECO猪专用技能
            skillHandlers.Add(15179, new ImSoAngry()); //ECO猪专用技能
            skillHandlers.Add(15180, new EcoDay()); //ECO猪专用技能
            skillHandlers.Add(6918, new Kirieredison()); //雾绘专用技能
            skillHandlers.Add(6919, new PleaseTakeCareOfMe()); //雾绘专用技能
            skillHandlers.Add(6920, new TakeCareOfYou()); //雾绘专用技能
            skillHandlers.Add(6921, new ISupport()); //雾绘专用技能
            skillHandlers.Add(7127, new AllShot()); //咕咕鸡阿鲁玛专用技能
            skillHandlers.Add(15421, new MostThingsBurn()); //咕咕鸡阿鲁玛专用技能
            skillHandlers.Add(15422, new ContinuousShooting()); //咕咕鸡阿鲁玛专用技能
            skillHandlers.Add(9294, new BlowOfFriendship()); //装备咕咕鸡阿鲁玛后玩家可用技能
            skillHandlers.Add(7144, new Demolition()); //炸脖龙专用技能
            skillHandlers.Add(7145, new TimeDeath()); //炸脖龙专用技能
            skillHandlers.Add(7146, new BanderSnatch()); //炸脖龙专用技能
            skillHandlers.Add(15458, new SoBusy()); //炸脖龙专用技能
            skillHandlers.Add(15459, new YouCanPraiseMore()); //炸脖龙专用技能
            skillHandlers.Add(15518, new ThePowerOfLoveMayle()); //守护神之心专用技能

            //#endregion
        }

        private void SendPetGrowth(Actor actor, SSMG_ACTOR_PET_GROW.GrowType growType, uint value) {
            if (actor.type != ActorType.PET)
                return;
            var pet = (ActorPet)actor;
            if (pet.Owner == null)
                return;
            if (!pet.Owner.Online)
                return;

            var p = new SSMG_ACTOR_PET_GROW();
            if (pet.Ride)
                p.PetActorID = pet.Owner.ActorID;
            else
                p.PetActorID = pet.ActorID;
            p.OwnerActorID = pet.Owner.ActorID;
            p.Type = growType;
            p.Value = value;
            MapClient.FromActorPC(pet.Owner).NetIo.SendPacket(p);
        }

        public void ProcessPetGrowth(Actor actor, PetGrowthReason reason) {
            if (actor.type != ActorType.PET)
                return;
            var pet = (ActorPet)actor;
            if (!pet.Owner.Online)
                return;
            var growType = SSMG_ACTOR_PET_GROW.GrowType.HP;
            if (pet.Owner.Inventory.Equipments.ContainsKey(EnumEquipSlot.PET)) {
                var item = pet.Owner.Inventory.Equipments[EnumEquipSlot.PET];
                int rate;
                switch (reason) {
                    case PetGrowthReason.CriticalHit:
                    case PetGrowthReason.UseSkill:
                    case PetGrowthReason.SkillHit:
                        rate = 10;
                        break;
                    case PetGrowthReason.PhysicalHit:
                    case PetGrowthReason.ItemRecover:
                    case PetGrowthReason.MagicalBeenHit:
                    case PetGrowthReason.PhysicalBeenHit:
                        rate = 3;
                        break;
                }

                if (pet.Ride)
                    rate = 15;
                else
                    rate = 5;
                if (Global.Random.Next(0, 99) < rate) {
                    item.Refine = 1;
                    if (!pet.Ride) {
                        int type;
                        switch (reason) {
                            case PetGrowthReason.PhysicalBeenHit:
                            case PetGrowthReason.MagicalBeenHit:
                                type = Global.Random.Next(0, 8);
                                switch (type) {
                                    case 0:
                                        if (pet.Limits.hp > item.HP) {
                                            item.HP++;
                                            pet.MaxHP++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.HP;
                                        }

                                        break;
                                    case 1:
                                        if (pet.Limits.atk_max > item.Atk1) {
                                            item.Atk1++;
                                            item.Atk2++;
                                            item.Atk3++;

                                            pet.Status.min_atk1++;
                                            pet.Status.max_atk1++;
                                            pet.Status.min_atk2++;
                                            pet.Status.max_atk2++;
                                            pet.Status.min_atk3++;
                                            pet.Status.max_atk3++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.ATK1;
                                        }

                                        break;
                                    case 2:
                                        if (pet.Limits.hit_melee > item.HitMelee) {
                                            item.HitMelee++;
                                            pet.Status.hit_melee++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.HitMelee;
                                        }

                                        break;
                                    case 3:
                                        if (pet.Limits.hit_ranged > item.HitRanged) {
                                            item.HitRanged++;
                                            pet.Status.hit_ranged++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.HitRanged;
                                        }

                                        break;
                                    case 4:
                                        if (pet.Limits.aspd > item.ASPD) {
                                            item.ASPD++;
                                            pet.Status.aspd++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.ASPD;
                                        }

                                        break;
                                    case 5:
                                        if (pet.Limits.def_add > item.Def) {
                                            item.Def++;
                                            pet.Status.def_add++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.Def;
                                        }

                                        break;
                                    case 6:
                                        if (pet.Limits.mdef_add > item.MDef) {
                                            item.MDef++;
                                            pet.Status.mdef_add++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.MDef;
                                        }

                                        break;
                                    case 7:
                                        if (pet.Limits.avoid_melee > item.AvoidMelee) {
                                            item.AvoidMelee++;
                                            pet.Status.avoid_melee++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.AvoidMelee;
                                        }

                                        break;
                                    case 8:
                                        if (pet.Limits.avoid_ranged > item.AvoidRanged) {
                                            item.AvoidRanged++;
                                            pet.Status.avoid_ranged++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.AvoidRanged;
                                        }

                                        break;
                                }

                                break;
                            case PetGrowthReason.UseSkill:
                                type = Global.Random.Next(0, 2);
                                switch (type) {
                                    case 0:
                                        if (pet.Limits.matk_max > item.MAtk) {
                                            item.MAtk++;
                                            pet.Status.min_matk++;
                                            pet.Status.max_matk++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.MATK;
                                        }

                                        break;
                                    case 1:
                                        if (pet.Limits.hit_ranged > item.HitMagic) {
                                            item.HitMagic++;
                                            pet.Status.hit_magic++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.HitMagic;
                                        }

                                        break;
                                    case 2:
                                        if (pet.Limits.cspd > item.CSPD) {
                                            item.CSPD++;
                                            pet.Status.cspd++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.CSPD;
                                        }

                                        break;
                                }

                                break;
                            case PetGrowthReason.PhysicalHit:
                                type = Global.Random.Next(0, 3);
                                switch (type) {
                                    case 0:
                                        if (pet.Limits.hp > item.HP) {
                                            item.HP++;
                                            pet.MaxHP++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.HP;
                                        }

                                        break;
                                    case 1:
                                        if (pet.Limits.def_add > item.Def) {
                                            item.Def++;
                                            pet.Status.def_add++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.Def;
                                        }

                                        break;
                                    case 2:
                                        if (pet.Limits.avoid_melee > item.AvoidMelee) {
                                            item.AvoidMelee++;
                                            pet.Status.avoid_melee++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.AvoidMelee;
                                        }

                                        break;
                                    case 3:
                                        if (pet.Limits.avoid_ranged > item.AvoidRanged) {
                                            item.AvoidRanged++;
                                            pet.Status.avoid_ranged++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.AvoidRanged;
                                        }

                                        break;
                                }

                                break;
                            case PetGrowthReason.SkillHit:
                                type = Global.Random.Next(0, 5);
                                switch (type) {
                                    case 0:
                                        if (pet.Limits.hp > item.HP) {
                                            item.HP++;
                                            pet.MaxHP++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.HP;
                                        }

                                        break;
                                    case 1:
                                        if (pet.Limits.mdef_add > item.MDef) {
                                            item.MDef++;
                                            pet.Status.mdef_add++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.MDef;
                                        }

                                        break;
                                    case 2:
                                        if (pet.Limits.mp > item.MPRecover) {
                                            item.MPRecover++;
                                            pet.Status.mp_recover_skill++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.MPRecover;
                                        }

                                        break;
                                    case 3:
                                        if (pet.Limits.avoid_ranged > item.AvoidRanged) {
                                            item.AvoidRanged++;
                                            pet.Status.avoid_ranged++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.AvoidRanged;
                                        }

                                        break;
                                    case 4:
                                        if (pet.Limits.def_add > item.Def) {
                                            item.Def++;
                                            pet.Status.def_add++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.Def;
                                        }

                                        break;
                                    case 5:
                                        if (pet.Limits.avoid_magic > item.AvoidMagic) {
                                            item.AvoidMagic++;
                                            pet.Status.avoid_magic++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.AvoidMagic;
                                        }

                                        break;
                                }

                                break;
                        }
                    }
                    else {
                        int type;
                        switch (reason) {
                            case PetGrowthReason.PhysicalBeenHit:
                                type = Global.Random.Next(0, 14);
                                switch (type) {
                                    case 0:
                                        if (pet.Limits.hp > item.HP) {
                                            item.HP++;
                                            pet.MaxHP++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.HP;
                                        }

                                        break;
                                    case 1:
                                        if (pet.Limits.hit_melee > item.HitMelee) {
                                            item.HitMelee++;
                                            pet.Status.hit_melee++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.HitMelee;
                                        }

                                        break;
                                    case 2:
                                        if (pet.Limits.hit_ranged > item.HitRanged) {
                                            item.HitRanged++;
                                            pet.Status.hit_ranged++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.HitRanged;
                                        }

                                        break;
                                    case 3:
                                        if (pet.Limits.aspd > item.ASPD) {
                                            item.ASPD++;
                                            pet.Status.aspd++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.ASPD;
                                        }

                                        break;
                                    case 4:
                                        if (pet.Limits.matk_max > item.MAtk) {
                                            item.MAtk++;
                                            pet.Status.min_matk++;
                                            pet.Status.max_matk++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.MATK;
                                        }

                                        break;
                                    case 5:
                                        if (pet.Limits.hit_ranged > item.HitMagic) {
                                            item.HitMagic++;
                                            pet.Status.hit_magic++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.HitMagic;
                                        }

                                        break;
                                    case 6:
                                        if (pet.Limits.cspd > item.CSPD) {
                                            item.CSPD++;
                                            pet.Status.cspd++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.CSPD;
                                        }

                                        break;
                                    case 7:
                                        if (pet.Limits.aspd > item.ASPD) {
                                            item.ASPD++;
                                            pet.Status.aspd++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.ASPD;
                                        }

                                        break;
                                    case 8:
                                        if (pet.Limits.atk_max > item.Atk1) {
                                            item.Atk1++;
                                            item.Atk2++;
                                            item.Atk3++;

                                            pet.Status.min_atk1++;
                                            pet.Status.max_atk1++;
                                            pet.Status.min_atk2++;
                                            pet.Status.max_atk2++;
                                            pet.Status.min_atk3++;
                                            pet.Status.max_atk3++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.ATK1;
                                        }

                                        break;
                                    case 9:
                                        if (pet.Limits.cri > item.HitCritical) {
                                            item.HitCritical++;
                                            pet.Status.hit_critical++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.Critical;
                                        }

                                        break;
                                    case 10:
                                        if (pet.Limits.criavd > item.AvoidCritical) {
                                            item.AvoidCritical++;
                                            pet.Status.avoid_critical++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.AvoidCri;
                                        }

                                        break;
                                    case 11:
                                        if (pet.Limits.def_add > item.Def) {
                                            item.Def++;
                                            pet.Status.def_add++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.Def;
                                        }

                                        break;
                                    case 12:
                                        if (pet.Limits.mdef_add > item.MDef) {
                                            item.MDef++;
                                            pet.Status.mdef_add++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.MDef;
                                        }

                                        break;
                                    case 13:
                                        if (pet.Limits.avoid_melee > item.AvoidMelee) {
                                            item.AvoidMelee++;
                                            pet.Status.avoid_melee++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.AvoidMelee;
                                        }

                                        break;
                                    case 14:
                                        if (pet.Limits.avoid_ranged > item.AvoidRanged) {
                                            item.AvoidRanged++;
                                            pet.Status.avoid_ranged++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.AvoidRanged;
                                        }

                                        break;
                                }

                                break;
                            case PetGrowthReason.UseSkill:
                                type = Global.Random.Next(0, 6);
                                switch (type) {
                                    case 0:
                                        if (pet.Limits.hit_ranged > item.HitMagic) {
                                            item.HitMagic++;
                                            pet.Status.hit_magic++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.HitMagic;
                                        }

                                        break;
                                    case 1:
                                        if (pet.Limits.matk_max > item.MAtk) {
                                            item.MAtk++;
                                            pet.Status.min_matk++;
                                            pet.Status.max_matk++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.MATK;
                                        }

                                        break;
                                    case 2:
                                        if (pet.Limits.cspd > item.CSPD) {
                                            item.CSPD++;
                                            pet.Status.cspd++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.CSPD;
                                        }

                                        break;
                                    case 3:
                                        if (pet.Limits.mdef_add > item.MDef) {
                                            item.MDef++;
                                            pet.Status.mdef_add++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.MDef;
                                        }

                                        break;
                                    case 4:
                                        if (pet.Limits.mp > item.MPRecover) {
                                            item.MPRecover++;
                                            pet.Status.mp_recover_skill++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.MPRecover;
                                        }

                                        break;
                                    case 5:
                                        if (pet.Limits.def_add > item.Def) {
                                            item.Def++;
                                            pet.Status.def_add++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.Def;
                                        }

                                        break;
                                    case 6:
                                        if (pet.Limits.avoid_magic > item.AvoidMagic) {
                                            item.AvoidMagic++;
                                            pet.Status.avoid_magic++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.AvoidMagic;
                                        }

                                        break;
                                }

                                break;
                            case PetGrowthReason.ItemRecover:
                                type = Global.Random.Next(0, 2);
                                switch (type) {
                                    case 0:
                                        if (pet.Limits.hp > item.HP) {
                                            item.HP++;
                                            pet.MaxHP++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.HP;
                                        }

                                        break;
                                    case 1:
                                        if (pet.Limits.mp > item.MPRecover) {
                                            item.MPRecover++;
                                            pet.Status.mp_recover_skill++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.MPRecover;
                                        }

                                        break;
                                    case 2:
                                        if (pet.Limits.hp > item.HPRecover) {
                                            item.HPRecover++;
                                            pet.Status.hp_recover_skill++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.Recover;
                                        }

                                        break;
                                }

                                break;
                        }
                    }

                    if (pet.Owner.Online) {
                        if (pet.Ride) {
                            StatusFactory.Instance.CalcStatus(pet.Owner);
                            MapClient.FromActorPC(pet.Owner).SendPlayerInfo();
                        }

                        MapClient.FromActorPC(pet.Owner).SendItemInfo(item);
                    }

                    SendPetGrowth(actor, growType, 1);
                }
            }
        }
    }
}