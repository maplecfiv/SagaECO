using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_FF_FISHBAIT_EQUIP : Packet
    {
        public CSMG_FF_FISHBAIT_EQUIP()
        {
            offset = 2;
            data = new byte[6];
        }

        public uint InventorySlot => GetUInt(2);

        public override Packet New()
        {
            return new CSMG_FF_FISHBAIT_EQUIP();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnFishBaitsEquip(this);
        }
    }
}