using SagaLib;

namespace SagaLogin.Packets.Server
{
    public class SSMG_FRIEND_ADD_OK : Packet
    {
        public SSMG_FRIEND_ADD_OK()
        {
            data = new byte[6];
            ID = 0x00D5;
        }

        public uint CharID
        {
            set => PutUInt(value, 2);
        }
    }
}