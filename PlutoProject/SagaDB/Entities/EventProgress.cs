using System;
using SqlSugar;

namespace SagaDB.Entities;

public class EventProgress {
    [SugarColumn(IsPrimaryKey = true)] public uint CharacterId { get; set; }
    [SugarColumn(IsPrimaryKey = true)] public uint EventId { get; set; }
    public uint Progress { get; set; }
    public DateTime LastUpdate { get; set; }
}