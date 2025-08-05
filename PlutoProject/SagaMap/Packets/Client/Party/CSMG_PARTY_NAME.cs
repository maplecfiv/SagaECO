using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.Party
{
    public class CSMG_PARTY_NAME : Packet
    {
        public CSMG_PARTY_NAME()
        {
            offset = 2;
        }

        public string Name => Global.Unicode.GetString(GetBytes((ushort)(GetByte(2) - 1), 3));

        public override Packet New()
        {
            return new CSMG_PARTY_NAME();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnPartyName(this);
        }
    }
}