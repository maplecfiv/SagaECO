using System;
using System.Collections.Generic;
using System.Threading;
using SagaDB.Actor;
using SagaLib;
using SagaLib.Tasks;
using SagaMap.Network.Client;

namespace SagaMap.Scripting
{
    public class Timer : MultiRunTask
    {
        public Timer(string name, int period, int due)
        {
            Name = name;
            Period = period;
            DueTime = due;
        }

        /// <summary>
        ///     挂钩的玩家
        /// </summary>
        public ActorPC AttachedPC { get; set; }

        /// <summary>
        ///     自定义物件
        /// </summary>
        public List<object> CustomObjects { get; } = new List<object>();

        /// <summary>
        ///     是否需要用到脚本
        /// </summary>
        public bool NeedScript { get; set; }

        public event TimerCallback OnTimerCall;

        public override void CallBack()
        {
            try
            {
                if (NeedScript && AttachedPC != null)
                {
                    if (MapClient.FromActorPC(AttachedPC).scriptThread != null)
                        return;
                    MapClient.FromActorPC(AttachedPC).scriptThread = new Thread(Run);
                    MapClient.FromActorPC(AttachedPC).scriptThread.Start();
                }
                else
                {
                    if (AttachedPC != null)
                    {
                        if (OnTimerCall != null)
                            OnTimerCall(this, AttachedPC);
                    }
                    else
                    {
                        OnTimerCall(this, null);
                        Deactivate();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.GetLogger().Error(ex, ex.Message);
            }
        }

        private void Run()
        {
            ClientManager.EnterCriticalArea();
            try
            {
                if (AttachedPC != null)
                    OnTimerCall(this, AttachedPC);
                else
                    Deactivate();
            }
            catch (Exception ex)
            {
                Logger.GetLogger().Error(ex, ex.Message);
            }

            ClientManager.LeaveCriticalArea();
            MapClient.FromActorPC(AttachedPC).scriptThread = null;
        }
    }
}