using SagaLib;

namespace SagaMap.Packets.Server.Bond
{
    public class SSMG_BOND_INFO_INDEX : Packet
    {
        public SSMG_BOND_INFO_INDEX()
        {
            data = new byte[7];
            offset = 2;
            ID = 0x1FEA;
        }

        public uint TargetCharID
        {
            set => PutUInt(value, 2);
        }

        public byte Index
        {
            set => PutByte(value, 6);
        }
    }
}