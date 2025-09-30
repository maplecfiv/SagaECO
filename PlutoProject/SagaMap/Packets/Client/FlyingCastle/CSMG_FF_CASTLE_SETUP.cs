using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.FlyingCastle
{
    public class CSMG_FF_CASTLE_SETUP : Packet
    {
        public CSMG_FF_CASTLE_SETUP()
        {
            offset = 2;
        }

        public uint InventorySlot => GetUInt(2);

        public short X => GetShort(6);

        public short Z => GetShort(8);

        public short Yaxis => GetShort(10);

        public override Packet New()
        {
            return new CSMG_FF_CASTLE_SETUP();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnFFurnitureCastleSetup(this);
        }
    }
}