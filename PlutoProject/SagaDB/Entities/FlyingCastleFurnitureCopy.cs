using System;
using SqlSugar;

namespace SagaDB.Entities;

public class FlyingCastleFurnitureCopy {
    [SugarColumn(IsPrimaryKey = true)] public uint FlyingCastleId { get; set; }

    public byte Place { get; set; }

    public uint ItemId { get; set; }

    public uint PictId { get; set; }

    public short X { get; set; }

    public short Y { get; set; }

    public short Z { get; set; }

    public short AxisX { get; set; }

    public short AxisY { get; set; }

    public short AxisZ { get; set; }

    public string Name { get; set; }

    public ushort Motion { get; set; }
}