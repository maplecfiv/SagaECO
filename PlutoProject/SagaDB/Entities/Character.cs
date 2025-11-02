using System;
using SqlSugar;

namespace SagaDB.Entities;

public class Character {
    [SugarColumn(IsPrimaryKey = true)] public uint CharacterId { get; set; }

    public uint AccountId { get; set; }

    public string Name { get; set; }

    public string FirstName { get; set; }

    public byte ShowFirstName { get; set; }

    public byte Race { get; set; }

    public ushort UsingPaperId { get; set; }

    public ushort TitleId { get; set; }

    public byte Gender { get; set; }

    public ushort HairStyle { get; set; }

    public byte HairColor { get; set; }

    public ushort Wig { get; set; }

    public ushort Face { get; set; }

    public byte Job { get; set; }

    public byte Lv { get; set; }

    public byte Lv1 { get; set; }

    public byte JointJobLv { get; set; }

    public byte JobLv1 { get; set; }

    public byte JobLv2x { get; set; }

    public byte JobLv2t { get; set; }

    public byte JobLv3 { get; set; }

    public ushort QuestRemaining { get; set; }

    public uint Fame { get; set; }

    public DateTime QuestResetTime { get; set; }

    public byte Slot { get; set; }

    public uint MapId { get; set; }

    public byte X { get; set; }

    public byte Y { get; set; }

    public uint SaveMap { get; set; }

    public byte SaveX { get; set; }

    public byte SaveY { get; set; }

    public byte Dir { get; set; }

    public uint Hp { get; set; }

    public uint MaxHp { get; set; }

    public uint Mp { get; set; }

    public uint MaxMp { get; set; }

    public uint Sp { get; set; }

    public uint MaxSp { get; set; }

    public uint Ep { get; set; }

    public DateTime EpLoginDate { get; set; }

    public DateTime EpGreetingDate { get; set; }

    public short EpUsed { get; set; }

    public byte TailStyle { get; set; }

    public byte WingStyle { get; set; }

    public byte WingColor { get; set; }

    public bool Online { get; set; }

    public short Cl { get; set; }

    public ushort Str { get; set; }

    public ushort Dex { get; set; }

    public ushort Int { get; set; }

    public ushort Vit { get; set; }

    public ushort Agi { get; set; }

    public ushort Mag { get; set; }

    public ushort StatsPoint { get; set; }

    public ushort SkillPoint { get; set; }

    public ushort SkillPoint2x { get; set; }

    public ushort SkillPoint2t { get; set; }

    public ushort SkillPoint3 { get; set; }

    public ulong ExplorerExp { get; set; }

    public long Gold { get; set; }

    public uint Cp { get; set; }

    public uint ECoin { get; set; }

    public ulong CExp { get; set; }

    public ulong JExp { get; set; }

    public ulong JointJobExp { get; set; }

    public int Wrp { get; set; }

    public uint PossessionTarget { get; set; }

    public uint QuestId { get; set; }

    public DateTime QuestEndTime { get; set; }

    public byte QuestStatus { get; set; }

    public int QuestCurrentCount1 { get; set; }

    public int QuestCurrentCount2 { get; set; }

    public int QuestCurrentCount3 { get; set; }

    public uint Party { get; set; }

    public uint Ring { get; set; }

    public uint Golem { get; set; }

    [SugarColumn(IsNullable = true)] public byte WaitType { get; set; }

    public int AbyssFloor { get; set; }

    public byte DualJobId { get; set; }

    public ushort ExStatPoint { get; set; }

    public byte ExSkillPoint { get; set; }
}