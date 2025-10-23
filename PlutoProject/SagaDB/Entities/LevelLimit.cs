using System;
using SqlSugar;

namespace SagaDB.Entities;

public class LevelLimit {
    [SugarColumn(IsPrimaryKey = true)] public Guid RecordId { get; set; }
    public uint NowLevelLimit { get; set; }
    public uint NextLevelLimit { get; set; }
    public uint SetNextUpLevelLimit { get; set; }
    public uint SetNextUpDays { get; set; }
    public DateTime ReachTime { get; set; }
    public DateTime NextTime { get; set; }
    public uint FirstPlayer { get; set; }
    public uint SecondPlayer { get; set; }
    public uint Thirdlayer { get; set; }
    public uint FourthPlayer { get; set; }
    public uint FifthPlayer { get; set; }
    public uint LastTimeLevelLimit { get; set; }
    public byte IsLock { get; set; }
}