using SagaDB.Actor;
using SagaDB.Entities;

namespace SagaDB.Repository;

public class InventoryRepository {
    public static Inventory SaveInventory(ActorPC aChar) {
        return SqlSugarHelper.Db.Storageable<Inventory>(new Inventory {
            CharacterId = aChar.CharID,
            Data = aChar.Inventory.ToBytes()
        }).ExecuteReturnEntity();
    }
}