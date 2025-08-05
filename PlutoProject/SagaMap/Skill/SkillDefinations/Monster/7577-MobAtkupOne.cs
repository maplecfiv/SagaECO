using System.Collections.Generic;
using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Monster
{
    /// <summary>
    ///     應援
    /// </summary>
    public class MobAtkupOne : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 60000;
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var affected = map.GetActorsArea(sActor, 500, false);
            var realAffected = new List<Actor>();
            foreach (var act in affected)
                if (act.type == ActorType.MOB)
                    realAffected.Add(act);

            realAffected.Add(sActor);
            foreach (var i in realAffected)
                if (!i.Status.Additions.ContainsKey("MobAtkupOne"))
                {
                    var skill1 = new DefaultBuff(args.skill, i, "MobAtkupOne", lifetime);
                    skill1.OnAdditionStart += StartEventHandler;
                    skill1.OnAdditionEnd += EndEventHandler;
                    SkillHandler.ApplyAddition(i, skill1);
                }
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            //最大攻擊
            actor.Status.max_atk1_skill += 10;

            //最大攻擊
            actor.Status.max_atk2_skill += 10;

            //最大攻擊
            actor.Status.max_atk3_skill += 10;

            //最小攻擊
            actor.Status.min_atk1_skill += 10;

            //最小攻擊
            actor.Status.min_atk2_skill += 10;

            //最小攻擊
            actor.Status.min_atk3_skill += 10;

            //最大魔攻
            actor.Status.max_matk_skill += 10;
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            //最大攻擊
            actor.Status.max_atk1_skill -= 10;

            //最大攻擊
            actor.Status.max_atk2_skill -= 10;

            //最大攻擊
            actor.Status.max_atk3_skill -= 10;

            //最小攻擊
            actor.Status.min_atk1_skill -= 10;

            //最小攻擊
            actor.Status.min_atk2_skill -= 10;

            //最小攻擊
            actor.Status.min_atk3_skill -= 10;

            //最大魔攻
            actor.Status.max_matk_skill -= 10;
        }

        #endregion
    }
}