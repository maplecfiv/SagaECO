using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_ODWAR_RESULT : Packet
    {
        public SSMG_ODWAR_RESULT()
        {
            data = new byte[16];
            offset = 2;
            ID = 0x1B85;
            PutByte(1, 2);
        }

        public bool Win
        {
            set
            {
                if (value)
                    PutByte(1, 3);
            }
        }

        public uint EXP
        {
            set => PutUInt(value, 4);
        }

        public uint JEXP
        {
            set => PutUInt(value, 8);
        }

        public uint CP
        {
            set => PutUInt(value, 12);
        }
    }
}