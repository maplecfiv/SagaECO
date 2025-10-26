using System;
using SagaDB.Actor;
using SagaLib;
using SagaLib.Tasks;
using SagaMap.Network.Client;

namespace SagaMap.Tasks.PC {
    public class Possession : MultiRunTask {
        private readonly MapClient client;
        private readonly string comment;
        private readonly PossessionPosition pos;
        private readonly ActorPC target;

        public Possession(MapClient client, ActorPC target, PossessionPosition position, string comment, int reduce) {
            if (reduce > 9)
                reduce = 9;
            DueTime = 10000 - reduce * 1000;
            Period = 10000 - reduce * 1000;
            this.client = client;
            this.target = target;
            pos = position;
            this.comment = comment;
        }

        public override void CallBack() {
            ClientManager.EnterCriticalArea();
            try {
                client.Character.Buff.GetReadyPossession = false;
                client.Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, client.Character, true);
                client.PossessionPerform(target, pos, comment);
                if (client.Character.Tasks.ContainsKey("Possession"))
                    client.Character.Tasks.Remove("Possession");
                Deactivate();
            }
            catch (Exception ex) {
                Logger.ShowError(ex);
                Deactivate();
            }

            ClientManager.LeaveCriticalArea();
        }
    }
}