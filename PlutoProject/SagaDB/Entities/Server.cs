using System;
using SqlSugar;

namespace SagaDB.Entities;

public class Server {
    [SugarColumn(IsPrimaryKey = true)] public Guid RecordId { get; set; }
    public string ServerIp { get; set; }
    public int Port { get; set; }
    public string Type { get; set; }
}