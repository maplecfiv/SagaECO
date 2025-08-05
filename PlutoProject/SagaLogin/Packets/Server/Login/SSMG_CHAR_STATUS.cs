using SagaLib;

namespace SagaLogin.Packets.Server.Login
{
    public class SSMG_CHAR_STATUS : Packet
    {
        public SSMG_CHAR_STATUS()
        {
            data = new byte[5];
            offset = 14;
            ID = 0xDD;

            PutByte(1, 2);
            PutByte(1, 3);
        }

        public uint MapID
        {
            set => PutUInt(value, 2);
        }
    }
}