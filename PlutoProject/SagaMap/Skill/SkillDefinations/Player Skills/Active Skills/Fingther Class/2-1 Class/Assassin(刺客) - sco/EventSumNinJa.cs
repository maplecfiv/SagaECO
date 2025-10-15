using System;
using SagaDB.Actor;
using SagaLib;
using SagaLib.Tasks;
using SagaMap.ActorEventHandlers;
using SagaMap.Manager;
using SagaMap.Mob;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._2_1_Class.Assassin_刺客____sco
{
    /// <summary>
    ///     分身術（幻視形代）
    /// </summary>
    public class EventSumNinJa : ISkill
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
                    Logger.GetLogger().Error(ex, ex.Message);
                }
                //解开同步锁
                //测试去除技能同步锁ClientManager.LeaveCriticalArea();
            }
        }

        //#endregion

        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var pc = (ActorPC)sActor;
            if (level == 1)
            {
                if (sActor.Slave.Count >= 1)
                {
                    var eh2 = (PetEventHandler)sActor.Slave[0].e;
                    eh2.AI.Pause();
                    eh2.AI.map.DeleteActor(eh2.AI.Mob);
                    sActor.Slave.Remove(eh2.AI.Mob);
                    eh2.AI.Mob.Tasks["Shadow"].Deactivate();
                    eh2.AI.Mob.Tasks.Remove("Shadow");
                }

                var actor = new ActorShadow(pc);
                actor.Name = LocalManager.Instance.Strings.SKILL_DECOY + pc.Name;
                actor.MapID = pc.MapID;
                actor.X = (short)(sActor.X + SagaLib.Global.Random.Next(1, 10));
                actor.Y = (short)(sActor.Y + SagaLib.Global.Random.Next(1, 10));
                var eh = new PetEventHandler(actor);
                actor.e = eh;
                actor.Int = pc.Int >= 10 ? (ushort)(pc.Int - 10) : (ushort)0;
                actor.Str = pc.Str >= 10 ? (ushort)(pc.Str - 10) : (ushort)0;
                actor.Mag = pc.Mag >= 10 ? (ushort)(pc.Mag - 10) : (ushort)0;
                actor.Dex = pc.Dex >= 10 ? (ushort)(pc.Dex - 10) : (ushort)0;
                actor.Agi = pc.Agi >= 10 ? (ushort)(pc.Agi - 10) : (ushort)0;
                actor.Vit = pc.Vit >= 10 ? (ushort)(pc.Vit - 10) : (ushort)0;
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
            else
            {
                if (sActor.Slave.Count >= level - 1)
                {
                    sActor.Slave[0].ClearTaskAddition();
                    map.DeleteActor(sActor.Slave[0]);
                    sActor.Slave.Remove(sActor.Slave[0]);
                }

                var actor = new ActorShadow(pc);
                actor.Name = LocalManager.Instance.Strings.SKILL_DECOY + pc.Name;
                actor.MapID = pc.MapID;
                actor.X = pc.X;
                actor.Y = pc.Y;
                actor.Int = pc.Int >= 10 ? (ushort)(pc.Int - 10) : (ushort)0;
                actor.Str = pc.Str >= 10 ? (ushort)(pc.Str - 10) : (ushort)0;
                actor.Mag = pc.Mag >= 10 ? (ushort)(pc.Mag - 10) : (ushort)0;
                actor.Dex = pc.Dex >= 10 ? (ushort)(pc.Dex - 10) : (ushort)0;
                actor.Agi = pc.Agi >= 10 ? (ushort)(pc.Agi - 10) : (ushort)0;
                actor.Vit = pc.Vit >= 10 ? (ushort)(pc.Vit - 10) : (ushort)0;
                actor.MaxHP = (uint)(pc.MaxHP * (0.5f * level));
                actor.HP = (uint)(pc.HP * (0.5f * level));
                actor.Speed = pc.Speed;
                actor.BaseData.range = 1;
                var eh = new PetEventHandler(actor);
                actor.e = eh;

                eh.AI.Mode = new AIMode(1);
                eh.AI.Master = pc;
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