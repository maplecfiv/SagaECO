namespace SagaDB.DEMIC
{
    public class Chip
    {
        public Chip(BaseData baseData)
        {
            Data = baseData;
        }

        public short ChipID => Data.chipID;

        public uint ItemID => Data.itemID;

        public BaseData Data { get; }

        public Model Model => Data.model;

        public byte X { get; set; }

        public byte Y { get; set; }

        public bool IsNear(byte x, byte y)
        {
            foreach (var i in Model.Cells)
            {
                var X = this.X + i[0] - Model.CenterX;
                var Y = this.Y + i[1] - Model.CenterY;

                if (X == x + 1 && Y == y)
                    return true;
                if (X == x - 1 && Y == y)
                    return true;
                if (X == x && Y == y + 1)
                    return true;
                if (X == x && Y == y - 1)
                    return true;
            }

            return false;
        }

        public override string ToString()
        {
            return Data.name;
        }

        public class BaseData
        {
            public short chipID;
            public short engageTaskChip;
            public short hp, mp, sp;
            public uint itemID;
            public Model model;
            public string name;
            public byte possibleLv;
            public uint skill1, skill2, skill3;
            public short str, mag, vit, dex, agi, intel;
            public byte type;

            public override string ToString()
            {
                return name;
            }
        }
    }
}