using SagaLib;

namespace SagaMap.Packets.Client
{
    public class CSMG_POSSESSION_PARTNER_CANCEL : Packet
    {
        public CSMG_POSSESSION_PARTNER_CANCEL()
        {
            data = new byte[3];
            offset = 2;
        }

        public PossessionPosition PossessionPosition => (PossessionPosition)GetByte(2);

        public override Packet New()
        {
            return new CSMG_POSSESSION_PARTNER_CANCEL();
        }

        public override void Parse(SagaLib.Client client)
        {
            //((MapClient)(client)).OnPossessionCancel(this);
        }
    }
}