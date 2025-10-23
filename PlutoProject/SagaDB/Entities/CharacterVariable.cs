using System;
using SqlSugar;

namespace SagaDB.Entities;

public class CharacterVariable {
    [SugarColumn(IsPrimaryKey = true)] public uint CharacterId { get; set; }
    public byte[] Values { get; set; }
}