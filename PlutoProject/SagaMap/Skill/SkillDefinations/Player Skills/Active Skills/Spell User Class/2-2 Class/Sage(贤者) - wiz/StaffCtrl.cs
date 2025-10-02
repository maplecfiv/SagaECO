using SagaDB.Actor;
using SagaMap.ActorEventHandlers;
using SagaMap.Manager;
using SagaMap.Mob;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._2_2_Class.Sage_贤者____wiz
{
    /// <summary>
    ///     活化杖（リビングスタッフ）
    /// </summary>
    public class StaffCtrl : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 20000 * level;
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var mob = map.SpawnMob(26330000, SagaLib.Global.PosX8to16(args.x, map.Width)
                , SagaLib.Global.PosY8to16(args.y, map.Height)
                , 2500, sActor);

            //mob.type = ActorType.ANOTHERMOB;
            mob.type = ActorType.ANOTHERMOB;
            mob.Owner = sActor;
            var eE = (MobEventHandler)mob.e;
            eE.AI.Master = sActor;
            //修复
            eE.AI.Mode = new AIMode(1);
            eE.AI.Mode.EventAttacking.Add(3281, 100); //魔法衝擊波
            eE.AI.Mode.EventAttackingSkillRate = 100;
            var skill = new StaffCtrlBuff(mob, args.skill, sActor, lifetime);
            SkillHandler.ApplyAddition(sActor, skill);
        }

        public class StaffCtrlBuff : DefaultBuff
        {
            private readonly Actor mob;

            public StaffCtrlBuff(Actor mob, SagaDB.Skill.Skill skill, Actor actor, int lifetime)
                : base(skill, actor, "StaffCtrl", lifetime)
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
                if (mob != null)
                {
                    mob.ClearTaskAddition();
                    map.DeleteActor(mob);
                }
            }
        }

        //#endregion
    }
}