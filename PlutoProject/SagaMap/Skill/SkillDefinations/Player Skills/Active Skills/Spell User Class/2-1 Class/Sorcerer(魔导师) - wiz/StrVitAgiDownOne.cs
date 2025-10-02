using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._2_1_Class.Sorcerer_魔导师____wiz
{
    /// <summary>
    ///     弱化（ディビリテイト）
    /// </summary>
    public class StrVitAgiDownOne : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var rate = 10 + 10 * level;
            var lifetime = new[] { 0, 15000, 20000, 25000, 27000, 30000 }[level];
            if (SagaLib.Global.Random.Next(0, 99) < rate)
            {
                var skill = new DefaultBuff(args.skill, dActor, "StrVitAgiDownOne", lifetime);
                skill.OnAdditionStart += StartEventHandler;
                skill.OnAdditionEnd += EndEventHandler;
                SkillHandler.ApplyAddition(dActor, skill);
            }
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            int level = skill.skill.Level;
            if (actor is ActorPC)
            {
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
                actor.Buff.DefDown = true;
            }
            else if (actor is ActorMob)
            {
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
                actor.Buff.DefRateDown = true;
            }

            if (actor is ActorPC)
                MapManager.Instance.GetMap(actor.MapID)
                    .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
            else
                MapManager.Instance.GetMap(actor.MapID)
                    .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, false);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            if (actor is ActorPC)
            {
                //STR
                actor.Status.str_skill += (short)skill.Variable["StrVitAgiDownOne_str"];

                //AGI
                actor.Status.agi_skill += (short)skill.Variable["StrVitAgiDownOne_agi"];

                //VIT
                actor.Status.vit_skill += (short)skill.Variable["StrVitAgiDownOne_vit"];
                actor.Buff.STRDown = false;
                actor.Buff.AGIDown = false;
                actor.Buff.DefDown = false;
            }
            else if (actor is ActorMob)
            {
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
                actor.Buff.DefRateDown = false;
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