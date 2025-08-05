using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.Item
{
    public class CSMG_ITEM_ENHANCE_CONFIRM : Packet
    {
        public CSMG_ITEM_ENHANCE_CONFIRM()
        {
            offset = 2;
        }

        public uint InventorySlot => GetUInt(2);

        public uint Material => GetUInt(6);

        public uint ProtectItem => GetUInt(10);

        public uint SupportItem => GetUInt(14);

        public byte BaseLevel => GetByte(18);

        public byte JobLevel => GetByte(19);

        public ushort ExpRate => GetUShort(20);

        public ushort JExpRate => GetUShort(22);


        public override Packet New()
        {
            return new CSMG_ITEM_ENHANCE_CONFIRM();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnItemEnhanceConfirm(this);
        }
    }
}