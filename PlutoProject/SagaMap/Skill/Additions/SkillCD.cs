using System;
using System.Collections.Generic;
using SagaDB.Actor;
using SagaLib;
using SagaMap.Network.Client;
using SagaMap.PC;

namespace SagaMap.Skill.Additions
{
    public class SkillCD : Addition
    {
        public delegate void EndEventHandler(Actor actor, SkillCD skill);

        public delegate void StartEventHandler(Actor actor, SkillCD skill);

        public delegate void UpdateEventHandler(Actor actor, SkillCD skill);

        public delegate void ValidCheckEventHandler(ActorPC sActor, Actor dActor, out int result);

        private readonly int period;
        private DateTime endTime;
        private int lifeTime;
        public ValidCheckEventHandler OnCheckValid;
        public SagaDB.Skill.Skill skill;

        public Dictionary<string, int> Variable = new Dictionary<string, int>();


        public SkillCD(SagaDB.Skill.Skill skill, Actor actor, string name, int lifetime)
            : this(skill, actor, name, lifetime, lifetime)
        {
        }

        public SkillCD(SagaDB.Skill.Skill skill, Actor actor, string name, int lifetime, int period)
        {
            Name = name;
            this.skill = skill;
            AttachedActor = actor;
            lifeTime = lifetime;
            this.Period = period;
        }

        public int this[string name]
        {
            get
            {
                var blocked = ClientManager.Blocked;
                if (!blocked)
                    ClientManager.EnterCriticalArea();
                int value;
                if (Variable.ContainsKey(name))
                    value = Variable[name];
                else
                    value = 0;
                if (!blocked)
                    ClientManager.LeaveCriticalArea();
                return value;
            }
            set
            {
                var blocked = ClientManager.Blocked;
                if (!blocked)
                    ClientManager.EnterCriticalArea();

                if (Variable.ContainsKey(name))
                    Variable.Remove(name);
                Variable.Add(name, value);

                if (!blocked)
                    ClientManager.LeaveCriticalArea();
            }
        }

        public override int RestLifeTime => (int)(endTime - DateTime.Now).TotalMilliseconds;

        public override int TotalLifeTime
        {
            get => lifeTime;
            set
            {
                var delta = value - lifeTime;
                lifeTime = value;
                var span = new TimeSpan(0, 0, 0, 0, delta);
                endTime += span;
            }
        }

        public event StartEventHandler OnAdditionStart;
        public event EndEventHandler OnAdditionEnd;
        public event UpdateEventHandler OnUpdate;

        public override void AdditionEnd()
        {
            SkillHandler.RemoveAddition(AttachedActor, this, true);
            TimerEnd();
            if (OnAdditionEnd != null && AttachedActor.Status != null)
                OnAdditionEnd.Invoke(AttachedActor, this);

            if (AttachedActor.type == ActorType.PC && AttachedActor.Status != null)
            {
                var pc = (ActorPC)AttachedActor;
                StatusFactory.Instance.CalcStatus(pc);
                MapClient.FromActorPC(pc).SendPlayerInfo();
            }
        }

        public override void AdditionStart()
        {
            if (period != int.MaxValue)
            {
                endTime = DateTime.Now + new TimeSpan(0, lifeTime / 60000, lifeTime / 1000 % 60);
                InitTimer(period, 0);
                TimerStart();
            }

            if (OnAdditionStart != null && AttachedActor.Status != null) OnAdditionStart.Invoke(AttachedActor, this);

            if (AttachedActor.type == ActorType.PC)
            {
                var pc = (ActorPC)AttachedActor;
                StatusFactory.Instance.CalcStatus(pc);
                MapClient.FromActorPC(pc).SendPlayerInfo();
            }
        }

        public override void OnTimerUpdate()
        {
            if (OnUpdate != null)
                OnUpdate.Invoke(AttachedActor, this);
        }

        public override void OnTimerEnd()
        {
            AdditionEnd();
        }
    }
}