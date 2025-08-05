using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.Item
{
    public class CSMG_ITEM_EQUIPT : Packet
    {
        public CSMG_ITEM_EQUIPT()
        {
            offset = 2;
        }

        public uint InventoryID
        {
            set
            {
                data = new byte[6];
                PutUInt(value, 2);
            }
            get => GetUInt(2);
        }

        public byte EquipSlot
        {
            set => PutByte(value, 6);
            get => GetByte(6);
        }

        public override Packet New()
        {
            return new CSMG_ITEM_EQUIPT();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnItemEquipt(this);
        }
    }
}