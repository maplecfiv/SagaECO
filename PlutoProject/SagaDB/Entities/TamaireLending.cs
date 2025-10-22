using System;
using SqlSugar;

namespace SagaDB.Entities;

public class TamaireLending {
    [SugarColumn(IsPrimaryKey = true)] public uint CharacterId { get; set; }

    public byte BaseLevel { get; set; }
    public byte JobType { get; set; }

    public DateTime PostDue { get; set; }

    public string Comment { get; set; }

    public uint Renter1 { get; set; }

    public uint Renter2 { get; set; }

    public uint Renter3 { get; set; }

    public uint Renter4 { get; set; }
}