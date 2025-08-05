using System.Collections.Generic;

namespace SagaDB.Title
{
    public class Title
    {
        public Dictionary<uint, ushort> Bonus = new Dictionary<uint, ushort>();
        public int cri, cri_avoid, aspd, cspd;
        public int def, mdef, hit_melee, hit_range, hit_magic, avoid_melee, avoid_range, avoid_magic;
        public byte difficulty;
        public int hp, sp, mp, atk_min, atk_max, matk_min, matk_max;
        public uint ID;
        public string name, category;
        public uint PrerequisiteCount;
        public Dictionary<uint, ulong> Prerequisites = new Dictionary<uint, ulong>();

        public override string ToString()
        {
            return name;
        }
    }
}