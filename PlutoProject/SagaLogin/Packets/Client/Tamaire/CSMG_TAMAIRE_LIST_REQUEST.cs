using SagaLib;
using SagaLogin.Network.Client;

namespace SagaLogin.Packets.Client
{
    public class CSMG_TAMAIRE_LIST_REQUEST : Packet
    {
        public CSMG_TAMAIRE_LIST_REQUEST()
        {
            offset = 2;
        }

        public byte page => GetByte(3);

        public byte minlevel => GetByte(5);

        public byte maxlevel => GetByte(6);

        public byte JobType => GetByte(7);


        public override Packet New()
        {
            return new CSMG_TAMAIRE_LIST_REQUEST();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((LoginClient)client).OnTamaireListRequest(this);
        }
    }
}