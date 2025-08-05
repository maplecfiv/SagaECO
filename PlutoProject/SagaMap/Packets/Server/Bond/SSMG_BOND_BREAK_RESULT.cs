using SagaLib;

namespace SagaMap.Packets.Server.Bond
{
    public class SSMG_BOND_BREAK_RESULT : Packet
    {
        public SSMG_BOND_BREAK_RESULT()
        {
            data = new byte[6];
            offset = 2;
            ID = 0x1FE9;
        }

        public int Result
        {
            set => PutInt(value, 2);
        }
    }
}