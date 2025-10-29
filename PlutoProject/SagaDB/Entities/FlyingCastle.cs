using System;
using SqlSugar;

namespace SagaDB.Entities;

public class FlyingCastle {
    [SugarColumn(IsPrimaryKey = true)] public uint FfId { get; set; }

    public uint RingId { get; set; }

    public string Name { get; set; }

    public string Content { get; set; }

    public uint Level { get; set; }

    [SugarColumn(IsNullable = true)] public byte ObMode { get; set; }
}