using System.Collections.Generic;
using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_GOLEM_SHOP_BUY_SELL : Packet
    {
        public CSMG_GOLEM_SHOP_BUY_SELL()
        {
            offset = 2;
        }

        public uint ActorID => GetUInt(2);

        public Dictionary<uint, ushort> Items
        {
            get
            {
                var num = GetByte(6);
                var items = new Dictionary<uint, ushort>();
                for (var i = 0; i < num; i++)
                {
                    var id = GetUInt((ushort)(8 + num * 4 + i * 4));
                    var count = GetUShort((ushort)(9 + num * 8 + i * 2));
                    items.Add(id, count);
                }

                return items;
            }
        }

        public override Packet New()
        {
            return new CSMG_GOLEM_SHOP_BUY_SELL();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnGolemShopBuySell(this);
        }
    }
}