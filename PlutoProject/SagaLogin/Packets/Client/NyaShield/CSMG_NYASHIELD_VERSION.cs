using SagaLib;

namespace SagaLogin.Packets.Client.NyaShield
{
    public class CSMG_NYASHIELD_VERSION : Packet
    {
        public CSMG_NYASHIELD_VERSION()
        {
            size = 6;
            offset = 2;
        }

        public ushort ver => GetByte(2);

        public override Packet New()
        {
            return new CSMG_NYASHIELD_VERSION();
        }

        public override void Parse(SagaLib.Client client)
        {
            //((LoginClient)(client)).OnNya(this);
        }
    }
}