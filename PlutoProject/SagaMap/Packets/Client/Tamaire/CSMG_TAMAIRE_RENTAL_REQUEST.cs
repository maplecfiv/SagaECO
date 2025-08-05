using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.Tamaire
{
    public class CSMG_TAMAIRE_RENTAL_REQUEST : Packet
    {
        public CSMG_TAMAIRE_RENTAL_REQUEST()
        {
            offset = 2;
        }

        public uint Lender => GetUInt(2);


        public override Packet New()
        {
            return new CSMG_TAMAIRE_RENTAL_REQUEST();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnTamaireRentalRequest(this);
        }
    }
}