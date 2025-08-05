using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.Possession
{
    public class CSMG_POSSESSION_CANCEL : Packet
    {
        public CSMG_POSSESSION_CANCEL()
        {
            data = new byte[5];
            offset = 2;
        }

        public PossessionPosition PossessionPosition
        {
            get => (PossessionPosition)GetByte(2);
            set => PutByte((byte)value, 2);
        }

        public override Packet New()
        {
            return new CSMG_POSSESSION_CANCEL();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnPossessionCancel(this);
        }
    }
}