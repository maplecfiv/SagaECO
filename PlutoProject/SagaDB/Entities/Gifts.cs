using System;
using SqlSugar;

namespace SagaDB.Entities;

public class Gifts {
    [SugarColumn(IsPrimaryKey = true)] public uint GiftId { get; set; }

    public uint AccountId { get; set; }
    public string Sender { get; set; }
    public string Title { get; set; }

    public DateTime PostDate { get; set; }
    public uint MailId { get; set; }

    public uint ItemId1 { get; set; }
    public ushort ItemCount1 { get; set; }

    public uint ItemId2 { get; set; }
    public ushort ItemCount2 { get; set; }

    public uint ItemId3 { get; set; }
    public ushort ItemCount3 { get; set; }

    public uint ItemId4 { get; set; }
    public ushort ItemCount4 { get; set; }

    public uint ItemId5 { get; set; }
    public ushort ItemCount5 { get; set; }

    public uint ItemId6 { get; set; }
    public ushort ItemCount6 { get; set; }

    public uint ItemId7 { get; set; }
    public ushort ItemCount7 { get; set; }

    public uint ItemId8 { get; set; }
    public ushort ItemCount8 { get; set; }

    public uint ItemId9 { get; set; }
    public ushort ItemCount9 { get; set; }

    public uint ItemId10 { get; set; }
    public ushort ItemCount10 { get; set; }
}