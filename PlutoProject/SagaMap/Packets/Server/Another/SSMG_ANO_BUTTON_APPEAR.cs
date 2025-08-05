using SagaLib;

namespace SagaMap.Packets.Server.Another
{
    public class SSMG_ANO_DIALOG_BOX : Packet
    {
        public SSMG_ANO_DIALOG_BOX()
        {
            data = new byte[4];
            offset = 2;
            ID = 0x1FB4;
        }

        public ushort DID
        {
            set => PutUShort(value, 2);
        }
    }
}