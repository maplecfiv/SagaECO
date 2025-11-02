using System;
using SqlSugar;

namespace SagaDB.Entities;

public class Warehouse {
    [SugarColumn(IsPrimaryKey = true)] public uint AccountId { get; set; }


    public byte[] Data { get; set; }
}