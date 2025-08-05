namespace SagaMap.Dungeon
{
    public enum GateType
    {
        Entrance,
        East,
        West,
        South,
        North,
        Central,
        Exit
    }

    public enum Direction
    {
        In,
        Out
    }

    public class DungeonGate
    {
        public GateType GateType { get; set; }

        public byte X { get; set; }

        public byte Y { get; set; }

        public uint NPCID { get; set; }

        public DungeonMap ConnectedMap { get; set; }

        public Direction Direction { get; set; }

        public DungeonGate Clone()
        {
            var gate = new DungeonGate();
            gate.GateType = GateType;
            gate.X = X;
            gate.Y = Y;
            gate.NPCID = NPCID;
            return gate;
        }
    }
}