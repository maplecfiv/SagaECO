using System.Threading;

namespace SagaLib
{
    public class SingleRunTask
    {
        public int dueTime;
        private Timer myTimer;

        public SingleRunTask()
        {
        }


        public SingleRunTask(int dueTime)
        {
            this.dueTime = dueTime;
        }

        public virtual void CallBack(object o)
        {
        }


        public void Activate()
        {
            myTimer = new Timer(CallBack, null, dueTime, Timeout.Infinite);
        }

        public void Deactivate()
        {
            myTimer.Dispose();
        }
    }
}