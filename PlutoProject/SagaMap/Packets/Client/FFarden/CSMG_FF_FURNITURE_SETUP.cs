using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.FFarden
{
    public class CSMG_FF_FURNITURE_SETUP : Packet
    {
        public CSMG_FF_FURNITURE_SETUP()
        {
            offset = 2;
        }

        public uint InventorySlot => GetUInt(2);


        public short X => GetShort(6);

        public short Y => GetShort(8);

        public short Z => GetShort(10);

        public short Xaxis => GetShort(12);

        public short Yaxis => GetShort(14);

        public short Zaxis => GetShort(16);

        public override Packet New()
        {
            return new CSMG_FF_FURNITURE_SETUP();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnFFurnitureSetup(this);
        }
    }
}