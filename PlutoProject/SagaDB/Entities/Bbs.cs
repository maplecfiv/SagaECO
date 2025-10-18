using System;
using SqlSugar;

namespace SagaDB.Entities;

public class Bbs {
    [SugarColumn(IsPrimaryKey = true)] public Guid PostId { get; set; }

    public uint BbsId { get; set; }

    public DateTime PostDate { get; set; }

    public uint CharId { get; set; }

    public string Name { get; set; }

    public string Title { get; set; }

    public string Content { get; set; }
}