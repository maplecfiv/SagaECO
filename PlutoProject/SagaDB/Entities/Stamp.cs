using System;
using SqlSugar;

namespace SagaDB.Entities;

public class Stamp {
    [SugarColumn(IsPrimaryKey = true)] public uint CharacterId { get; set; }
    [SugarColumn(IsPrimaryKey = true)] public byte StampId { get; set; }
    public int Value { get; set; }
}