using System;

namespace SagaDB.Tamaire
{
    public class TamaireRental
    {
        public short def, mdef, hit_melee, hit_range, avoid_melee, avoid_range, aspd, cspd, payload, capacity;

        public short hp, sp, mp, atk_min, atk_max, matk_min, matk_max;

        public uint Renter { get; set; }

        public uint CurrentLender { get; set; }

        public uint LastLender { get; set; }

        public byte LevelDiff { get; set; }

        public DateTime RentDue { get; set; }
    }
}