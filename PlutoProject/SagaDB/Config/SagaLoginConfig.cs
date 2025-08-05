using System;
using SagaDB.Item;

namespace SagaDB.Config
{
    [Serializable]
    public class StartupSetting
    {
        public uint StartMap;
        public ushort Str, Dex, Int, Vit, Agi, Mag;
        public byte X, Y;

        public override string ToString()
        {
            return string.Format("Stats:[S:{0},D:{1},I:{2},V:{3},A:{4},M:{5}\r\n       StartPoint:[{6}({7},{8})]",
                Str, Dex, Int, Vit, Agi, Mag, StartMap, X, Y);
        }
    }

    [Serializable]
    public class StartItem
    {
        public byte Count;
        public uint ItemID;
        public ContainerType Slot;
    }
}