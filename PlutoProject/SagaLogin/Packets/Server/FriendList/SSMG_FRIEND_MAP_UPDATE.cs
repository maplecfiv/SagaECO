using SagaLib;

namespace SagaLogin.Packets.Server
{
    public class SSMG_FRIEND_MAP_UPDATE : Packet
    {
        public SSMG_FRIEND_MAP_UPDATE()
        {
            data = new byte[10];
            ID = 0x00E8;
        }

        public uint CharID
        {
            set => PutUInt(value, 2);
        }

        public uint MapID
        {
            set => PutUInt(value, 6);
        }
    }
}