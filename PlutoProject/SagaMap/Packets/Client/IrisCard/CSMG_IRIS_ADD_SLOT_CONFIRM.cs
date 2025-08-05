using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.IrisCard
{
    public class CSMG_IRIS_ADD_SLOT_CONFIRM : Packet
    {
        public CSMG_IRIS_ADD_SLOT_CONFIRM()
        {
            offset = 2;
        }

        public uint InventorySlot => GetUInt(2);

        public uint Material => GetUInt(6);

        public uint SupportItem => GetUInt(10);

        public uint ProtectItem => GetUInt(14);

        public byte BaseLevel => GetByte(18);

        public byte JobLevel => GetByte(19);

        public ushort ExpRate => GetUShort(20);

        public ushort JExpRate => GetUShort(22);


        public override Packet New()
        {
            return new CSMG_IRIS_ADD_SLOT_CONFIRM();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnIrisAddSlotConfirm(this);
        }
    }
}