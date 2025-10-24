using System;
using SqlSugar;

namespace SagaDB.Entities;

public class ApiItem {
    [SugarColumn(IsPrimaryKey = true)] public Guid ApiItemId { get; set; }

    public uint CharacterId { get; set; }
    public uint ItemId { get; set; }
    public ushort Qty { get; set; }

    public DateTime ProcessTime { get; set; }
    public DateTime RequestTime { get; set; }
    public byte Status { get; set; }
}