using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.Actor
{
    public class CSMG_PLAYER_MIRROR_OPEN : Packet
    {
        public CSMG_PLAYER_MIRROR_OPEN()
        {
            data = new byte[10];
            ID = 0x02B2;
            offset = 2;
        }

        public override Packet New()
        {
            return new CSMG_PLAYER_MIRROR_OPEN();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnMirrorOpenRequire(this);
        }
    }
}