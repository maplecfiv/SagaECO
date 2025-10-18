using System.Collections.Generic;

namespace SagaDB.Actor
{
    public enum GolemType
    {
        Sell,
        Buy,
        Plant = 3,
        Mineral,
        Food,
        Magic,
        TreasureBox,
        Excavation,
        Any,
        Strange,
        None = 0xff
    }

    public class GolemShopItem
    {
        public uint InventoryID { get; set; }

        public uint ItemID { get; set; }

        public ushort Count { get; set; }

        public uint Price { get; set; }
    }

    /// <summary>
    ///     石像Actor
    /// </summary>
    public class ActorGolem : ActorMob
    {
        private readonly Dictionary<uint, GolemShopItem> buyShop = new Dictionary<uint, GolemShopItem>();
        public byte AIMode;
        public bool motion_loop;

        public ActorGolem()
        {
            type = ActorType.GOLEM;
            Speed = 410;
            sightRange = 1500;
            GolemType = GolemType.None;
        }

        /// <summary>
        ///     石像道具
        /// </summary>
        public Item.Item Item { get; set; }

        /// <summary>
        ///     标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///     石像拥有者
        /// </summary>
        public new ActorPC Owner { get; set; }

        /// <summary>
        ///     石像类型
        /// </summary>
        public GolemType GolemType { get; set; }

        /// <summary>
        ///     石像收购金额上限
        /// </summary>
        public uint BuyLimit { get; set; }

        /// <summary>
        ///     石像贩卖的道具
        /// </summary>
        public Dictionary<uint, GolemShopItem> SellShop { get; set; } = new Dictionary<uint, GolemShopItem>();

        /// <summary>
        ///     石像收购的道具
        /// </summary>
        public Dictionary<uint, GolemShopItem> BuyShop
        {
            get => buyShop;
            set => BuyShop = value;
        }

        /// <summary>
        ///     石像已收购道具
        /// </summary>
        public Dictionary<uint, GolemShopItem> BoughtItem { get; } = new Dictionary<uint, GolemShopItem>();

        /// <summary>
        ///     石像已贩卖道具
        /// </summary>
        public Dictionary<uint, GolemShopItem> SoldItem { get; } = new Dictionary<uint, GolemShopItem>();


        /// <summary>
        ///     动作
        /// </summary>
        public ushort Motion { get; set; }

        public bool MotionLoop
        {
            get => motion_loop;
            set => motion_loop = value;
        }
    }
}