using System;

namespace SagaDB.Actor
{
    public class ActorItem : Actor
    {
        public bool Roll;

        public ActorItem(Item.Item item)
        {
            this.Item = item;
            Name = item.BaseData.name;
            type = ActorType.ITEM;
        }

        public Item.Item Item { get; set; }

        public bool PossessionItem => Item.PossessionedActor != null;

        public string Comment { get; set; }

        public uint LootedBy { get; set; } = 0xFFFFFFFF;

        public Actor Owner { get; set; }

        public DateTime CreateTime { get; set; } = DateTime.Now;

        public bool Party { get; set; }
    }
}