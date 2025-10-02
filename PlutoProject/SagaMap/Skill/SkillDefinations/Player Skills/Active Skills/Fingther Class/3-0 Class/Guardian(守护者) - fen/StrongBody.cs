using System.Collections.Generic;
using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._3_0_Class.Guardian_守护者____fen
{
    /// <summary>
    ///     ストロングボディ
    /// </summary>
    public class StrongBody : ISkill
    {
        private readonly float[] hp_add = { 0, 0.1f, 0.15f, 0.2f, 0.25f, 0.3f };

        private readonly int[] left_add = { 0, 5, 7, 9, 11, 13 };
        private readonly int[] left_add_another = { 0, 1, 3, 5, 7, 9 };
        private readonly int[] lifetime = { 0, 30000, 40000, 50000, 60000, 70000 };
        private readonly float[] right_add = { 0, 0.02f, 0.04f, 0.06f, 0.08f, 0.1f };
        private Actor me;

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            me = sActor;
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var affected = map.GetActorsArea(sActor, 300, true);
            var realAffected = new List<Actor>();
            var sPC = (ActorPC)sActor;
            if (sPC.Party != null)
            {
                foreach (var act in affected)
                    if (act.type == ActorType.PC)
                    {
                        var aPC = (ActorPC)act;
                        if (aPC.Party != null && sPC.Party != null)
                            if (aPC.Party.ID == sPC.Party.ID && aPC.Party.ID != 0 && !aPC.Buff.Dead &&
                                aPC.PossessionTarget == 0)
                            {
                                if (act.Buff.NoRegen) continue;

                                if (aPC.Party.ID == sPC.Party.ID) realAffected.Add(act);
                            }
                    }
            }
            else
            {
                realAffected.Add(sActor);
            }

            foreach (var act in realAffected)
                if (act.type == ActorType.PC)
                {
                    var skill = new DefaultBuff(args.skill, act, "RustBody", lifetime[level]);
                    skill.OnAdditionStart += StartEventHandler;
                    skill.OnAdditionEnd += EndEventHandler;
                    SkillHandler.ApplyAddition(act, skill);
                }
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            if (me == actor)
            {
                if (skill.Variable.ContainsKey("Rust_LEFT_DEF"))
                    skill.Variable.Remove("Rust_LEFT_DEF");
                skill.Variable.Add("Rust_LEFT_DEF", left_add[skill.skill.Level]);
                actor.Status.def_skill += (short)left_add[skill.skill.Level];
            }
            else
            {
                if (skill.Variable.ContainsKey("Rust_LEFT_DEF"))
                    skill.Variable.Remove("Rust_LEFT_DEF");
                skill.Variable.Add("Rust_LEFT_DEF", left_add_another[skill.skill.Level]);
                actor.Status.def_skill += (short)left_add_another[skill.skill.Level];
            }


            var rust_def_add = (int)(actor.Status.def_add * right_add[skill.skill.Level]);
            if (skill.Variable.ContainsKey("Rust_RIGHT_DEF"))
                skill.Variable.Remove("Rust_RIGHT_DEF");
            skill.Variable.Add("Rust_RIGHT_DEF", rust_def_add);
            actor.Status.def_add_skill += (short)rust_def_add;

            var rust_hp_add = (int)(actor.MaxHP * hp_add[skill.skill.Level]);
            if (skill.Variable.ContainsKey("Rust_MAXHP"))
                skill.Variable.Remove("Rust_MAXHP");
            skill.Variable.Add("Rust_MAXHP", rust_hp_add);
            actor.Status.hp_skill += (short)rust_hp_add;

            actor.Buff.DefUp = true;
            actor.Buff.DefRateUp = true;
            actor.Buff.MaxHPUp = true;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.Status.def_skill -= (short)skill.Variable["Rust_LEFT_DEF"];
            actor.Status.def_add_skill -= (short)skill.Variable["Rust_RIGHT_DEF"];
            actor.Status.hp_skill -= (short)skill.Variable["Rust_MAXHP"];

            actor.Buff.DefUp = false;
            actor.Buff.DefRateUp = false;
            actor.Buff.MaxHPUp = false;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }
    }
}