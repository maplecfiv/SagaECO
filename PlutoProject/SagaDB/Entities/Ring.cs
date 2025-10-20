using System;
using SqlSugar;

namespace SagaDB.Entities;

public class Ring {
    [SugarColumn(IsPrimaryKey = true)] public uint RingId { get; set; }

    public uint Leader { get; set; }

    public string Name { get; set; }

    public uint Fame { get; set; }

    public uint FfId { get; set; }

    public byte[] Emblem { get; set; }

    public DateTime EmblemDate { get; set; }
}