using System;
using SagaDB.Actor;
using SagaLib;
using SagaMap.ActorEventHandlers;
using SagaMap.Dungeon;
using SagaMap.Manager;

namespace SagaMap.Tasks.Dungeon
{
    public class Dungeon : MultiRunTask
    {
        private readonly SagaMap.Dungeon.Dungeon dungeon;
        public int counter;
        public int lifeTime;

        public Dungeon(SagaMap.Dungeon.Dungeon dungeon, int lifeTime)
        {
            period = 1000;
            dueTime = 1000;
            this.dungeon = dungeon;
            this.lifeTime = lifeTime;
        }

        public override void CallBack()
        {
            try
            {
                counter++;
                var rest = lifeTime - counter;

                if (rest == 0)
                {
                    Deactivate();
                    dungeon.Destory(DestroyType.TimeOver);
                    return;
                }

                if (rest == 7200 ||
                    rest == 3600 ||
                    rest == 1800 ||
                    rest == 900 ||
                    rest == 600 ||
                    rest == 300 ||
                    rest == 240 ||
                    rest == 180 ||
                    rest == 120 ||
                    rest == 60 ||
                    rest == 50 ||
                    rest == 40 ||
                    rest == 30 ||
                    rest == 20 ||
                    rest <= 15)
                {
                    var time = "";
                    if (rest >= 3600)
                        time = (rest / 3600) + LocalManager.Instance.Strings.ITD_HOUR;
                    if (rest < 3600 && rest >= 60)
                        time = (rest / 60) + LocalManager.Instance.Strings.ITD_MINUTE;
                    if (rest < 60)
                        time = rest + LocalManager.Instance.Strings.ITD_SECOND;
                    var announce = string.Format(LocalManager.Instance.Strings.ITD_CRASHING, time);
                    foreach (var i in dungeon.Maps)
                    foreach (var j in i.Map.Actors.Values)
                        if (j.type == ActorType.PC)
                            if (((ActorPC)j).Online)
                            {
                                var eh = (PCEventHandler)j.e;
                                eh.Client.SendAnnounce(announce);
                            }
                }
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);
            }
        }
    }
}