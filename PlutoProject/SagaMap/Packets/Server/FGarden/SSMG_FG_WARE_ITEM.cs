using SagaLib;
using SagaMap.Packets.Server.Util;

namespace SagaMap.Packets.Server.FGarden
{
    public class SSMG_FG_WARE_ITEM : HasItemDetail
    {
        public SSMG_FG_WARE_ITEM()
        {
            if (Configuration.Configuration.Instance.Version < Version.Saga9_Iris)
                data = new byte[170];
            else
                data = new byte[217];
            offset = 2;
            ID = 0x1c26;
        }

        public SagaDB.Item.Item Item
        {
            set
            {
                offset = 7;
                ItemDetail = value;
                PutByte((byte)(data.Length - 3), 2);
            }
        }

        public uint InventorySlot
        {
            set => PutUInt(value, 3);
        }
    }
}