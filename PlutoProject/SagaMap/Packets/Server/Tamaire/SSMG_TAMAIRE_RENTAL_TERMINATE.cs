using SagaLib;

namespace SagaMap.Packets.Server.Tamaire
{
    public class SSMG_TAMAIRE_RENTAL_TERMINATE : Packet
    {
        public SSMG_TAMAIRE_RENTAL_TERMINATE()
        {
            data = new byte[3];
            offset = 2;
            ID = 0x22B5;
        }

        public byte Reason
        {
            set => PutByte(value, 2); //00 = expired, 01 = terminated
        }
    }
}