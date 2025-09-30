using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.FlyingGarden
{
    public class CSMG_FGARDEN_FURNITURE_RECONFIG : Packet
    {
        public CSMG_FGARDEN_FURNITURE_RECONFIG()
        {
            offset = 2;
        }

        public uint ActorID => GetUInt(2);

        public short X => GetShort(6);

        public short Y => GetShort(8);

        public short Z => GetShort(10);

        public ushort Dir => GetUShort(12);

        public override Packet New()
        {
            return new CSMG_FGARDEN_FURNITURE_RECONFIG();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnFGardenFurnitureReconfig(this);
        }
    }
}