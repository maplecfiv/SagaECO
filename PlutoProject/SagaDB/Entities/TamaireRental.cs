using System;
using SqlSugar;

namespace SagaDB.Entities;

public class TamaireRental {
    [SugarColumn(IsPrimaryKey = true)] public uint CharacterId { get; set; }


    public DateTime RentDue { get; set; }

    public uint CurrentLender { get; set; }

    public uint LastLender { get; set; }
}