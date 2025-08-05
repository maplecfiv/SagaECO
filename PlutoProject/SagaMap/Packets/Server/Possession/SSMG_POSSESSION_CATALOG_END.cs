using SagaLib;

namespace SagaMap.Packets.Server.Possession
{
    public class SSMG_POSSESSION_CATALOG_END : Packet
    {
        public SSMG_POSSESSION_CATALOG_END()
        {
            data = new byte[4];
            offset = 2;
            ID = 0x1790;
        }

        public ushort Page
        {
            set => PutUShort(value, 2);
        }
    }
}