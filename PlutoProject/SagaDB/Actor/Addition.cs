using System;
using SagaLib.Tasks;

namespace SagaDB.Actor
{
    /// <summary>
    ///     A class which contains the information about a players addition bonus (such as Buff)
    /// </summary>
    public abstract class Addition : MultiRunTask
    {
        public enum AdditionType
        {
            PassiveSkill,
            Buff,
            Debuff,
            Control,
            Other
        }

        public override string ToString()
        {
            return Name;
        }

        //#region Internal Methods

        public override void CallBack()
        {
            if (RestLifeTime > 100)
            {
                OnTimerUpdate();
            }
            else
            {
                Deactivate();
                OnTimerEnd();
            }
        }

        //#endregion


        //#region Fields

        /// <summary>
        /// Task instance for this addition
        /// </summary>
        //internal MultiRunTask m_task;

        /// <summary>
        ///     A time stamp of when it's get applied to player
        /// </summary>
        private DateTime m_starttime;

        public AdditionType MyType;

        public bool enabled = true;

        //#endregion

        //#region Properties

        /// <summary>
        ///     是否启用
        /// </summary>
        public bool Enabled
        {
            get => enabled;
            set => enabled = value;
        }

        /// <summary>
        ///     Actor that get attached to this addition
        /// </summary>
        public Actor AttachedActor { get; set; }

        /// <summary>
        ///     Name of this Addition
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Returns the activation interval
        /// </summary>
        public int Interval
        {
            get
            {
                if (this != null) return Period;

                return -1;
            }
        }

        /// <summary>
        ///     Returns if this addition is activated
        /// </summary>
        public bool Activated { get; set; }


        /// <summary>
        ///     Time that this addition get started
        /// </summary>
        public DateTime StartTime
        {
            get => m_starttime;
            set => m_starttime = value;
        }

        /// <summary>
        ///     Returns if this addition should be activated
        /// </summary>
        public virtual bool IfActivate => true;

        //#endregion

        //#region Virtual methodes

        /// <summary>
        ///     Total Life time for this Addition
        /// </summary>
        public virtual int TotalLifeTime
        {
            get => int.MaxValue;
            set { }
        }

        /// <summary>
        ///     Rest Life time for this Addition
        /// </summary>
        public virtual int RestLifeTime => int.MaxValue;

        /// <summary>
        ///     Method to be called on Addition start
        /// </summary>
        public abstract void AdditionStart();

        /// <summary>
        ///     Method to be called on Addition End
        /// </summary>
        public abstract void AdditionEnd();

        /// <summary>
        ///     Method that be called once Timer call back function get invoked
        /// </summary>
        public virtual void OnTimerUpdate()
        {
        }

        /// <summary>
        ///     Method that be called once Timer get started
        /// </summary>
        public virtual void OnTimerStart()
        {
        }

        /// <summary>
        ///     Method that be called once Timer get stoped
        /// </summary>
        public virtual void OnTimerEnd()
        {
        }

        //#endregion

        //#region Protected Methods

        /// <summary>
        ///     Initialize the timer
        /// </summary>
        /// <param name="interval">Interval</param>
        /// <param name="duetime">Due Time</param>
        protected void InitTimer(int interval, int duetime)
        {
            DueTime = duetime;
            Period = interval; //= new MultiRunTask(duetime, interval);
            Name = Name;
        }

        protected void TimerStart()
        {
            if (this != null)
                Activate();
        }

        protected void TimerEnd()
        {
            if (this != null)
                Deactivate();
        }

        //#endregion
    }
}