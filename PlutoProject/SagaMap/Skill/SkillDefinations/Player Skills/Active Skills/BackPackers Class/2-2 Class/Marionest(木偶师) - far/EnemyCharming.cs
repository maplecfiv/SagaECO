using SagaLib;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._2_2_Class.Marionest_木偶师____far
{
    /// <summary>
    ///     調教馴化（モンスターテイミング）
    /// </summary>
    public class EnemyCharming : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            if (dActor.type == ActorType.PC) return 0;

            if (dActor.type == ActorType.MOB)
            {
                var dActorMob = (ActorMob)dActor;
                if (!SkillHandler.Instance.isBossMob(dActorMob)) return 0;
            }

            return -13;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var rate = 0;
            if (dActor.type == ActorType.PC)
            {
                rate = 10 + 10 * level;
                if (SagaLib.Global.Random.Next(0, 99) < rate)
                {
                    dActor.HP = 0;
                    dActor.e.OnDie();
                    args.affectedActors.Add(dActor);
                    args.Init();
                    args.flag[0] = AttackFlag.DIE;
                }
            }
            else if (dActor.type == ActorType.MOB)
            {
                rate = 40 + 10 * level;
                if (SagaLib.Global.Random.Next(0, 99) < rate)
                {
                    var map = MapManager.Instance.GetMap(dActor.MapID);
                    var dActorMob = (ActorMob)dActor;
                    var MobID = dActorMob.BaseData.id;
                    var x = dActor.X;
                    var y = dActor.Y;
                    map.DeleteActor(dActor);
                    var mob = map.SpawnMob(MobID, x, y, 2500, sActor);
                    var skill = new EnemyCharmingBuff(args.skill, sActor, mob, 600000);
                    SkillHandler.ApplyAddition(sActor, skill);
                }
            }
        }

        public class EnemyCharmingBuff : DefaultBuff
        {
            private readonly ActorMob mob;

            public EnemyCharmingBuff(SagaDB.Skill.Skill skill, Actor actor, ActorMob mob, int lifetime)
                : base(skill, actor, "EnemyCharming", lifetime)
            {
                OnAdditionStart += StartEvent;
                OnAdditionEnd += EndEvent;
                this.mob = mob;
            }

            private void StartEvent(Actor actor, DefaultBuff skill)
            {
            }

            private void EndEvent(Actor actor, DefaultBuff skill)
            {
                var map = MapManager.Instance.GetMap(actor.MapID);
                mob.ClearTaskAddition();
                map.DeleteActor(mob);
            }
        }

        //#endregion
    }
}