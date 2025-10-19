using System;
using SqlSugar;

namespace SagaDB.Entities;

public class Party {
    [SugarColumn(IsPrimaryKey = true)] public uint PartyId { get; set; }

    public string Name { get; set; }

    public uint Leader { get; set; }
}