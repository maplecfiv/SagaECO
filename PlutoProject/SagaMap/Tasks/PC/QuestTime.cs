using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Tasks.PC
{
    public class QuestTime : MultiRunTask
    {
        private readonly MapClient client;

        public QuestTime(MapClient client)
        {
            dueTime = 60000;
            period = 60000;
            this.client = client;
        }

        public override void CallBack()
        {
            ClientManager.EnterCriticalArea();
            if (client.Character.Quest != null)
                client.SendQuestTime();
            ClientManager.LeaveCriticalArea();
        }
    }
}