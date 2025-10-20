using System;
using SqlSugar;

namespace SagaDB.Entities;

public class DualJobSkill {
    [SugarColumn(IsPrimaryKey = true)] public uint CharacterId { get; set; }

    [SugarColumn(IsPrimaryKey = true)] public byte SeriesId { get; set; }

    [SugarColumn(IsPrimaryKey = true)] public uint SkillId { get; set; }

    public byte SkillLevel { get; set; }
}