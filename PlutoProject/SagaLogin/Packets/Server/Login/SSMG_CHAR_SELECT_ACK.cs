using SagaLib;

namespace SagaLogin.Packets.Server.Login
{
    public class SSMG_CHAR_SELECT_ACK : Packet
    {
        public SSMG_CHAR_SELECT_ACK()
        {
            data = new byte[6];
            offset = 14;
            ID = 0xA8;
        }

        public uint MapID
        {
            set => PutUInt(value, 2);
        }
    }
}