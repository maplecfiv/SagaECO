using SagaLib;

namespace SagaMap.Packets.Server.Bond
{
    public class SSMG_BOND_INVITE_MASTER_RESULT : Packet
    {
        public SSMG_BOND_INVITE_MASTER_RESULT()
        {
            data = new byte[6];
            offset = 2;
            ID = 0x1FE1;
        }

        public int Result
        {
            set => PutInt(value, 2);
        }
    }
}