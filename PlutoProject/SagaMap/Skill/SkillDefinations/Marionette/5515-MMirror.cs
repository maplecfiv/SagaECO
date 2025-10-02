using System.Linq;
using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Marionette
{
    /// <summary>
    ///     小丘比分身
    /// </summary>
    public class MMirror : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 60000;
            var skill = new DefaultBuff(args.skill, dActor, "MMirror", lifetime);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(dActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            var map = MapManager.Instance.GetMap(actor.MapID);
            actor.Slave.Add(map.SpawnMob(26160006, (short)(actor.X + SagaLib.Global.Random.Next(-1, 1)),
                (short)(actor.Y + SagaLib.Global.Random.Next(-1, 1)), 2500, actor));
            actor.Slave.Add(map.SpawnMob(26160006, (short)(actor.X + SagaLib.Global.Random.Next(-1, 1)),
                (short)(actor.Y + SagaLib.Global.Random.Next(-1, 1)), 2500, actor));
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            var map = MapManager.Instance.GetMap(actor.MapID);
            var mobs = from ActorMob m in from Actor a in actor.Slave where a.type == ActorType.MOB select a
                       where m.MobID == 26160006
                       select m;
            foreach (var m in mobs)
            {
                m.ClearTaskAddition();
                map.DeleteActor(m);
            }
        }

        //#endregion
    }
}