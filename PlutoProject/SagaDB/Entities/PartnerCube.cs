using System;
using SqlSugar;

namespace SagaDB.Entities;

public class PartnerCube
{
    [SugarColumn(IsPrimaryKey = true)] public uint ActorPartnerId { get; set; }

    public ushort UniqueId { get; set; }
    public byte Type { get; set; }
}