using System.Collections.Generic;
using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.Trade
{
    public class CSMG_TRADE_ITEM : Packet
    {
        public CSMG_TRADE_ITEM()
        {
            offset = 2;
        }

        public List<uint> InventoryID
        {
            get
            {
                var list = new List<uint>();
                var count = GetByte(2);
                for (var i = 0; i < count; i++) list.Add(GetUInt((ushort)(3 + i * 4)));
                return list;
            }
        }

        public List<ushort> Count
        {
            get
            {
                var list = new List<ushort>();
                var count = GetByte(2);
                for (var i = 0; i < count; i++) list.Add(GetUShort((ushort)(4 + count * 4 + i * 2)));
                return list;
            }
        }

        public uint Gold
        {
            get
            {
                var count = GetByte(2);
                return GetUInt((ushort)(8 + count * 6));
            }
        }

        public override Packet New()
        {
            return new CSMG_TRADE_ITEM();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnTradeItem(this);
        }
    }
}