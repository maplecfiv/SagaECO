using System;
using SqlSugar;

namespace SagaDB.Entities;

public class Friend {
    [SugarColumn(IsPrimaryKey = true)] public uint CharacterId { get; set; }
    [SugarColumn(IsPrimaryKey = true)] public uint FriendCharacterId { get; set; }
}