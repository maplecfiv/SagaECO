using System.Collections.Generic;
using SagaDB.Item;
using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.Actor
{
    public class CSMG_PLAYER_SHOP_SELL_BUY : Packet
    {
        public CSMG_PLAYER_SHOP_SELL_BUY()
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
                    var id = GetUInt((ushort)(7 + i * 4));
                    var count = GetUShort((ushort)(8 + num * 4 + i * 2));
                    items.Add(id, count);
                }

                return items;
            }
        }

        public ContainerType Container
        {
            get
            {
                var num = GetByte(6);
                return (ContainerType)GetByte((ushort)(8 + num * 6));
            }
        }

        public override Packet New()
        {
            return new CSMG_PLAYER_SHOP_SELL_BUY();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnPlayerShopSellBuy(this);
        }
    }
}