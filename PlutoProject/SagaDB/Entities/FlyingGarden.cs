using System;
using SqlSugar;

namespace SagaDB.Entities;

public class FlyingGarden {
    [SugarColumn(IsPrimaryKey = true)] public uint FlyingGardenId { get; set; }
    public uint AccountId { get; set; }

    public uint Part1 { get; set; }
    public uint Part2 { get; set; }
    public uint Part3 { get; set; }
    public uint Part4 { get; set; }
    public uint Part5 { get; set; }
    public uint Part6 { get; set; }
    public uint Part7 { get; set; }
    public uint Part8 { get; set; }
    public uint Fuel { get; set; }
}