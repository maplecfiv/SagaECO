using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.FlyingGarden
{
    public class CSMG_FGARDEN_FURNITURE_SETUP : Packet
    {
        public CSMG_FGARDEN_FURNITURE_SETUP()
        {
            offset = 2;
        }

        public uint InventorySlot => GetUInt(2);


        public short X => GetShort(6);

        public short Y => GetShort(8);

        public short Z => GetShort(10);

        public short AxleX => GetShort(12);

        public short AxleY => GetShort(14);

        public short AxleZ => GetShort(16);

        public override Packet New()
        {
            return new CSMG_FGARDEN_FURNITURE_SETUP();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnFGardenFurnitureSetup(this);
        }
    }
}