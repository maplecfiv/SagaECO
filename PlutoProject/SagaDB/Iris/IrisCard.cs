using System;
using System.Collections.Generic;
using SagaLib;

namespace SagaDB.Iris
{
    /// <summary>
    ///     伊利斯卡片
    /// </summary>
    [Serializable]
    public class IrisCard
    {
        [NonSerialized] private Dictionary<AbilityVector, int> abilities = new Dictionary<AbilityVector, int>();
        private uint beforeCard;
        [NonSerialized] private Dictionary<Elements, int> elements = new Dictionary<Elements, int>();
        private uint id;
        private string name;
        private uint nextCard;
        private uint page;
        private int rank;
        private Rarity rarity;
        private string serial;
        private uint slot;
        [NonSerialized] private BitMask<CardSlot> slots = new BitMask<CardSlot>();

        public uint ID
        {
            get => id;
            set => id = value;
        }

        /// <summary>
        ///     名称
        /// </summary>
        public string Name
        {
            get => name;
            set => name = value;
        }

        /// <summary>
        ///     编号
        /// </summary>
        public string Serial
        {
            get => serial;
            set => serial = value;
        }

        /// <summary>
        ///     卡册页面
        /// </summary>
        public uint Page
        {
            get => page;
            set => page = value;
        }

        /// <summary>
        ///     卡册顺序
        /// </summary>
        public uint Slot
        {
            get => slot;
            set => slot = value;
        }

        /// <summary>
        ///     等级
        /// </summary>
        public int Rank
        {
            get => rank;
            set => rank = value;
        }

        /// <summary>
        ///     上一等级的卡片
        /// </summary>
        public uint BeforeCard
        {
            get => beforeCard;
            set => beforeCard = value;
        }

        /// <summary>
        ///     下一等级的卡片
        /// </summary>
        public uint NextCard
        {
            get => nextCard;
            set => nextCard = value;
        }

        /// <summary>
        ///     稀有度
        /// </summary>
        public Rarity Rarity
        {
            get => rarity;
            set => rarity = value;
        }

        /// <summary>
        ///     是否可以插入项链
        /// </summary>
        public bool CanNeck
        {
            get => slots.Test(CardSlot.胸);
            set => slots.SetValue(CardSlot.胸, value);
        }

        /// <summary>
        ///     是否可以插入武器
        /// </summary>
        public bool CanWeapon
        {
            get => slots.Test(CardSlot.武器);
            set => slots.SetValue(CardSlot.武器, value);
        }

        /// <summary>
        ///     是否可以插入衣服
        /// </summary>
        public bool CanArmor
        {
            get => slots.Test(CardSlot.服);
            set => slots.SetValue(CardSlot.服, value);
        }

        /// <summary>
        ///     属性补正
        /// </summary>
        public Dictionary<Elements, int> Elements => elements;

        /// <summary>
        ///     能力向量值
        /// </summary>
        public Dictionary<AbilityVector, int> Abilities => abilities;

        public override string ToString()
        {
            return name;
        }
    }
}