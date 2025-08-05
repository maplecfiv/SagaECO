using SagaDB.Actor;
using SagaLib;

namespace SagaLogin.Packets.Server.FriendList
{
    public class SSMG_FRIEND_DETAIL_UPDATE : Packet
    {
        public SSMG_FRIEND_DETAIL_UPDATE()
        {
            data = new byte[12];
            ID = 0x00E3;
        }

        public uint CharID
        {
            set => PutUInt(value, 2);
        }

        public PC_JOB Job
        {
            set => PutUShort((ushort)value, 6);
        }

        public byte Level
        {
            set => PutUShort(value, 8);
        }

        public byte JobLevel
        {
            set => PutUShort(value, 10);
        }
    }
}