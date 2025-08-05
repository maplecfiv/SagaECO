namespace SagaDB.Actor
{
    public partial class Buff
    {
        #region Buffs

        public bool 三转机器人攻速上升
        {
            get => Buffs[8].Test(0x00000001);
            set => Buffs[8].SetValue(0x00000001, value);
        }

        public bool 三转机器人攻速下降
        {
            get => Buffs[8].Test(0x00000002);
            set => Buffs[8].SetValue(0x00000002, value);
        }

        public bool 三转ウィークネスショット
        {
            get => Buffs[8].Test(0x00000004);
            set => Buffs[8].SetValue(0x00000004, value);
        }

        public bool 点火紫火
        {
            get => Buffs[8].Test(0x00000008);
            set => Buffs[8].SetValue(0x00000008, value);
        }

        public bool Unknow27
        {
            get => Buffs[8].Test(0x00000010);
            set => Buffs[8].SetValue(0x00000010, value);
        }

        public bool 三转红锤子ウェポンエンハンス

        {
            get => Buffs[8].Test(0x00000020);
            set => Buffs[8].SetValue(0x00000020, value);
        }

        public bool 三转レトリック
        {
            get => Buffs[8].Test(0x00000040);
            set => Buffs[8].SetValue(0x00000040, value);
        }

        public bool 师匠的加护
        {
            get => Buffs[8].Test(0x00000080);
            set => Buffs[8].SetValue(0x00000080, value);
        }

        public bool 三转モンスターチェンジ
        {
            get => Buffs[8].Test(0x00000100);
            set => Buffs[8].SetValue(0x00000100, value);
        }

        public bool 三转知识的书
        {
            get => Buffs[8].Test(0x00000200);
            set => Buffs[8].SetValue(0x00000200, value);
        }

        public bool 三转植物寄生
        {
            get => Buffs[8].Test(0x00000400);
            set => Buffs[8].SetValue(0x00000400, value);
        }

        public bool 三转パクティオ
        {
            get => Buffs[8].Test(0x00000800);
            set => Buffs[8].SetValue(0x00000800, value);
        }

        public bool 三转アドバンスアビリテイー
        {
            get => Buffs[8].Test(0x00001000);
            set => Buffs[8].SetValue(0x00001000, value);
        }

        public bool 三转フエンリル
        {
            get => Buffs[8].Test(0x00002000);
            set => Buffs[8].SetValue(0x00002000, value);
        }

        public bool Blocking
        {
            get => Buffs[8].Test(0x00004000);
            set => Buffs[8].SetValue(0x00004000, value);
        }

        /// <summary>
        ///     FishingState
        /// </summary>
        public bool FishingState
        {
            get => Buffs[8].Test(0x00008000);
            set => Buffs[8].SetValue(0x00008000, value);
        }

        public bool 三转せーチウィークポイント
        {
            get => Buffs[8].Test(0x00010000);
            set => Buffs[8].SetValue(0x00010000, value);
        }

        public bool Unknow4
        {
            get => Buffs[8].Test(0x00020000);
            set => Buffs[8].SetValue(0x00020000, value);
        }

        /// <summary>
        ///     冒险3转JOB50技能BUFF
        /// </summary>
        public bool アートフルトラップ
        {
            get => Buffs[8].Test(0x00040000);
            set => Buffs[8].SetValue(0x00040000, value);
        }

        public bool Unknow6
        {
            get => Buffs[8].Test(0x00080000);
            set => Buffs[8].SetValue(0x00080000, value);
        }

        public bool Unknow7
        {
            get => Buffs[8].Test(0x00100000);
            set => Buffs[8].SetValue(0x00100000, value);
        }

        public bool Unknow8
        {
            get => Buffs[8].Test(0x00200000);
            set => Buffs[8].SetValue(0x00200000, value);
        }

        public bool Unknow9
        {
            get => Buffs[8].Test(0x00400000);
            set => Buffs[8].SetValue(0x00400000, value);
        }

        /// <summary>
        ///     流水攻势
        /// </summary>
        public bool SwordEaseSp
        {
            get => Buffs[8].Test(0x00800000);
            set => Buffs[8].SetValue(0x00800000, value);
        }

        public bool Unknow11
        {
            get => Buffs[8].Test(0x01000000);
            set => Buffs[8].SetValue(0x01000000, value);
        }

        public bool Unknow12
        {
            get => Buffs[8].Test(0x02000000);
            set => Buffs[8].SetValue(0x02000000, value);
        }

        public bool Unknow13
        {
            get => Buffs[8].Test(0x04000000);
            set => Buffs[8].SetValue(0x04000000, value);
        }

        public bool Unknow14
        {
            get => Buffs[8].Test(0x08000000);
            set => Buffs[8].SetValue(0x08000000, value);
        }

        public bool Unknow15
        {
            get => Buffs[8].Test(0x10000000);
            set => Buffs[8].SetValue(0x10000000, value);
        }

        /// <summary>
        ///     2转MP吸收
        /// </summary>
        public bool Unknow16
        {
            get => Buffs[8].Test(0x20000000);
            set => Buffs[8].SetValue(0x20000000, value);
        }

        public bool Unknow17
        {
            get => Buffs[8].Test(0x40000000);
            set => Buffs[8].SetValue(0x40000000, value);
        }

        #endregion
    }
}