using System;
using System.Collections.Generic;
using SagaLib;

namespace SagaDB.Item
{
    [Serializable]
    public class Another
    {
        public ushort agi;
        public ushort avoid_magic_add;
        public ushort avoid_melee_add;
        public uint awakeSkillID;
        public byte awakeSkillMaxLV;
        public ushort def_add;
        public ushort dex;
        public ushort hit_magic_add;
        public ushort hit_melee_add;
        public short hp_add;
        public uint id;
        public ushort ing;
        public byte lv;
        public ushort mag;
        public ushort max_atk_add;
        public ushort max_matk_add;
        public ushort mdef_add;
        public ushort min_atk_add;
        public ushort min_matk_add;
        public short mp_add;
        public string name;
        public List<uint> paperItems1 = new List<uint>();
        public List<uint> paperItems2 = new List<uint>();
        public uint requestItem1;
        public uint requestItem2;
        public Dictionary<uint, List<byte>> skills = new Dictionary<uint, List<byte>>();
        public short sp_add;
        public ushort str;
        public byte type;
        public ushort vit;
    }

    public class AnotherDetail
    {
        public byte lv;
        public Dictionary<uint, ulong> skills = new Dictionary<uint, ulong>();
        public BitMask_Long value;
    }
}