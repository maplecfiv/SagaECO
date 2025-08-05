namespace SagaDB.Actor
{
    public partial class Buff
    {
        #region Buffs

        /// <summary>
        ///     武器の無属性上昇
        /// </summary>
        public bool WeaponNatureElementUp
        {
            get => Buffs[2].Test(0x1);
            set => Buffs[2].SetValue(0x1, value);
        }

        /// <summary>
        ///     武器の火属性上昇
        /// </summary>
        public bool WeaponFireElementUp
        {
            get => Buffs[2].Test(0x2);
            set => Buffs[2].SetValue(0x2, value);
        }

        /// <summary>
        ///     武器の水属性上昇
        /// </summary>
        public bool WeaponWaterElementUp
        {
            get => Buffs[2].Test(0x4);
            set => Buffs[2].SetValue(0x4, value);
        }

        /// <summary>
        ///     武器の水属性上昇
        /// </summary>
        public bool WeaponWindElementUp
        {
            get => Buffs[2].Test(0x8);
            set => Buffs[2].SetValue(0x8, value);
        }

        /// <summary>
        ///     武器の土属性上昇
        /// </summary>
        public bool WeaponEarthElementUp
        {
            get => Buffs[2].Test(0x10);
            set => Buffs[2].SetValue(0x10, value);
        }

        /// <summary>
        ///     武器の光属性上昇
        /// </summary>
        public bool WeaponHolyElementUp
        {
            get => Buffs[2].Test(0x20);
            set => Buffs[2].SetValue(0x20, value);
        }

        /// <summary>
        ///     武器の闇属性上昇
        /// </summary>
        public bool WeaponDarkElementUp
        {
            get => Buffs[2].Test(0x40);
            set => Buffs[2].SetValue(0x40, value);
        }

        /// <summary>
        ///     武器の無属性減少
        /// </summary>
        public bool WeaponNatureElementDown
        {
            get => Buffs[2].Test(0x00000080);
            set => Buffs[2].SetValue(0x00000080, value);
        }

        /// <summary>
        ///     武器の火属性減少
        /// </summary>
        public bool WeaponFireElementDown
        {
            get => Buffs[2].Test(0x00000100);
            set => Buffs[2].SetValue(0x00000100, value);
        }

        /// <summary>
        ///     武器の水属性減少
        /// </summary>
        public bool WeaponWaterElementDown
        {
            get => Buffs[2].Test(0x00000200);
            set => Buffs[2].SetValue(0x00000200, value);
        }

        /// <summary>
        ///     武器の風属性減少
        /// </summary>
        public bool WeaponWindElementDown
        {
            get => Buffs[2].Test(0x00000400);
            set => Buffs[2].SetValue(0x00000400, value);
        }

        /// <summary>
        ///     武器の土属性減少
        /// </summary>
        public bool WeaponEarthElementDown
        {
            get => Buffs[2].Test(0x00000800);
            set => Buffs[2].SetValue(0x00000800, value);
        }

        /// <summary>
        ///     武器の光属性減少
        /// </summary>
        public bool WeaponLightElementDown
        {
            get => Buffs[2].Test(0x00001000);
            set => Buffs[2].SetValue(0x00001000, value);
        }

        /// <summary>
        ///     武器の闇属性減少
        /// </summary>
        public bool WeaponDarkElementDown
        {
            get => Buffs[2].Test(0x00002000);
            set => Buffs[2].SetValue(0x00002000, value);
        }

        /// <summary>
        ///     体の無属性上昇
        /// </summary>
        public bool BodyNatureElementUp
        {
            get => Buffs[2].Test(0x4000);
            set => Buffs[2].SetValue(0x4000, value);
        }

        /// <summary>
        ///     体の火属性上昇
        /// </summary>
        public bool BodyFireElementUp
        {
            get => Buffs[2].Test(0x8000);
            set => Buffs[2].SetValue(0x8000, value);
        }

        /// <summary>
        ///     体の水属性上昇
        /// </summary>
        public bool BodyWaterElementUp
        {
            get => Buffs[2].Test(0x10000);
            set => Buffs[2].SetValue(0x10000, value);
        }

        /// <summary>
        ///     体の風属性上昇
        /// </summary>
        public bool BodyWindElementUp
        {
            get => Buffs[2].Test(0x20000);
            set => Buffs[2].SetValue(0x20000, value);
        }

        /// <summary>
        ///     体の土属性上昇
        /// </summary>
        public bool BodyEarthElementUp
        {
            get => Buffs[2].Test(0x40000);
            set => Buffs[2].SetValue(0x40000, value);
        }

        /// <summary>
        ///     体の光属性上昇
        /// </summary>
        public bool BodyHolyElementUp
        {
            get => Buffs[2].Test(0x80000);
            set => Buffs[2].SetValue(0x80000, value);
        }

        /// <summary>
        ///     体の闇属性上昇
        /// </summary>
        public bool BodyDarkElementUp
        {
            get => Buffs[2].Test(0x100000);
            set => Buffs[2].SetValue(0x100000, value);
        }

        /// <summary>
        ///     体の無属性減少
        /// </summary>
        public bool BodyNatureElementDown
        {
            get => Buffs[2].Test(0x00200000);
            set => Buffs[2].SetValue(0x00200000, value);
        }

        /// <summary>
        ///     体の火属性減少
        /// </summary>
        public bool BodyFireElementDown
        {
            get => Buffs[2].Test(0x00400000);
            set => Buffs[2].SetValue(0x00400000, value);
        }

        /// <summary>
        ///     体の火属性減少
        /// </summary>
        public bool BodyWaterElementDown
        {
            get => Buffs[2].Test(0x100000);
            set => Buffs[2].SetValue(0x100000, value);
        }

        /// <summary>
        ///     体の風属性減少
        /// </summary>
        public bool BodyWindElementDown
        {
            get => Buffs[2].Test(0x01000000);
            set => Buffs[2].SetValue(0x01000000, value);
        }

        /// <summary>
        ///     体の土属性減少
        /// </summary>
        public bool BodyEarthElementDown
        {
            get => Buffs[2].Test(0x02000000);
            set => Buffs[2].SetValue(0x02000000, value);
        }

        /// <summary>
        ///     体の光属性減少
        /// </summary>
        public bool BodyLightElementDown
        {
            get => Buffs[2].Test(0x04000000);
            set => Buffs[2].SetValue(0x04000000, value);
        }

        /// <summary>
        ///     体の闇属性減少
        /// </summary>
        public bool BodyDarkElementDown
        {
            get => Buffs[2].Test(0x08000000);
            set => Buffs[2].SetValue(0x08000000, value);
        }

        #endregion
    }
}