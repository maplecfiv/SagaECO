using SagaLib;

namespace SagaMap.Packets.Server.Bond
{
    public class SSMG_BOND_INVITE_TO_PUPILIN : Packet
    {
        public SSMG_BOND_INVITE_TO_PUPILIN()
        {
            data = new byte[6];
            offset = 2;
            ID = 0x1FE2;
        }

        public uint MasterID
        {
            set => PutUInt(value, 2);
        }
    }
}