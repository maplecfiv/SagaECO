using SagaDB.Actor;
using SagaDB.Fish;
using SagaDB.Item;
using SagaMap.Manager;
using SagaMap.Network.Client;
using SagaMap.Packets.Server;

namespace SagaMap.Skill.Additions.Global
{
    public class Fish : DefaultBuff
    {
        private bool isFiset;
        private bool isMarionette = false;

        public Fish(SagaDB.Skill.Skill skill, Actor actor, int lifetime, int period)
            : base(skill, actor, "fish", lifetime, period)
        {
            OnAdditionStart += StartEvent;
            OnAdditionEnd += EndEvent;
            OnUpdate += TimerUpdate;
        }

        private void StartEvent(Actor actor, DefaultBuff skill)
        {
            isFiset = true;
            var map = MapManager.Instance.GetMap(actor.MapID);
            actor.Buff.FishingState = true;
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEvent(Actor actor, DefaultBuff skill)
        {
            var map = MapManager.Instance.GetMap(actor.MapID);
            actor.Buff.FishingState = false;
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void TimerUpdate(Actor actor, DefaultBuff skill)
        {
            if (isFiset)
            {
                isFiset = false;
            }
            else
            {
                var client = MapClient.FromActorPC((ActorPC)actor);
                var p = new SSMG_FISHING_RESULT();

                //取得當前魚餌
                var temppc = (ActorPC)actor;
                var baititem = temppc.EquipedBaitID;

                //SagaDB.Item.Item bait = client.Character.Inventory.GetItem(10104900, SagaDB.Item.Inventory.SearchType.ITEM_ID);
                var bait = client.Character.Inventory.GetItem(baititem, Inventory.SearchType.ITEM_ID);


                if (((ActorPC)actor).Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND))
                {
                    SkillHandler.Instance.WeaponWorn((ActorPC)actor);
                    if (bait != null)
                    {
                        client.DeleteItemID(bait.ItemID, 1, false);
                        if (SagaLib.Global.Random.Next(0, 100) > 90) //lose
                        {
                            p.ItemID = 0;
                            p.IsSucceed = 0;
                            if (SagaLib.Global.Random.Next(0, 60) == 2)
                            {
                                var map = MapManager.Instance.GetMap(actor.MapID);
                                map.SpawnMob(26100000, client.Character.X, client.Character.Y, 5000, null);
                                client.SendSystemMessage("釣出了奇怪的東西！！");
                            }
                        }
                        else
                        {
                            SagaDB.Fish.Fish fish = null;
                            fish = FishFactory.Instance.GetRandomItem("钓鱼");
                            p.ItemID = fish.ID;
                            p.IsSucceed = 2;

                            var item = ItemFactory.Instance.GetItem(fish.ID);
                            client.AddItem(item, false);
                        }

                        client.netIO.SendPacket(p);
                    }
                    else
                    {
                        SkillHandler.RemoveAddition(actor, "fish");
                    }
                }
                else
                {
                    SkillHandler.RemoveAddition(actor, "fish");
                }
            }
        }
    }
}