using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.FGarden
{
    public class CSMG_FGARDEN_EQUIPT : Packet
    {
        public CSMG_FGARDEN_EQUIPT()
        {
            offset = 2;
        }

        public uint InventorySlot => GetUInt(2);


        public FGardenSlot Place => (FGardenSlot)GetUInt(6);

        public override Packet New()
        {
            return new CSMG_FGARDEN_EQUIPT();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnFGardenEquipt(this);
        }
    }
}