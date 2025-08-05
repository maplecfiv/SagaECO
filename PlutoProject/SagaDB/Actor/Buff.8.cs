namespace SagaDB.Actor
{
    public partial class Buff
    {
        #region Buffs

        /// <summary>
        ///     三转HP吸收
        /// </summary>
        public bool HPDrain3RD
        {
            get => Buffs[7].Test(0x00000001);
            set => Buffs[7].SetValue(0x00000001, value);
        }

        /// <summary>
        ///     三转MP吸收
        /// </summary>
        public bool MPDrain3RD
        {
            get => Buffs[7].Test(0x00000002);
            set => Buffs[7].SetValue(0x00000002, value);
        }

        /// <summary>
        ///     三转SP吸收
        /// </summary>
        public bool SPDrain3RD
        {
            get => Buffs[7].Test(0x00000004);
            set => Buffs[7].SetValue(0x00000004, value);
        }

        public bool 无1
        {
            get => Buffs[7].Test(0x00000008);
            set => Buffs[7].SetValue(0x00000008, value);
        }

        public bool 无2
        {
            get => Buffs[7].Test(0x00000010);
            set => Buffs[7].SetValue(0x00000010, value);
        }

        public bool 无3
        {
            get => Buffs[7].Test(0x00000020);
            set => Buffs[7].SetValue(0x00000020, value);
        }

        public bool 三转波动伤害固定
        {
            get => Buffs[7].Test(0x00000040);
            set => Buffs[7].SetValue(0x00000040, value);
        }

        public bool 三转枪连弹
        {
            get => Buffs[7].Test(0x00000080);
            set => Buffs[7].SetValue(0x00000080, value);
        }

        public bool KillingMark
        {
            get => Buffs[7].Test(0x00000100);
            set => Buffs[7].SetValue(0x00000100, value);
        }

        public bool 三转ATK与MATK互换
        {
            get => Buffs[7].Test(0x00000200);
            set => Buffs[7].SetValue(0x00000200, value);
        }

        public bool 三转元素身体属性赋予
        {
            get => Buffs[7].Test(0x00000400);
            set => Buffs[7].SetValue(0x00000400, value);
        }

        public bool 三转元素武器属性赋予
        {
            get => Buffs[7].Test(0x00000800);
            set => Buffs[7].SetValue(0x00000800, value);
        }

        public bool 三转2足ATKUP
        {
            get => Buffs[7].Test(0x00001000);
            set => Buffs[7].SetValue(0x00001000, value);
        }

        public bool RobotUnknowStateDown3RD
        {
            get => Buffs[7].Test(0x00002000);
            set => Buffs[7].SetValue(0x00002000, value);
        }

        /// <summary>
        ///     FO物质拘束
        /// </summary>
        public bool WeaponFobbiden3RD
        {
            get => Buffs[7].Test(0x00004000);
            set => Buffs[7].SetValue(0x00004000, value);
        }

        /// <summary>
        ///     三转受伤害提升伤害标记
        /// </summary>
        public bool GetDamageUpDamageMark3RD
        {
            get => Buffs[7].Test(0x00008000);
            set => Buffs[7].SetValue(0x00008000, value);
        }

        /// <summary>
        ///     三转伤害降低精神标记
        /// </summary>
        public bool DamageReduceSpriteMark3RD
        {
            get => Buffs[7].Test(0x00010000);
            set => Buffs[7].SetValue(0x00010000, value);
        }

        /// <summary>
        ///     三转J速
        /// </summary>
        public bool JSpeed3RD
        {
            get => Buffs[7].Test(0x00020000);
            set => Buffs[7].SetValue(0x00020000, value);
        }

        /// <summary>
        ///     三转人血管
        /// </summary>
        public bool 三转人血管
        {
            get => Buffs[7].Test(0x00040000);
            set => Buffs[7].SetValue(0x00040000, value);
        }

        /// <summary>
        ///     三转荆棘刺
        /// </summary>
        public bool 三转荆棘刺
        {
            get => Buffs[7].Test(0x00080000);
            set => Buffs[7].SetValue(0x00080000, value);
        }

        /// <summary>
        ///     三转鬼人斩
        /// </summary>
        public bool DevilStance
        {
            get => Buffs[7].Test(0x00100000);
            set => Buffs[7].SetValue(0x00100000, value);
        }

        /// <summary>
        ///     三转宙斯盾イージス
        /// </summary>
        public bool Aegis3RD
        {
            get => Buffs[7].Test(0x00200000);
            set => Buffs[7].SetValue(0x00200000, value);
        }

        public bool 三转凭依者封印
        {
            get => Buffs[7].Test(0x00400000);
            set => Buffs[7].SetValue(0x00400000, value);
        }

        public bool 三转四属性赋予アンプリエレメント
        {
            get => Buffs[7].Test(0x00800000);
            set => Buffs[7].SetValue(0x00800000, value);
        }

        public bool 三转铁匠2足DEFUP
        {
            get => Buffs[7].Test(0x01000000);
            set => Buffs[7].SetValue(0x01000000, value);
        }

        public bool 三转机器人UNKNOWS
        {
            get => Buffs[7].Test(0x02000000);
            set => Buffs[7].SetValue(0x02000000, value);
        }

        public bool 三转禁言レストスキル
        {
            get => Buffs[7].Test(0x04000000);
            set => Buffs[7].SetValue(0x04000000, value);
        }

        public bool 三转指定对象被会心率UPクリティカルマーキング
        {
            get => Buffs[7].Test(0x08000000);
            set => Buffs[7].SetValue(0x08000000, value);
        }

        public bool 三转凭依保护ソウルプロテクト
        {
            get => Buffs[7].Test(0x10000000);
            set => Buffs[7].SetValue(0x10000000, value);
        }

        public bool 三转見切り
        {
            get => Buffs[7].Test(0x20000000);
            set => Buffs[7].SetValue(0x20000000, value);
        }

        public bool 三转魔法抗体
        {
            get => Buffs[7].Test(0x40000000);
            set => Buffs[7].SetValue(0x40000000, value);
        }

        #endregion
    }
}