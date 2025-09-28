using SagaLib;

namespace SagaMap.Packets.Server.DEM
{
    public class SSMG_DEM_FORM_CHANGE : Packet
    {
        public SSMG_DEM_FORM_CHANGE()
        {
            data = new byte[3];
            offset = 2;
            ID = 0x1E7E;
        }

        public DEM_FORM Form
        {
            set => PutByte((byte)value, 2);
        }
    }
}