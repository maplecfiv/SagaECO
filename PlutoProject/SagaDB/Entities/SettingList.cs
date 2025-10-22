using System;
using SqlSugar;

namespace SagaDB.Entities;

public class SettingList {
    [SugarColumn(IsPrimaryKey = true)] public String Name { get; set; }
    [SugarColumn(IsPrimaryKey = true)] public String Key { get; set; }
    public byte Type { get; set; }
    public int Content { get; set; }
}