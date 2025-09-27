using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._2_1_Class.Sorcerer_魔导师____wiz
{
    /// <summary>
    ///     神聖光界（ディバインバリア）
    /// </summary>
    public class DevineBarrier : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 110000 + 15000 * level;
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var affected = map.GetActorsArea(sActor, 100, true);

            foreach (var act in affected)
                if (act.type == ActorType.PC || act.type == ActorType.PARTNER || act.type == ActorType.PET)
                {
                    if (act.type == ActorType.PC)
                    {
                        var pc = (ActorPC)act;
                        if (pc.PossessionTarget > 0) continue;
                    }

                    var skill = new DefaultBuff(args.skill, act, "DevineBarrier", lifetime);
                    skill.OnAdditionStart += StartEventHandler;
                    skill.OnAdditionEnd += EndEventHandler;
                    SkillHandler.ApplyAddition(act, skill);
                    var arg2 = new EffectArg();
                    arg2.effectID = 4019;
                    arg2.actorID = act.ActorID;


                    MapManager.Instance.GetMap(act.MapID)
                        .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SHOW_EFFECT, arg2, act, true);
                }
            //sActor. = 4019;
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            short LDef = 0, RDef = 0, LMDef = 0, RMDef = 0;
            int level = skill.skill.Level;
            switch (level)
            {
                case 1:
                    LDef = 5;
                    RDef = 5;
                    LMDef = 5;
                    RMDef = 10;
                    break;
                case 2:
                    LDef = 7;
                    RDef = 10;
                    LMDef = 7;
                    RMDef = 15;
                    break;
                case 3:
                    LDef = 9;
                    RDef = 10;
                    LMDef = 12;
                    RMDef = 15;
                    break;
                case 4:
                    LDef = 11;
                    RDef = 15;
                    LMDef = 15;
                    RMDef = 20;
                    break;
                case 5:
                    LDef = 15;
                    RDef = 20;
                    LMDef = 25;
                    RMDef = 20;
                    break;
            }

            if (actor.Status.Additions.ContainsKey("SoulOfEarth")) actor.Status.Additions["SoulOfEarth"].AdditionEnd();
            if (actor.Status.Additions.ContainsKey("SoulOfWater")) actor.Status.Additions["SoulOfWater"].AdditionEnd();
            if (actor.Status.Additions.ContainsKey("MagicBarrier"))
                actor.Status.Additions["MagicBarrier"].AdditionEnd();
            if (actor.Status.Additions.ContainsKey("EnergyBarrier"))
                actor.Status.Additions["EnergyBarrier"].AdditionEnd();


            //RemoveAddition(actor, "SoulOfEarth");
            //RemoveAddition(actor, "SoulOfWater");
            //RemoveAddition(actor, "MagicBarrier");
            //RemoveAddition(actor, "EnergyBarrier");
            //左防
            if (skill.Variable.ContainsKey("DevineBarrier_LDef"))
                skill.Variable.Remove("DevineBarrier_LDef");
            skill.Variable.Add("DevineBarrier_LDef", LDef);
            //右防
            if (skill.Variable.ContainsKey("DevineBarrier_RDef"))
                skill.Variable.Remove("DevineBarrier_RDef");
            skill.Variable.Add("DevineBarrier_RDef", RDef);
            //左魔防
            if (skill.Variable.ContainsKey("DevineBarrier_LMDef"))
                skill.Variable.Remove("DevineBarrier_LMDef");
            skill.Variable.Add("DevineBarrier_LMDef", LMDef);
            //右魔防
            if (skill.Variable.ContainsKey("DevineBarrier_RMDef"))
                skill.Variable.Remove("DevineBarrier_RMDef");
            skill.Variable.Add("DevineBarrier_RMDef", RMDef);

            //左防
            actor.Status.def_skill += LDef;
            //右防
            actor.Status.def_add_skill += RDef;
            //左魔防
            actor.Status.mdef_skill += LMDef;
            //右魔防
            actor.Status.mdef_add_skill += RMDef;

            actor.Buff.DefUp = true;
            actor.Buff.DefRateUp = true;
            actor.Buff.MagicDefUp = true;
            actor.Buff.MagicDefRateUp = true;

            var arg = new EffectArg();
            arg.effectID = 4019;
            arg.actorID = actor.ActorID;


            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, arg, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            //左防
            actor.Status.def_skill -= (short)skill.Variable["DevineBarrier_LDef"];
            //右防
            actor.Status.def_add_skill -= (short)skill.Variable["DevineBarrier_RDef"];
            //左魔防
            actor.Status.mdef_skill -= (short)skill.Variable["DevineBarrier_LMDef"];
            //右魔防
            actor.Status.mdef_add_skill -= (short)skill.Variable["DevineBarrier_RMDef"];
            actor.Buff.DefUp = false;
            actor.Buff.DefRateUp = false;
            actor.Buff.MagicDefUp = false;
            actor.Buff.MagicDefRateUp = false;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        public void RemoveAddition(Actor actor, string additionName)
        {
            if (actor.Status.Additions.ContainsKey(additionName))
            {
                var addition = actor.Status.Additions[additionName];
                actor.Status.Additions.Remove(additionName);
                if (addition.Activated) addition.AdditionEnd();
                addition.Activated = false;
            }
        }

        #endregion
    }
}