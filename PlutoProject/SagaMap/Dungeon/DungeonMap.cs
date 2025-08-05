using System.Collections.Generic;

namespace SagaMap.Dungeon
{
    public enum MapType
    {
        Start,
        End,
        Room,
        Cross,
        Floor
    }

    public enum Theme
    {
        Normal,
        Maimai,
        Yard
    }

    public class DungeonMap
    {
        public uint ID { get; set; }

        public MapType MapType { get; set; }

        public Theme Theme { get; set; }

        public Dictionary<GateType, DungeonGate> Gates { get; } = new Dictionary<GateType, DungeonGate>();

        public Map Map { get; set; }

        public byte Dir { get; private set; } = 4;

        public byte X { get; set; }

        public byte Y { get; set; }

        public int FreeGates
        {
            get
            {
                var j = 0;
                foreach (var i in Gates.Values)
                {
                    if (i.GateType == GateType.Entrance ||
                        i.GateType == GateType.Central ||
                        i.GateType == GateType.Exit)
                        continue;
                    if (i.ConnectedMap == null)
                        j++;
                }

                return j;
            }
        }

        public DungeonMap Clone()
        {
            var newMap = new DungeonMap();
            newMap.ID = ID;
            newMap.MapType = MapType;
            newMap.Theme = Theme;
            foreach (var i in Gates.Keys) newMap.Gates.Add(i, Gates[i].Clone());
            return newMap;
        }

        public byte GetXForGate(GateType type)
        {
            if (Gates.ContainsKey(type))
                switch (type)
                {
                    case GateType.North:
                        return X;
                    case GateType.East:
                        return (byte)(X + 1);
                    case GateType.South:
                        return X;
                    case GateType.West:
                        return (byte)(X - 1);
                    default:
                        return 255;
                }

            return 255;
        }

        public byte GetYForGate(GateType type)
        {
            if (Gates.ContainsKey(type))
                switch (type)
                {
                    case GateType.North:
                        return (byte)(Y - 1);
                    case GateType.East:
                        return Y;
                    case GateType.South:
                        return (byte)(Y + 1);
                    case GateType.West:
                        return Y;
                    default:
                        return 255;
                }

            return 255;
        }

        public void Rotate()
        {
            Dir = (byte)((Dir + 2) % 8);
            DungeonGate east = null, south = null, west = null, north = null;
            if (Gates.ContainsKey(GateType.North))
                north = Gates[GateType.North];
            if (Gates.ContainsKey(GateType.East))
                east = Gates[GateType.East];
            if (Gates.ContainsKey(GateType.South))
                south = Gates[GateType.South];
            if (Gates.ContainsKey(GateType.West))
                west = Gates[GateType.West];
            Gates.Clear();
            if (north != null)
            {
                north.GateType = GateType.West;
                Gates.Add(GateType.West, north);
            }

            if (east != null)
            {
                east.GateType = GateType.North;
                Gates.Add(GateType.North, east);
            }

            if (south != null)
            {
                south.GateType = GateType.East;
                Gates.Add(GateType.East, south);
            }

            if (west != null)
            {
                west.GateType = GateType.South;
                Gates.Add(GateType.South, west);
            }
        }
    }
}