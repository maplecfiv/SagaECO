using System;
using SqlSugar;

namespace SagaDB.Entities;

public class Inventory {
    [SugarColumn(IsPrimaryKey = true)] public uint CharacterId { get; set; }


    public byte[] Data { get; set; }
}