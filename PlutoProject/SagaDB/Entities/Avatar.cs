using System;
using SqlSugar;

namespace SagaDB.Entities;

public class Avatar {
    [SugarColumn(IsPrimaryKey = true)] public Guid RecordId { get; set; }

    [SugarColumn(IsPrimaryKey = true)] public uint AccountId { get; set; }

    public byte[] Valuess { get; set; }
}