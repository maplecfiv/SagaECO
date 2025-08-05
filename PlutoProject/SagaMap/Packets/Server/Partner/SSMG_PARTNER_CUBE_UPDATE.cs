using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_PARTNER_CUBE_UPDATE : Packet
    {
        public SSMG_PARTNER_CUBE_UPDATE()
        {
            data = new byte[8];
            offset = 2;
            ID = 0x2187;
        }

        public uint PartnerInventorySlot
        {
            set => PutUInt(value, 2);
        }

        public ushort CubeUniqueID
        {
            set => PutUShort(value, 6);
        }
    }
}