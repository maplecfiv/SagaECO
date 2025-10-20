using System;
using SqlSugar;

namespace SagaDB.Entities;

public class DualJob {
    [SugarColumn(IsPrimaryKey = true)] public uint CharacterId { get; set; }

    [SugarColumn(IsPrimaryKey = true)] public byte SeriesId { get; set; }

    public byte Level { get; set; }

    public ulong Exp { get; set; }
}