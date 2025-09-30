using SagaDB.Actor;
using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.DEM
{
    public class CSMG_DEM_FORM_CHANGE : Packet
    {
        public CSMG_DEM_FORM_CHANGE()
        {
            offset = 2;
        }

        public DEM_FORM Form => (DEM_FORM)GetByte(2);

        public override Packet New()
        {
            return new CSMG_DEM_FORM_CHANGE();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnDEMFormChange(this);
        }
    }
}