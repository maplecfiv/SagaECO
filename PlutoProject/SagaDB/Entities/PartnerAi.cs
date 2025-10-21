using System;
using SqlSugar;

namespace SagaDB.Entities;

public class PartnerAi
{
    [SugarColumn(IsPrimaryKey = true)] public uint ActorPartnerId { get; set; }

    public byte Index { get; set; }
    public byte Type { get; set; }
    public ushort Value { get; set; }
}