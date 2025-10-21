using System;
using SqlSugar;

namespace SagaDB.Entities;

public class PartnerEquip
{
    [SugarColumn(IsPrimaryKey = true)] public uint ActorPartnerId { get; set; }

    public ushort Count { get; set; }
    public byte Type { get; set; }
    public uint ItemId { get; set; }
}