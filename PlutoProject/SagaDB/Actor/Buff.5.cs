namespace SagaDB.Actor
{
    public partial class Buff
    {
        #region Buffs

        /// <summary>
        ///     最大HP減少
        /// </summary>
        public bool MaxHPDown
        {
            get => Buffs[4].Test(0x00000001);
            set => Buffs[4].SetValue(0x00000001, value);
        }

        /// <summary>
        ///     最大MP減少
        /// </summary>
        public bool MaxMPDown
        {
            get => Buffs[4].Test(0x00000002);
            set => Buffs[4].SetValue(0x00000002, value);
        }

        /// <summary>
        ///     最大SP減少
        /// </summary>
        public bool MaxSPDown
        {
            get => Buffs[4].Test(0x00000004);
            set => Buffs[4].SetValue(0x00000004, value);
        }

        /// <summary>
        ///     最小攻撃力減少
        /// </summary>
        public bool MinAtkDown
        {
            get => Buffs[4].Test(0x00000010);
            set => Buffs[4].SetValue(0x00000010, value);
        }

        /// <summary>
        ///     最大攻撃力減少
        /// </summary>
        public bool MaxAtkDown
        {
            get => Buffs[4].Test(0x00000020);
            set => Buffs[4].SetValue(0x00000020, value);
        }

        /// <summary>
        ///     最小魔法攻撃力減少
        /// </summary>
        public bool MinMagicAtkDown
        {
            get => Buffs[4].Test(0x00000040);
            set => Buffs[4].SetValue(0x00000040, value);
        }

        /// <summary>
        ///     最大魔法攻撃力減少
        /// </summary>
        public bool MaxMagicAtkDown
        {
            get => Buffs[4].Test(0x00000080);
            set => Buffs[4].SetValue(0x00000080, value);
        }

        /// <summary>
        ///     防御率減少
        /// </summary>
        public bool DefRateDown
        {
            get => Buffs[4].Test(0x00000100);
            set => Buffs[4].SetValue(0x00000100, value);
        }

        /// <summary>
        ///     防御力減少
        /// </summary>
        public bool DefDown
        {
            get => Buffs[4].Test(0x00000200);
            set => Buffs[4].SetValue(0x00000200, value);
        }

        /// <summary>
        ///     魔法防御率減少
        /// </summary>
        public bool MagicDefRateDown
        {
            get => Buffs[4].Test(0x00000400);
            set => Buffs[4].SetValue(0x00000400, value);
        }

        /// <summary>
        ///     魔法防御力減少
        /// </summary>
        public bool MagicDefDown
        {
            get => Buffs[4].Test(0x00000800);
            set => Buffs[4].SetValue(0x00000800, value);
        }

        /// <summary>
        ///     近距離命中率減少
        /// </summary>
        public bool ShortHitDown
        {
            get => Buffs[4].Test(0x00001000);
            set => Buffs[4].SetValue(0x00001000, value);
        }

        /// <summary>
        ///     遠距離命中率減少
        /// </summary>
        public bool LongHitDown
        {
            get => Buffs[4].Test(0x00002000);
            set => Buffs[4].SetValue(0x00002000, value);
        }

        /// <summary>
        ///     魔法命中率減少
        /// </summary>
        public bool MagicHitDown
        {
            get => Buffs[4].Test(0x00004000);
            set => Buffs[4].SetValue(0x00004000, value);
        }

        /// <summary>
        ///     近距離回避率減少
        /// </summary>
        public bool ShortDodgeDown
        {
            get => Buffs[4].Test(0x00008000);
            set => Buffs[4].SetValue(0x00008000, value);
        }

        /// <summary>
        ///     遠距離回避率減少
        /// </summary>
        public bool LongDodgeDown
        {
            get => Buffs[4].Test(0x00010000);
            set => Buffs[4].SetValue(0x00010000, value);
        }

        /// <summary>
        ///     魔法抵抗率減少
        /// </summary>
        public bool MagicAvoidDown
        {
            get => Buffs[4].Test(0x00020000);
            set => Buffs[4].SetValue(0x00020000, value);
        }

        /// <summary>
        ///     クリティカル率減少
        /// </summary>
        public bool CriticalRateDown

        {
            get => Buffs[4].Test(0x00040000);
            set => Buffs[4].SetValue(0x00040000, value);
        }

        /// <summary>
        ///     クリティカル回避率減少
        /// </summary>
        public bool CriticalDodgeDown
        {
            get => Buffs[4].Test(0x00080000);
            set => Buffs[4].SetValue(0x00080000, value);
        }

        /// <summary>
        ///     HP回復率減少
        /// </summary>
        public bool HPRegenDown
        {
            get => Buffs[4].Test(0x00100000);
            set => Buffs[4].SetValue(0x00100000, value);
        }

        /// <summary>
        ///     MP回復率減少
        /// </summary>
        public bool MPRegenDown
        {
            get => Buffs[4].Test(0x00200000);
            set => Buffs[4].SetValue(0x00200000, value);
        }

        /// <summary>
        ///     SP回復率減少
        /// </summary>
        public bool SPRegenDown
        {
            get => Buffs[4].Test(0x00400000);
            set => Buffs[4].SetValue(0x00400000, value);
        }

        /// <summary>
        ///     攻撃スピード減少
        /// </summary>
        public bool AttackSpeedDown
        {
            get => Buffs[4].Test(0x00800000);
            set => Buffs[4].SetValue(0x00800000, value);
        }

        /// <summary>
        ///     詠唱スピード減少
        /// </summary>
        public bool CastSpeedDown
        {
            get => Buffs[4].Test(0x01000000);
            set => Buffs[4].SetValue(0x01000000, value);
        }

        /// <summary>
        ///     STR減少
        /// </summary>
        public bool STRDown
        {
            get => Buffs[4].Test(0x02000000);
            set => Buffs[4].SetValue(0x02000000, value);
        }

        /// <summary>
        ///     DEX減少
        /// </summary>
        public bool DEXDown
        {
            get => Buffs[4].Test(0x04000000);
            set => Buffs[4].SetValue(0x04000000, value);
        }

        /// <summary>
        ///     INT減少
        /// </summary>
        public bool INTDown
        {
            get => Buffs[4].Test(0x08000000);
            set => Buffs[4].SetValue(0x08000000, value);
        }

        /// <summary>
        ///     VIT減少
        /// </summary>
        public bool VITDown
        {
            get => Buffs[4].Test(0x10000000);
            set => Buffs[4].SetValue(0x10000000, value);
        }

        /// <summary>
        ///     AGI減少
        /// </summary>
        public bool AGIDown
        {
            get => Buffs[4].Test(0x20000000);
            set => Buffs[4].SetValue(0x20000000, value);
        }

        /// <summary>
        ///     MAG減少
        /// </summary>
        public bool MAGDown
        {
            get => Buffs[4].Test(0x40000000);
            set => Buffs[4].SetValue(0x40000000, value);
        }

        #endregion
    }
}