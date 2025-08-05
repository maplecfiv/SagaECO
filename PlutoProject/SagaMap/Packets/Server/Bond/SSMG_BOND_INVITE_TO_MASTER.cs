using SagaLib;

namespace SagaMap.Packets.Server.Bond
{
    public class SSMG_BOND_INVITE_TO_MASTER : Packet
    {
        public SSMG_BOND_INVITE_TO_MASTER()
        {
            data = new byte[6];
            offset = 2;
            ID = 0x1FE6;
        }

        public uint PupilinID
        {
            set => PutUInt(value, 2);
        }
    }
}