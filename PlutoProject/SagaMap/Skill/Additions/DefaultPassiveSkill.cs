using System;
using System.Collections.Generic;
using SagaDB.Actor;
using SagaMap.Network.Client;
using SagaMap.PC;

namespace SagaMap.Skill.Additions.Global
{
    public class DefaultPassiveSkill : Addition
    {
        public delegate void EndEventHandler(Actor actor, DefaultPassiveSkill skill);

        public delegate void StartEventHandler(Actor actor, DefaultPassiveSkill skill);

        public delegate void UpdateEventHandler(Actor actor, DefaultPassiveSkill skill);

        private readonly bool activate;
        private readonly int lifeTime;

        private readonly int period;
        private bool canEnd;
        private DateTime endTime;
        public SagaDB.Skill.Skill skill;

        public Dictionary<string, int> Variable = new Dictionary<string, int>();

        /// <summary>
        ///     Constructor for Addition: Short Sword Mastery
        /// </summary>
        /// <param name="actor">Actor, which this addition get attached to</param>
        public DefaultPassiveSkill(SagaDB.Skill.Skill skill, Actor actor, string name, bool ifActivate)
        {
            Name = name;
            this.skill = skill;
            AttachedActor = actor;
            activate = ifActivate;
        }

        public DefaultPassiveSkill(SagaDB.Skill.Skill skill, Actor actor, string name, bool ifActivate, int period,
            int lifetime)
        {
            Name = name;
            this.skill = skill;
            AttachedActor = actor;
            activate = ifActivate;
            this.period = period;
            lifeTime = lifetime;
        }

        public override bool IfActivate => activate;

        public int this[string name]
        {
            get
            {
                if (Variable.ContainsKey(name))
                    return Variable[name];
                return 0;
            }
            set
            {
                if (Variable.ContainsKey(name))
                    Variable.Remove(name);
                Variable.Add(name, value);
            }
        }

        public override int RestLifeTime => (int)(endTime - DateTime.Now).TotalMilliseconds;

        public override int TotalLifeTime => lifeTime;
        public event StartEventHandler OnAdditionStart;
        public event EndEventHandler OnAdditionEnd;
        public event UpdateEventHandler OnUpdate;

        public override void AdditionEnd()
        {
            if (lifeTime != 0)
                TimerEnd();
            if (canEnd && AttachedActor.Status != null)
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
            canEnd = true;
            if (lifeTime != 0)
            {
                endTime = DateTime.Now + new TimeSpan(0, lifeTime / 60000, lifeTime / 1000 % 60);
                InitTimer(period, 0);
                TimerStart();
            }

            if (AttachedActor.Status != null)
                OnAdditionStart.Invoke(AttachedActor, this);
            if (AttachedActor.type == ActorType.PC)
            {
                var pc = (ActorPC)AttachedActor;
                StatusFactory.Instance.CalcStatus(pc);
                MapClient.FromActorPC(pc).SendPlayerInfo();
            }
        }

        public override void OnTimerUpdate()
        {
            OnUpdate.Invoke(AttachedActor, this);
        }

        public override void OnTimerEnd()
        {
            AdditionEnd();
        }
    }
}