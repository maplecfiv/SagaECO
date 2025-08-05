﻿using System;

namespace SagaDB.Item
{
    [Serializable]
    public class ItemBonus
    {
        public byte EffectType { get; set; }
        public byte BonusType { get; set; }
        public string Attribute { get; set; }
        public int Values1 { get; set; }
        public int Values2 { get; set; }
        public int Values3 { get; set; }
        public int Values4 { get; set; }
    }
}