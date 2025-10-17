using System;
using System.Collections.Generic;
using System.Linq;
using SagaDB.Actor;
using SagaLib;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._3_0_Class.
    ForceMaster_原力导师____wiz {
    /// <summary>
    ///     フリークブラスト
    /// </summary>
    public class ThunderSpray : ISkill {
        //#region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args) {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level) {
            var factor = 2.0f + 1.0f * level;
            int[] lifetime = { 0, 4000, 6000, 8000, 10000, 12000 };
            if (sActor.type == ActorType.PC) {
                var pc = (ActorPC)sActor;
                if (pc.Skills2_2.ContainsKey(2330) || pc.DualJobSkills.Exists(x => x.ID == 2330)) factor += 0.7f;
            }

            var map = MapManager.Instance.GetMap(sActor.MapID);
            var actors = map.GetActorsArea(dActor, 200, true);
            var affected = new List<Actor>();
            foreach (var i in actors)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, i)) {
                    affected.Add(i);
                    if (!SkillHandler.Instance.isBossMob(i))
                        if (sActor.type == ActorType.PC) {
                            var pc = (ActorPC)sActor;
                            if (pc.Skills.ContainsKey(3135) || pc.DualJobSkills.Exists(x => x.ID == 3135)) //剧毒诅咒
                            {
                                var duallv = 0;
                                if (pc.DualJobSkills.Exists(x => x.ID == 3135))
                                    duallv = pc.DualJobSkills.FirstOrDefault(x => x.ID == 3135).Level;

                                var mainlv = 0;
                                if (pc.Skills.ContainsKey(3135))
                                    mainlv = pc.Skills[3135].Level;

                                var maxlv = Math.Max(duallv, mainlv);
                                if (SkillHandler.Instance.CanAdditionApply(sActor, i,
                                        SkillHandler.DefaultAdditions.Poison, 15 + maxlv * 15)) {
                                    var skill = new Poison(args.skill, i, 2000 + level * 2000);
                                    SkillHandler.ApplyAddition(i, skill);
                                }
                            }

                            if (pc.Skills.ContainsKey(3136) || pc.DualJobSkills.Exists(x => x.ID == 3136)) //石化诅咒
                            {
                                var duallv = 0;
                                if (pc.DualJobSkills.Exists(x => x.ID == 3136))
                                    duallv = pc.DualJobSkills.FirstOrDefault(x => x.ID == 3136).Level;

                                var mainlv = 0;
                                if (pc.Skills.ContainsKey(3136))
                                    mainlv = pc.Skills[3136].Level;

                                var maxlv = Math.Max(duallv, mainlv);
                                if (SkillHandler.Instance.CanAdditionApply(sActor, i,
                                        SkillHandler.DefaultAdditions.Stone, 15 + maxlv * 15)) {
                                    var skill = new Stone(args.skill, i, 2000 + level * 2000);
                                    SkillHandler.ApplyAddition(i, skill);
                                }
                            }

                            if (pc.Skills.ContainsKey(3139) || pc.DualJobSkills.Exists(x => x.ID == 3139)) //沉默诅咒
                            {
                                var duallv = 0;
                                if (pc.DualJobSkills.Exists(x => x.ID == 3139))
                                    duallv = pc.DualJobSkills.FirstOrDefault(x => x.ID == 3139).Level;

                                var mainlv = 0;
                                if (pc.Skills.ContainsKey(3139))
                                    mainlv = pc.Skills[3139].Level;

                                var maxlv = Math.Max(duallv, mainlv);
                                if (SkillHandler.Instance.CanAdditionApply(sActor, i,
                                        SkillHandler.DefaultAdditions.Silence, 15 + maxlv * 15)) {
                                    var skill = new Silence(args.skill, i, 2000 + level * 2000);
                                    SkillHandler.ApplyAddition(i, skill);
                                }
                            }
                        }

                    if (sActor.type == ActorType.PC) {
                        var pc = (ActorPC)sActor;
                        if (pc.Skills2_1.ContainsKey(3255) || pc.DualJobSkills.Exists(x => x.ID == 3255)) {
                            var StrVitAgiDownOne = new DefaultBuff(args.skill, i, "StrVitAgiDownOne", lifetime[level]);
                            StrVitAgiDownOne.OnAdditionStart += StartEventHandler;
                            StrVitAgiDownOne.OnAdditionEnd += EndEventHandler;
                            SkillHandler.ApplyAddition(i, StrVitAgiDownOne);
                        }

                        if (pc.Skills2_1.ContainsKey(3256) || pc.DualJobSkills.Exists(x => x.ID == 3256)) {
                            var MagIntDexDownOne = new DefaultBuff(args.skill, i, "MagIntDexDownOne", lifetime[level]);
                            MagIntDexDownOne.OnAdditionStart += StartEventHandler2;
                            MagIntDexDownOne.OnAdditionEnd += EndEventHandler2;
                            SkillHandler.ApplyAddition(i, MagIntDexDownOne);
                        }
                    }
                }

            SkillHandler.Instance.MagicAttack(sActor, affected, args, Elements.Neutral, factor);
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill) {
            int level = skill.skill.Level;
            if (actor is ActorPC) {
                //STR
                var str_add = new[] { 0, 5, 6, 7, 8, 10 }[level];
                if (skill.Variable.ContainsKey("StrVitAgiDownOne_str"))
                    skill.Variable.Remove("StrVitAgiDownOne_str");
                skill.Variable.Add("StrVitAgiDownOne_str", str_add);
                actor.Status.str_skill -= (short)str_add;

                //AGI
                var agi_add = new[] { 0, 9, 12, 14, 16, 18 }[level];
                if (skill.Variable.ContainsKey("StrVitAgiDownOne_agi"))
                    skill.Variable.Remove("StrVitAgiDownOne_agi");
                skill.Variable.Add("StrVitAgiDownOne_agi", agi_add);
                actor.Status.agi_skill -= (short)agi_add;

                //VIT
                var vit_add = new[] { 0, 6, 7, 8, 11, 12 }[level];
                if (skill.Variable.ContainsKey("StrVitAgiDownOne_vit"))
                    skill.Variable.Remove("StrVitAgiDownOne_vit");
                skill.Variable.Add("StrVitAgiDownOne_vit", vit_add);
                actor.Status.vit_skill -= (short)vit_add;
                actor.Buff.STRDown = true;
                actor.Buff.AGIDown = true;
                actor.Buff.VITDown = true;
            }
            else if (actor is ActorMob) {
                var min_atk1_add = (int)(actor.Status.min_atk1 * (0.1f + 0.04f * level));
                var min_atk2_add = (int)(actor.Status.min_atk2 * (0.1f + 0.04f * level));
                var min_atk3_add = (int)(actor.Status.min_atk3 * (0.1f + 0.04f * level));
                var max_atk1_add = (int)(actor.Status.max_atk1 * (0.1f + 0.04f * level));
                var max_atk2_add = (int)(actor.Status.max_atk2 * (0.1f + 0.04f * level));
                var max_atk3_add = (int)(actor.Status.max_atk3 * (0.1f + 0.04f * level));
                var savoid_add = (int)(actor.Status.avoid_melee * (0.1f + 0.04f * level));
                var def_add = 10 + 4 * level;

                if (skill.Variable.ContainsKey("StrVitAgiDownOne_minatk1"))
                    skill.Variable.Remove("StrVitAgiDownOne_minatk1");
                skill.Variable.Add("StrVitAgiDownOne_minatk1", min_atk1_add);

                if (skill.Variable.ContainsKey("StrVitAgiDownOne_minatk2"))
                    skill.Variable.Remove("StrVitAgiDownOne_minatk2");
                skill.Variable.Add("StrVitAgiDownOne_minatk2", min_atk2_add);

                if (skill.Variable.ContainsKey("StrVitAgiDownOne_minatk3"))
                    skill.Variable.Remove("StrVitAgiDownOne_minatk3");
                skill.Variable.Add("StrVitAgiDownOne_minatk3", min_atk3_add);

                if (skill.Variable.ContainsKey("StrVitAgiDownOne_maxatk1"))
                    skill.Variable.Remove("StrVitAgiDownOne_maxatk1");
                skill.Variable.Add("StrVitAgiDownOne_maxatk1", max_atk1_add);

                if (skill.Variable.ContainsKey("StrVitAgiDownOne_maxatk2"))
                    skill.Variable.Remove("StrVitAgiDownOne_maxatk2");
                skill.Variable.Add("StrVitAgiDownOne_maxatk2", max_atk2_add);

                if (skill.Variable.ContainsKey("StrVitAgiDownOne_maxatk3"))
                    skill.Variable.Remove("StrVitAgiDownOne_maxatk3");
                skill.Variable.Add("StrVitAgiDownOne_maxatk3", max_atk3_add);

                if (skill.Variable.ContainsKey("StrVitAgiDownOne_savoid"))
                    skill.Variable.Remove("StrVitAgiDownOne_savoid");
                skill.Variable.Add("StrVitAgiDownOne_savoid", savoid_add);

                if (skill.Variable.ContainsKey("StrVitAgiDownOne_def"))
                    skill.Variable.Remove("StrVitAgiDownOne_def");
                skill.Variable.Add("StrVitAgiDownOne_def", def_add);

                actor.Status.min_atk1_skill -= (short)min_atk1_add;
                actor.Status.min_atk2_skill -= (short)min_atk2_add;
                actor.Status.min_atk3_skill -= (short)min_atk3_add;
                actor.Status.max_atk1_skill -= (short)max_atk1_add;
                actor.Status.max_atk2_skill -= (short)max_atk2_add;
                actor.Status.max_atk3_skill -= (short)max_atk3_add;
                actor.Status.avoid_melee_skill -= (short)savoid_add;
                actor.Status.def_skill -= (short)def_add;

                actor.Buff.MinAtkDown = true;
                actor.Buff.MaxAtkDown = true;
                actor.Buff.ShortDodgeDown = true;
                actor.Buff.DefDown = true;
            }

            if (actor is ActorPC)
                MapManager.Instance.GetMap(actor.MapID)
                    .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
            else
                MapManager.Instance.GetMap(actor.MapID)
                    .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, false);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill) {
            if (actor is ActorPC) {
                //STR
                actor.Status.str_skill += (short)skill.Variable["StrVitAgiDownOne_str"];

                //AGI
                actor.Status.agi_skill += (short)skill.Variable["StrVitAgiDownOne_agi"];

                //VIT
                actor.Status.vit_skill += (short)skill.Variable["StrVitAgiDownOne_vit"];
                actor.Buff.STRDown = false;
                actor.Buff.AGIDown = false;
                actor.Buff.VITDown = false;
            }
            else if (actor is ActorMob) {
                actor.Status.min_atk1_skill += (short)skill.Variable["StrVitAgiDownOne_minatk1"];
                actor.Status.min_atk2_skill += (short)skill.Variable["StrVitAgiDownOne_minatk2"];
                actor.Status.min_atk3_skill += (short)skill.Variable["StrVitAgiDownOne_minatk3"];
                actor.Status.max_atk1_skill += (short)skill.Variable["StrVitAgiDownOne_maxatk1"];
                actor.Status.max_atk2_skill += (short)skill.Variable["StrVitAgiDownOne_maxatk2"];
                actor.Status.max_atk3_skill += (short)skill.Variable["StrVitAgiDownOne_maxatk3"];
                actor.Status.avoid_melee_skill += (short)skill.Variable["StrVitAgiDownOne_savoid"];
                actor.Status.def_skill += (short)skill.Variable["StrVitAgiDownOne_def"];
                actor.Buff.MinAtkDown = false;
                actor.Buff.MaxAtkDown = false;
                actor.Buff.ShortDodgeDown = false;
                actor.Buff.DefDown = false;
            }

            if (actor is ActorPC)
                MapManager.Instance.GetMap(actor.MapID)
                    .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
            else
                MapManager.Instance.GetMap(actor.MapID)
                    .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, false);
        }


        private void StartEventHandler2(Actor actor, DefaultBuff skill) {
            int level = skill.skill.Level;
            if (actor is ActorPC) {
                //INT
                var int_add = new[] { 0, 6, 7, 9, 11, 12 }[level] * -1;
                if (skill.Variable.ContainsKey("MagIntDexDownOne_int"))
                    skill.Variable.Remove("MagIntDexDownOne_int");
                skill.Variable.Add("MagIntDexDownOne_int", int_add);
                actor.Status.int_skill -= (short)int_add;

                //MAG
                var mag_add = new[] { 0, 6, 7, 9, 11, 12 }[level] * -1;
                if (skill.Variable.ContainsKey("MagIntDexDownOne_mag"))
                    skill.Variable.Remove("MagIntDexDownOne_mag");
                skill.Variable.Add("MagIntDexDownOne_mag", mag_add);
                actor.Status.mag_skill -= (short)mag_add;

                //DEX
                var dex_add = -(6 + level * 2);
                if (skill.Variable.ContainsKey("MagIntDexDownOne_dex"))
                    skill.Variable.Remove("MagIntDexDownOne_dex");
                skill.Variable.Add("MagIntDexDownOne_dex", dex_add);
                actor.Status.dex_skill -= (short)dex_add;
                actor.Buff.INTDown = true;
                actor.Buff.DEXDown = true;
                actor.Buff.MAGDown = true;
            }
            else if (actor is ActorMob) {
                var max_matk_add = (int)(actor.Status.max_matk * (0.10f + 0.04f * level));
                var min_matk_add = (int)(actor.Status.min_matk * (0.10f + 0.04f * level));
                var magic_reduce = (int)((0.10f + 0.04f * level) * 100.0f);
                var mdef_add = 10 + 4 * level;

                if (skill.Variable.ContainsKey("MagIntDexDownOne_MinMatk"))
                    skill.Variable.Remove("MagIntDexDownOne_MinMatk");
                skill.Variable.Add("MagIntDexDownOne_MinMatk", min_matk_add);
                actor.Status.min_matk_skill -= (short)min_matk_add;

                if (skill.Variable.ContainsKey("MagIntDexDownOne_MaxMatk"))
                    skill.Variable.Remove("MagIntDexDownOne_MaxMatk");
                skill.Variable.Add("MagIntDexDownOne_MaxMatk", max_matk_add);
                actor.Status.max_matk_skill -= (short)max_matk_add;

                if (skill.Variable.ContainsKey("MagIntDexDownOne_MagicReduce"))
                    skill.Variable.Remove("MagIntDexDownOne_MagicReduce");
                skill.Variable.Add("MagIntDexDownOne_MagicReduce", magic_reduce);
                actor.Status.MagicRuduceRate -= magic_reduce;

                if (skill.Variable.ContainsKey("MagIntDexDownOne_MDef"))
                    skill.Variable.Remove("MagIntDexDownOne_MDef");
                skill.Variable.Add("MagIntDexDownOne_MDef", mdef_add);
                actor.Status.mdef_skill -= (short)mdef_add;

                actor.Buff.MinMagicAtkDown = true;
                actor.Buff.MaxMagicAtkDown = true;
                actor.Buff.MagicDefRateDown = true;
                actor.Buff.MagicDefDown = true;
            }

            if (actor is ActorPC)
                MapManager.Instance.GetMap(actor.MapID)
                    .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
            else
                MapManager.Instance.GetMap(actor.MapID)
                    .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, false);
        }

        private void EndEventHandler2(Actor actor, DefaultBuff skill) {
            if (actor is ActorPC) {
                //INT
                actor.Status.int_skill += (short)skill.Variable["MagIntDexDownOne_int"];

                //MAG
                actor.Status.mag_skill += (short)skill.Variable["MagIntDexDownOne_mag"];

                //DEX
                actor.Status.dex_skill += (short)skill.Variable["MagIntDexDownOne_dex"];
                actor.Buff.INTDown = false;
                actor.Buff.DEXDown = false;
                actor.Buff.MAGDown = false;
            }
            else if (actor is ActorMob) {
                actor.Status.min_matk_skill += (short)skill.Variable["MagIntDexDownOne_MinMatk"];
                actor.Status.max_matk_skill += (short)skill.Variable["MagIntDexDownOne_MaxMatk"];
                actor.Status.MagicRuduceRate += skill.Variable["MagIntDexDownOne_MagicReduce"];
                actor.Status.mdef_skill += (short)skill.Variable["MagIntDexDownOne_MDef"];
                actor.Buff.MinMagicAtkDown = false;
                actor.Buff.MaxMagicAtkDown = false;
                actor.Buff.MagicDefRateDown = false;
                actor.Buff.MagicDefDown = false;
            }

            if (actor is ActorPC)
                MapManager.Instance.GetMap(actor.MapID)
                    .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
            else
                MapManager.Instance.GetMap(actor.MapID)
                    .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, false);
        }

        //#endregion
    }
}