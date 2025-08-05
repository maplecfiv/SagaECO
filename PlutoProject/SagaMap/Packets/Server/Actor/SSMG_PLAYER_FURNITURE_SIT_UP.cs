using SagaLib;

namespace SagaMap.Packets.Server.Actor
{
    public class SSMG_PLAYER_FURNITURE_SIT_UP : Packet
    {
        public SSMG_PLAYER_FURNITURE_SIT_UP()
        {
            data = new byte[14];
            ID = 0x2066;
        }

        public uint ActorID
        {
            set => PutUInt(value, 2);
        }

        public uint FurnitureID
        {
            set => PutUInt(value, 6);
        }

        public int unknown
        {
            set => PutInt(value, 10);
        }
    }
}