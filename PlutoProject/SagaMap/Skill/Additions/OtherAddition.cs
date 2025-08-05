using System;
using System.Collections.Generic;
using SagaDB.Actor;
using SagaLib;
using SagaMap.Network.Client;
using SagaMap.PC;

namespace SagaMap.Skill.Additions
{
    public class OtherAddition : Addition
    {
        public delegate void EndEventHandler(Actor actor, OtherAddition skill);

        public delegate void StartEventHandler(Actor actor, OtherAddition skill);

        public delegate void UpdateEventHandler(Actor actor, OtherAddition skill);

        public delegate void UpdateEventHandler2(Actor sActor, Actor dActor, OtherAddition skill, SkillArg arg,
            int damage);

        public delegate void ValidCheckEventHandler(ActorPC sActor, Actor dActor, out int result);

        private readonly SkillArg args;
        private readonly int damage;
        private readonly int period;
        public bool donotsendinfo;
        public DateTime endTime;
        public int lifeTime;
        public ValidCheckEventHandler OnCheckValid;
        public Actor sactor;
        public SagaDB.Skill.Skill skill;

        public Dictionary<string, int> Variable = new Dictionary<string, int>();

        public OtherAddition(SagaDB.Skill.Skill skill, Actor actor, string name, int lifetime)
            : this(skill, actor, name, lifetime, lifetime)
        {
        }

        public OtherAddition(SagaDB.Skill.Skill skill, Actor actor, string name, int lifetime,
            bool donotsendinfo = false)
            : this(skill, actor, name, lifetime, lifetime)
        {
            this.donotsendinfo = donotsendinfo;
        }

        public OtherAddition(SagaDB.Skill.Skill skill, Actor actor, string name, int lifetime, int period)
            : this(skill, null, actor, name, lifetime, period, 0, null)
        {
        }

        public OtherAddition(SagaDB.Skill.Skill skill, Actor actor, Actor dActor, string name, int lifetime, int period)
            : this(skill, actor, dActor, name, lifetime, period, 0, null)
        {
        }

        public OtherAddition(SagaDB.Skill.Skill skill, Actor actor, Actor dActor, string name, int lifetime, int period,
            int damage)
            : this(skill, actor, dActor, name, lifetime, period, damage, null)
        {
        }

        public OtherAddition(SagaDB.Skill.Skill skill, Actor sActor, Actor dActor, string name, int lifetime,
            int period, int damage, SkillArg arg, bool donotsendinfo = false)
        {
            Name = name;
            this.skill = skill;
            sactor = sActor;
            AttachedActor = dActor;
            lifeTime = lifetime;
            this.period = period;
            this.damage = damage;
            args = arg;
            this.donotsendinfo = donotsendinfo;
            MyType = AdditionType.Other;
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
        public event UpdateEventHandler2 OnUpdate2;

        public void AddBuff(string s, int value)
        {
            if (Variable.ContainsKey(s))
                Variable.Remove(s);
            Variable.Add(s, value);
        }

        public override void AdditionEnd()
        {
            SkillHandler.RemoveAddition(AttachedActor, this, true);
            TimerEnd();
            if (OnAdditionEnd != null && AttachedActor.Status != null)
                OnAdditionEnd.Invoke(AttachedActor, this);

            if (AttachedActor.type == ActorType.PC && AttachedActor.Status != null && !donotsendinfo)
            {
                var pc = (ActorPC)AttachedActor;
                StatusFactory.Instance.CalcStatusOnSkillEffect(pc);
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

            if (AttachedActor.type == ActorType.PC && !donotsendinfo)
            {
                var pc = (ActorPC)AttachedActor;
                StatusFactory.Instance.CalcStatusOnSkillEffect(pc);
                MapClient.FromActorPC(pc).SendPlayerInfo();
            }
        }

        public override void OnTimerUpdate()
        {
            if (OnUpdate != null)
                OnUpdate.Invoke(AttachedActor, this);
            if (OnUpdate2 != null)
                OnUpdate2.Invoke(sactor, AttachedActor, this, args, damage);
        }

        public override void OnTimerEnd()
        {
            AdditionEnd();
        }
    }
}