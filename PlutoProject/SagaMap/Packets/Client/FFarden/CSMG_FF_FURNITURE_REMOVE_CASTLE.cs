using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.FFGarden
{
    public class CSMG_FF_FURNITURE_REMOVE_CASTLE : Packet
    {
        public CSMG_FF_FURNITURE_REMOVE_CASTLE()
        {
            offset = 2;
        }

        public uint ItemID => GetUInt(2);

        public override Packet New()
        {
            return new CSMG_FF_FURNITURE_REMOVE_CASTLE();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnFFFurnitureRemoveCastle(this);
        }
    }
}