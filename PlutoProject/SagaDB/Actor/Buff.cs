using SagaLib;

namespace SagaDB.Actor
{
    public partial class Buff
    {
        public BitMask[] Buffs { get; set; } = new BitMask[12]
        {
            new BitMask(), new BitMask(), new BitMask(), new BitMask(), new BitMask(), new BitMask(), new BitMask(),
            new BitMask(), new BitMask(), new BitMask(), new BitMask(), new BitMask()
        };

        public void Clear()
        {
            Buffs[0].Value = 0;
            Buffs[1].Value = 0;
            Buffs[2].Value = 0;
            Buffs[3].Value = 0;
            Buffs[4].Value = 0;
            Buffs[5].Value = 0;
            Buffs[6].Value = 0;
            Buffs[7].Value = 0;
            Buffs[8].Value = 0;
            Buffs[9].Value = 0;
            Buffs[10].Value = 0;
            Buffs[11].Value = 0;
        }
    }
}