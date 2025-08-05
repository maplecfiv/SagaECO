using System.Collections.Generic;
using SagaDB.Mob;

namespace SagaDB.Marionette
{
    public enum GatherType
    {
        Plant,
        Mineral,
        Food,
        Magic,
        Treasurebox,
        Excavation,
        Any,
        Strange
    }

    /// <summary>
    ///     活动木偶
    /// </summary>
    public class Marionette
    {
        public short aspd, cspd;
        public short avoid_melee, avoid_ranged, avoid_magic, avoid_cri;
        public short def, def_add, mdef, mdef_add;
        public Dictionary<GatherType, bool> gather = new Dictionary<GatherType, bool>();
        public short hit_melee, hit_ranged, hit_magic, hit_cri;
        public short hp, mp, sp;
        public short hp_recover, mp_recover;
        public short min_atk1, min_atk2, min_atk3, max_atk1, max_atk2, max_atk3, min_matk, max_matk;
        public short move_speed;
        public List<ushort> skills = new List<ushort>();
        public short str, dex, vit, intel, agi, mag;

        /// <summary>
        ///     活动木偶的名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     活动木偶的ID
        /// </summary>
        public uint ID { get; set; }

        /// <summary>
        ///     活动木偶的显示ID
        /// </summary>
        public uint PictID { get; set; }

        /// <summary>
        ///     活动木偶的怪物类型
        /// </summary>
        public MobType MobType { get; set; }

        /// <summary>
        ///     变身时间
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        ///     变身延迟
        /// </summary>
        public int Delay { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}