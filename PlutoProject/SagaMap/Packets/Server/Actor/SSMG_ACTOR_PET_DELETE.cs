using SagaLib;

namespace SagaMap.Packets.Server.Actor
{
    public class SSMG_ACTOR_PET_DELETE : Packet
    {
        public SSMG_ACTOR_PET_DELETE()
        {
            data = new byte[7];
            offset = 2;
            ID = 0x1234;
        }

        public uint ActorID
        {
            set => PutUInt(value, 2);
        }
    }
}