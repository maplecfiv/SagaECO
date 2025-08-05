namespace SagaDB.Actor
{
    public partial class Buff
    {
        #region Buffs

        public bool Poison
        {
            get => Buffs[0].Test(0x1);
            set => Buffs[0].SetValue(0x1, value);
        }

        public bool Stone
        {
            get => Buffs[0].Test(0x2);
            set => Buffs[0].SetValue(0x2, value);
        }

        public bool Paralysis
        {
            get => Buffs[0].Test(0x4);
            set => Buffs[0].SetValue(0x4, value);
        }

        public bool Sleep
        {
            get => Buffs[0].Test(0x8);
            set => Buffs[0].SetValue(0x8, value);
        }

        public bool Silence
        {
            get => Buffs[0].Test(0x10);
            set => Buffs[0].SetValue(0x10, value);
        }

        public bool SpeedDown
        {
            get => Buffs[0].Test(0x20);
            set => Buffs[0].SetValue(0x20, value);
        }

        public bool Confused
        {
            get => Buffs[0].Test(0x40);
            set => Buffs[0].SetValue(0x40, value);
        }

        public bool Frosen
        {
            get => Buffs[0].Test(0x80);
            set => Buffs[0].SetValue(0x80, value);
        }

        public bool Stun
        {
            get => Buffs[0].Test(0x100);
            set => Buffs[0].SetValue(0x100, value);
        }

        public bool Dead
        {
            get => Buffs[0].Test(0x200);
            set => Buffs[0].SetValue(0x200, value);
        }

        public bool CannotMove
        {
            get => Buffs[0].Test(0x400);
            set => Buffs[0].SetValue(0x400, value);
        }

        public bool PoisonResist
        {
            get => Buffs[0].Test(0x800);
            set => Buffs[0].SetValue(0x800, value);
        }

        public bool StoneResist
        {
            get => Buffs[0].Test(0x1000);
            set => Buffs[0].SetValue(0x1000, value);
        }

        public bool ParalysisResist
        {
            get => Buffs[0].Test(0x2000);
            set => Buffs[0].SetValue(0x2000, value);
        }

        public bool SleepResist
        {
            get => Buffs[0].Test(0x4000);
            set => Buffs[0].SetValue(0x4000, value);
        }

        public bool SilenceResist
        {
            get => Buffs[0].Test(0x8000);
            set => Buffs[0].SetValue(0x8000, value);
        }

        public bool SpeedDownResist
        {
            get => Buffs[0].Test(0x10000);
            set => Buffs[0].SetValue(0x10000, value);
        }

        public bool ConfuseResist
        {
            get => Buffs[0].Test(0x20000);
            set => Buffs[0].SetValue(0x20000, value);
        }

        public bool FrosenResist
        {
            get => Buffs[0].Test(0x40000);
            set => Buffs[0].SetValue(0x40000, value);
        }

        public bool FaintResist
        {
            get => Buffs[0].Test(0x80000);
            set => Buffs[0].SetValue(0x80000, value);
        }

        public bool Sit
        {
            get => Buffs[0].Test(0x100000);
            set => Buffs[0].SetValue(0x100000, value);
        }

        public bool Spirit
        {
            get => Buffs[0].Test(0x200000);
            set => Buffs[0].SetValue(0x200000, value);
        }

        #endregion
    }
}