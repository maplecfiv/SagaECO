using System;
using SqlSugar;

namespace SagaDB.Entities;

public class ServerVariable {
    [SugarColumn(IsPrimaryKey = true)] public String Name { get; set; }
    [SugarColumn(IsPrimaryKey = true)] public byte Type { get; set; }
    public String Content { get; set; }
}