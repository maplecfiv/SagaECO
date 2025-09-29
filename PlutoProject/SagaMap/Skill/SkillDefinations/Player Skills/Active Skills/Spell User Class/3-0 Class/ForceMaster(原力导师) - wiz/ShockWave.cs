using System;
using SagaDB.Actor;
using SagaLib;
using SagaLib.Tasks;
using SagaMap.ActorEventHandlers;
using SagaMap.Manager;
using SagaMap.Skill.SkillDefinations.Global.Active;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._3_0_Class.ForceMaster_原力导师____wiz
{
    internal class ShockWave : Groove, ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var actor = new ActorSkill(args.skill, sActor);
            var map = MapManager.Instance.GetMap(sActor.MapID);
            //设定技能体位置            
            actor.MapID = sActor.MapID;
            var vx = (short)SagaLib.Global.Random.Next(-5, 5);
            var vy = (short)SagaLib.Global.Random.Next(-5, 5);
            actor.X = (short)(sActor.X + vx);
            actor.Y = (short)(sActor.Y + vy);
            //actor.Speed = 1000;
            //设定技能体的事件处理器，由于技能体不需要得到消息广播，因此创建个空处理器
            actor.e = new NullEventHandler();
            //在指定地图注册技能体Actor
            map.RegisterActor(actor);
            //设置Actor隐身属性为非
            actor.invisble = true;
            //广播隐身属性改变事件，以便让玩家看到技能体
            map.OnActorVisibilityChange(actor);
            var timer = new ActivatorA(actor, dActor, sActor, args, level);
            timer.Activate(); //Call ActivatorA.CallBack 500ms later.
            //创建技能效果处理对象
        }

        #endregion
    }

    internal class ActivatorA : MultiRunTask
    {
        private readonly Actor AimActor;
        private readonly SkillArg Arg;
        private readonly int countMax = 3;
        private readonly float factor = 1;
        private readonly Map map;
        private readonly Actor sActor;
        private readonly ActorSkill SkillBody;
        private readonly SkillArg SkillFireBolt = new SkillArg();
        private int count;
        private bool stop = false;

        public ActivatorA(ActorSkill actor, Actor dActor, Actor sActor, SkillArg args, byte level)
        {
            dueTime = 100;
            period = 1000;
            AimActor = dActor;
            Arg = args;
            SkillBody = actor;
            this.sActor = sActor;
            map = MapManager.Instance.GetMap(AimActor.MapID);
            var Me = (ActorPC)sActor; //Get the total skill level of skill with fire element.
            switch (level)
            {
                case 1:
                    factor = 0.25f;
                    countMax = 4;
                    period = 1000;
                    break;
                case 2:
                    factor = 0.5f;
                    countMax = 6;
                    period = 800;
                    break;
                case 3:
                    factor = 0.75f;
                    countMax = 10;
                    period = 700;
                    break;
                case 4:
                    factor = 1.0f;
                    countMax = 12;
                    period = 400;
                    break;
                case 5:
                    factor = 1.25f;
                    countMax = 16;
                    period = 100;
                    break;
            }
        }

        public override void CallBack()
        {
            //测试去除技能同步锁
            //ClientManager.EnterCriticalArea();
            var DistanceA = Map.Distance(SkillBody, AimActor);
            var Diss = new short[] { 550, 650, 750, 850, 950 };
            if (count <= countMax)
            {
                if (DistanceA <= Diss[Arg.skill.Level - 1]) //If mob is out the range that FireBolt can cast, skip out.
                {
                    var actor = new ActorSkill(Arg.skill, SkillBody);
                    var map = MapManager.Instance.GetMap(SkillBody.MapID);
                    //设定技能体位置            
                    actor.MapID = SkillBody.MapID;
                    var vx = (short)SagaLib.Global.Random.Next(-200, 200);
                    var vy = (short)SagaLib.Global.Random.Next(-200, 200);
                    actor.X = (short)(SkillBody.X + vx);
                    actor.Y = (short)(SkillBody.Y + vy);
                    actor.Speed = 1000;
                    //设定技能体的事件处理器，由于技能体不需要得到消息广播，因此创建个空处理器
                    actor.e = new NullEventHandler();
                    //在指定地图注册技能体Actor
                    map.RegisterActor(actor);
                    //设置Actor隐身属性为非
                    actor.invisble = false;
                    //广播隐身属性改变事件，以便让玩家看到技能体
                    map.OnActorVisibilityChange(actor);
                    var timers = new ActivatorC(AimActor, actor, Arg, Arg.skill.Level);
                    timers.Activate();
                    var pos2 = new short[2];
                    pos2[0] = AimActor.X;
                    pos2[1] = AimActor.Y;
                    map.MoveActor(Map.MOVE_TYPE.START, actor, pos2, 0, 1000, true, MoveType.BATTLE_MOTION);
                    if (AimActor.type == ActorType.MOB || AimActor.type == ActorType.PET ||
                        AimActor.type == ActorType.SHADOW)
                    {
                        var mob = (MobEventHandler)AimActor.e;
                        mob.AI.OnPathInterupt();
                    }

                    Arg.affectedActors.Clear();
                    //SkillHandler.Instance.MagicAttack(sActor, AimActor, Arg, Elements.Neutral, factor);
                    var dmg = SkillHandler.Instance.CalcDamage(false, sActor, AimActor, Arg, SkillHandler.DefType.Def,
                        Elements.Neutral, 0, factor);
                    //SkillHandler.Instance.ApplyDamage(sActor, AimActor, dmg);
                    SkillHandler.Instance.CauseDamage(sActor, AimActor, dmg);
                    SkillHandler.Instance.ShowVessel(AimActor, dmg);
                    map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SKILL, Arg, actor, true);
                    var arg = new EffectArg();
                    arg.effectID = 4353;
                    arg.actorID = AimActor.ActorID;
                    map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SHOW_EFFECT, arg, AimActor, true);
                    if (SkillFireBolt.flag.Contains(AttackFlag.DIE | AttackFlag.HP_DAMAGE |
                                                    AttackFlag.ATTACK_EFFECT)) //If mob died,terminate the proccess.
                    {
                        map.DeleteActor(actor);
                        Deactivate();
                    }
                }

                count++;
                var affected = map.GetActorsArea(AimActor, 50, false);
            }
            else
            {
                map.DeleteActor(SkillBody);
                Deactivate();
            }
            //测试去除技能同步锁ClientManager.LeaveCriticalArea();
        }

        private class ActivatorC : MultiRunTask
        {
            private readonly ActorSkill actor;
            private readonly int countMax = 0;
            private readonly Map map;
            private Actor caster;
            private int count, lifetime = 0;
            private SkillArg skill;

            public ActivatorC(Actor caster, ActorSkill actor, SkillArg args, byte level)
            {
                this.actor = actor;
                this.caster = caster;
                skill = args.Clone();
                map = MapManager.Instance.GetMap(actor.MapID);
                period = 0;
                dueTime = 800;
            }

            public override void CallBack()
            {
                //同步锁，表示之后的代码是线程安全的，也就是，不允许被第二个线程同时访问ClientManager.EnterCriticalArea();
                try
                {
                    var actors = map.GetActorsArea(actor, 50, false);
                    if (count < countMax)
                    {
                        //广播技能效果
                        count++;
                    }
                    else
                    {
                        Deactivate();
                        //在指定地图删除技能体（技能效果结束）
                        map.DeleteActor(actor);
                    }
                }
                catch (Exception ex)
                {
                    Logger.ShowError(ex);
                }
                //解开同步锁ClientManager.LeaveCriticalArea();
            }
        }
    }
}