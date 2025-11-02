using System.Linq;
using SagaDB.Actor;
using SagaDB.Entities;

namespace SagaDB.Repository;

public class WarehouseRepository {
    public static Warehouse CreateWarehouse(ActorPC aChar, uint accountId) {
        var result = SqlSugarHelper.Db.Queryable<Warehouse>().Where(item => item.AccountId == accountId).ToList();

        return result.Count != 0
            ? result.First()
            : SqlSugarHelper.Db.Insertable<Warehouse>(new Warehouse {
                AccountId = accountId,
                Data = aChar.Inventory.WareToBytes()
            }).ExecuteReturnEntity();
    }

    public static Warehouse SaveWarehouse(ActorPC aChar, uint accountId) {
        return SqlSugarHelper.Db.Storageable<Warehouse>(new Warehouse {
            AccountId = accountId,
            Data = aChar.Inventory.WareToBytes()
        }).ExecuteReturnEntity();
    }
}