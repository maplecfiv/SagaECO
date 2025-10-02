using System;
using SagaLib.Tasks;
namespace SagaMap.Tasks.System
{
    public class LevelLimit : MultiRunTask
    {
        public LevelLimit()
        {
            period = 60000;
            dueTime = 0;
        }
        static LevelLimit instance;

        public static LevelLimit Instance
        {
            get
            {
                if (instance == null)
                    instance = new LevelLimit();
                return instance;
            }
        }
        public override void CallBack()
        {
            SagaDB.LevelLimit.LevelLimit LL = SagaDB.LevelLimit.LevelLimit.Instance;
            if (DateTime.Now > LL.NextTime)
            {
                SagaMap.LevelLimit.LevelLimitManager.Instance.UpdataLevelLimit();
            }
        }
    }
}
