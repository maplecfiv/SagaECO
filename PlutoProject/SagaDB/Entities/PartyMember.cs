using System;
using SqlSugar;

namespace SagaDB.Entities;

public class PartyMember {
    [SugarColumn(IsPrimaryKey = true)] public Guid RecordId { get; set; }

    public uint PartyId { get; set; }

    public uint CharId { get; set; }
}