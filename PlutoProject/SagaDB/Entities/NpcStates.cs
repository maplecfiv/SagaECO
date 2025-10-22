using System;
using SqlSugar;

namespace SagaDB.Entities;

public class NpcStates {
    [SugarColumn(IsPrimaryKey = true)] public uint CharacterId { get; set; }
    [SugarColumn(IsPrimaryKey = true)] public uint NpcId { get; set; }

    public bool State { get; set; }
}