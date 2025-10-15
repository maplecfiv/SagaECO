using System;
using SagaDB.Actor;
using SagaLib;
using SagaLib.Tasks;
using SagaMap.ActorEventHandlers;
using SagaMap.Manager;
using SagaMap.Mob;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._1_0_Class.Wizard_魔法师_
{
    public class Decoy : ISkill
    {
        //#region Timer

        private class Activator : MultiRunTask
        {
            private readonly ActorShadow actor;
            private readonly Actor castor;
            private readonly Map map;

            public Activator(Actor castor, ActorShadow actor, int lifetime)
            {
                map = MapManager.Instance.GetMap(actor.MapID);
                Period = lifetime;
                DueTime = lifetime;
                this.actor = actor;
                this.castor = castor;
            }

            public override void CallBack()
            {
                //同步锁，表示之后的代码是线程安全的，也就是，不允许被第二个线程同时访问
                //测试去除技能同步锁ClientManager.EnterCriticalArea();
                try
                {
                    var eh = (PetEventHandler)actor.e;
                    eh.AI.Pause();
                    map.DeleteActor(actor);
                    castor.Slave.Remove(actor);
                    actor.Tasks.Remove("Shadow");
                    Deactivate();
                }
                catch (Exception ex)
                {
                    Logger.getLogger().Error(ex, ex.Message);
                }
                //解开同步锁
                //测试去除技能同步锁ClientManager.LeaveCriticalArea();
            }
        }

        //#endregion

        //#region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            if (sActor.type == ActorType.PC)
            {
                if (sActor.Slave.Count >= 3)
                {
                    var eh2 = (PetEventHandler)sActor.Slave[0].e;
                    eh2.AI.Pause();
                    eh2.AI.map.DeleteActor(eh2.AI.Mob);
                    sActor.Slave.Remove(eh2.AI.Mob);
                    eh2.AI.Mob.Tasks["Shadow"].Deactivate();
                    eh2.AI.Mob.Tasks.Remove("Shadow");
                }

                var pc = (ActorPC)sActor;
                var actor = new ActorShadow(pc);
                var map = MapManager.Instance.GetMap(pc.MapID);
                actor.Name = LocalManager.Instance.Strings.SKILL_DECOY + pc.Name;
                actor.MapID = pc.MapID;
                actor.X = SagaLib.Global.PosX8to16(args.x, map.Width);
                actor.Y = SagaLib.Global.PosY8to16(args.y, map.Height);
                actor.PictID = 19010032;
                var eh = new PetEventHandler(actor);
                actor.e = eh;
                eh.AI.Mode = new AIMode(0);
                map.RegisterActor(actor);
                actor.invisble = false;
                map.OnActorVisibilityChange(actor);
                map.SendVisibleActorsToActor(actor);
                eh.AI.Start();
                var task = new Activator(sActor, actor, level * 10000);
                actor.Tasks.Add("Shadow", task);
                task.Activate();
                sActor.Slave.Add(actor);
            }
        }

        //#endregion
    }
}