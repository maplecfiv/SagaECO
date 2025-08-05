using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_PARTY_INVITE_RESULT : Packet
    {
        public SSMG_PARTY_INVITE_RESULT()
        {
            data = new byte[6];
            offset = 2;
            ID = 0x19CC;
        }

        public int InviteResult
        {
            set => PutInt(value, 2);
        }
    }
}