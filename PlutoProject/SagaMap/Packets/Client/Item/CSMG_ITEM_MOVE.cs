using SagaDB.Item;
using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.Item
{
    public class CSMG_ITEM_MOVE : Packet
    {
        public CSMG_ITEM_MOVE()
        {
            offset = 2;
        }

        public uint InventoryID
        {
            get => GetUInt(2);
            set => PutUInt(value, 2);
        }

        public ContainerType Target
        {
            get => (ContainerType)GetByte(6);
            set => PutByte((byte)value, 6);
        }

        public ushort Count
        {
            get => GetUShort(7);
            set => PutUShort(value, 7);
        }

        public override Packet New()
        {
            return new CSMG_ITEM_MOVE();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnItemMove(this);
        }
    }
}