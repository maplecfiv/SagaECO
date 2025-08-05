using System.Collections.Generic;
using System.Linq;
using SagaDB.Actor;
using SagaDB.Item;
using SagaLib;

namespace SagaLogin.Packets.Server.Login
{
    public class SSMG_CHAR_EQUIP : Packet
    {
        public SSMG_CHAR_EQUIP()
        {
            if (Configuration.Configuration.Instance.Version >= Version.Saga10)
                data = new byte[230];
            else
                data = new byte[161];
            offset = 14;
            ID = 0x0029;

            PutByte(0xE, 2);
            PutByte(0xE, 59);
            PutByte(0xE, 116);
            if (Configuration.Configuration.Instance.Version >= Version.Saga10) PutByte(0xE, 173);
        }


        public List<ActorPC> Characters
        {
            set
            {
                int count;
                if (Configuration.Configuration.Instance.Version >= Version.Saga10)
                    count = 4;
                else
                    count = 3;
                for (var i = 0; i < count; i++)
                {
                    var pcs =
                        from p in value
                        where p.Slot == i
                        select p;
                    if (pcs.Count() == 0)
                        continue;
                    var pc = pcs.First();
                    for (var j = 0; j < 15; j++)
                        if (pc.Inventory.Equipments.ContainsKey((EnumEquipSlot)j))
                        {
                            var item = pc.Inventory.Equipments[(EnumEquipSlot)j];
                            if (item.PictID == 0)
                                PutUInt(item.ItemID, (ushort)(3 + i * 57 + j * 4));
                            else if (item.BaseData.itemType != ItemType.PET_NEKOMATA &&
                                     item.BaseData.itemType != ItemType.PARTNER &&
                                     item.BaseData.itemType != ItemType.RIDE_PARTNER)
                                PutUInt(item.PictID, (ushort)(3 + i * 57 + j * 4));
                        }
                }
            }
        }
    }
}