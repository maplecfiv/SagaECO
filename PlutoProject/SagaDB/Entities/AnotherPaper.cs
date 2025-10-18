using System;
using SqlSugar;

namespace SagaDB.Entities;

public class AnotherPaper {
    [SugarColumn(IsPrimaryKey = true)] public Guid RecordId { get; set; }

    public uint PaperId { get; set; }

    public uint CharId { get; set; }

    public ulong PaperValue { get; set; }

    public byte PaperLv { get; set; }
}