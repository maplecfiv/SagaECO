using System;
using SqlSugar;

namespace SagaDB.Entities;

public class Partner
{
    [SugarColumn(IsPrimaryKey = true)] public uint ActorPartnerId { get; set; }

    public uint PartnerId { get; set; }
    public string Name { get; set; }
    public byte Level { get; set; }
    public byte TrustLevel { get; set; }
    public byte Rebirth { get; set; }
    public byte Rank { get; set; }
    public ushort PerkPoints { get; set; }
    public byte Perk0 { get; set; }
    public byte Perk1 { get; set; }
    public byte Perk2 { get; set; }
    public byte Perk3 { get; set; }
    public byte Perk4 { get; set; }
    public byte Perk5 { get; set; }
    public byte AiMode { get; set; }
    public byte BasicAiMode1 { get; set; }
    public byte BasicAiMode2 { get; set; }
    public uint Hp { get; set; }
    public uint MaxHp { get; set; }
    public uint Mp { get; set; }
    public uint MaxMp { get; set; }
    public uint Sp { get; set; }
    public uint MaxSp { get; set; }

    public ulong Exp { get; set; }

    public uint PictId { get; set; }

    public DateTime NextFeedTime { get; set; }

    public ushort ReliabilityUprate { get; set; }

    public ulong TrustExp { get; set; }
}