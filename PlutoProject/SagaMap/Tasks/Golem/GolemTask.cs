using System;
using SagaDB.Actor;
using SagaDB.Item;
using SagaDB.Marionette;
using SagaDB.Treasure;
using SagaLib;
using SagaLib.Tasks;
using SagaMap.ActorEventHandlers;
using SagaMap.Manager;

namespace SagaMap.Tasks.Golem
{
    public class GolemTask : MultiRunTask
    {
        private readonly TimeSpan gatherSpan;
        private readonly ActorGolem golem;
        private int counter;
        private DateTime nextGatherTime = DateTime.Now + new TimeSpan(2, 0, 0, 0);

        public GolemTask(ActorGolem golem)
        {
            DueTime = 60000;
            Period = 60000;
            this.golem = golem;

            var map = MapManager.Instance.GetMap(golem.MapID);
            switch (golem.GolemType)
            {
                case GolemType.Plant:
                    if (map.Info.gatherInterval.ContainsKey(GatherType.Plant))
                    {
                        gatherSpan = new TimeSpan(0, map.Info.gatherInterval[GatherType.Plant], 0);
                        nextGatherTime = DateTime.Now + gatherSpan;
                    }

                    break;
                case GolemType.Mineral:
                    if (map.Info.gatherInterval.ContainsKey(GatherType.Mineral))
                    {
                        gatherSpan = new TimeSpan(0, map.Info.gatherInterval[GatherType.Mineral], 0);
                        nextGatherTime = DateTime.Now + gatherSpan;
                    }

                    break;
                case GolemType.Magic:
                    if (map.Info.gatherInterval.ContainsKey(GatherType.Magic))
                    {
                        gatherSpan = new TimeSpan(0, map.Info.gatherInterval[GatherType.Magic], 0);
                        nextGatherTime = DateTime.Now + gatherSpan;
                    }

                    break;
                case GolemType.TreasureBox:
                    if (map.Info.gatherInterval.ContainsKey(GatherType.Treasurebox))
                    {
                        gatherSpan = new TimeSpan(0, map.Info.gatherInterval[GatherType.Treasurebox], 0);
                        nextGatherTime = DateTime.Now + gatherSpan;
                    }

                    break;
                case GolemType.Excavation:
                    if (map.Info.gatherInterval.ContainsKey(GatherType.Excavation))
                    {
                        gatherSpan = new TimeSpan(0, map.Info.gatherInterval[GatherType.Excavation], 0);
                        nextGatherTime = DateTime.Now + gatherSpan;
                    }

                    break;
                case GolemType.Any:
                    if (map.Info.gatherInterval.ContainsKey(GatherType.Any))
                    {
                        gatherSpan = new TimeSpan(0, map.Info.gatherInterval[GatherType.Any], 0);
                        nextGatherTime = DateTime.Now + gatherSpan;
                    }

                    break;
                case GolemType.Strange:
                    if (map.Info.gatherInterval.ContainsKey(GatherType.Strange))
                    {
                        gatherSpan = new TimeSpan(0, map.Info.gatherInterval[GatherType.Strange], 0);
                        nextGatherTime = DateTime.Now + gatherSpan;
                    }

                    break;
                case GolemType.Food:
                    if (map.Info.gatherInterval.ContainsKey(GatherType.Food))
                    {
                        gatherSpan = new TimeSpan(0, map.Info.gatherInterval[GatherType.Food], 0);
                        nextGatherTime = DateTime.Now + gatherSpan;
                    }

                    break;
            }
        }

        public override void CallBack()
        {
            counter++;
            try
            {
                if (counter == 24 * 60)
                {
                    var map = MapManager.Instance.GetMap(golem.MapID);
                    if (golem.GolemType >= GolemType.Plant && golem.GolemType <= GolemType.Strange)
                    {
                        var eh = (MobEventHandler)golem.e;
                        golem.e = new NullEventHandler();
                        eh.AI.Pause();
                    }

                    golem.invisble = true;
                    map.OnActorVisibilityChange(golem);
                    golem.Tasks.Remove("GolemTask");
                    Deactivate();
                }

                if (nextGatherTime <= DateTime.Now)
                {
                    if (golem.GolemType >= GolemType.Plant && golem.GolemType <= GolemType.Strange)
                    {
                        var mario = MarionetteFactory.Instance[golem.Item.BaseData.marionetteID];
                        if (mario != null)
                        {
                            TreasureItem item = null;
                            var det = 0;
                            switch (golem.GolemType)
                            {
                                case GolemType.Plant:
                                    if (!mario.gather[GatherType.Plant])
                                        det = Global.Random.Next(0, 99);
                                    if (det <= 10)
                                        item = TreasureFactory.Instance.GetRandomItem("Gather_Plant");
                                    break;
                                case GolemType.Mineral:
                                    if (!mario.gather[GatherType.Mineral])
                                        det = Global.Random.Next(0, 99);
                                    if (det <= 10)
                                        item = TreasureFactory.Instance.GetRandomItem("Gather_Mineral");
                                    break;
                                case GolemType.Food:
                                    if (!mario.gather[GatherType.Food])
                                        det = Global.Random.Next(0, 99);
                                    if (det <= 10)
                                        item = TreasureFactory.Instance.GetRandomItem("Gather_Food");
                                    break;
                                case GolemType.Magic:
                                    if (!mario.gather[GatherType.Magic])
                                        det = Global.Random.Next(0, 99);
                                    if (det <= 10)
                                        item = TreasureFactory.Instance.GetRandomItem("Gather_Magic");
                                    break;
                                case GolemType.TreasureBox:
                                    if (!mario.gather[GatherType.Treasurebox])
                                        det = Global.Random.Next(0, 99);
                                    if (det <= 10)
                                        item = TreasureFactory.Instance.GetRandomItem("Gather_Treasurebox");
                                    break;
                                case GolemType.Excavation:
                                    if (!mario.gather[GatherType.Excavation])
                                        det = Global.Random.Next(0, 99);
                                    if (det <= 10)
                                        item = TreasureFactory.Instance.GetRandomItem("Gather_Excavation");
                                    break;
                                case GolemType.Any:
                                    if (!mario.gather[GatherType.Any])
                                        det = Global.Random.Next(0, 99);
                                    if (det <= 10)
                                        item = TreasureFactory.Instance.GetRandomItem("Gather_Any");
                                    break;
                                case GolemType.Strange:
                                    if (!mario.gather[GatherType.Strange])
                                        det = Global.Random.Next(0, 99);
                                    if (det <= 10)
                                        item = TreasureFactory.Instance.GetRandomItem("Gather_Strange");
                                    break;
                            }

                            if (item != null)
                                if (item.ID != 0)
                                {
                                    var newItem = ItemFactory.Instance.GetItem(item.ID, true); //免鉴定
                                    newItem.Stack = (ushort)item.Count;
                                    if (newItem.Stack > 0)
                                        Logger.LogItemGet(Logger.EventType.ItemGolemGet,
                                            golem.Owner.Name + "(" + golem.Owner.CharID + ")",
                                            newItem.BaseData.name + "(" + newItem.ItemID + ")",
                                            string.Format("GolemCollect Count:{0}", newItem.Stack), false);

                                    golem.Owner.Inventory.AddItem(ContainerType.GOLEMWAREHOUSE, newItem);
                                }
                        }
                    }

                    nextGatherTime += gatherSpan;
                }
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);
                golem.Tasks.Remove("GolemTask");
                Deactivate();
            }
        }
    }
}