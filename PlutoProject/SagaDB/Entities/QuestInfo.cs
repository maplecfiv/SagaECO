using System;
using SqlSugar;

namespace SagaDB.Entities;

public class QuestInfo {
    [SugarColumn(IsPrimaryKey = true)] public Guid QuestInfoId { get; set; }

    public uint CharacterId { get; set; }

    public uint ObjectId { get; set; }
    public int Count { get; set; }

    public int TotalCount { get; set; }

    public byte Status { get; set; }
}