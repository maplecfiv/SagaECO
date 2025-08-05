using System;
using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_TAMAIRE_RENTAL : Packet
    {
        public SSMG_TAMAIRE_RENTAL()
        {
            data = new byte[10];
            offset = 2;
            ID = 0x22B3;
        }

        public TimeSpan RentalDue
        {
            set => PutInt((int)value.TotalSeconds, 3);
        }

        public short Factor
        {
            set => PutShort(value, 7);
        }

        public byte JobType
        {
            set => PutByte(value, 9);
        }
    }
}