using System;
using SqlSugar;

namespace SagaDB.Entities;

public class Login {
    [SugarColumn(IsPrimaryKey = true)] public int AccountId { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string DeletePassword { get; set; }
    public byte Banned { get; set; }
    public byte GameMasterLevel { get; set; }
    public uint Bank { get; set; }
    public uint VShopPoints { get; set; }
    public uint UsedVShopPoints { get; set; }
    public string LastIp { get; set; }
    public string LastIp2 { get; set; }
    public DateTime QuestResetTime { get; set; }
    public DateTime LastLoginTime { get; set; }
    public string MacAddress { get; set; }
    public string PlayerNames { get; set; }
}