using System;
using SqlSugar;

namespace SagaDB.Entities;

public class MobStates {
    [SugarColumn(IsPrimaryKey = true)] public uint CharacterId { get; set; }
    [SugarColumn(IsPrimaryKey = true)] public uint MobId { get; set; }

    public bool State { get; set; }
}