namespace SagaDB.Actor
{
    public partial class Buff
    {
        #region Buffs

        /// <summary>
        ///     最大HP上昇
        /// </summary>
        public bool MaxHPUp
        {
            get => Buffs[3].Test(0x00000001);
            set => Buffs[3].SetValue(0x00000001, value);
        }

        /// <summary>
        ///     最大MP上昇
        /// </summary>
        public bool MaxMPUp
        {
            get => Buffs[3].Test(0x00000002);
            set => Buffs[3].SetValue(0x00000002, value);
        }

        /// <summary>
        ///     最大SP上昇
        /// </summary>
        public bool MaxSPUp
        {
            get => Buffs[3].Test(0x00000004);
            set => Buffs[3].SetValue(0x00000004, value);
        }

        /// <summary>
        ///     移動力上昇
        /// </summary>
        public bool MoveSpeedUp
        {
            get => Buffs[3].Test(0x00000008);
            set => Buffs[3].SetValue(0x00000008, value);
        }

        /// <summary>
        ///     最小攻撃力上昇
        /// </summary>
        public bool MinAtkUp
        {
            get => Buffs[3].Test(0x00000010);
            set => Buffs[3].SetValue(0x00000010, value);
        }

        /// <summary>
        ///     最大攻撃力上昇
        /// </summary>
        public bool MaxAtkUp
        {
            get => Buffs[3].Test(0x00000020);
            set => Buffs[3].SetValue(0x00000020, value);
        }

        /// <summary>
        ///     最小魔法攻撃力上昇
        /// </summary>
        public bool MinMagicAtkUp
        {
            get => Buffs[3].Test(0x00000040);
            set => Buffs[3].SetValue(0x00000040, value);
        }

        /// <summary>
        ///     最大魔法攻撃力上昇
        /// </summary>
        public bool MaxMagicAtkUp
        {
            get => Buffs[3].Test(0x00000080);
            set => Buffs[3].SetValue(0x00000080, value);
        }

        /// <summary>
        ///     防御率上昇
        /// </summary>
        public bool DefRateUp
        {
            get => Buffs[3].Test(0x00000100);
            set => Buffs[3].SetValue(0x00000100, value);
        }

        /// <summary>
        ///     防御力上昇
        /// </summary>
        public bool DefUp
        {
            get => Buffs[3].Test(0x00000200);
            set => Buffs[3].SetValue(0x00000200, value);
        }

        /// <summary>
        ///     魔法防御率上昇
        /// </summary>
        public bool MagicDefRateUp
        {
            get => Buffs[3].Test(0x00000400);
            set => Buffs[3].SetValue(0x00000400, value);
        }

        /// <summary>
        ///     魔法防御力上昇
        /// </summary>
        public bool MagicDefUp
        {
            get => Buffs[3].Test(0x00000800);
            set => Buffs[3].SetValue(0x00000800, value);
        }

        /// <summary>
        ///     近距離命中率上昇
        /// </summary>
        public bool ShortHitUp
        {
            get => Buffs[3].Test(0x00001000);
            set => Buffs[3].SetValue(0x00001000, value);
        }

        /// <summary>
        ///     遠距離命中率上昇
        /// </summary>
        public bool LongHitUp
        {
            get => Buffs[3].Test(0x00002000);
            set => Buffs[3].SetValue(0x00002000, value);
        }

        /// <summary>
        ///     魔法命中率上昇
        /// </summary>
        public bool MagicHitUp
        {
            get => Buffs[3].Test(0x00004000);
            set => Buffs[3].SetValue(0x00004000, value);
        }

        /// <summary>
        ///     近距離回避率上昇
        /// </summary>
        public bool ShortDodgeUp
        {
            get => Buffs[3].Test(0x00008000);
            set => Buffs[3].SetValue(0x00008000, value);
        }

        /// <summary>
        ///     遠距離回避上昇
        /// </summary>
        public bool LongDodgeUp
        {
            get => Buffs[3].Test(0x00010000);
            set => Buffs[3].SetValue(0x00010000, value);
        }

        /// <summary>
        ///     魔法抵抗上昇
        /// </summary>
        public bool MagicAvoidUp
        {
            get => Buffs[3].Test(0x00020000);
            set => Buffs[3].SetValue(0x00020000, value);
        }

        /// <summary>
        ///     クリティカル率上昇
        /// </summary>
        public bool CriticalRateUp
        {
            get => Buffs[3].Test(0x00040000);
            set => Buffs[3].SetValue(0x00040000, value);
        }

        /// <summary>
        ///     クリティカル回避率上昇
        /// </summary>
        public bool CriticalDodgeUp
        {
            get => Buffs[3].Test(0x00080000);
            set => Buffs[3].SetValue(0x00080000, value);
        }

        /// <summary>
        ///     HP回復率上昇
        /// </summary>
        public bool HPRegenUp
        {
            get => Buffs[3].Test(0x00100000);
            set => Buffs[3].SetValue(0x00100000, value);
        }

        /// <summary>
        ///     MP回復率上昇
        /// </summary>
        public bool MPRegenUp
        {
            get => Buffs[3].Test(0x00200000);
            set => Buffs[3].SetValue(0x00200000, value);
        }

        /// <summary>
        ///     SP回復率上昇
        /// </summary>
        public bool SPRegenUp
        {
            get => Buffs[3].Test(0x00400000);
            set => Buffs[3].SetValue(0x00400000, value);
        }

        /// <summary>
        ///     攻撃スピード上昇
        /// </summary>
        public bool AttackSpeedUp
        {
            get => Buffs[3].Test(0x00800000);
            set => Buffs[3].SetValue(0x00800000, value);
        }

        /// <summary>
        ///     詠唱スピード上昇
        /// </summary>
        public bool CastSpeedUp
        {
            get => Buffs[3].Test(0x01000000);
            set => Buffs[3].SetValue(0x01000000, value);
        }

        /// <summary>
        ///     STR上昇
        /// </summary>
        public bool STRUp
        {
            get => Buffs[3].Test(0x02000000);
            set => Buffs[3].SetValue(0x02000000, value);
        }

        /// <summary>
        ///     DEX上昇
        /// </summary>
        public bool DEXUp
        {
            get => Buffs[3].Test(0x04000000);
            set => Buffs[3].SetValue(0x04000000, value);
        }

        /// <summary>
        ///     INT上昇
        /// </summary>
        public bool INTUp
        {
            get => Buffs[3].Test(0x08000000);
            set => Buffs[3].SetValue(0x08000000, value);
        }

        /// <summary>
        ///     VIT上昇
        /// </summary>
        public bool VITUp
        {
            get => Buffs[3].Test(0x10000000);
            set => Buffs[3].SetValue(0x10000000, value);
        }

        /// <summary>
        ///     AGI上昇
        /// </summary>
        public bool AGIUp
        {
            get => Buffs[3].Test(0x20000000);
            set => Buffs[3].SetValue(0x20000000, value);
        }

        /// <summary>
        ///     MAG上昇
        /// </summary>
        public bool MagUp
        {
            get => Buffs[3].Test(0x40000000);
            set => Buffs[3].SetValue(0x40000000, value);
        }

        #endregion
    }
}