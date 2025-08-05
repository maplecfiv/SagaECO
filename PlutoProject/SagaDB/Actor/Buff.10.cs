namespace SagaDB.Actor
{
    public partial class Buff
    {
        public bool 魂之手
        {
            get => Buffs[9].Test(0x1);
            set => Buffs[9].SetValue(0x1, value);
        }

        public bool 精准攻击
        {
            get => Buffs[9].Test(0x2);
            set => Buffs[9].SetValue(0x2, value);
        }

        public bool 恶炎
        {
            get => Buffs[9].Test(0x4);
            set => Buffs[9].SetValue(0x4, value);
        }

        public bool 九尾狐魅惑
        {
            get => Buffs[9].Test(0x8);
            set => Buffs[9].SetValue(0x8, value);
        }

        public bool 武装化
        {
            get => Buffs[9].Test(0x10);
            set => Buffs[9].SetValue(0x10, value);
        }

        public bool 武装化副作用
        {
            get => Buffs[9].Test(0x20);
            set => Buffs[9].SetValue(0x20, value);
        }

        public bool 恶魂
        {
            get => Buffs[9].Test(0x40);
            set => Buffs[9].SetValue(0x40, value);
        }
    }
}