using System;

namespace SagaLib
{
    public abstract class MultiRunTask
    {
        /// <summary>
        ///     启动延迟(ms)
        /// </summary>
        public int dueTime;

        internal bool executing;

        /// <summary>
        ///     下次执行时间
        /// </summary>
        public DateTime NextUpdateTime = DateTime.Now;

        /// <summary>
        ///     运行周期(ms)
        /// </summary>
        public int period;

        internal DateTime TaskBeginTime;

        public MultiRunTask()
        {
        }

        /// <summary>
        ///     创建一个新任务实例
        /// </summary>
        /// <param name="dueTime">启动延迟</param>
        /// <param name="period">运行周期</param>
        /// <param name="name">名称</param>
        public MultiRunTask(int dueTime, int period, string name)
        {
            if (period <= 0)
                Logger.ShowWarning("period <= 0");
            this.dueTime = dueTime;
            this.period = period;
            this.Name = name;
        }

        /// <summary>
        ///     如果Callback执行时间较长，请将此属性设置为true
        /// </summary>
        public bool IsSlowTask { get; set; }

        /// <summary>
        ///     任务名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     任务是否处于活动状态
        /// </summary>
        public bool Activated { get; private set; }

        /// <summary>
        ///     启动延迟(ms)
        /// </summary>
        public int DueTime
        {
            get => dueTime;
            set => dueTime = value;
        }

        /// <summary>
        ///     运行周期(ms)
        /// </summary>
        public int Period
        {
            get => period;
            set => period = value;
        }


        /// <summary>
        ///     任务每次运行时调用的回调函数
        /// </summary>
        public abstract void CallBack();

        protected virtual void OnActivate()
        {
        }

        public bool getActivated()
        {
            return Activated;
        }

        /// <summary>
        ///     激活任务
        /// </summary>
        public void Activate()
        {
            NextUpdateTime = DateTime.Now.AddMilliseconds(dueTime);
            TaskManager.Instance.RegisterTask(this);
            Activated = true;
            OnActivate();
        }

        /// <summary>
        ///     将任务处于非激活状态
        /// </summary>
        public void Deactivate()
        {
            TaskManager.Instance.RemoveTask(this);
            if (Activated)
            {
                Activated = false;
                OnDeactivate();
            }
        }

        protected virtual void OnDeactivate()
        {
        }

        public override string ToString()
        {
            if (Name != null)
                return Name;
            return base.ToString();
        }
    }
}