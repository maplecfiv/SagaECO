using SagaLib;
using SagaValidation.Network.Client;

namespace SagaValidation.Packets.Client {
    public class CSMG_SERVERLET_ASK : Packet {
        public CSMG_SERVERLET_ASK() {
            this.offset = 2;
        }

        public override SagaLib.Packet New() {
            return (SagaLib.Packet)new SagaValidation.Packets.Client.CSMG_SERVERLET_ASK();
        }

        public override void Parse(SagaLib.Client client) {
            ((ValidationClient)(client)).OnServerLstSend(this);
        }
    }
}