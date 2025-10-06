using System.Collections.Generic;
using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._2_1_Class.Sorcerer_魔导师____wiz
{
    /// <summary>
    ///     エナジーバリア
    /// </summary>
    public class EnergyBarrier : ISkill
    {
        private readonly bool MobUse;

        public EnergyBarrier()
        {
            MobUse = false;
        }

        public EnergyBarrier(bool MobUse)
        {
            this.MobUse = MobUse;
        }

        //#region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var life = 0;
            if (MobUse) level = 5;
            switch (level)
            {
                case 1:
                    life = 600000;
                    break;
                case 2:
                    life = 500000;
                    break;
                case 3:
                    life = 400000;
                    break;
                case 4:
                    life = 300000;
                    break;
                case 5:
                    life = 200000;
                    break;
            }

            var map = MapManager.Instance.GetMap(sActor.MapID);
            var affected = map.GetActorsArea(sActor, 250, true);
            foreach (var act in affected)
                if (!SkillHandler.Instance.CheckValidAttackTarget(sActor, act))
                {
                    SkillHandler.RemoveAddition(act, "DevineBarrier");
                    var skill = new DefaultBuff(args.skill, act, "EnergyBarrier", life);
                    skill.OnAdditionStart += StartEventHandler;
                    skill.OnAdditionEnd += EndEventHandler;
                    SkillHandler.ApplyAddition(act, skill);
                    var arg2 = new EffectArg();
                    arg2.effectID = 5168;
                    arg2.actorID = act.ActorID;


                    MapManager.Instance.GetMap(act.MapID)
                        .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SHOW_EFFECT, arg2, act, true);
                }
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            int atk1 = 0, atk2 = 0;
            switch (skill.skill.Level)
            {
                case 1:
                    atk1 = 3;
                    atk2 = 5;
                    break;
                case 2:
                    atk1 = 3;
                    atk2 = 10;
                    break;
                case 3:
                    atk1 = 6;
                    atk2 = 10;
                    break;
                case 4:
                    atk1 = 6;
                    atk2 = 15;
                    break;
                case 5:
                    atk1 = 9;
                    atk2 = 15;
                    break;
            }

            if (skill.Variable.ContainsKey("Def"))
                skill.Variable.Remove("Def");
            skill.Variable.Add("Def", atk1);
            actor.Status.def_skill += (short)atk1;
            if (skill.Variable.ContainsKey("DefAdd"))
                skill.Variable.Remove("DefAdd");
            skill.Variable.Add("DefAdd", atk2);
            actor.Status.def_add_skill += (short)atk2;

            actor.Buff.DefUp = true;
            actor.Buff.DefRateUp = true;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            var value = skill.Variable["Def"];
            actor.Status.def_skill -= (short)value;
            value = skill.Variable["DefAdd"];
            actor.Status.def_add_skill -= (short)value;

            actor.Buff.DefUp = false;
            actor.Buff.DefRateUp = false;
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

        //#endregion
    }
}