using System;
using SqlSugar;

namespace SagaDB.Entities;

public class Skill {
    [SugarColumn(IsPrimaryKey = true)] public uint CharacterId { get; set; }
    public byte[] Skills { get; set; }
    [SugarColumn(IsPrimaryKey = true)] public int JobBasic { get; set; }
    public byte JobLevel { get; set; }
    public ulong JobExp { get; set; }
    public ushort SkillPoint { get; set; }
    public ushort SkillPoint2X { get; set; }
    public ushort SkillPoint2T { get; set; }
    public ushort SkillPoint3 { get; set; }
}