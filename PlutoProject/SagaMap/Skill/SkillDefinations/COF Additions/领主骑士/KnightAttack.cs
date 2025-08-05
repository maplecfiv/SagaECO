using System.Collections.Generic;
using SagaDB.Actor;
using SagaLib;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.COF_Additions.领主骑士
{
    public class KnightAttack : MobISkill
    {
        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            int value = actor.Status.def;
            if (skill.Variable.ContainsKey("KnightAttackDownDef"))
                skill.Variable.Remove("KnightAttackDownDef");
            skill.Variable.Add("KnightAttackDownDef", value);
            actor.Status.def_add_skill -= (short)value;
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.Status.def_add_skill += (short)skill.Variable["KnightAttackDownDef"];
        }

        #region ISkill Members

        public void BeforeCast(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var actors = map.GetActorsArea(dActor, 100, true, true);
            var realAffected = new List<Actor>();
            foreach (var act in actors)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, act))
                {
                    realAffected.Add(act);
                    if (act.Status.Additions.ContainsKey("Parry") && SagaLib.Global.Random.Next(0, 10) < 5)
                    {
                        act.Status.Additions.Remove("Parry");
                        var skill = new DefaultBuff(args.skill, act, "KnightAttack", 5000);
                        skill.OnAdditionStart += StartEventHandler;
                        skill.OnAdditionEnd += EndEventHandler;
                        SkillHandler.ApplyAddition(act, skill);
                    }
                }

            if (SagaLib.Global.Random.Next(0, 100) < 8)
            {
                var HpRate = 0.005f * realAffected.Count;
                var hpadd = (int)(sActor.MaxHP * HpRate);

                SkillHandler.Instance.ShowVessel(sActor, -hpadd);

                var arg2 = new SkillArg();
                arg2 = args.Clone();
                SkillHandler.Instance.FixAttack(sActor, sActor, arg2, Elements.Holy, -hpadd);
            }

            SkillHandler.Instance.PhysicalAttack(sActor, realAffected, args, Elements.Neutral, 1f);
        }

        #endregion
    }
}