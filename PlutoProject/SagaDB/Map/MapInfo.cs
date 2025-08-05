using System.Collections.Generic;
using SagaDB.Marionette;
using SagaLib;

namespace SagaDB.Map
{
    public enum MapFlags
    {
        Healing = 0x1,
        Cold = 0x2,
        Hot = 0x4,
        Wet = 0x8,
        Wrp = 0x10,
        Dominion = 0x20,
        FGarden = 0x40
    }

    public class MapInfo
    {
        public uint[,] canfish;
        public byte[,] dark;
        public byte[,] earth;
        public Dictionary<uint, byte[]> events = new Dictionary<uint, byte[]>();
        public byte[,] fire;

        public BitMask<MapFlags> Flag = new BitMask<MapFlags>(new BitMask());


        public Dictionary<GatherType, int> gatherInterval = new Dictionary<GatherType, int>();
        public ushort height;
        public byte[,] holy;
        public uint id;
        public string name;
        public byte[,] neutral;

        public int[,] unknown;
        public byte[,] unknown14;
        public byte[,] unknown15;
        public byte[,] unknown16;
        public byte[,] walkable; //(0=進入不可 2=進入可 4=向こう側が見えない 8=? 10=?
        public byte[,] water;
        public ushort width;
        public byte[,] wind;

        public bool Healing => Flag.Test(MapFlags.Healing);
        public bool Cold => Flag.Test(MapFlags.Cold);
        public bool Hot => Flag.Test(MapFlags.Hot);
        public bool Wet => Flag.Test(MapFlags.Wet);

        public override string ToString()
        {
            return name;
        }
    }
}