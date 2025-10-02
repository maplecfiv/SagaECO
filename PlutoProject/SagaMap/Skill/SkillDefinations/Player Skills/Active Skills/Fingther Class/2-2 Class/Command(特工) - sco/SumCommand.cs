using System.Collections.Generic;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._2_2_Class.Command_特工____sco
{
    /// <summary>
    ///     應援要請（応援要請）
    /// </summary>
    public class SumCommand : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 10000;
            var sumMob = new List<Actor>();
            var sumMobID = new List<uint>();
            switch (level)
            {
                case 1:
                    sumMobID.Add(60023400);
                    break;
                case 2:
                    sumMobID.Add(60091550);
                    break;
                case 3:
                    sumMobID.Add(60023400);
                    sumMobID.Add(60091550);
                    break;
            }

            var map = MapManager.Instance.GetMap(sActor.MapID);
            foreach (var i in sumMobID)
                sumMob.Add(map.SpawnMob(i, (short)(sActor.X + SagaLib.Global.Random.Next(-100, 100)),
                    (short)(sActor.Y + SagaLib.Global.Random.Next(-100, 100)), 2500, sActor));
            var skill = new SumCommandBuff(args.skill, sActor, sumMob, lifetime);
            SkillHandler.ApplyAddition(dActor, skill);
        }

        public class SumCommandBuff : DefaultBuff
        {
            private readonly List<Actor> sumMob;

            public SumCommandBuff(SagaDB.Skill.Skill skill, Actor actor, List<Actor> sumMob, int lifetime)
                : base(skill, actor, "SumCommand", lifetime)
            {
                OnAdditionStart += StartEvent;
                OnAdditionEnd += EndEvent;
                this.sumMob = sumMob;
            }

            private void StartEvent(Actor actor, DefaultBuff skill)
            {
            }

            private void EndEvent(Actor actor, DefaultBuff skill)
            {
                var map = MapManager.Instance.GetMap(actor.MapID);
                foreach (var act in sumMob)
                {
                    act.ClearTaskAddition();
                    map.DeleteActor(act);
                }
            }
        }

        //#endregion
    }
}