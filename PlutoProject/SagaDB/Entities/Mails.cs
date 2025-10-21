using System;
using SqlSugar;

namespace SagaDB.Entities;

public class Mails {
    [SugarColumn(IsPrimaryKey = true)] public uint MailId { get; set; }

    public uint CharacterId { get; set; }
    public string Sender { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }

    public DateTime PostDate { get; set; }
}