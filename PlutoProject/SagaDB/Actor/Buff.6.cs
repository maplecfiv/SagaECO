namespace SagaDB.Actor
{
    public partial class Buff
    {
        #region Buffs

        /// <summary>
        ///     Zombie
        /// </summary>
        public bool Zombie
        {
            get => Buffs[5].Test(0x00000001);
            set => Buffs[5].SetValue(0x00000001, value);
        }

        /// <summary>
        ///     リボーン
        /// </summary>
        public bool Reborn
        {
            get => Buffs[5].Test(0x00000002);
            set => Buffs[5].SetValue(0x00000002, value);
        }

        /// <summary>
        ///     演奏中
        /// </summary>
        public bool Playing
        {
            get => Buffs[5].Test(0x00000004);
            set => Buffs[5].SetValue(0x00000004, value);
        }

        /// <summary>
        /// </summary>
        public bool 羽交い絞め
        {
            get => Buffs[5].Test(0x00000008);
            set => Buffs[5].SetValue(0x00000008, value);
        }

        /// <summary>
        ///     光魔法封印
        /// </summary>
        public bool LightMagicSealed
        {
            get => Buffs[5].Test(0x00000010);
            set => Buffs[5].SetValue(0x00000010, value);
        }

        /// <summary>
        ///     オーバーレンジ
        /// </summary>
        public bool OverRange
        {
            get => Buffs[5].Test(0x00000020);
            set => Buffs[5].SetValue(0x00000020, value);
        }

        /// <summary>
        ///     2转吸血
        /// </summary>
        public bool LifeTake
        {
            get => Buffs[5].Test(0x00000040);
            set => Buffs[5].SetValue(0x00000040, value);
        }

        /// <summary>
        ///     恐怖
        /// </summary>
        public bool Horrible
        {
            get => Buffs[5].Test(0x00000080);
            set => Buffs[5].SetValue(0x00000080, value);
        }

        /// <summary>
        ///     経験値上昇
        /// </summary>
        public bool EXPUp
        {
            get => Buffs[5].Test(0x00000100);
            set => Buffs[5].SetValue(0x00000100, value);
        }

        /// <summary>
        ///     パッシング
        /// </summary>
        public bool Passing
        {
            get => Buffs[5].Test(0x00000200);
            set => Buffs[5].SetValue(0x00000200, value);
        }

        /// <summary>
        ///     回復不可能
        /// </summary>
        public bool NoRegen
        {
            get => Buffs[5].Test(0x00000400);
            set => Buffs[5].SetValue(0x00000400, value);
        }

        /// <summary>
        ///     エンチャントブロック
        /// </summary>
        public bool EnchantmentBlock
        {
            get => Buffs[5].Test(0x00000800);
            set => Buffs[5].SetValue(0x00000800, value);
        }

        /// <summary>
        ///     ソリッドボディ
        /// </summary>
        public bool SolidBody
        {
            get => Buffs[5].Test(0x00001000);
            set => Buffs[5].SetValue(0x00001000, value);
        }

        /// <summary>
        ///     ブラッディウエポン
        /// </summary>
        public bool BloodyWeapon
        {
            get => Buffs[5].Test(0x00002000);
            set => Buffs[5].SetValue(0x00002000, value);
        }

        /// <summary>
        ///     フレア
        /// </summary>
        public bool Flare
        {
            get => Buffs[5].Test(0x00004000);
            set => Buffs[5].SetValue(0x00004000, value);
        }

        /// <summary>
        ///     ガンディレイキャンセル
        /// </summary>
        public bool GunDelayCancel
        {
            get => Buffs[5].Test(0x00008000);
            set => Buffs[5].SetValue(0x00008000, value);
        }

        /// <summary>
        ///     ダブルアップ
        /// </summary>
        public bool DoubleUp
        {
            get => Buffs[5].Test(0x00010000);
            set => Buffs[5].SetValue(0x00010000, value);
        }

        /// <summary>
        ///     ATフィールド
        /// </summary>
        public bool ATField
        {
            get => Buffs[5].Test(0x00020000);
            set => Buffs[5].SetValue(0x00020000, value);
        }

        /// <summary>
        ///     根性
        /// </summary>
        public bool Spirit3RD
        {
            get => Buffs[5].Test(0x00040000);
            set => Buffs[5].SetValue(0x00040000, value);
        }

        /// <summary>
        ///     物理攻撃付加
        /// </summary>
        public bool PhysicAtkAddition
        {
            get => Buffs[5].Test(0x00080000);
            set => Buffs[5].SetValue(0x00080000, value);
        }

        /// <summary>
        ///     死んだふり
        /// </summary>
        public bool PlayingDead
        {
            get => Buffs[5].Test(0x00100000);
            set => Buffs[5].SetValue(0x00100000, value);
        }

        /// <summary>
        ///     パパ点火
        /// </summary>
        public bool PapaIgintion
        {
            get => Buffs[5].Test(0x00200000);
            set => Buffs[5].SetValue(0x00200000, value);
        }

        /// <summary>
        ///     TurningPurple
        /// </summary>
        public bool TurningPurple
        {
            get => Buffs[5].Test(0x00400000);
            set => Buffs[5].SetValue(0x00400000, value);
        }

        /// <summary>
        ///     精密射撃
        /// </summary>
        public bool PrecisionFire
        {
            get => Buffs[5].Test(0x00800000);
            set => Buffs[5].SetValue(0x00800000, value);
        }

        /// <summary>
        ///     オーバーチューン
        /// </summary>
        public bool OverTune
        {
            get => Buffs[5].Test(0x01000000);
            set => Buffs[5].SetValue(0x01000000, value);
        }

        /// <summary>
        ///     警戒
        /// </summary>
        public bool Warning
        {
            get => Buffs[5].Test(0x02000000);
            set => Buffs[5].SetValue(0x02000000, value);
        }

        /// <summary>
        ///     リフレクション
        /// </summary>
        public bool Reflection
        {
            get => Buffs[5].Test(0x04000000);
            set => Buffs[5].SetValue(0x04000000, value);
        }

        /// <summary>
        ///     エンチャントウエポン
        /// </summary>
        public bool EnchantWeapon
        {
            get => Buffs[5].Test(0x08000000);
            set => Buffs[5].SetValue(0x08000000, value);
        }

        /// <summary>
        ///     邪恶灵魂
        /// </summary>
        public bool Oritorio
        {
            get => Buffs[5].Test(0x10000000);
            set => Buffs[5].SetValue(0x10000000, value);
        }

        public bool イビルソウル
        {
            get => Buffs[5].Test(0x20000000);
            set => Buffs[5].SetValue(0x20000000, value);
        }

        /// <summary>
        ///     フレイムハート
        /// </summary>
        public bool FlameHart
        {
            get => Buffs[5].Test(0x40000000);
            set => Buffs[5].SetValue(0x40000000, value);
        }

        /*public bool アトラクトマーチ
        {
            get
            {
                return buffs[5].Test(0x80000000);
            }
            set
            {
                buffs[5].SetValue(0x80000000, value);
            }
        }*/

        #endregion
    }
}