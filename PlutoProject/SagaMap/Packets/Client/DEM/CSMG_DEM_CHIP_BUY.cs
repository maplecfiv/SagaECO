using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_DEM_CHIP_BUY : Packet
    {
        public CSMG_DEM_CHIP_BUY()
        {
            offset = 2;
        }

        public uint[] ItemIDs
        {
            get
            {
                var items = new uint[GetByte(2)];
                for (var i = 0; i < items.Length; i++) items[i] = GetUInt((ushort)(3 + i * 4));
                return items;
            }
        }

        public int[] Counts
        {
            get
            {
                var count = GetByte(2);
                var items = new int[count];
                for (var i = 0; i < count; i++) items[i] = GetInt((ushort)(4 + count * 4 + i * 4));
                return items;
            }
        }

        public override Packet New()
        {
            return new CSMG_DEM_CHIP_BUY();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnDEMChipBuy(this);
        }
    }
}