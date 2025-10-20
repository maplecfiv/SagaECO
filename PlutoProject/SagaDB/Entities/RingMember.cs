using System;
using SqlSugar;

namespace SagaDB.Entities;

public class RingMember {
    [SugarColumn(IsPrimaryKey = true)] public uint RingId { get; set; }
    [SugarColumn(IsPrimaryKey = true)] public uint CharacterId { get; set; }
    public int Right { get; set; }
}